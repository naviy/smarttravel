
using System;
using System.Collections.Generic;
using System.Security;

using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Services;
using Luxena.Base.Text;
using Luxena.Castle;
using Luxena.Travel.Domain;
using Luxena.Travel.Reports;
using Luxena.Travel.Services;

using NHibernateConfiguration = NHibernate.Cfg.Configuration;
using CastleConfiguration = Castle.Windsor.Installer.Configuration;
using LuxenaBaseService = Luxena.Base.Services.BaseService;


namespace Luxena.Travel.Config
{
	public class ServicesInstaller : IWindsorInstaller
	{
		static ServicesInstaller()
		{
			_mainTranslator = new CompositeErrorTranslator(
				new List<IErrorTranslator>
				{
					new DirectErrorTranslator(new List<Type> {
						typeof (SecurityException),
						typeof (DomainException),
						typeof (OperationDeniedException),
						typeof (ObjectsNotFoundException),
						typeof (DocumentClosedException)
					}),
					new PostgreSqlErrorTranlator(),
					new SubstringErrorTranslator(
						new Dictionary<string, string>
						{
							{ Exceptions.RequestParameterMissing_Error, Exceptions.RequestParameterMissing_Translation },
							{ Exceptions.NoRowById_Error, Exceptions.NoRowById_Translation },
							{ Exceptions.RowAlreadyModified_Error, Exceptions.RowAlreadyModified_Translation },
							{ Exceptions.RowAlreadyModifiedByUser_Error, Exceptions.RowAlreadyModified_Translation }
						},
						new SimpleInterpretationStrategy()), // !!! resources aren't loaded acording to current thread culture
				});

			_defaultTranslator = new DefaultErrorTranslator(Exceptions.UnexpectedErrorDefaultTranslation, new SimpleInterpretationStrategy());
		}

		public ServicesInstaller(bool showUnknownExceptions)
		{
			_showUnknownExceptions = showUnknownExceptions;
		}

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<IErrorTranslator>().Instance(_mainTranslator),
				Component.For<ServiceMethodInterceptor>()
					.DependsOn(
						Property.ForKey("MainTranslator").Eq(_mainTranslator),
						Property.ForKey("DefaultTranslator").Eq(_defaultTranslator),
						Property.ForKey("AllowUntranslated").Eq(_showUnknownExceptions)
					),
				Component.For<GenericService>()
					.LifeStyle.Scoped()
					.Interceptors(InterceptorReference.ForType<ServiceMethodInterceptor>()).Anywhere,
				Component.For<AppStateChangesHolding>().LifeStyle.Singleton,
				Component.For<IGenericExporter>().ImplementedBy<GenericExporter>().LifeStyle.Scoped(),
				AllTypes.FromAssemblyNamed("Luxena.Travel")
					.BasedOn<LuxenaBaseService>()
					.WithService.Self()
					.WithService.DefaultInterface()
					.Configure(conf =>
						conf.LifeStyle.Scoped()
						.Interceptors(InterceptorReference.ForType<ServiceMethodInterceptor>()).Anywhere
					)
				);
		}

		private static readonly IErrorTranslator _mainTranslator;
		private static readonly IErrorTranslator _defaultTranslator;
	
		private readonly bool _showUnknownExceptions;
	}
}
