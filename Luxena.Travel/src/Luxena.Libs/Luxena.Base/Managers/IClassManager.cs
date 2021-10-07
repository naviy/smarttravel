//using System.Collections.Generic;

//using Luxena.Base.Data;
//using Luxena.Base.Domain;
//using Luxena.Base.Metamodel;


//namespace Luxena.Base.Managers
//{
//	public interface IClassManager
//	{
//		OperationStatus CanCreate();

//		OperationStatus CanCreate(object obj);

//		object Create();

//		object Create(object data);

//		OperationStatus CanCopy();

//		void Save(object obj);

//		OperationStatus CanList();

//		object[] List(RecordConfig config);

//		RangeResponse List(RangeRequest request, RecordConfig config);

//		object[] Refresh(Class clazz, object[] ids, string[] visibleProperties, string[] hiddenProperties, RecordConfig config);

//		RangeResponse Suggest(RangeRequest request, RecordConfig config);

//		OperationStatus CanView(object obj);

//		void View(object obj);

//		OperationStatus CanUpdate();

//		OperationStatus CanUpdate(object obj);

//		void Update(object obj, object newValue);

//		OperationStatus CanDelete();

//		OperationStatus CanDelete(object obj);

//		void Delete(object obj);

//		byte[] Export(string className, DocumentExportArgs args, RecordConfig config);

//		GenericManager.Permissions GetCustomPermissions();

//		GenericManager.Permissions GetCustomPermissions(object obj);

//		object[] GetUndeletableObjects(object[] ids);

//		OperationStatus CanReplace(object obj);

//		void Replace(object oldObject, object newObject);

//		IList<T> GetObjectList<T>(object[] ids)
//			where T : Entity2;
//	}
//}