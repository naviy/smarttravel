using System;
using System.Collections.Generic;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Base.Services;


namespace Luxena.Base.Managers
{

	public class GenericManager : BaseManager//, IGenericManager
	{
		public IGenericDao GenericDao { get; set; }

		public ISecurityContext SecurityContext { get; set; }

		public IGenericExporter GenericExporter { get; set; }

		public Class Class { get; set; }

		public virtual OperationStatus CanCreate()
		{
			return CanDoOperation(attribute => attribute.Create);
		}

		public virtual OperationStatus CanCreate(object obj)
		{
			return CanCreate();
		}

		public virtual object Create()
		{
			return Class.CreateInstance<object>();
		}

		public virtual object Create(object data)
		{
			var obj = Create();

			SetValues(obj, (Dictionary<string, object>) data);

			return obj;
		}

		public virtual OperationStatus CanCopy()
		{
			return CanDoOperation(attribute => attribute.Copy);
		}

		public virtual void Save(object obj)
		{
			TransactionManager.Save(obj);
		}

		public virtual OperationStatus CanList()
		{
			return CanDoOperation(attribute => attribute.List);
		}

		public virtual object[] List(RecordConfig config)
		{
			return GenericDao.List(Class, config);
		}

		public virtual RangeResponse List(RangeRequest request, RecordConfig config)
		{
			if (request == null)
			{
				var response = new RangeResponse
				{
					List = List(config)
				};

				response.TotalCount = response.List.Length;

				return response;
			}

			return GenericDao.List(Class, request, config);
		}

		public virtual object[] Refresh(Class clazz, object[] ids, string[] visibleProperties, string[] hiddenProperties, RecordConfig config)
		{
			return GenericDao.Refresh(clazz, ids, visibleProperties, hiddenProperties, config);
		}

		public IList<T> GetObjectList<T>(object[] ids)
			where T : class, IEntity2
		{
			return GenericDao.GetObjectList<T>(ids);
		}

		public virtual RangeResponse Suggest(RangeRequest request, RecordConfig config)
		{
			return List(request, config);
		}

		public virtual OperationStatus CanView(object obj)
		{
			return CanDoOperation(attribute => attribute.View);
		}

		public virtual void View(object obj)
		{
		}

		public virtual OperationStatus CanUpdate()
		{
			return CanDoOperation(attribute => attribute.Update);
		}

		public virtual OperationStatus CanUpdate(object obj)
		{
			return CanUpdate();
		}

		public virtual void Update(object obj, object newValue)
		{
			SetValues(obj, (Dictionary<string, object>) newValue);
		}

		public virtual OperationStatus CanDelete()
		{
			return CanDoOperation(attribute => attribute.Delete);
		}

		public virtual OperationStatus CanDelete(object obj)
		{
			return CanDelete();
		}

		public virtual void Delete(object obj)
		{
			TransactionManager.Delete(obj);
		}

		public virtual byte[] Export(string className, DocumentExportArgs args, RecordConfig config)
		{
			PrepareExportArgs(args);

			args.Request.Limit = 0;

			var clazz = Class.Of(className);

			var rangeResponse = GenericDao.List(clazz, args.Request, config);

			return GenericExporter.Export(clazz, args.Request.VisibleProperties, config, rangeResponse.List);
		}

		protected static void PrepareExportArgs(DocumentExportArgs args)
		{
			if (args.Mode == DocumentExportMode.Selected || args.Mode == DocumentExportMode.ExceptSelected)
			{
				var filters = args.Mode == DocumentExportMode.ExceptSelected && args.Request.Filters != null
					? new List<PropertyFilter>(args.Request.Filters)
					: new List<PropertyFilter>();

				filters.Add(new PropertyFilter
				{
					Property = "Id",
					Conditions = new[]
					{
						new PropertyFilterCondition
						{
							Not = args.Mode == DocumentExportMode.ExceptSelected,
							Operator = FilterOperator.IsIn,
							Value = args.SelectedDocuments
						}
					}
				});

				args.Request.Filters = filters.ToArray();
			}
		}

		public class Permissions : Dictionary<string, OperationStatus> { }


		public virtual Permissions GetCustomPermissions()
		{
			return null;
		}

		public virtual Permissions GetCustomPermissions(object obj)
		{
			return GetCustomPermissions();
		}

		public virtual OperationStatus CanReplace(object obj)
		{
			return CanDoOperation(attribute => attribute.Replace);
		}

		public virtual void Replace(object oldObject, object newObject)
		{
			GenericDao.Replace(oldObject, newObject);
		}

		public object[] GetUndeletableObjects(object[] ids)
		{
			return GenericDao.GetUndeletableObjects(Class, ids);
		}

		protected void SetValues(object obj, Dictionary<string, object> data)
		{
			var propertyNames = new List<string>(data.Keys);

			foreach (var propertyName in propertyNames)
			{
				var property = Class.GetProperty(propertyName);

				var value = data[propertyName];

				if (property.IsTypePersistent)
				{
					object val;

					if (value == null)
						val = null;
					else if (value is object[])
						val = TransactionManager.Refer(property.Type, ((object[]) value)[EntityReference.IdPos]);
					else
						val = TransactionManager.Refer(property.Type, ((Dictionary<string, object>) value)["Id"]);

					property.SetValue(obj, val);
				}
				else
				{
					property.SetValue(obj, value);
				}
			}
		}

		protected OperationStatus CanDoOperation(Func<GenericPrivilegesAttribute, object[]> priveleges)
		{
			var attribute = Class.Type.GetAttribute<GenericPrivilegesAttribute>();

			if (attribute == null)
				throw new Exception(string.Format("GenericPrivilegesAttribute is required for class {0}", Class.Type.Name));

			if (SecurityContext.IsGranted(priveleges(attribute)))
				return OperationStatus.Enabled();

			return OperationStatus.Hidden();
		}
	}
}