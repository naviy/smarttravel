using System;
using System.Linq;

using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Registration.Lifestyle;
using Castle.Windsor;


namespace Luxena.Castle
{
	public static class WindsorExtensions
	{
		public static ComponentRegistration<T> Scoped<T>(this LifestyleGroup<T> @group) where T : class
		{
			return group.Custom<ScopedLifestyle>();
		}

		public static IDisposable OpenScope(this IWindsorContainer container)
		{
			return GetScopeSubsystem(container.Kernel).OpenScope();
		}

		public static void CloseScope(this IWindsorContainer container)
		{
			GetScopeSubsystem(container.Kernel).CloseScope();
		}

		public static void Resolve(this IWindsorContainer container, object instance)
		{
			instance.GetType().GetProperties()
				.Where(property => property.CanWrite && property.PropertyType.IsPublic)
				.Where(property => container.Kernel.HasComponent(property.PropertyType))
				.ForEach(property => property.SetValue(instance, container.Resolve(property.PropertyType), null));
		}

		public static void AddScopeSubsystem(this IWindsorContainer container, IScopeAccessor accessor)
		{
			container.Kernel.AddSubSystem(_scopeSubsystemKey, new ScopeSubsystem(accessor));
		}

		internal static ScopeSubsystem GetScopeSubsystem(this IKernel kernel)
		{
			var scopeSubsystem = (ScopeSubsystem) kernel.GetSubSystem(_scopeSubsystemKey);

			if (scopeSubsystem == null)
				throw new InvalidOperationException("Scope Subsystem not found.  Did you forget to add it?");

			return scopeSubsystem;
		}

		private static readonly string _scopeSubsystemKey = typeof(ScopeSubsystem).Name;
	}
}