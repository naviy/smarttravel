using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.SqlCommand;
using NHibernate.Type;

using Property = Luxena.Base.Metamodel.Property;
using NHibernateProperty = NHibernate.Mapping.Property;



namespace Luxena.Base.Data.NHibernate
{


	public class GenericDao : IGenericDao
	{

		public GenericDao(TransactionManager transManager, Configuration configuration)
		{
			_transManager = transManager;
			_configuration = configuration;
		}


		protected TransactionManager TransactionManager => _transManager;


		public object[] List(Class clazz, RecordConfig config)
		{
			var criteria = _transManager.Session.CreateCriteria(clazz.Type);

			SetCriteriaProjections(criteria, clazz, config, null, null);

			SetCriteriaOrder(criteria, clazz, null, null, null);

			var list = criteria.List<object>().ToArray();

			ConvertRecords(list, clazz, config, null, null);

			return list;
		}


		public RangeResponse List(Class clazz, RangeRequest request, RecordConfig config)
		{
			var criteria = _transManager.Session.CreateCriteria(clazz.Type);

			criteria.SetProjection(Projections.RowCount(), Projections.Max(Projections.Id()));

			SetCriteriaRestrictions(clazz, criteria, request);

			var totalCount = criteria.UniqueResult<object[]>();

			var response = new RangeResponse
			{
				TotalCount = (int)totalCount[0]
			};

			if (response.TotalCount == 0)
			{
				response.List = _emptyList;
			}
			else
			{
				if (response.TotalCount == 1)
				{
					criteria = _transManager.Session.CreateCriteria(clazz.Type);

					SetCriteriaProjections(criteria, clazz, config, request.VisibleProperties, request.HiddenProperties);

					var id = totalCount[1];

					criteria.Add(Restrictions.IdEq(id));

					if (request.PositionableObjectId == id)
						response.SelectedRow = 0;
				}
				else
				{
					if (response.TotalCount < request.Limit)
						request.Limit = response.TotalCount;

					if (request.PositionableObjectId != null && !IsObjectUnderRestrictions(clazz, request.PositionableObjectId, request))
					{
						criteria = _transManager.Session.CreateCriteria(clazz.Type);

						criteria.SetProjection(Projections.RowCount());

						SetCriteriaRestrictions(clazz, criteria, request);

						SetCriteriaOrder(criteria, clazz, request.Sort, request.Dir, response);

						SetPositionRestrictions(clazz, request.PositionableObjectId, criteria, response.Sort, response.Dir == "ASC");

						criteria.ClearOrders();

						var position = criteria.UniqueResult<int>();

						if (request.Limit != 0)
							request.Start = ((position - 1) / request.Limit) * request.Limit;

						response.SelectedRow = position - request.Start - 1;
					}

					criteria = _transManager.Session.CreateCriteria(clazz.Type);

					SetCriteriaProjections(criteria, clazz, config, request.VisibleProperties, request.HiddenProperties);

					SetCriteriaRestrictions(clazz, criteria, request);

					SetCriteriaOrder(criteria, clazz, request.Sort, request.Dir, response);

					if (request.Limit != 0)
					{
						if (request.Start >= response.TotalCount)
							request.Start = ((response.TotalCount - 1) / request.Limit) * request.Limit;

						criteria.SetFirstResult(request.Start).SetMaxResults(request.Limit);
					}
				}

				var list = criteria.List<object>().ToArray();

				ConvertRecords(list, clazz, config, request.VisibleProperties, request.HiddenProperties);

				response.List = list;
			}

			response.Start = response.List.Length == 0 ? 0 : request.Start;

			return response;
		}


		public object[] Refresh(Class clazz, object[] ids, string[] visibleProperties, string[] hiddenProperties, RecordConfig config)
		{
			if (!config.IncludeIdentifier)
				return null;

			var criteria = _transManager.Session.CreateCriteria(clazz.Type);

			SetCriteriaProjections(criteria, clazz, config, visibleProperties, hiddenProperties);

			var list = criteria
				.Add(Restrictions.In(Projections.Id(), ids))
				.List<object>()
				.ToArray();

			var result = new object[ids.Length];

			if (list.Length == 0)
				return result;

			var objMap = _arrayMode ?
				list.ToDictionary(obj => ((object[])obj)[0], obj => obj) :
				list.ToDictionary(obj => _transManager.Session.GetIdentifier(obj), obj => obj);

			visibleProperties = visibleProperties ?? _emptyPropertyNameList;
			hiddenProperties = hiddenProperties ?? _emptyPropertyNameList;

