using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


namespace Luxena.Domain
{

	partial class Domain<TDomain>
	{

		public static EntityInfo[] CreateEntityInfos(DefaultLocalizationTypesSource lng, params Type[] additionalEntityTypes)
		{
			if (lng == null)
				lng = new DefaultLocalizationTypesSource();

			var assemblyTypes = typeof(TDomain).Assembly.DefinedTypes.ToArray();

			var baseEntities =
				from a in additionalEntityTypes
				select new EntityInfo(a, lng);

			var dbSetType = typeof(DbSet<>);
			var sets = (
				from a in typeof(TDomain).GetProperties()
				where a.PropertyType.IsRawGeneric(dbSetType)
				let info = new EntityInfo(a, lng)
				orderby info.EntityTypeDeep, info.EntityName
				select info
			).ToArray();

			var entityQueryType = typeof(EntityQuery<>);
			var entityQueries = (
				from a in typeof(TDomain).GetProperties()
				where a.PropertyType.BaseType.IsRawGeneric(entityQueryType)
				let info = new EntityInfo(a, lng)
				orderby info.EntityName
				select info
			).ToArray();

			var dbQueryType = typeof(DbQuery<,>);
			var dbQueries = (
				from a in typeof(TDomain).GetProperties()
				where a.PropertyType.BaseType.IsRawGeneric(dbQueryType)
				let info = new EntityInfo(a, lng)
				orderby info.EntityName
				select info
			).ToArray();

			var objectType = typeof(object);
			var dbQueries0 = (
				from a in dbQueries
				let type = a.EntityType.BaseType
				where type != objectType
				let info = new EntityInfo(type, lng) { IsClass = true, }
				orderby info.EntityName
				select info
			).ToArray();


			var queryParams = (
				from a in dbQueries
				let info = new EntityInfo(a.QueryParamsType, lng) { IsQueryParams = true }
				orderby info.EntityName
				select info
			).ToArray();

			var queryParams0 = (
				from a in queryParams
				let type = a.EntityType.BaseType
				where type != objectType
				let info = new EntityInfo(type, lng) { IsQueryParams = true }
				orderby info.EntityName
				select info
			).ToArray();

			var type1 = typeof(DomainAction);
			var actions = (
				from type in assemblyTypes
				where type.IsSubclassOf(type1)
				let info = new EntityInfo(type, lng)
				{
					IsDomainAction = true,
					IsBaseEntity = false,
					EntitySetName = type.Name,
				}
				orderby info.EntityName
				select info
			).ToArray();

			var byAttrs = (
				from type in assemblyTypes
				where type.HasAttribute<SemanticEntityAttribute>()
				let info = new EntityInfo(type, lng) { IsClass = true }
				orderby info.EntityName
				select info
			).ToArray();


			var domainType = typeof(TDomain);
			var funcs = (
				from method in domainType.GetMethods()
				where method.DeclaringType == domainType && !method.IsSpecialName
					&& method.HasAttribute<EntityActionAttribute>()
				let info = new EntityInfo(method, lng) { IsDomainFunction = true }
				orderby info.EntityName
				select info
			).ToArray();

			var funcResults = (
				from func in funcs
				let eltype = func.EntityMember.ResultType().GetElementType()
				where eltype != null
				let info = new EntityInfo(eltype, lng) { IsClass = true }
				orderby info.EntityName
				select info
			).ToArray();

			var entities = baseEntities
				.Concat(sets)
				.Concat(entityQueries)
				.Concat(dbQueries0)
				.Concat(dbQueries)
				.Concat(queryParams0)
				.Concat(queryParams)
				.Concat(actions)
				.Concat(byAttrs)
				.Concat(funcResults)
				.Concat(funcs)
				.Distinct()
				.ToArray();

			entities.ForEach(a => a.Init(entities, assemblyTypes));

			return entities;
		}

		public static EntityInfo[] CreateEntityInfos(params Type[] additionalEntityTypes)
		{
			return CreateEntityInfos(new DefaultLocalizationTypesSource(), additionalEntityTypes);
		}

		public static EntityInfo GetEntityInfo<TEntity>(
			Expression<Func<TDomain, DbSet<TEntity>>> getEntitySet,
			DefaultLocalizationTypesSource lng = null
		)
			where TEntity : Entity
		{
			var setProp = getEntitySet.GetProperties().One();

			if (lng == null)
				lng = new DefaultLocalizationTypesSource();

			return new EntityInfo(setProp, lng);
		}

