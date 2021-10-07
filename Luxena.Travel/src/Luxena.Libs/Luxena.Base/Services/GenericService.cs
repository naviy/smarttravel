using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Managers;
using Luxena.Base.Metamodel;
using Luxena.Base.Serialization;
using Luxena.Base.Text;


namespace Luxena.Base.Services
{
	public class GenericService : BaseService
	{
		public GenericManager GenericManager { get; set; }

		public IClassManagerProvider ClassManagerProvider { get; set; }

		public IErrorTranslator ErrorTranslator { get; set; }

		public virtual RangeResponse GetRange<T>(RangeRequest prms, bool useCommit)
		{
			var cls = prms.ClassName != null ? Class.Of(prms.ClassName) : Class.Of<T>();
			SetClass(cls);

			var result = GetRange(prms);

			if (useCommit)
				TransactionManager.Commit();

			return result;
		}

		public virtual RangeResponse GetRange<T>(RangeRequest prms)
		{
			return GetRange<T>(prms, true);
		}

		public virtual RangeResponse GetRange(string className, RangeRequest @params, bool useCommit)
		{
			SetClass(className);

			var result = GetRange(@params);

			if (useCommit)
				TransactionManager.Commit();

			return result;
		}

		public virtual RangeResponse GetRange(string className, RangeRequest @params)
		{
			return GetRange(className, @params, true);
		}

		public virtual object[] Refresh(string className, object[] ids, string[] visibleProperties, string[] hiddenProperties)
		{
			SetClass(className);

			CanDoAction(() => _classManager.CanList(), Resources.ListOperationDenied_Msg);

			var result = _classManager.Refresh(_class, ids, visibleProperties, hiddenProperties, GetRecordConfig());

			TransactionManager.Commit();

			return result;
		}

		public virtual ListConfig GetRangeConfig(string className)
		{
			SetClass(className);

			return new ListConfig
			{
				Caption = _class.ListCaption,
				IsCreationAllowed = _classManager.CanCreate(),
				IsCopyingAllowed = _classManager.CanCopy(),
				IsEditAllowed = _classManager.CanUpdate(),
				IsQuickEditAllowed = false,
				IsRemovingAllowed = _classManager.CanDelete(),
				CustomActionPermissions = _classManager.GetCustomPermissions(),
				Filterable = true,
				Columns = GetListableProperties()
					.Select(property => GetColumnConfig(property, true))
					.Where(cfg => cfg != null)
					.ToArray()
			};
		}

		public virtual object Get(string className, object id, bool viewMode)
		{
			SetClass(className);

			return MakeGetResult(TransactionManager.Get(_class.Type, id), viewMode);
		}

		public virtual ItemConfig GetItemConfig(string className, bool viewMode)
		{
			SetClass(className);

			return new ItemConfig
			{
				Caption = _class.Caption,
				ListCaption = _class.ListCaption,
				IsListAllowed = _classManager.CanList(),
				IsCreationAllowed = _classManager.CanCreate(),
				IsCopyingAllowed = _classManager.CanCopy(),
				IsEditAllowed = _classManager.CanUpdate(),
				IsRemovingAllowed = _classManager.CanDelete(),
				CustomActionPermissions = _classManager.GetCustomPermissions(),
				Columns = (!viewMode ? GetEditableProperties() : GetListableProperties())
					.Select(property => GetColumnConfig(property, false))
					.Where(cfg => cfg != null)
					.ToArray()
			};
		}

		[ContractMember(Ignore = true)]
		public virtual bool CanUpdate<T>()
		{
			SetClass(Class.Of<T>());

			OperationStatus status = _classManager.CanUpdate();

			TransactionManager.Commit();

			return status.IsEnabled;
		}

		public virtual OperationStatus CanUpdate(string className, object id)
		{
			SetClass(className);

			OperationStatus status = _classManager.CanUpdate(TransactionManager.Refer(_class.Type, id));

			TransactionManager.Commit();

			return status;
		}