			for (var i = 0; i < ids.Length; ++i)
			{
				object obj;

				if (objMap.TryGetValue(ids[i], out obj))
					result[i] = MakeRecord(obj, clazz, config, visibleProperties, hiddenProperties);
			}

			return result;
		}

		public void Replace(object oldObject, object newObject)
		{
			var objClass = oldObject.GetClass();

			var dependencies = GetDependentClasses(objClass);

			//var oldId = oldObject == null ? null : _transManager.Session.GetIdentifier(oldObject);
			//var newId = (newObject as IEntity)?.Id;

			foreach (var pair in dependencies)
			{
				var cls = pair.Key;

				//var criteria = _transManager.Session.CreateCriteria(cls.MappedClass);

				//var disjunction = Restrictions.Disjunction();

				foreach (var property in pair.Value)
				{
					if (!property.IsComposite)
					{

						try
						{
							var query = _transManager.Session
								.CreateQuery($"update {cls.EntityName} set {property.Name} = :newObject where {property.Name} = :oldObject")
								.SetEntity("oldObject", oldObject);

							if (newObject == null)
								query.SetParameter("newObject", null);
							else
								query.SetEntity("newObject", newObject);

							query.ExecuteUpdate();
						}
						catch (GenericADOException ex)
						{
							throw new Exception(
								$"Ошибка при изменении {cls.EntityName}.{property.Name}: {(ex.InnerException ?? ex).Message}",
								ex
							);
						}

						/*var objs = _transManager.Session.CreateQuery(string.Format("from {0} where {1} = :oldObject", cls.EntityName, property.Name))
							.SetEntity("oldObject", oldObject)
							.List();

						foreach (var obj in objs)
						{
							property.GetSetter(obj.GetType()).Set(obj, newObject);
						}*/
					}
					else
					{
						var componentType = (ComponentType)property.Type;

						for (var i = 0; i < componentType.Subtypes.Length; i++)
						{
							var subtype = componentType.Subtypes[i];

							if (objClass.Type.Is(subtype.ReturnedClass))
							{
								_transManager.Session.CreateQuery(string.Format("update {0} set {1}.{2} = :newObject where {1}.{2} = :oldObject", cls.EntityName, property.Name, componentType.PropertyNames[i]))
									.SetEntity("oldObject", oldObject)
									.SetEntity("newObject", newObject)
									.ExecuteUpdate();

								/*var objs = _transManager.Session.CreateQuery(string.Format("from {0} where {1}.{2} = :oldObject", cls.EntityName, property.Name, componentType.PropertyNames[i]))
									.SetEntity("oldObject", oldObject)
									.List();

								foreach (var obj in objs)
								{
									var comp = property.GetGetter(obj.GetType()).Get(obj);

									var values = componentType.GetPropertyValues(comp, EntityMode.Poco);

									values[i] = newObject;

									componentType.SetPropertyValues(comp, values, EntityMode.Poco);
								}*/
							}
						}
					}
				}
			}
		}

		public object[] GetUndeletableObjects(Class clazz, object[] ids)
		{
			var classes = GetDependentClasses(clazz);

			if (classes.Count == 0)
				return null;

			var whereClause = new StringBuilder();
			var separator = string.Empty;

			foreach (var cls in classes.Keys)
			{
				var className = cls.MappedClass.Name;

				var where = new StringBuilder();
				var sep = string.Empty;

				foreach (var property in classes[cls])
				{
					var propName = property.Name;

					if (property.IsComposite)
					{
						var componentType = (ComponentType)property.Type;

						for (var i = 0; i < componentType.Subtypes.Length; i++)
						{
							var subtype = componentType.Subtypes[i];

							if (clazz.Type.Is(subtype.ReturnedClass))
							{
								propName += "." + componentType.PropertyNames[i];

								break;
							}
						}
					}

					where.Append(sep);
					where.AppendFormat("_{0}.{1} = t", className, propName);

					sep = " or ";
				}

				whereClause.Append(separator);
				whereClause.AppendFormat(@"
					exists (from {0} _{0}
							where {1})", className, where);

				separator = " or ";
			}

			var hql = new StringBuilder();

			hql.AppendFormat(@"
				select t.Id, t.{1}, t.{3}
				from {0} t
				where t.Id in (:ids)
					and ({2})
				order by t.{1}",
				clazz.Type.Name,
				clazz.EntityNameProperty.DataPath,
				whereClause,
				_configuration.GetClassMapping(clazz.Type).HasSubclasses ? "class" : "Id");

			var result = new List<object>();

			var query = _transManager.Session.CreateQuery(hql.ToString());
			query.SetParameterList("ids", ids);

			query.List(result);

			ConvertRecords(result, clazz, new RecordConfig { IncludeIdentifier = true, IncludeDisplayString = true, IncludeType = true }, null, null);

			return result.ToArray();
		}

