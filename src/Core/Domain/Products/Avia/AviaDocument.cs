using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public interface IItineraryContainer
	{
		string Itinerary { get; set; }
		string GetItinerary(Func<Airport, string> airportToString, bool withSpaces, bool withDates);
	}


	[RU("Авиадокумент", "Авиадокументы")]
	public abstract partial class AviaDocument : Product, IItineraryContainer
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<AviaDocument> se)
		{
			se.For(a => a.ReissueFor)
				.Suggest<AviaDocument>();

			se.For(a => a.Producer)
				.Suggest<Airline>();

			se.For(a => a.Provider)
				.RU("Поставщик");
		}

		protected AviaDocument()
		{
			Originator = GdsOriginator.Unknown;
			Origin = ProductOrigin.Manual;
		}

		public override string Name => IsReservation ? $"{PnrCode}-{PassengerName}" : FullNumber;

		public override string PureNumber => Number.AsString();


		public override string PassengerName { get => GetPassengerName(); set => SetPassengerName(value); }

		[Patterns.Passenger]
		public virtual Person Passenger { get => GetPassenger(); set => SetPassenger(value); }

		public virtual string AirlineIataCode { get; set; }

		public override string ProducerOrProviderAirlineIataCode =>
			base.ProducerOrProviderAirlineIataCode ?? AirlineIataCode;

		[RU("Код АК")]
		public virtual string AirlinePrefixCode
		{
			get => _airlinePrefixCode;
			set
			{
				if (value.Yes() && value.Length < 3)
					value = value.PadLeft(3, '0');

				_airlinePrefixCode = value;
			}
		}

		public virtual string AirlineName { get; set; }

		[Patterns.Number]
		public virtual string Number { get; set; }


		public override bool IsReservation => Number == null;

		public virtual string ConjunctionNumbers { get; set; }

		[RU("Паспорт в GDS"), DefaultValue(0)]
		public virtual GdsPassportStatus GdsPassportStatus { get; set; }

		public virtual string GdsPassport { get; set; }

		[RU("Маршрут")]
		public virtual string Itinerary
		{
			get => null;
			// ReSharper disable once ValueParameterNotUsed
			set { }
		}


		public virtual string PaymentForm { get; set; }

		public virtual string PaymentDetails { get; set; }


		public virtual string AirlinePnrCode { get; set; }

		public virtual string Remarks { get; set; }

		public virtual IList<AviaDocumentFee> Fees { get => _fees;
			set => _fees = value;
		}


		public virtual IList<AviaDocumentVoiding> Voidings => _voidings;

		public virtual string FullNumber =>
			IsReservation ? null : $"{AirlinePrefixCode}-{Number:0000000000}";


		public virtual bool GdsFileIsExported { get; set; }




		//		[RU("Печатать все сегменты бронировки")]
		//		public virtual bool PrintUnticketedFlightSegments { get; set; }

		public override object Clone()
		{
			var clone = (AviaDocument)base.Clone();

			clone._fees = _fees
				.Select(a => a.Clone<AviaDocumentFee>())
				.ToList()
				.Do(list => list.ForEach(a => a.Document = clone));

			clone._voidings = _voidings
				.Select(a => a.Clone<AviaDocumentVoiding>())
				.ToList()
				.Do(list => list.ForEach(a => a.Document = clone));

			return clone;
		}

		public virtual void AddFee(string code, Money amount, bool updateTotal = true, bool ignoreOtherCode = true)
		{
			if (code.Yes())
				AddFee(new AviaDocumentFee { Code = code, Amount = amount }, updateTotal, ignoreOtherCode);
		}

		public virtual void AddFee(AviaDocumentFee fee, bool updateTotal = true, bool ignoreOtherCode = true)
		{
			if (ignoreOtherCode && fee.Code == AviaDocumentFee.OtherCode)
				return;

			fee.Document = this;

			if (fee.Code == AviaDocumentFee.ServiceCode)
			{
				ServiceFee = fee.Amount;
			}
			else
			{
				if (updateTotal)
					FeesTotal += fee.Amount;

				if (_fees == null)
					_fees = new List<AviaDocumentFee>();

				_fees.Add(fee);
			}
		}

		public virtual void AddVoiding(AviaDocumentVoiding voiding)
		{
			_voidings.Add(voiding);

			voiding.Document = this;

			IsVoid = voiding.IsVoid;
		}

		public virtual string GetItinerary(Func<Airport, string> airportToString, bool withSpaces, bool withDates)
		{
			return null;
		}

		public virtual Passport ParseGdsPassport()
		{
			if (GdsPassport.No() || Origin == ProductOrigin.SirenaXml)
				return null;

			DateTime birthday;
			DateTime expiredOn;

			var gdsPassport = GdsPassport.Split('/', '-');

			DateTime.TryParseExact(gdsPassport.By(5), "ddMMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday);
			DateTime.TryParseExact(gdsPassport.By(7), "ddMMMyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out expiredOn);

			Gender? gender = null;

			if (gdsPassport.By(6).Yes())
				gender = gdsPassport.By(6) == "M" ? Gender.Male : Gender.Female;

			if (gdsPassport.Length > 1)
				return new Passport
				{
					LastName = gdsPassport.By(8),
					FirstName = gdsPassport.By(9),
					Number = gdsPassport.By(3),
					Citizenship = new Country { TwoCharCode = gdsPassport.By(2) },
					IssuedBy = new Country { TwoCharCode = gdsPassport.By(4) },
					Gender = gender,
					Birthday = birthday == DateTime.MinValue ? (DateTime?)null : birthday.AsUtc(),
					ExpiredOn = expiredOn == DateTime.MinValue ? (DateTime?)null : expiredOn.AsUtc(),
				};

			return null;
		}

		public override void SetVoidStatus(Domain db, bool value)
		{
			var voiding = new AviaDocumentVoiding
			{
				Agent = db.Security.Person,
				IsVoid = value,
				Origin = Origin,
				Originator = Originator,
				TimeStamp = DateTime.Now.AsUtc()
			};

			AddVoiding(voiding);

			db.Save(voiding);

			if (value)
				Order?.Remove(db, this);
		}



		private string _airlinePrefixCode;

		private IList<AviaDocumentFee> _fees = new List<AviaDocumentFee>();
		private IList<AviaDocumentVoiding> _voidings = new List<AviaDocumentVoiding>();
	}

}