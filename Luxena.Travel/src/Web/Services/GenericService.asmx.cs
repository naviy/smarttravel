using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Base.Services;


namespace Luxena.Travel.Web.Services
{
	public class GenericService : BaseWebService<Base.Services.GenericService>
	{

		[WebMethod]
		public RangeResponse GetRange(string className, RangeRequest @params)
		{
			return Service.GetRange(className, @params);
		}

		[WebMethod]
		public object[] Refresh(string className, object[] ids, string[] visibleProperties, string[] hiddenProperties)
		{
			return Service.Refresh(className, ids, visibleProperties, hiddenProperties);
		}

		[WebMethod]
		public ListConfig GetRangeConfig(string className)
		{
			return Service.GetRangeConfig(className);
		}

		[WebMethod]
		public object Get(string className, object id, bool viewMode)
		{
			var result = Service.Get(className, id, viewMode);
			return result;
		}

		[WebMethod]
		public ItemConfig GetItemConfig(string className, bool viewMode)
		{
			return Service.GetItemConfig(className, viewMode);
		}

		[WebMethod]
		public RangeResponse Suggest(string className, RangeRequest @params)
		{
			return Service.Suggest(className, @params);
		}

		[WebMethod]
		public OperationStatus CanUpdate(string className, object id)
		{
			return Service.CanUpdate(className, id);
		}

		[WebMethod]
		public ItemResponse Update(string className, object id, object version, object data, RangeRequest @params)
		{
			return Service.Update(className, id, version, data, @params);
		}

		[WebMethod]
		public OperationStatus CanDelete(string className, object[] ids)
		{
			return Service.CanDelete(className, ids);
		}

		[WebMethod]
		public DeleteOperationResponse Delete(string className, object[] ids, RangeRequest @params)
		{
			return Service.Delete(className, ids, @params);
		}

		[WebMethod]
		public OperationStatus CanList(string className)
		{
			return Service.CanList(className);
		}

		[WebMethod]
		public object GetDependencies(string className, object id)
		{
			return Service.GetDependencies(className, id);
		}

		[WebMethod]
		public bool CanReplace(string className, object id)
		{
			return Service.CanReplace(className, id);
		}

		[WebMethod]
		public void Replace(string className, object oldId, object newId, bool deleteOld)
		{
			Service.Replace(className, oldId, newId, deleteOld);
		}

		public RangeResponse GetRange<T>(RangeRequest @params)
		{
			return Service.GetRange<T>(@params);
		}

		public bool CanUpdate<T>()
		{
			return Service.CanUpdate<T>();
		}

		[WebMethod]
		public byte[] Export(string className, DocumentExportArgs args)
		{
			return Service.Export(className, args);
		}
	}
}