		public IList<T> GetObjectList<T>(object[] ids)
			where T : class, IEntity2
		{
			return _transManager.Session.CreateCriteria(typeof(T))
				.Add(Restrictions.In(Projections.Id(), ids))
				.List<T>();
		}

		protected void SetCriteriaProjections(ICriteria criteria, Class clazz, RecordConfig config, string[] visibleProperties, string[] hiddenProperties)
		{
			visibleProperties = visibleProperties ?? _emptyPropertyNameList;
			hiddenProperties = hiddenProperties ?? _emptyPropertyNameList;

			var projections = Projections.ProjectionList();

			if (config.IncludeIdentifier)
				projections.Add(Projections.Id());

			if (config.IncludeDisplayString)
			{
				if (clazz.EntityNameProperty == null)
					throw new ArgumentException($"Reference property must be set in class {clazz}");

				projections.Add(Projections.Property(clazz.EntityNameProperty.DataPath));
			}

			if (config.IncludeType)
			{
				var persistentClass = _configuration.GetClassMapping(clazz.Type);

				if (persistentClass.HasSubclasses)
					projections.Add(Projections.Property("class"));
				else
					projections.Add(Projections.Id());
			}

			if (config.IncludeVersion)
			{
				var classMetadata = GetClassMetadata(clazz.Type);

				projections.Add(Projections.Property(classMetadata.PropertyNames[classMetadata.VersionProperty]));
			}

			foreach (var columnConfig in config.Columns)
			{
				if (visibleProperties.Any(s => columnConfig.Name == s) || hiddenProperties.Any(value => columnConfig.Name == value))
				{
					var property = clazz.GetProperty(columnConfig.Name);

					if (!property.IsPersistent)
					{
						_arrayMode = false;

						return;
					}

					SetPropertyProjection(clazz, columnConfig, criteria, projections);
				}
			}

			criteria.SetProjection(projections);

			_arrayMode = true;
		}


		public virtual void SetCriteriaRestrictions(Class clazz, ICriteria criteria, RangeRequest request)
		{
			if (!string.IsNullOrEmpty(request.Query))
			{
				if (clazz.EntityNameProperty == null)
					throw new ArgumentException($"Reference property must be set in class {clazz}");

				if (clazz.EntityNameProperty.IsString)
					criteria.Add(Restrictions.Like(clazz.EntityNameProperty.DataPath, request.Query, MatchMode.Start));
				else
					criteria.Add(Restrictions.Like(Projections.Cast(NHibernateUtil.Character, Projections.Property(clazz.EntityNameProperty.DataPath)), request.Query, MatchMode.End));
			}

			if (!string.IsNullOrEmpty(request.GeneralFilter))
				SetGeneralFilterRestrictions(criteria, request, clazz);

			if (request.Filters != null)
				SetPropertyFilterRestrictions(criteria, request, clazz);
		}


		public virtual void SetCriteriaOrder(ICriteria criteria, Class clazz, string sortProperty, string dir, RangeResponse response)
		{
			var asceding = true;

			if (!string.IsNullOrEmpty(sortProperty))
			{
				var property = clazz.GetProperty(sortProperty);

				var propertyPath = ResolveProperty(ResolveDataPath(property), clazz, criteria);

				asceding = string.IsNullOrEmpty(dir) || dir == "ASC";

				criteria.AddOrder(new Order(propertyPath, asceding));

				if (response != null)
				{
					response.Sort = sortProperty;
					response.Dir = asceding ? "ASC" : "DESC";
				}
			}

			if (clazz.EntityNameProperty != null && clazz.EntityNameProperty.Name != sortProperty)
			{
				var propertyPath = ResolveProperty(ResolveDataPath(clazz.EntityNameProperty), clazz, criteria);

				criteria.AddOrder(new Order(propertyPath, asceding));

				if (response != null && string.IsNullOrEmpty(sortProperty))
				{
					response.Sort = clazz.EntityNameProperty.Name;
					response.Dir = asceding ? "ASC" : "DESC";
				}
			}
		}

