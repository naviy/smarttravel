namespace Luxena.Domain.Web
{

	public static class EntityMvcHelper
	{

		public static string GetListUrl(string typeExpression, string idExpression)
		{
			return "#/" + typeExpression + "List/" + idExpression;
		}

		public static string GetViewUrl(string typeExpression, string idExpression)
		{
			return "#/" + typeExpression + "/" + idExpression;
		}

		public static string GetEditUrl(string typeExpression, string idExpression)
		{
			return "#/" + typeExpression + "Edit/" + idExpression;
		}

		//		public static string GetApiViewUrl(Type entityType, object id)
//		{
//			var info = _infos.FirstOrDefault(a => a.EntityType == entityType);
//			if (info == null)
//				throw new ArgumentException("EntityApiControllerExtentions.GetViewUrl: can't find EntityApiController for entityType " + entityType.Name);
//
//			var name = info.EntityApiControllerType.Name.TrimEnd("Controller");
//			return "/api/" + name + "/view#" + id;
//		}
//
//
//		#region Infos
//
//		public static void Register<TDomain, TEntity, TEntityService>(Type entityApiControllerType)
//			where TDomain : Domain<TDomain>
//			where TEntity : class
//			where TEntityService : EntityService<TDomain, TEntity>, new()
//		{
//			Register(typeof(TDomain), typeof(TEntity), typeof(TEntityService), entityApiControllerType);
//		}
//
//		public static void Register(Type domainType, Type entityType, Type entityServiceType, Type entityApiControllerType)
//		{
//			_infos.Add(new ControllerInfo(domainType, entityType, entityServiceType, entityApiControllerType));
//		}
//
//		private readonly static List<ControllerInfo> _infos = new List<ControllerInfo>();
//
//
//		public class ControllerInfo
//		{
//			public ControllerInfo(Type domainType, Type entityType, Type entityServiceType, Type entityApiControllerType)
//			{
//				DomainType = domainType;
//				EntityType = entityType;
//				EntityServiceType = entityServiceType;
//				EntityApiControllerType = entityApiControllerType;
//			}
//
//			public readonly Type DomainType;
//			public readonly Type EntityType;
//			public readonly Type EntityServiceType;
//			public readonly Type EntityApiControllerType;
//		}
//
//		#endregion

	}

}