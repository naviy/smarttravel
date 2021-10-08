using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Luxena.Travel.Config;
using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using NHibernate;
using NHibernate.Linq;


namespace Luxena.Travel.FaresParser
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				var cfg = ConfigurationBuilder.Build(null);
				var sessionFactory = cfg.BuildSessionFactory();

				using (db = sessionFactory.OpenStatelessSession())
				{
					//ClearFareSegments();
					LoadDictionaries();
					LoadGdsFiles();

					Log("Обработка {0} GDS-файлов:", files.Count);

					var processedTickets = new HashSet<string>();
					tickets = new List<AviaTicket>();

					for (var i = 0; i < files.Count;)
					{
						if (!ParseTickets(1000, ref i))
							continue;

						var numbers = tickets.Select(a => a.Number).Distinct().ToArray();

						Dictionary<string, AviaTicket> existingTickets;
						using (var sess = sessionFactory.OpenSession(db.Connection))
						{
							existingTickets = sess.Query<AviaTicket>()
								.Where(t => numbers.Contains(t.Number))
								.FetchMany(t => t.Segments)
								.ToDictionary(t => t.DisplayString);
						}

						using (var txn = db.BeginTransaction())
						{
							foreach (var ticket in tickets)
							{
								var existing = existingTickets.GetOrDefault(ticket.DisplayString);

								if (existing == null || processedTickets.Contains(ticket.DisplayString))
									continue;

								UpdateFareTotal(ticket, existing);

								processedTickets.Add(ticket.DisplayString);

								if (!CopyFareSegments(ticket, existing))
									continue;

								existing.CalculateFares();

								SaveFareSegments(existing);
							}

							if (sql.Length > 0)
								db.CreateSQLQuery(sql.ToString()).ExecuteUpdate();

							sql.Clear();

							txn.Commit();
						}

						Log("Обработано {0} файлов", i);
					}
				}

				Log("Импорт звершён");
			}
			catch (Exception ex)
			{
				Log(ex);
			}
		}

		private static bool CopyFareSegments(AviaTicket src, AviaTicket dest)
		{
			if (dest.Segments.Count != src.Segments.Count)
				return false;

			for (var j = 0; j < dest.Segments.Count; j++)
			{
				var srcSeg = src.Segments[j];
				var destSeg = dest.Segments[j];

				destSeg.Surcharges = srcSeg.Surcharges;
				destSeg.IsInclusive = srcSeg.IsInclusive;
				destSeg.Fare = srcSeg.Fare;
				destSeg.StopoverOrTransferCharge = srcSeg.StopoverOrTransferCharge;
				destSeg.IsSideTrip = srcSeg.IsSideTrip;

				destSeg.FromAirport = airports.GetOrDefault(destSeg.FromAirportCode);
				destSeg.ToAirport = airports.GetOrDefault(destSeg.ToAirportCode);
				destSeg.Distance = Airport.GetDistance(destSeg.FromAirport, destSeg.ToAirport);
			}

			return true;
		}

		private static void ClearFareSegments()
		{
			Log("Очистка тарифных сегментов (длительная)...");

			db.CreateSQLQuery(@"
				update
					lt_avia_ticket
				set
					faretotal_amount = null,
					faretotal_currency = null")
				.ExecuteUpdate();

			db.CreateSQLQuery(@"
				update
					lt_flight_segment
				set
					surcharges = null,
					isinclusive = false,
					fare = null,
					stopoverortransfercharge = null,
					issidetrip = false,
					distance = 0,
					amount_amount = null,
					amount_currency = null")
				.ExecuteUpdate();

			Log("OK");
		}

		private static void LoadDictionaries()
		{
			Log("Загрузка справочников...");

			airports = NewDictionary<Airport>(a => a.Code);
			currenciesById = NewDictionary<object, Currency>(c => c.Id);
			currencies = NewDictionary<Currency>(c => c.Code);
			uah = currencies.GetOrDefault("UAH") ?? new Currency("UAH");

			Log("OK");
		}

		private static void LoadGdsFiles()
		{
			Log("Загрузка GDS-файлов...");

			using (var txn = db.BeginTransaction())
			{
				var minTimeStamp = db
					.CreateSQLQuery(@"
						select
							min(f.TimeStamp)
						from
							lt_flight_segment s
							inner join lt_avia_document d on d.id = s.ticket
							inner join lt_gds_file f on d.originaldocument = f.id
						where
							s.departuretime >= '2013-1-1' and s.Type = 0"
					)
					.UniqueResult<DateTime>().Date;

				files = db.QueryOver<GdsFile>()
					.Where(f => f.TimeStamp >= minTimeStamp)
					.AndRestrictionOn(f => f.FileType).IsIn(new[] { GdsFileType.AirFile, GdsFileType.MirFile })
					.OrderBy(f => f.TimeStamp).Asc
					.List();

				txn.Commit();
			}

			Log("OK");
		}

		private static bool ParseTickets(int count, ref int i)
		{
			tickets.Clear();

			for (var j = 0; j < count && i < files.Count; ++j, ++i)
			{
				try
				{
					var docs = files[i].FileType == GdsFileType.AirFile
						? AirParser.Parse(files[i].Content, uah)
						: MirParser.Parse(files[i].Content, uah);

					tickets.AddRange(docs.OfType<AviaTicket>().Where(a => a.Number != null));
				}
				catch (Exception ex)
				{
					Log("{0} {1}: {2}", files[i].FileType, files[i].Name, ex.Message);
				}
			}

			return tickets.Count > 0;
		}

		private static void UpdateFareTotal(AviaTicket ticket, AviaTicket existing)
		{
			if (existing.Total != null)
				existing.Total.Currency = currenciesById[existing.Total.Currency.Id];

			if (ticket.FareTotal != null)
			{
				ticket.FareTotal.Currency = currencies.GetOrDefault(ticket.FareTotal.Currency.Code);
				if (ticket.FareTotal.Currency == null)
					return;

				existing.FareTotal = ticket.FareTotal;

				db.CreateSQLQuery("update lt_avia_ticket set faretotal_currency = :currency, faretotal_amount = :amount where id = :id")
					.SetParameter("currency", ticket.FareTotal.Currency.Id)
					.SetParameter("amount", ticket.FareTotal.Amount)
					.SetParameter("id", existing.Id)
					.ExecuteUpdate();
			}
			else
			{
				db.CreateSQLQuery("update lt_avia_ticket set faretotal_currency = null, faretotal_amount = null where id = :id")
					.SetParameter("id", existing.Id)
					.ExecuteUpdate();
			}
		}

		private static void SaveFareSegments(AviaTicket ticket)
		{
			Func<decimal?, string> toNumber = a => a == null ? "null" : a.Value.ToString(CultureInfo.InvariantCulture);
			Func<bool, string> toBool = a => a ? "true" : "false";

			foreach (var seg in ticket.Segments)
			{
				sql.AppendFormat(@"
					update
						lt_flight_segment
					set
						surcharges = {1},
						isinclusive = {2},
						fare = {3},
						stopoverortransfercharge = {4},
						issidetrip = {5},
						distance = {6},
						amount_amount = {7},
						amount_currency = {8}
					where
						id = '{0}';
					",
					seg.Id,
					toNumber(seg.Surcharges), toBool(seg.IsInclusive), toNumber(seg.Fare), toNumber(seg.StopoverOrTransferCharge),
					toBool(seg.IsSideTrip), seg.Distance.ToString(CultureInfo.InvariantCulture),
					seg.Amount != null ? seg.Amount.Amount.ToString(CultureInfo.InvariantCulture) : "null",
					seg.Amount != null && seg.Amount.Currency != null ? "'" + seg.Amount.Currency.Id + "'" : "null"
				);
			}
		}

		private static IDictionary<TKey, T> NewDictionary<TKey, T>(Func<T, TKey> keySelector) where T : class
		{
			using (var txn = db.BeginTransaction())
			{
				var dic = db.QueryOver<T>().List().ToDictionary(keySelector);
				txn.Commit();
				return dic;
			}
		}

		private static IDictionary<string, T> NewDictionary<T>(Func<T, string> keySelector) where T : class
		{
			return NewDictionary<string, T>(keySelector);
		}

		private static void Log(object msg)
		{
			Console.WriteLine("{0:HH:mm:ss}    {1}", DateTime.Now, msg);
		}

		private static void Log(string format, params object[] args)
		{
			Console.WriteLine("{0:HH:mm:ss}    {1}", DateTime.Now, string.Format(format, args));
		}

		private static IStatelessSession db;
		private static StringBuilder sql = new StringBuilder();
		private static IDictionary<string, Airport> airports;

		private static IDictionary<string, Currency> currencies;
		private static IDictionary<object, Currency> currenciesById;
		private static Currency uah;

		private static IList<GdsFile> files;
		private static List<AviaTicket> tickets;
	}
}