		protected virtual void SetPropertyProjection(Class clazz, ColumnConfig config, ICriteria criteria, ProjectionList projections)
		{
			var property = clazz.GetProperty(config.Name);

			if (config.RecordConfig != null)
			{
				var propertyClass = property.Type.GetClass();

				var persistentClass = _configuration.GetClassMapping(property.Type);

				if (config.RecordConfig.IncludeIdentifier)
					projections.Add(Projections.Property(ResolveProperty(property.DataPath + "." + persistentClass.IdentifierProperty.Name, clazz, criteria)));

				if (config.RecordConfig.IncludeDisplayString)
					projections.Add(Projections.Property(ResolveProperty(property.DataPath + "." + propertyClass.EntityNameProperty.DataPath, clazz, criteria)));

				if (config.RecordConfig.IncludeType)
				{
					if (persistentClass.HasSubclasses)
						projections.Add(Projections.Property(ResolveProperty(property.DataPath + ".class", clazz, criteria)));
					else
						projections.Add(Projections.Id());
				}

				if (config.RecordConfig.IncludeVersion)
					projections.Add(Projections.Property(ResolveProperty(property.DataPath + "." + persistentClass.Version.Name, clazz, criteria)));
			}
			else
				projections.Add(Projections.Property(ResolveProperty(property.DataPath, clazz, criteria)));
		}

		private void SetGeneralFilterRestrictions(ICriteria criteria, RangeRequest request, Class clazz)
		{
			var parts = request.GeneralFilter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var properties = new List<string>(request.VisibleProperties);

			if (!request.HiddenProperties.IsNullOrEmpty())
				properties.AddRange(request.HiddenProperties);

			foreach (var query in parts)
			{
				ICriterion criterion = null;

				foreach (var propertyName in properties)
				{
					var property = clazz.GetProperty(propertyName);

					if (!property.IsPersistent)
						continue;

					criterion = GetOrRestriction(criterion, SetGeneralFilterRestriction(query, clazz, property, criteria));
				}

				if (criterion != null)
					criteria.Add(criterion);
			}
		}

