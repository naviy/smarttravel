namespace Luxena
{
	public static class ServiceResolver
	{
		public static IServiceResolverProvider Provider { get; set; }

		public static IServiceResolver Current
		{
			get { return Provider == null ? null : Provider.Get(); }
		}
	}
}