		public virtual ItemResponse Update(string className, object id, object version, object data, RangeRequest @params)
		{
			SetClass(className);

			var obj = id == null ? Create(data) : Update(id, version, data);

			TransactionManager.Flush();

			var response = new ItemResponse
			{
				Item = GetInstance(obj, null)
			};

			if (@params != null)
			{
				if (!string.IsNullOrEmpty(@params.ClassName))
					SetClass(@params.ClassName);

				@params.PositionableObjectId = _class.IdentifierProperty.GetValue(obj);

				response.RangeResponse = GetRange(@params);
			}

			TransactionManager.Commit();

			return response;
		}

		public virtual OperationStatus CanDelete(string className, object[] ids)
		{
			SetClass(className);

			OperationStatus result = OperationStatus.Enabled();

			foreach (object id in ids)
			{
				object obj = TransactionManager.Refer(_class.Type, id);

				var status = _classManager.CanDelete(obj);

				if (!status.IsEnabled)
				{
					result = status;

					break;
				}
			}

			return result;
		}

		public virtual DeleteOperationResponse Delete(string className, object[] ids, RangeRequest @params)
		{
			SetClass(className);

			var response = new DeleteOperationResponse();

			try
			{
				foreach (object id in ids)
				{
					object obj = TransactionManager.Get(_class.Type, id);

					CanDoAction(() => _classManager.CanDelete(obj), Resources.DeleteOperationDenied_Msg);

					_classManager.Delete(obj);
				}

				TransactionManager.Commit();

				response.Success = true;
			}
			catch (Exception ex)
			{
				if (ErrorTranslator.Translate(ex) is ForeignKeyViolation)
				{
					TransactionManager.Rollback();

					response.Success = false;

					response.UndeletableObjects = ids.Length == 1
						? EntityReference.ToArray((IEntity)TransactionManager.Get(_class.Type, ids[0]))
						: _classManager.GetUndeletableObjects(ids);

					return response;
				}

				throw;
			}

			if (@params != null)
				response.RangeResponse = GetRange(className, @params);

			return response;
		}

		public virtual OperationStatus CanList(string className)
		{
			SetClass(className);

			return _classManager.CanList();
		}

		public virtual RangeResponse Suggest(string className, RangeRequest @params)
		{
			SetClass(className);

			var config = new RecordConfig
			{
				IncludeIdentifier = true,
				IncludeDisplayString = true,
				IncludeType = true
			};

			if (_class.EntityNameProperty != null)
				@params.Sort = _class.EntityNameProperty.DataPath;

			if (@params.Limit == 0)
				@params.Limit = 100;

			return _classManager.Suggest(@params, config);
		}

		public virtual object GetDependencies(string className, object id)
		{
			SetClass(className);

			var result = new EntityReference((IEntity)TransactionManager.Get(_class.Type, id));

			TransactionManager.Commit();

			return result;
		}

		public virtual bool CanReplace(string className, object id)
		{
			SetClass(className);

			return _classManager.CanReplace(TransactionManager.Refer(_class.Type, id)).IsEnabled;
		}

		public virtual void Replace(string className, object oldId, object newId, bool deleteOld)
		{
			SetClass(className);

			var oldObject = TransactionManager.Refer(_class.Type, oldId);
			var newObject = TransactionManager.Refer(_class.Type, newId);

			CanDoAction(() => _classManager.CanReplace(oldObject), Resources.ReplaceOperationDenied_Msg);

			_classManager.Replace(oldObject, newObject);

			if (deleteOld)
			{
				CanDoAction(() => _classManager.CanDelete(oldObject), Resources.DeleteOperationDenied_Msg);

				_classManager.Delete(oldObject);
			}

			TransactionManager.Commit();
		}

		public virtual byte[] Export(string className, DocumentExportArgs args)
		{
			SetClass(Class.Of(className));

			var config = GetRecordConfig();
			config.IncludeIdentifier = false;
			config.IncludeVersion = false;
			config.IncludeDisplayString = false;
			config.IncludeType = false;

			return _classManager.Export(className, args, config);
		}

