using System.Web;


namespace Luxena.Travel.Web.Castle
{
	public class ServiceResolverProvider : IServiceResolverProvider
	{
		public IServiceResolver WebResolver { get; set; }

		public IServiceResolver TaskResolver { get; set; }

		public IServiceResolver Get()
		{
			return HttpContext.Current != null ? WebResolver : TaskResolver;
		}
	}
}