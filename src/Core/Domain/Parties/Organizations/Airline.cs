using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Domain;

using NHibernate;
using NHibernate.Criterion;

using HQueryOver = NHibernate.Criterion.QueryOver;


namespace Luxena.Travel.Domain
{

	[RU("Авиакомпания", "Авиакомпании", ruShort: "авиакомпания")]
	[Extends(typeof(Organization))]
	public class Airline
	{

		public class Service : Entity3Service<Organization>
		{

			#region Read

			public Organization ByPrefixCode(string prefixCode)
			{
				return prefixCode.No() ? null : By(a => a.AirlinePrefixCode == prefixCode);
			}

			public Organization ByIataCode(string iataCode)
			{
				return iataCode.No() ? null : By(a => a.AirlineIataCode == iataCode);
			}


			public override RangeResponse Suggest(RangeRequest prms)
			{
				//var queryStr = string.Format("{0}%", prms.Query.Replace('*', '%').Trim('%'));

				var list = (
					from a in Query
					where
						a.IsAirline && (
							a.AirlinePrefixCode.StartsWith(prms.Query) ||
								a.AirlineIataCode.StartsWith(prms.Query) ||
								a.Name.StartsWith(prms.Query)
							)
					orderby a.Name
					select EntityReference.FromArray(a.Id, a.Name, a.GetType().Name)
					)
					.ToArray();

				return new RangeResponse(list);
			}

			#endregion


			#region Calculations

			public IList<AviaDocument> GetNoAirlineDocuments(Organization airline, DateTime? dateFrom, DateTime? dateTo)
			{
				return GetNoAirlineDocumentsQuery(airline, dateFrom, dateTo).List();
			}

			public int GetNoAirlineDocumentCount(Organization airline, DateTime? dateFrom, DateTime? dateTo)
			{
				return GetNoAirlineDocumentsQuery(airline, dateFrom, dateTo).ToRowCountQuery().RowCount();
			}

			private IQueryOver<AviaDocument, AviaDocument> GetNoAirlineDocumentsQuery(Organization airline, DateTime? dateFrom, DateTime? dateTo)
			{
				AviaDocument doc = null;

				var codesFilter = Restrictions.Conjunction()
					.Add(Restrictions.Where<AviaDocument>(d => d.Producer == null || d.Producer != airline));

				var segmentsFilter = HQueryOver.Of<FlightSegment>()
					.Select(s => s.Id)
					.Where(s => s.Ticket.Id == doc.Id && (s.Carrier == null || s.Carrier != airline));

				if (airline.AirlinePrefixCode.Yes() && airline.AirlineIataCode.Yes())
				{
					codesFilter.Add(Restrictions.Where<AviaDocument>(d =>
						(d.AirlinePrefixCode != null || d.AirlineIataCode != null)
							&& (d.AirlinePrefixCode == airline.AirlinePrefixCode || d.AirlinePrefixCode == null)
							&& (d.AirlineIataCode == airline.AirlineIataCode || d.AirlineIataCode == null)));

					segmentsFilter.And(s => (s.CarrierPrefixCode != null || s.CarrierIataCode != null)
						&& (s.CarrierPrefixCode == airline.AirlinePrefixCode || s.CarrierPrefixCode == null)
						&& (s.CarrierIataCode == airline.AirlineIataCode || s.CarrierIataCode == null));
				}
				else if (airline.AirlinePrefixCode.Yes())
				{
					codesFilter.Add(Restrictions.Where<AviaDocument>(d => d.AirlinePrefixCode == airline.AirlinePrefixCode));

					segmentsFilter.And(s => s.CarrierPrefixCode == airline.AirlinePrefixCode);
				}
				else if (airline.AirlineIataCode.Yes())
				{
					codesFilter.Add(Restrictions.Where<AviaDocument>(d => d.AirlineIataCode == airline.AirlineIataCode));

					segmentsFilter.And(s => s.CarrierIataCode == airline.AirlineIataCode);
				}

				var query = Session.QueryOver(() => doc)
					.Where(Restrictions.Or(codesFilter, Subqueries.Exists(segmentsFilter.DetachedCriteria)));

				if (dateFrom.HasValue)
					query.And(d => d.IssueDate >= dateFrom.Value);

				if (dateTo.HasValue)
					query.And(d => d.IssueDate < dateTo.Value.AddDays(1));

				return query;
			}