		private void SetClass(string className)
		{
			SetClass(Class.Of(className));
		}

		private void SetClass(Class clazz)
		{
			_class = clazz;

			_classManager = ClassManagerProvider.GetClassManager(_class) ?? GenericManager;

			_classManager.Class = _class;
		}

		private object MakeGetResult(object obj, bool viewMode)
		{
			IList<Property> properties;

			if (viewMode)
			{
				CanDoAction(() => _classManager.CanView(obj), Resources.ViewOperationDenied_Msg);

				properties = GetListableProperties();
			}
			else
			{
				CanDoAction(() => _classManager.CanUpdate(obj), Resources.UpdateOperationDenied_Msg);

				properties = GetEditableProperties();
			}

			var result = GetInstance(obj, properties);

			TransactionManager.Commit();

			return result;
		}

		private RangeResponse GetRange(RangeRequest rangeRequest)
		{
			CanDoAction(() => _classManager.CanList(), Resources.ListOperationDenied_Msg);

			return _classManager.List(rangeRequest, GetRecordConfig());
		}

		private RecordConfig GetRecordConfig()
		{
			var listableProperties = GetListableProperties();

			var config = new RecordConfig();

			foreach (var property in listableProperties)
			{
				if (property.IsTypePersistent)
				{
					var recordConfig = new RecordConfig
					{
						IncludeIdentifier = true,
						IncludeDisplayString = true,
						IncludeType = true
					};

					config.Add(property.Name, recordConfig);
				}
				else
					config.Add(property.Name);
			}

			config.IncludeIdentifier = true;
			config.IncludeType = true;
			config.IncludeVersion = true;

			return config;
		}

		private IList<Property> GetListableProperties()
		{
			return _class.Properties
				.Where(a =>
					(!a.IsCollection || a.Type.IsArray) && a.Name != "Id" && a.Name != "Version" &&
					a.Type != typeof(byte[]) && a.HiddenOptions != HiddenOptions.Hidden
				)
				.ToList();
		}

		private IList<Property> GetEditableProperties()
		{
			var properties = new List<Property>();

			var notAvailableProperties = typeof(IEntity).GetProperties();

			foreach (var property in _class.Properties)
			{
				var isAvailable = true;

				foreach (var propertyInfo in notAvailableProperties)
				{
					if (property.Name == propertyInfo.Name)
						isAvailable = false;
				}

				if (isAvailable && !property.IsCollection && !property.IsReadOnly)
					properties.Add(property);
			}

			return properties;
		}

		private Dictionary<string, object> GetInstance(object obj, IList<Property> properties)
		{
			var result = new ObjectSerializer { Properties = properties }.Serialize(obj);

			var permissions = new OperationPermissions
			{
				CanDelete = _classManager.CanDelete(obj),
				CanUpdate = _classManager.CanUpdate(obj),
				CustomActionPermissions = _classManager.GetCustomPermissions(obj)
			};

			result.Add("Permissions", permissions);

			return result;
		}

		private object Create(object data)
		{
			CanDoAction(() => _classManager.CanCreate(data), Resources.UpdateOperationDenied_Msg);

			var obj = _classManager.Create(data);

			_classManager.Save(obj);

			return obj;
		}

		private object Update(object id, object version, object data)
		{
			var obj = TransactionManager.Get(_class.Type, id, version);

			CanDoAction(() => _classManager.CanUpdate(obj), Resources.UpdateOperationDenied_Msg);

			_classManager.Update(obj, data);

			_classManager.Save(obj);

			return obj;
		}