		public static EntityInfo GetEntityInfo(
			Expression<Func<TDomain, object>> getEntitySet,
			DefaultLocalizationTypesSource lng = null
		)
		{
			var setProp = getEntitySet.GetProperties().One();

			if (lng == null)
				lng = new DefaultLocalizationTypesSource();

			return new EntityInfo(setProp, lng);
		}


		public class EntityInfo
		{

			internal EntityInfo(Type entityType, DefaultLocalizationTypesSource lng = null)
			{
				EntityType = entityType;

				EntityBaseType = EntityType.BaseType;
				IsBaseEntity = true;

				EntityName = EntityType.Name;
				EntityTypeDeep = GetEntityTypeDeep();

				LocalizationTypesSource = lng;
				Localization = EntityType.Localization(lng);
			}

			internal EntityInfo(PropertyInfo setProp, DefaultLocalizationTypesSource lng)
			{
				var propType = setProp.PropertyType;

				if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(DbSet<>))
				{
					IsEntity = true;
					EntityType = propType.GenericTypeArguments[0];
					EntityName = EntityType.Name;
					EntityBaseType = EntityType.BaseType;

					while (EntityBaseType != null && !EntityBaseType.IsUiMapped())
					{
						EntityBaseType = EntityBaseType.BaseType;
					}

				}
				else if (propType.BaseType.IsRawGeneric(typeof(EntityQuery<>)))
				{
					IsEntityQuery = true;
					// ReSharper disable once PossibleNullReferenceException
					EntityType = propType.BaseType.GenericTypeArguments[1];
					EntityBaseType = EntityType;
					EntityName = setProp.PropertyType.Name;
				}
				else if (propType.BaseType.IsRawGeneric(typeof(DbQuery<,>)))
				{
					IsQueryResult = true;
					QueryType = propType;
					// ReSharper disable once PossibleNullReferenceException
					QueryParamsType = propType.BaseType.GenericTypeArguments[1];
					QueryResultType = propType.BaseType.GenericTypeArguments[2];
					EntityType = QueryResultType;
					EntityBaseType = EntityType.BaseType;
					EntityName = EntityType.Name;
				}
				else
					throw new NotImplementedException();

				//EntityBaseType = EntityType.Semantic<ExtendsAttribute>().As(a => a.BaseType) ?? EntityType.BaseType;
				EntitySetType = setProp.PropertyType;
				EntitySetName = setProp.Name;

				EntityTypeDeep = GetEntityTypeDeep();

				LocalizationTypesSource = lng;
				Localization = EntitySetType.Localization(lng);
				EntityType.Localization(lng).AppendTo(Localization, lng);
			}

			internal EntityInfo(MethodInfo domainMethod, DefaultLocalizationTypesSource lng = null)
			{
				EntityBaseType = domainMethod.ReturnType.GetElementType();

				EntityMember = domainMethod;

				EntityName = EntitySetName = domainMethod.Name;

				LocalizationTypesSource = lng;
				Localization = domainMethod.Localization(lng);
			}

			private static MethodInfo GetLookupMethod(Type lookupSource)
			{
				return lookupSource
					.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
					.FirstOrDefault(a => a.Name == "Lookup");
			}


			public void Init(EntityInfo[] entities, IList<Type> assemblyTypes)
			{
				if (EntityType == null) return;

				IdProperty = EntityType.PropertyWithAttribute<KeyAttribute>();
				NameProperty = EntityType.PropertyWithAttribute<EntityNameAttribute>().If(a => a.PropertyType == typeof(string));
				TypeProperty = EntityType.PropertyWithAttribute<EntityTypeAttribute>();

				if (IsEntity || IsEntityQuery)
				{
					var entityTypeLookup = GetLookupMethod(EntityType);
					var entitySetLookup = GetLookupMethod(EntitySetType);

					LookupCallerType = entitySetLookup != null
						? entitySetLookup.DeclaringType
						: entityTypeLookup.As(a => a.DeclaringType);

					HasLookup = NameProperty != null;

					var lookupName = EntityName + "Lookup";
					LookupTypeName =
						assemblyTypes.Where(a => a.Name == lookupName).One().As(a => a.Name) ??
						(HasLookup ? EntityType.Name + "Lookup" : null);
				}


				if (!IsBaseEntity)
				{
					var rootType = IsEntityQuery ? EntityBaseType : EntityType;
					var rootBaseEntity = entities.By(rootType.BaseType);

					while (rootBaseEntity != null && !rootBaseEntity.IsBaseEntity && rootType != null)
					{
						rootType = rootType.BaseType;
						rootBaseEntity = entities.By(rootType.BaseType);
					}

					RootEntity = entities.By(a => a.EntityType == rootType);
				}

				BaseEntity = entities.By(EntityBaseType);
				DerivedEntities =
					IsEntityQuery ? new EntityInfo[0] :
					entities.Where(a => a.EntityType != null && (a.EntityType.IsSubclassOf(EntityType) || (a.IsEntityQuery && a.EntityType == EntityType))).ToArray();

				if (IsQueryResult)
				{
					var queryParamsEntity = entities.By(QueryParamsType);
					if (queryParamsEntity != null)
						queryParamsEntity.QueryResultType = QueryResultType;
				}
			}