		protected virtual ICriterion SetGeneralFilterRestriction(string query, Class clazz, Property property, ICriteria criteria)
		{
			if (property.IsString || property.IsTypePersistent)
			{
				var propertyDataPath = ResolveProperty(ResolveDataPath(property), clazz, criteria);

				return Restrictions.Like(propertyDataPath, query, MatchMode.Anywhere);
			}

			if (property.IsNumber)
			{
				if (property.Type == typeof(int) || property.Type == typeof(long))
				{
					long value;

					if (long.TryParse(query, out value))
						return Restrictions.Like(Projections.Cast(NHibernateUtil.StringClob, Projections.Property(property.DataPath)), query, MatchMode.Start);
				}
				else if (property.Type == typeof(double))
				{
					double value;

					if (double.TryParse(query, out value))
						return Restrictions.Eq(property.DataPath, value);
				}
			}
			else if (property.IsDateTime)
			{
				DateTime dateTime;

				if (DateTime.TryParseExact(query, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
					return dateTime.Year < DateTime.MaxValue.Year ? Restrictions.And(Restrictions.Ge(property.DataPath, dateTime), Restrictions.Lt(property.DataPath, dateTime.AddYears(1))) : null;

				if (DateTime.TryParseExact(query, "MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime)
					|| DateTime.TryParseExact(query, "MM.yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) || DateTime.TryParseExact(query, "MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime)
						|| DateTime.TryParseExact(query, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
				{
					return dateTime.Year < DateTime.MaxValue.Year || dateTime.Month < DateTime.MaxValue.Month ? Restrictions.And(Restrictions.Ge(property.DataPath, dateTime), Restrictions.Lt(property.DataPath, dateTime.AddMonths(1))) : null;
				}

				if (DateTime.TryParse(query, out dateTime))
					return dateTime.Year < DateTime.MaxValue.Year || dateTime.Month < DateTime.MaxValue.Month || dateTime.Day < DateTime.MaxValue.Day ? Restrictions.And(Restrictions.Ge(property.DataPath, dateTime), Restrictions.Lt(property.DataPath, dateTime.AddDays(1))) : null;

				if (DateTime.TryParseExact(query, "dd.MM.yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) || DateTime.TryParseExact(query, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) || DateTime.TryParseExact(query, "dd/MM/yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) || DateTime.TryParseExact(query, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
					return Restrictions.Eq(property.DataPath, dateTime);
			}
			else if (property.IsEnum)
			{
				foreach (var value in Enum.GetValues(property.Type))
				{
					var name = Enum.GetName(property.Type, value).ToLower();

					if (name.Contains(query.ToLower()))
						return Restrictions.Eq(property.DataPath, value);
				}
			}

			return null;
		}

		private void SetPropertyFilterRestrictions(ICriteria criteria, RangeRequest request, Class clazz)
		{
			foreach (var filter in request.Filters)
			{
				if (filter.Conditions == null)
					continue;

				foreach (var filterCondition in filter.Conditions)
				{
					if (filterCondition.Operator == FilterOperator.None)
						continue;

					SetPropertyFilterRestriction(clazz, filter, filterCondition, criteria);
				}
			}
		}



		protected virtual void SetPropertyFilterRestriction(
			Class clazz,
			PropertyFilter filter,
			PropertyFilterCondition filterCondition,
			ICriteria criteria
		)
		{

			var property = clazz.GetProperty(filter.Property);

			if (!property.IsPersistent)
				return;


			var propertyName = property.DataPath;


			if (!string.IsNullOrEmpty(filter.InternalPath))
			{
				propertyName = $"{propertyName}.{filter.InternalPath}";
			}

			else if (property.IsTypePersistent)
			{

				if (filterCondition.Operator == FilterOperator.IsIdIn ||
					filterCondition.Operator == FilterOperator.IsIdInOrIsNull ||
					filterCondition.Operator == FilterOperator.IsIdNotInOrIsNull
				)
				{
					propertyName = ResolveProperty(propertyName, clazz, criteria);
					propertyName += "." + property.Class.IdentifierProperty.Name;
				}
				else
				{
					propertyName += "." + property.Class.EntityNameProperty.DataPath;
				}

			}



			propertyName = ResolveProperty(propertyName, clazz, criteria);



			ICriterion criterion;


			if (filterCondition.Operator == FilterOperator.Equals)
			{

				if (filterCondition.Value is DateTime)
				{

					var date = (DateTime)filterCondition.Value;

					if (date.TimeOfDay == TimeSpan.Zero)
						criterion = Restrictions.And(Restrictions.Ge(propertyName, date), Restrictions.Lt(propertyName, date.AddDays(1)));
					else
						criterion = Restrictions.Eq(propertyName, date);

				}

				else if (property.IsNumber)
				{
					criterion = Restrictions.Eq(propertyName, Convert.ChangeType(filterCondition.Value, property.Type));
				}

				else if (property.IsEnum)
				{
					criterion = Restrictions.Eq(propertyName, Enum.Parse(property.Type, filterCondition.Value.ToString()));
				}

				else if (property.IsBool && (bool)filterCondition.Value == false)
				{
					criterion = Restrictions.Not(Restrictions.And(
						Restrictions.IsNotNull(propertyName),
						Restrictions.Eq(propertyName, true)
					));
				}

				else if (property.IsString && filterCondition.Value != null)
				{

					var values = filterCondition.Value.ToString().Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

					if (values.No() || values.Length == 1)
					{
						criterion = Restrictions.Eq(propertyName, filterCondition.Value);
					}
					else
					{
						criterion = Restrictions.In(propertyName, values.Select(a => a.Trim()).ToList());
					}

				}

				else
				{
					criterion = Restrictions.Eq(propertyName, filterCondition.Value);
				}

			}

			else if (filterCondition.Operator == FilterOperator.IsNull)
			{

				criterion = property.IsString
					? Restrictions.Or(Restrictions.IsNull(propertyName), Restrictions.Eq(propertyName, ""))
					: Restrictions.IsNull(propertyName)
				;

			}

			else if (filterCondition.Operator == FilterOperator.StartsWith)
			{
				criterion = Restrictions.Like(propertyName, (string)filterCondition.Value, MatchMode.Start);
			}

			else if (filterCondition.Operator == FilterOperator.Contains)
			{
				criterion = Restrictions.Like(propertyName, (string)filterCondition.Value, MatchMode.Anywhere);
			}

			else if (filterCondition.Operator == FilterOperator.EndsWith)
			{
				criterion = Restrictions.Like(propertyName, (string)filterCondition.Value, MatchMode.End);
			}

			else if (filterCondition.Operator == FilterOperator.Less)
			{
				if (property.IsNumber)
					criterion = Restrictions.Lt(propertyName, Convert.ChangeType(filterCondition.Value, property.Type));
				else
					criterion = Restrictions.Lt(propertyName, filterCondition.Value);
			}

			else if (filterCondition.Operator == FilterOperator.LessOrEquals)
			{
				if (filterCondition.Value is DateTime)
				{
					var date = (DateTime)filterCondition.Value;

					if (date.TimeOfDay == TimeSpan.Zero)
						criterion = Restrictions.Lt(propertyName, date.AddDays(1));
					else
						criterion = Restrictions.Le(propertyName, date);
				}
				else if (property.IsNumber)
					criterion = Restrictions.Le(propertyName, Convert.ChangeType(filterCondition.Value, property.Type));
				else
					criterion = Restrictions.Le(propertyName, filterCondition.Value);
			}

			else if (filterCondition.Operator == FilterOperator.GreaterOrEquals)
			{
				if (property.IsNumber)
					criterion = Restrictions.Ge(propertyName, Convert.ChangeType(filterCondition.Value, property.Type));
				else
					criterion = Restrictions.Ge(propertyName, filterCondition.Value);
			}

			else if (filterCondition.Operator == FilterOperator.Greater)
			{
				if (property.IsNumber)
					criterion = Restrictions.Gt(propertyName, Convert.ChangeType(filterCondition.Value, property.Type));
				else
					criterion = Restrictions.Gt(propertyName, filterCondition.Value);
			}

			else if (filterCondition.Operator == FilterOperator.IsIn || filterCondition.Operator == FilterOperator.IsIdIn)
			{
				criterion = Restrictions.In(propertyName, (ICollection)filterCondition.Value);
			}

			else if (filterCondition.Operator == FilterOperator.IsIdInOrIsNull)
			{
				criterion = Restrictions.Or(
					Restrictions.IsNull(propertyName),
					Restrictions.In(propertyName, (ICollection)filterCondition.Value)
				);
			}

			else if (filterCondition.Operator == FilterOperator.IsIdNotInOrIsNull)
			{
				criterion = Restrictions.Or(
					Restrictions.IsNull(propertyName),
					Restrictions.Not(Restrictions.In(propertyName, (ICollection)filterCondition.Value))
				);
			}

			else
			{
				throw new ArgumentException($@"Invalid property '{propertyName}' filter operator", "request");
			}


			if (filterCondition.Not)
				criterion = Restrictions.Not(criterion);


			criteria.Add(criterion);

		}



		private void SetPositionRestrictions(Class clazz, object positionTo, ICriteria criteria, string sortPropertyName, bool ascending)
		{
			//if (clazz.EntityNameProperty == null) return;

			var obj = _transManager.Get(clazz.Type, positionTo);

			var nameProp = clazz.EntityNameProperty;

			var referencePath = ResolveProperty(ResolveDataPath(nameProp), clazz, criteria);
			var referenceValue = nameProp.GetValue(obj);

			if (nameProp.IsTypePersistent)
				referenceValue = nameProp.Class.EntityNameProperty.GetValue(referenceValue);

			if (sortPropertyName == nameProp.Name)
			{
				if (ascending)
					criteria.Add(Restrictions.Le(referencePath, referenceValue));
				else
					criteria.Add(Restrictions.Ge(referencePath, referenceValue));
			}
			else
			{
				var property = clazz.GetProperty(sortPropertyName);

				var path = ResolveProperty(ResolveDataPath(property), clazz, criteria);

				var value = property.GetValue(obj);

				if (value != null && property.IsTypePersistent)
					value = property.Class.EntityNameProperty.GetValue(value);

				// Depends on NULLs sort rules. Current theme assumes that NULLs are bigger then any non NULL value (PostgreSQL)

				if (value == null)
				{
					if (ascending)
						criteria.Add(Restrictions.Or(Restrictions.IsNotNull(path), Restrictions.Le(referencePath, referenceValue)));
					else
						criteria.Add(Restrictions.And(Restrictions.IsNull(path), Restrictions.Ge(referencePath, referenceValue)));
				}
				else
				{
					if (ascending)
					{
						criteria.Add(Restrictions.Or(
							Restrictions.Lt(path, value),
							Restrictions.And(Restrictions.Eq(path, value), Restrictions.Le(referencePath, referenceValue))
						));
					}
					else
					{
						criteria.Add(Restrictions.Or(
							Restrictions.Or(Restrictions.IsNull(path), Restrictions.Gt(path, value)),
							Restrictions.And(Restrictions.Eq(path, value), Restrictions.Ge(referencePath, referenceValue))
						));
					}
				}
			}
		}

		private bool IsObjectUnderRestrictions(Class clazz, object objId, RangeRequest request)
		{
			var criteria = _transManager.Session.CreateCriteria(clazz.Type);

			criteria.SetProjection(Projections.RowCount());

			SetCriteriaRestrictions(clazz, criteria, request);

			criteria.Add(Restrictions.Eq(Projections.Id(), objId));

			return criteria.UniqueResult<int>() == 0;
		}

		private void ConvertRecords(IList<object> list, Class clazz, RecordConfig config, string[] visibleProperties, string[] hiddenProperties)
		{
			visibleProperties = visibleProperties ?? _emptyPropertyNameList;
			hiddenProperties = hiddenProperties ?? _emptyPropertyNameList;

			for (var i = 0; i < list.Count; i++)
				list[i] = MakeRecord(list[i], clazz, config, visibleProperties, hiddenProperties);
		}

		private object[] MakeRecord(object obj, Class clazz, RecordConfig config, string[] visibleProperties, string[] hiddenProperties)
		{
			if (_arrayMode)
				return MakeRecord((object[])obj, clazz, config, visibleProperties, hiddenProperties);

			var result = new List<object>();

			var objClass = obj.GetClass();

			if (config.IncludeIdentifier)
				result.Add(_transManager.Session.GetIdentifier(obj));

			if (config.IncludeDisplayString)
				result.Add(objClass.EntityNameProperty.GetValue(obj));

			if (config.IncludeType)
				result.Add(objClass.Id);

			if (config.IncludeVersion)
				result.Add(GetClassMetadata(objClass.Type).GetVersion(obj, EntityMode.Poco));

			foreach (var columnConfig in config.Columns)
			{
				if (visibleProperties.Any(s => s == columnConfig.Name) || hiddenProperties.Any(s => s == columnConfig.Name))
				{
					result.Add(GetPropertyValue(clazz.GetProperty(columnConfig.Name).GetValue(obj), columnConfig));
				}
				else
				{
					result.Add(null);
				}
			}

			return result.ToArray();
		}

		private object[] MakeRecord(object[] values, Class clazz, RecordConfig config, string[] visibleProperties, string[] hiddenProperties)
		{
			var items = values.GetEnumerator();

			items.MoveNext();

			var result = new ArrayList();

			if (config.IncludeIdentifier)
			{
				result.Add(items.Current);
				items.MoveNext();
			}

			if (config.IncludeDisplayString)
			{
				result.Add(items.Current);
				items.MoveNext();
			}

			if (config.IncludeType)
			{
				result.Add(GetTypeName(items.Current, clazz.Type));
				items.MoveNext();
			}

			if (config.IncludeVersion)
			{
				result.Add(items.Current);
				items.MoveNext();
			}

			foreach (var columnConfig in config.Columns)
			{
				if (visibleProperties.Any(s => s == columnConfig.Name) || hiddenProperties.Any(s => s == columnConfig.Name))
				{
					var value = GetPropertyValue(items, clazz.GetProperty(columnConfig.Name), columnConfig);
					result.Add(value);
				}
				else
				{
					result.Add(null);
				}
			}

			return result.ToArray();
		}

		protected virtual object GetPropertyValue(object value, ColumnConfig config)
		{
			if (value == null || config.RecordConfig == null) return value;

			var recordConfig = config.RecordConfig;

			var data = new ArrayList();

			if (recordConfig.IncludeIdentifier)
				data.Add(_transManager.Session.GetIdentifier(value));

			var clazz = value.GetClass();

			if (recordConfig.IncludeDisplayString)
				data.Add(clazz.EntityNameProperty.GetValue(value));

			if (recordConfig.IncludeType)
				data.Add(clazz.Type.Name);

			return data.ToArray();
		}

		protected virtual object GetPropertyValue(IEnumerator items, Property property, ColumnConfig config)
		{
			if (config.RecordConfig != null)
			{
				var recordConfig = config.RecordConfig;

				if (items.Current == null)
				{
					if (recordConfig.IncludeIdentifier)
						items.MoveNext();

					if (recordConfig.IncludeDisplayString)
						items.MoveNext();

					if (recordConfig.IncludeType)
						items.MoveNext();

					if (recordConfig.IncludeVersion)
						items.MoveNext();

					return null;
				}

				var data = new List<object>();

				if (recordConfig.IncludeIdentifier)
				{
					data.Add(items.Current);
					items.MoveNext();
				}

				if (recordConfig.IncludeDisplayString)
				{
					data.Add(items.Current);
					items.MoveNext();
				}

				if (recordConfig.IncludeType)
				{
					data.Add(GetTypeName(items.Current, property.Type));
					items.MoveNext();
				}

				if (recordConfig.IncludeVersion)
				{
					data.Add(items.Current);
					items.MoveNext();
				}

				return data.ToArray();
			}

			var result = items.Current;

			items.MoveNext();

			return result;
		}

		protected IClassMetadata GetClassMetadata(System.Type type)
		{
			return _transManager.Session.SessionFactory.GetClassMetadata(type);
		}

		private object GetTypeName(object val, System.Type type)
		{
			var mapping = _configuration.GetClassMapping(type);

			if (!mapping.HasSubclasses)
				return type.Name;

			if (mapping.Discriminator == null)
				return GetTypeName(mapping, (int)val);

			return GetTypeName(mapping, (string)val);
		}

		private static string GetTypeName(PersistentClass persistentClass, int typeId)
		{
			if (persistentClass.SubclassId == typeId)
				return System.Type.GetType(persistentClass.ClassName).Name;

			foreach (Subclass subclass in persistentClass.SubclassIterator)
			{
				var typeName = GetTypeName(subclass, typeId);

				if (typeName != null)
					return typeName;
			}

			return null;
		}

		private static string GetTypeName(PersistentClass persistentClass, string discriminator)
		{
			if (persistentClass.DiscriminatorValue == discriminator)
				return System.Type.GetType(persistentClass.ClassName)?.Name;

			foreach (Subclass subclass in persistentClass.SubclassIterator)
			{
				var typeName = GetTypeName(subclass, discriminator);

				if (typeName != null)
					return typeName;
			}

			return null;
		}

		private static string ResolveDataPath(Property property)
		{
			if (string.IsNullOrEmpty(property.DataPath))
				return null;

			var dataPath = property.DataPath;

			if (property.IsTypePersistent)
				dataPath += "." + property.Class.EntityNameProperty.DataPath;

			return dataPath;
		}

		protected static string ResolveProperty(string dataPath, Class clazz, ICriteria criteria)
		{
			if (string.IsNullOrEmpty(dataPath)) return null;

			var parts = dataPath.Split('.');

			var currentClass = clazz;

			string alias = null;

			for (var i = 0; i < parts.Length - 1; ++i)
			{
				var part = parts[i].TrimStart("_");

				var property = currentClass.GetProperty(part);

				var path = alias.As(a => a + ".") + property.DataPath;

				alias += "_" + property.Name;

				if (criteria.GetCriteriaByAlias(alias) == null)
					criteria.CreateAlias(path, alias, JoinType.LeftOuterJoin);

				currentClass = property.Class;
			}

			return alias.As(a => a + ".") + parts[parts.Length - 1];
		}

		private static ICriterion GetOrRestriction(ICriterion left, ICriterion right)
		{
			if (left == null)
				return right;

			if (right == null)
				return left;

			return Restrictions.Or(left, right);
		}

		private Dictionary<PersistentClass, List<NHibernateProperty>> GetDependentClasses(Class clazz)
		{
			var clazzMapping = _configuration.GetClassMapping(clazz.Type);

			var exceptedColumns = new Dictionary<System.Type, List<ISelectable>>();

			foreach (var prop in clazz.Properties)
			{
				if (!prop.IsPersistent) continue;

				try
				{
					var property = clazzMapping.GetProperty(prop.Name);

					if (!prop.IsCollection || !property.CascadeStyle.DoCascade(CascadingAction.Delete))
						continue;

					var value = (Collection)property.Value;

					var key = value.Key;
					var element = value.Element;

					if (!exceptedColumns.ContainsKey(element.Type.ReturnedClass))
						exceptedColumns.Add(element.Type.ReturnedClass, new List<ISelectable>());

					exceptedColumns[element.Type.ReturnedClass].AddRange(key.ColumnIterator);
				}
				catch
				{
				}
			}

			var dependencies = new Dictionary<PersistentClass, List<NHibernateProperty>>();

			foreach (var cls in _configuration.ClassMappings)
			{
				if (cls.IsAbstract.HasValue && cls.IsAbstract.Value)
					continue;

				foreach (var property in cls.PropertyClosureIterator)
				{
					//if (property.PersistentClass != cls)
					//	continue;

					var findReference = false;

					if (clazz.Type.Is(property.Type.ReturnedClass))
					{
						findReference = true;

						foreach (var column in property.ColumnIterator)
							if (exceptedColumns.ContainsKey(cls.MappedClass) && exceptedColumns[cls.MappedClass].Contains(column))
							{
								findReference = false;
								break;
							}
					}
					else if (property.IsComposite && property.Type.IsComponentType)
					{
						var componentType = (ComponentType)property.Type;

						findReference = componentType.Subtypes.Any(subtype => clazz.Type.Is(subtype.ReturnedClass));
					}

					if (findReference)
					{
						if (!dependencies.ContainsKey(cls))
							dependencies.Add(cls, new List<NHibernateProperty>());

						dependencies[cls].Add(property);
					}
				}
			}

			return dependencies;
		}

		public bool IsValueInUse<T>(string value, string propertyName)
		{
			var result = _transManager.Session.CreateCriteria(typeof(T))
				.Add(Restrictions.Eq(propertyName, value))
				.SetProjection(Projections.RowCount())
				.UniqueResult<int>();

			return result > 0;
		}

		private static readonly object[] _emptyList = new object[0];
		private static readonly string[] _emptyPropertyNameList = new string[] { };

		private readonly TransactionManager _transManager;
		private readonly Configuration _configuration;

		private bool _arrayMode;
	}

}