		private static ColumnConfig GetColumnConfig(Property property, bool listMode)
		{
			if (property.HiddenOptions == HiddenOptions.Hidden)
				return null;

			ColumnConfig columnConfig;

			if (property.IsString)
			{
				columnConfig = new TextColumnConfig
				{
					Type = TypeEnum.String
				};

				((TextColumnConfig)columnConfig).Length = property.Length;

				if (property.Length > 255)
					((TextColumnConfig)columnConfig).Lines = 3;
			}
			else if (property.IsTypePersistent)
			{
				columnConfig = new ClassColumnConfig
				{
					Type = TypeEnum.Object
				};

				var config = (ClassColumnConfig)columnConfig;

				config.Clazz = property.Class.Id;
				config.FilterType = GetPropertyType(property.Class.EntityNameProperty);
				config.Length = property.Class.EntityNameProperty.Length;
			}
			else if (property.IsNumber)
			{
				columnConfig = new NumberColumnConfig
				{
					Type = TypeEnum.Number
				};
				((NumberColumnConfig)columnConfig).IsInteger = (property.Type == typeof(int) || property.Type == typeof(long));
			}
			else if (property.IsEnum)
			{
				columnConfig = new ListColumnConfig
				{
					Type = TypeEnum.List
				};

				Array enumItems = Enum.GetValues(property.Type);
				var listColumnConfigItems = new object[enumItems.Length];
				int i = 0;

				foreach (Enum enumItem in enumItems)
					listColumnConfigItems[i++] = new object[]
					{
						enumItem, enumItem.ToDisplayString()
					};

				((ListColumnConfig)columnConfig).Items = listColumnConfigItems;
			}
			else if (property.IsDateTime)
			{
				columnConfig = new DateTimeColumnConfig
				{
					Type = TypeEnum.Date
				};

				((DateTimeColumnConfig)columnConfig).FormatString = property.DisplayFormat;
			}
			else if (property.IsBool)
			{
				columnConfig = new ColumnConfig
				{
					Type = TypeEnum.Bool
				};
			}
			else
			{
				columnConfig = new CustomTypeColumnConfig
				{
					Type = TypeEnum.Custom
				};
				((CustomTypeColumnConfig)columnConfig).TypeName = property.Type.Name;
			}

			columnConfig.Name = property.Name;
			columnConfig.Caption = property.Caption;
			columnConfig.IsRequired = property.IsRequired;
			columnConfig.IsPersistent = property.IsPersistent;
			columnConfig.IsReadOnly = property.IsReadOnly;
			columnConfig.IsReference = property.IsEntityName;
			columnConfig.Hidden = property.HiddenOptions != HiddenOptions.Visible;
			columnConfig.DefaultValue = property.HasDefaultValue ? property.DefaultValue : null;

			if (listMode)
			{
				foreach (PropertyInfo propertyInfo in typeof(ICreateAware).GetProperties())
					if (propertyInfo.Name == property.Name)
					{
						columnConfig.Hidden = true;
						columnConfig.IsReadOnly = true;
					}

				foreach (PropertyInfo propertyInfo in typeof(IModifyAware).GetProperties())
					if (propertyInfo.Name == property.Name)
					{
						columnConfig.Hidden = true;
						columnConfig.IsReadOnly = true;
					}
			}

			return columnConfig;
		}

		private static TypeEnum GetPropertyType(Property property)
		{
			if (property == null || property.IsString)
				return TypeEnum.String;

			if (property.IsTypePersistent)
				return TypeEnum.Object;

			if (property.IsNumber)
				return TypeEnum.Number;

			if (property.IsEnum)
				return TypeEnum.List;

			if (property.IsDateTime)
				return TypeEnum.Date;

			if (property.IsBool)
				return TypeEnum.Bool;

			throw new ArgumentException();
		}

		private static void CanDoAction(Func<OperationStatus> canDo, string exceptionMsg)
		{
			OperationStatus operationStatus = canDo();

			if (!operationStatus.IsEnabled)
				throw new OperationDeniedException(operationStatus.DisableInfo ?? exceptionMsg);
		}

		private Class _class;
		private GenericManager _classManager;
	}
}