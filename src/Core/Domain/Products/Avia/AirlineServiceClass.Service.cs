using System;
using System.Collections.Generic;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class AirlineServiceClass
	{

		public class Service : Entity2Service<AirlineServiceClass>
		{

			public ServiceClass? GetServiceClass(string code, Organization airline)
			{
				var r = 
					By(a => a.Airline == airline && a.Code == code) ?? 
					Save(new AirlineServiceClass
					{
						Airline = airline, 
						Code = code,
						ServiceClass = DefaultServiceClassByCode(code),
					});

				return r.ServiceClass;
			}

			public ServiceClass DefaultServiceClassByCode(string code)
			{
				var serviceClass = _defaultSerbiceClassByCode.By(code);
				return serviceClass != ServiceClass.Unknown ? serviceClass : ServiceClass.Economy;
			}

			private static readonly Dictionary<string, ServiceClass> _defaultSerbiceClassByCode = new Dictionary<string, ServiceClass>
			{
				{ "Z", ServiceClass.Business },
				{ "D", ServiceClass.Business },
				{ "C", ServiceClass.Business },
				{ "J", ServiceClass.Business },
				{ "F", ServiceClass.First },
			};

			public OperationStatus CanApply()
			{
				return db.Granted(UserRole.Administrator, UserRole.Supervisor);
			}

			public int Apply(AirlineServiceClass serviceClass, DateTime? dateFrom, DateTime? dateTo)
			{
				var tickets = db.Airline.GetNoServiceClassTickets(serviceClass, dateFrom, dateTo);

				foreach (var ticket in tickets)
				{
					foreach (var segment in ticket.Segments)
						segment.ServiceClass = serviceClass.ServiceClass;
				}

				return tickets.Count;
			}

		}

	}


	partial class AirlineServiceClassManager
	{

		public override Permissions GetCustomPermissions()
		{
			return new Permissions
			{
				{ "ApplyServiceClassToDocuments", db.AirlineServiceClass.CanApply() }
			};
		}

	}

}