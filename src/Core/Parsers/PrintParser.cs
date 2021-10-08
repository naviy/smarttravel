using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Parsers
{
	public class PrintParser
	{
		public IList<AviaDocument> Parse(string path)
		{
			CheckFilesExisting(path);

			ReadData(path);

			return Parse();
		}

		public string NeutralAirlineCode { get; set; }

		private static void CheckFilesExisting(string path)
		{
			var checkFile = new Action<string>(
				fileName =>
				{
					if (!System.IO.File.Exists(Path.Combine(path, fileName)))
						throw new GdsImportException(string.Format("File {0} doesn't exist", fileName));
				});

			checkFile(MainFileName);
			checkFile(FareFileName);
			checkFile(FlightFileName);
		}

		private void ReadData(string path)
		{
			var documentMap = new Dictionary<int, DocumentRecord>();
			var conjDocumentMap = new Dictionary<long, DocumentRecord>();

			var conjunctions = new List<DbfDataRecord>();

			using (var provider = new DbfRecordProvider(Path.Combine(path, MainFileName)))
			{
				var numbers = new List<long>();

				while (provider.Read())
				{
					if (!IsValidDocumentRecord(provider.Record))
						continue;

					if (IsConjunction(provider.Record))
					{
						conjunctions.Add(provider.Record);
						continue;
					}

					var number = long.Parse(provider.Get<string>("seria") + provider.Get<string>("numb"));

					if (numbers.Contains(number))
						throw new GdsImportException("Tickets whith the same numbers where founded");

					var documentRecord = new DocumentRecord { Attributes = provider.Record };

					documentMap.Add(provider.Get<int>("id"), documentRecord);
					conjDocumentMap.Add(number, documentRecord);

					_documentRecords.Add(documentRecord);
				}
			}

			foreach (DbfDataRecord conjunction in conjunctions)
			{
				var num = long.Parse(conjunction.Get<string>("seria") + conjunction.Get<string>("conjnumb"));

				DocumentRecord documentRecord = conjDocumentMap[num];

				if (IsNeutralAirline(documentRecord) && IsNeutralAirline(conjunction) ||
					conjunction.Get<string>("registryko") == documentRecord.Get<string>("registryko"))
				{
					documentRecord.Conjunctions.Add(conjunction);
					documentMap.Add(conjunction.Get<int>("id"), documentRecord);
				}
			}

			using (var provider = new DbfRecordProvider(Path.Combine(path, FareFileName)))
			{
				while (provider.Read())
				{
					if (IsValidFinanceRecord(provider.Record))
						documentMap[provider.Get<int>("ticket_id")].FinanceRecords.Add(provider.Record);
				}
			}

			using (var provider = new DbfRecordProvider(Path.Combine(path, FlightFileName)))
			{
				while (provider.Read())
					documentMap[provider.Get<int>("ticket_id")].SegmentRecords.Add(provider.Record);
			}
		}

		private bool IsNeutralAirline(DbfDataRecord mainRecord)
		{
			var registryko = mainRecord.Get<string>("registryko");

			return registryko.No() || registryko == NeutralAirlineCode;
		}

		private bool IsNeutralAirline(DocumentRecord record)
		{
			return IsNeutralAirline(record.Attributes);
		}

		private IList<AviaDocument> Parse()
		{
			foreach (var record in _documentRecords)
			{
				_documentRecord = record;

				ParseDocument();
			}

			return _documents;
		}

		private void ParseDocument()
		{
			if (_documentRecord.Get<string>("seria").StartsWith("40") && _documentRecord.SegmentRecords.Count == 0)
				_document = new AviaMco();
			else
				_document = new AviaTicket();

			ReadAttributes();

			ReadFinanceData();

			if (_document.Type == ProductType.AviaTicket)
				ReadSegments();

			_documents.Add(_document);
		}

		private void ReadAttributes()
		{
			_document.IssueDate = _documentRecord.Get<DateTime>("date_reg");

			if (!IsNeutralAirline(_documentRecord))
				_document.AirlinePrefixCode = _documentRecord.Get<string>("registryko");
			else
				_document.AirlinePrefixCode = NeutralAirlineCode;

			_document.Number = _documentRecord.Get<string>("seria") + _documentRecord.Get<string>("numb");

			if (_documentRecord.Conjunctions.Count > 0)
			{
				_documentRecord.Conjunctions.Sort(
				(record1, record2) => string.Compare(record2.Get<string>("numb"), record1.Get<string>("numb")));

				_document.ConjunctionNumbers = _documentRecord.Conjunctions[0].Get<string>("numb").Substring(4);
			}

			_document.PassengerName = _documentRecord.Get<string>("passname");

			if (_documentRecord.HasValue("pnr"))
				_document.PnrCode = _documentRecord.Get<string>("pnr");

			_document.Originator = GdsOriginator.Amadeus;
			_document.Origin = ProductOrigin.AmadeusPrint;
		}

		private void ReadFinanceData()
		{
			ReadFare();

			ReadFees();

			ReadVat();

			_document.Total = _document.EqualFare + _document.FeesTotal;
		}

		private void ReadSegments()
		{
			var ticket = ((AviaTicket)_document);

			foreach (DbfDataRecord record in _documentRecord.SegmentRecords)
			{
				int segmentCount = ticket.Segments.Count;

				var segment = new FlightSegment();
				segment.FromAirport = new Airport
				{
					Code = record.Get<string>("from_port"),
					Settlement = record.Get<string>("from_city")
				};

				segment.ToAirport = new Airport
				{
					Code = record.Get<string>("to_port"),
					Settlement = record.Get<string>("to_city")
				};

				segment.FromAirportCode = segment.FromAirport.Code;
				segment.ToAirportCode = segment.ToAirport.Code;

				segment.CarrierIataCode = record.Get<string>("carrier");

				segment.ServiceClassCode = record.Get<string>("clas");

				string race = record.Get<string>("race");

				if (race != "O" && race != "OPEN")
				{
					segment.FlightNumber = race;

					if (record.HasValue("flightdate") && record.HasValue("flighttime"))
					{
						DateTime date = record.Get<DateTime>("flightdate");
						DateTime time = DateTime.ParseExact(record.Get<string>("flighttime"), "HHmm", CultureInfo.CurrentCulture);

						segment.DepartureTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
					}
				}

				segment.FareBasis = record.Get<string>("farebasis");

				var tourCode = record.Get<string>("tourcode");

				if (record.Get<int>("ncoupon") == 1 && segmentCount == 0)
				{
					if (tourCode.Yes())
						_document.TourCode = tourCode;
				}
				else
				{
					if ((_document.TourCode.No() && tourCode.Yes()) || (_document.TourCode.Yes() && _document.TourCode != tourCode))
						throw new GdsImportException("Different segment tourcodes were found");
				}

				var stopover = record.Get<string>("XO");

				if (stopover == "X" && segmentCount > 0)
					ticket.Segments[segmentCount - 1].Stopover = false;

				segment.Stopover = true;

				segment.Position = segmentCount;

				ticket.AddSegment(segment);
			}
		}

		private void ReadFare()
		{
			DbfDataRecord fareRecord = _documentRecord.FinanceRecords.Find(x => x.Get<string>("vidplat") == "01");

			_document.Fare = new Money(Utility.ResolveCurrencyCode(fareRecord.Get<string>("currency")), fareRecord.Get<decimal>("sumr"));
			_document.EqualFare = new Money(Utility.ResolveCurrencyCode(fareRecord.Get<string>("curropl")), fareRecord.Get<decimal>("sumopl"));
		}

		private void ReadFees()
		{
			var currency = new Currency(Utility.ResolveCurrencyCode(_documentRecord.Get<string>("currency")));

			_document.FeesTotal = new Money(currency);

			List<DbfDataRecord> feesRecords = _documentRecord.FinanceRecords.FindAll(x => x.Get<string>("vidplat") == "TAX");

			foreach (DbfDataRecord record in feesRecords)
			{
				var taxtCode = record.Get<string>("taxcode");

				if (taxtCode.No())
					continue;

				_document.AddFee(new AviaDocumentFee
				{
					Code = taxtCode,
					Amount = new Money(Utility.ResolveCurrencyCode(record.Get<string>("curropl")), record.Get<decimal>("sumopl"))
				});
			}
		}

		private void ReadVat()
		{
			DbfDataRecord hfTaxRecord = _documentRecord.FinanceRecords.Find(x => x.Get<string>("vidplat") == "HF");
			DbfDataRecord ndcTaxRecord = _documentRecord.FinanceRecords.Find(x => x.Get<string>("vidplat") == "NDC");

			if (hfTaxRecord != null && ndcTaxRecord != null)
				throw new GdsImportException("HF and NDC taxes are presented");

			DbfDataRecord vatRecord = hfTaxRecord ?? ndcTaxRecord;

			if (vatRecord != null)
				_document.Vat = new Money(Utility.ResolveCurrencyCode(vatRecord.Get<string>("curropl")), vatRecord.Get<decimal>("sumopl"));
		}

		private static bool IsValidDocumentRecord(DbfDataRecord record)
		{
			var seria = record.Get<string>("seria");
			var numb = record.Get<string>("numb");

			return seria.Yes() && numb.Yes() && (seria + numb).Length == 10 && record.Get<int>("operation") == 1;
		}

		private static bool IsConjunction(DbfDataRecord record)
		{
			return (record["conjnumb"] != null && !Equals(record["conjnumb"], record["numb"]));
		}

		private static bool IsValidFinanceRecord(DbfDataRecord record)
		{
			var vidplat = record.Get<string>("vidplat");

			return (vidplat == "01" || vidplat == "TAX" || vidplat == "NDC") && record.Get<string>("taxcode") != "--";
		}

		private readonly List<AviaDocument> _documents = new List<AviaDocument>();
		private readonly List<DocumentRecord> _documentRecords = new List<DocumentRecord>();

		private AviaDocument _document;
		private DocumentRecord _documentRecord;

		private const string MainFileName = "Main.dbf";
		private const string FlightFileName = "Flight.dbf";
		private const string FareFileName = "Fare.dbf";

		private class DocumentRecord
		{
			public DocumentRecord()
			{
				SegmentRecords = new List<DbfDataRecord>();
				FinanceRecords = new List<DbfDataRecord>();
				Conjunctions = new List<DbfDataRecord>();
			}

			public DbfDataRecord Attributes { get; set; }

			public List<DbfDataRecord> SegmentRecords { get; private set; }

			public List<DbfDataRecord> FinanceRecords { get; private set; }

			public List<DbfDataRecord> Conjunctions { get; private set; }

			public bool HasValue(string name)
			{
				return Attributes.HasValue(name);
			}

			public T Get<T>(string name)
			{
				return Attributes.Get<T>(name);
			}
		}
	}
}