			public readonly Type EntitySetType;
			public string EntitySetName;
			public DefaultLocalizationTypesSource LocalizationTypesSource;

			public readonly MemberInfo EntityMember = null;
			public readonly Type EntityType;
			public readonly string EntityName;
			public readonly Type EntityBaseType;
			public readonly Type QueryType;
			public readonly Type QueryParamsType;
			public Type QueryResultType;
			public int EntityTypeDeep;

			public EntityInfo RootEntity;
			public EntityInfo BaseEntity;
			public EntityInfo[] DerivedEntities;

			public readonly LocalizationAttribute Localization;

			public string Title => Localization?.Default;


			public string EntityBaseTypeName =>
				EntityBaseType == typeof(object) || EntityBaseType == null
					? "SemanticEntity"
					: EntityBaseType.Name + "Semantic";

			public bool IsBaseEntity;
			public bool IsEntity;
			public bool IsEntityQuery;
			public bool IsQueryResult;
			public bool IsQueryParams;
			public bool IsDomainAction;
			public bool IsClass;
			public bool IsDomainFunction;

			public Type LookupCallerType;
			public bool HasLookup;
			public string LookupTypeName;

			public PropertyInfo IdProperty;
			public PropertyInfo NameProperty;
			public PropertyInfo TypeProperty;

			public int GetEntityTypeDeep()
			{
				var type = EntityType;
				var deep = IsEntityQuery ? 1 : 0;

				while (type != null)
				{
					deep++;
					type = type.BaseType;
				}

				return deep;
			}

			public bool Is<TEntity2>()
			{
				return typeof(TEntity2).IsAssignableFrom(EntityType);
			}


			public EntityMemberInfo[] AllDbProperties
			{
				get
				{
					if (_allDbProperties != null)
						return _allDbProperties;

					if (IsDomainFunction)
						return _allDbProperties = ((MethodInfo)EntityMember)
							.GetParameters()
							.Select(a => new EntityMemberInfo
							{
								Parameter = a,
								Name = a.Name,
								Type = a.ParameterType,
								MemberCanWrite = true,
								CanWrite = true,

							})
							.ToArray();

					var props = EntityType
						.GetProperties()
						.Where(a => a.IsUiMapped() && !a.IsCollection())
						.Where(a => !a.IsNavigation() || EntityType.GetField("_" + a.Name, BindingFlags.Instance | BindingFlags.NonPublic) == null)
						.Select(a =>
						{
							var getMethod = a.GetGetMethod(false);
							var isOverrided = getMethod.GetBaseDefinition() != getMethod;

							return new EntityMemberInfo
							{
								Member = a,
								Name = a.Name,
								Type = a.PropertyType,
								DeclaringType = a.DeclaringType,
								IsOverrided = isOverrided,
								IsNavigation = a.IsNavigation(),
								MemberCanWrite = a.CanWrite,
								CanWrite = a.CanWrite && a.SetMethod.IsPublic,
								IsDbMapped = a.IsDbMapped(),
								Localization = a.Localization(LocalizationTypesSource),
							};
						});

					var navigations = EntityType
						.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
						.Where(a => a.IsUiMapped() && a.IsNavigation() && a.Name.StartsWith("_") && char.IsUpper(a.Name, 1))
						.Select(a => new EntityMemberInfo
						{
							Member = a,
							FieldName = a.Name,
							Name = a.Name.TrimStart('_'),
							Type = a.FieldType,
							DeclaringType = a.DeclaringType,
							IsNavigation = true,
							MemberCanWrite = true,
							CanWrite = true,
							IsDbMapped = a.IsDbMapped(),
							Localization = a.Localization(LocalizationTypesSource),
						});

					return _allDbProperties = props.AsConcat(navigations).ToArray();
				}
			}
			private EntityMemberInfo[] _allDbProperties;