			public IList<AviaTicket> GetNoServiceClassTickets(AirlineServiceClass serviceClass, DateTime? dateFrom, DateTime? dateTo)
			{
				return GetNoServiceClassTicketQuery(serviceClass, dateFrom, dateTo).List();
			}

			public int GetNoServiceClassTicketCount(AirlineServiceClass serviceClass, DateTime? dateFrom, DateTime? dateTo)
			{
				return GetNoServiceClassTicketQuery(serviceClass, dateFrom, dateTo).ToRowCountQuery().RowCount();
			}

			private IQueryOver<AviaTicket> GetNoServiceClassTicketQuery(AirlineServiceClass serviceClass, DateTime? dateFrom, DateTime? dateTo)
			{
				AviaTicket ticket = null;

				var query = Session.QueryOver(() => ticket);

				if (dateFrom.HasValue)
					query.And(d => d.IssueDate >= dateFrom.Value);

				if (dateTo.HasValue)
					query.And(d => d.IssueDate < dateTo.Value.AddDays(1));

				return query.WithSubquery.WhereExists(HQueryOver.Of<FlightSegment>()
					.Select(s => s.Id)
					.Where(s => s.Ticket.Id == ticket.Id && s.Carrier == serviceClass.Airline && s.ServiceClassCode == serviceClass.Code && s.ServiceClass != serviceClass.ServiceClass)
					);
			}

			#endregion


			#region Modify

			public Organization Resolve(Organization r)
			{
				if (r == null) return null;

				if (r.Name.No() || r.AirlineIataCode.No() || r.AirlinePrefixCode.No())
				{
					if (r.AirlinePrefixCode.Yes())
						return ByPrefixCode(r.AirlinePrefixCode);
					if (r.AirlineIataCode.Yes())
						return ByIataCode(r.AirlineIataCode);
					if (r.Name.Yes())
						return ByName(r.Name);

					throw new ArgumentException("airline");
				}

				var saved = ByPrefixCode(r.AirlinePrefixCode);

				return saved ?? Save(r);
			}

			[DebuggerStepThrough]
			public static Organization operator +(Organization r, Service service)
			{
				return service.Resolve(r);
			}

			#endregion


			#region Operations

			public OperationStatus CanApply()
			{
				return db.Granted(UserRole.Administrator, UserRole.Supervisor);
			}

			public int Apply(Organization airline, DateTime? dateFrom, DateTime? dateTo)
			{
				var documents = GetNoAirlineDocuments(airline, dateFrom, dateTo);

				foreach (var document in documents)
				{
					if (CheckAirline(airline, document.Producer, document.AirlinePrefixCode, document.AirlineIataCode))
						document.Producer = airline;

					if (document.Type != ProductType.AviaTicket) continue;

					foreach (var segment in ((AviaTicket)document).Segments)
						if (CheckAirline(airline, segment.Carrier, segment.CarrierPrefixCode, segment.CarrierIataCode))
							segment.Carrier = airline;
				}

				return documents.Count;
			}

			private static bool CheckAirline(Organization airline, Organization docAirline, string prefixCode, string iataCode)
			{
				if (airline == docAirline)
					return false;

				var res = false;

				if (prefixCode.Yes() && airline.AirlinePrefixCode.Yes())
				{
					if (prefixCode == airline.AirlinePrefixCode)
						res = true;
					else
						return false;
				}

				if (iataCode.Yes() && airline.AirlineIataCode.Yes())
				{
					if (iataCode == airline.AirlineIataCode)
						res = true;
					else
						return false;
				}

				return res;
			}

			#endregion

		}

	}


	partial class AirlineManager
	{
		public override Permissions GetCustomPermissions()
		{
			return new Permissions { { "ApplyAirlineToDocuments", db.Airline.CanApply() } };
		}
	}

}