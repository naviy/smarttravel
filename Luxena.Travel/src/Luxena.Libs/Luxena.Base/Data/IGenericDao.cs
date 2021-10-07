using System.Collections.Generic;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;



namespace Luxena.Base.Data
{


	public interface IGenericDao
	{

		object[] List(Class clazz, RecordConfig config);

		RangeResponse List(Class clazz, RangeRequest request, RecordConfig config);

		object[] Refresh(Class clazz, object[] ids, string[] visibleProperties, string[] hiddenProperties, RecordConfig config);

		void Replace(object oldObject, object newObject);

		object[] GetUndeletableObjects(Class clazz, object[] ids);

		IList<T> GetObjectList<T>(object[] ids)
			where T : class, IEntity2;

		bool IsValueInUse<T>(string value, string propertyName);

	}


}