			public EntityMemberInfo[] DbProperties => _dbProperties ?? (_dbProperties =
				IsEntityQuery
					? new EntityMemberInfo[0]
					: AllDbProperties
						.Where(a => !a.IsOverrided)
						.Where(a => a.DeclaringType == EntityType || !a.DeclaringType.IsUiMapped())
						.ToArray()
			);

			private EntityMemberInfo[] _dbProperties;


			public EntityMemberInfo[] NavigationProperties => _navigationProperties ?? (_navigationProperties =
				DbProperties.Where(a => a.IsNavigation).ToArray()
			);

			private EntityMemberInfo[] _navigationProperties;

			public EntityMemberInfo[] AllNavigationProperties => _allNavigationProperties ?? (_allNavigationProperties =
				AllDbProperties.Where(a => a.IsNavigation).ToArray()
			);

			private EntityMemberInfo[] _allNavigationProperties;

			public PropertyInfo[] CollectionProperties
			{
				get
				{
					return _collectionProperties ?? (_collectionProperties = EntityType?
						.GetProperties()
						.Where(a => a.IsUiMapped())
						.Where(a => a.IsCollection())
						.Where(a => a.DeclaringType == EntityType || !a.DeclaringType.IsUiMapped())
						.ToArray()
						?? new PropertyInfo[0]
					);
				}
			}
			private PropertyInfo[] _collectionProperties;

			public EntityMemberInfo[] AllEntityActions => _allEntityActions ?? (_allEntityActions = EntityType?
				.GetMethods()
				.Where(a => a.HasAttribute<EntityActionAttribute>())
				.Select(a => new EntityMemberInfo
				{
					Member = a,
					Name = a.Name,
					DeclaringType = a.DeclaringType,
					IsOverrided = a.GetBaseDefinition() != a,
					Localization = a.Localization(LocalizationTypesSource),
					Type = a.ReturnType,
				})
				.ToArray()
				?? new EntityMemberInfo[0]
			);
			private EntityMemberInfo[] _allEntityActions;

			public EntityMemberInfo[] EntityActions => _entityActions ?? (_entityActions =
				AllEntityActions
					.Where(a => !a.IsOverrided)
					.Where(a => a.DeclaringType == EntityType)
					.ToArray()
			);
			private EntityMemberInfo[] _entityActions;


			public readonly Dictionary<PropertyInfo, PropertyInfo> Associations = new Dictionary<PropertyInfo, PropertyInfo>();


			public string PropName<TAttribute>(string defaultName = null)
				where TAttribute : Attribute
			{
				return EntityType.PropertyWithAttribute<TAttribute>().As(a => a.Name) ?? defaultName;
			}


			public static implicit operator Type(EntityInfo info)
			{
				return info?.EntityType;
			}

			public static implicit operator string (EntityInfo info)
			{
				return info?.EntityName;
			}

			public override string ToString()
			{
				return EntityName;
			}

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(this, obj))
					return true;

				var info = obj as EntityInfo;
				if (info == null)
					return false;

				if (IsDomainFunction && info.IsDomainFunction)
					return EntityMember == info.EntityMember;

				return
					EntityType == info.EntityType && EntitySetType == info.EntitySetType;
			}

			public override int GetHashCode()
			{
				// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
				return EntityType?.GetHashCode() ?? base.GetHashCode();
			}

		}


		public class EntityMemberInfo
		{
			public string Name;
			public string FieldName;
			public MemberInfo Member;
			public ParameterInfo Parameter;
			public Type Type;
			public Type DeclaringType;
			public bool IsOverrided;
			public bool IsNavigation;
			public bool MemberCanWrite;
			public bool CanWrite;
			public bool IsDbMapped;

			public LocalizationAttribute Localization;
			public string Title => Localization?.Default;

			public override string ToString() => Name;
		}

	}


	public static class EntityInfoExtensions
	{

		public static Domain<TDomain>.EntityInfo By<TDomain>(
			this IEnumerable<Domain<TDomain>.EntityInfo> infos,
			Type entityType
		)
			where TDomain : Domain<TDomain>, new()
		{
			if (infos == null || entityType == null) return null;

			//			while (entityType != null && entityType.HasAttribute<NotMappedAttribute>())
			//			{
			//				entityType = entityType.BaseType;
			//			}

			var list = infos.ToList();

			return list.By(a => !a.IsEntityQuery && a.EntityType == entityType) ?? list.By(a => a.EntityType == entityType);
		}

	}

}
