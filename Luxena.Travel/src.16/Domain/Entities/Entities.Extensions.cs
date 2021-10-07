using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Luxena.Domain;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCast
// ReSharper disable UnusedParameter.Local
#pragma warning disable 109
#pragma warning disable 108,114

namespace Luxena.Travel.Domain
{
	

	#region Accommodation

	partial class Accommodation
	{
	//1

		[ForeignKey("AccommodationType")]
		public string AccommodationTypeId
		{ 
			get { return _AccommodationTypeId; }
			set 
			{
				if (_AccommodationTypeId == value) return;
				_AccommodationTypeId = value;
				_AccommodationType = null;
			}
		}
		private string _AccommodationTypeId;

		public virtual AccommodationType AccommodationType
		{
			get 
			{ 
				return _AccommodationType ?? (_AccommodationType = db?.AccommodationTypes.ById(_AccommodationTypeId)); 
			}
			set
			{
				_AccommodationType = value;
				_AccommodationTypeId = value?.Id;
			}
		}

		[ForeignKey("CateringType")]
		public string CateringTypeId
		{ 
			get { return _CateringTypeId; }
			set 
			{
				if (_CateringTypeId == value) return;
				_CateringTypeId = value;
				_CateringType = null;
			}
		}
		private string _CateringTypeId;

		public virtual CateringType CateringType
		{
			get 
			{ 
				return _CateringType ?? (_CateringType = db?.CateringTypes.ById(_CateringTypeId)); 
			}
			set
			{
				_CateringType = value;
				_CateringTypeId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Accommodation);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Accommodations;

		internal Accommodation n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Accommodation)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Accommodation)value;
		}

		public class Lazy<TValue> : Lazy<Accommodation, TValue>
		{
			public Lazy(Func<Accommodation, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Accommodation, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Accommodation, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Accommodation operator +(Accommodation r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Accommodation operator |(Accommodation r1, Accommodation r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Accommodation
			{
				HotelName = e.Property(a => a.HotelName).OriginalValue,
				HotelOffice = e.Property(a => a.HotelOffice).OriginalValue,
				HotelCode = e.Property(a => a.HotelCode).OriginalValue,
				PlacementName = e.Property(a => a.PlacementName).OriginalValue,
				PlacementOffice = e.Property(a => a.PlacementOffice).OriginalValue,
				PlacementCode = e.Property(a => a.PlacementCode).OriginalValue,
				AccommodationTypeId = e.Property(a => a.AccommodationTypeId).OriginalValue,
				CateringTypeId = e.Property(a => a.CateringTypeId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Accommodation).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Accommodation> entity);
		public static void Config(Domain.EntityConfiguration<Accommodation> entity) => Config_(entity);

	}


	[Localization(typeof(Accommodation))]
	public class AccommodationReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public AccommodationReference() { }

		public AccommodationReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AccommodationReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AccommodationReference operator +(AccommodationReference reference, DbSet<Accommodation> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AccommodationReference operator +(AccommodationReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Accommodations.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator AccommodationReference(Accommodation entity)
		{
			return entity == null ? null : new AccommodationReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AccommodationReference OneRef(this IQueryable<Accommodation> query)
		{
			return query?
				.Select(a => new AccommodationReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AccommodationLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Accommodation> query, ref IEnumerable<AccommodationLookup> lookupList);

		public static IEnumerable<AccommodationLookup> SelectAndOrderByName(IQueryable<Accommodation> query)
		{
			IEnumerable<AccommodationLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AccommodationLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AccommodationLookup> DefaultLookup(LookupParams<Accommodation, AccommodationLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region AccommodationType

	partial class AccommodationType
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(AccommodationType);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).AccommodationTypes;

		internal AccommodationType n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (AccommodationType)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (AccommodationType)value;
		}

		public class Lazy<TValue> : Lazy<AccommodationType, TValue>
		{
			public Lazy(Func<AccommodationType, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<AccommodationType, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<AccommodationType, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static AccommodationType operator +(AccommodationType r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static AccommodationType operator |(AccommodationType r1, AccommodationType r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new AccommodationType
			{
				Description = e.Property(a => a.Description).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(AccommodationType).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<AccommodationType> entity);
		public static void Config(Domain.EntityConfiguration<AccommodationType> entity) => Config_(entity);

	}


	[Localization(typeof(AccommodationType))]
	public class AccommodationTypeReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public AccommodationTypeReference() { }

		public AccommodationTypeReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AccommodationTypeReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AccommodationTypeReference operator +(AccommodationTypeReference reference, DbSet<AccommodationType> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AccommodationTypeReference operator +(AccommodationTypeReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.AccommodationTypes.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator AccommodationTypeReference(AccommodationType entity)
		{
			return entity == null ? null : new AccommodationTypeReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AccommodationTypeReference OneRef(this IQueryable<AccommodationType> query)
		{
			return query?
				.Select(a => new AccommodationTypeReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AccommodationTypeLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<AccommodationType> query, ref IEnumerable<AccommodationTypeLookup> lookupList);

		public static IEnumerable<AccommodationTypeLookup> SelectAndOrderByName(IQueryable<AccommodationType> query)
		{
			IEnumerable<AccommodationTypeLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AccommodationTypeLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AccommodationTypeLookup> DefaultLookup(LookupParams<AccommodationType, AccommodationTypeLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region AirlineServiceClass

	partial class AirlineServiceClass
	{
	//1

		[ForeignKey("Airline")]
		public string AirlineId
		{ 
			get { return _AirlineId; }
			set 
			{
				if (_AirlineId == value) return;
				_AirlineId = value;
				_Airline = null;
			}
		}
		private string _AirlineId;

		public virtual Organization Airline
		{
			get 
			{ 
				return _Airline ?? (_Airline = db?.Organizations.ById(_AirlineId)); 
			}
			set
			{
				_Airline = value;
				_AirlineId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(AirlineServiceClass);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).AirlineServiceClasses;

		internal AirlineServiceClass n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (AirlineServiceClass)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (AirlineServiceClass)value;
		}

		public class Lazy<TValue> : Lazy<AirlineServiceClass, TValue>
		{
			public Lazy(Func<AirlineServiceClass, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<AirlineServiceClass, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<AirlineServiceClass, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static AirlineServiceClass operator +(AirlineServiceClass r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static AirlineServiceClass operator |(AirlineServiceClass r1, AirlineServiceClass r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new AirlineServiceClass
			{
				Code = e.Property(a => a.Code).OriginalValue,
				ServiceClass = e.Property(a => a.ServiceClass).OriginalValue,
				AirlineId = e.Property(a => a.AirlineId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(AirlineServiceClass).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<AirlineServiceClass> entity);
		public static void Config(Domain.EntityConfiguration<AirlineServiceClass> entity) => Config_(entity);

	}


	[Localization(typeof(AirlineServiceClass))]
	public class AirlineServiceClassReference : INameContainer
	{

		public string Id { get; set; }

		public string Code { get; set; }
			
		public AirlineServiceClassReference() { }

		public AirlineServiceClassReference(string id, string name)
		{
			Id = id;
			Code = name;
		}

		public string GetName() => Code;

		public static implicit operator bool(AirlineServiceClassReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AirlineServiceClassReference operator +(AirlineServiceClassReference reference, DbSet<AirlineServiceClass> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Code = set.Where(a => a.Id == id).Select(a => a.Code).FirstOrDefault();


			return reference;
		}

		public static AirlineServiceClassReference operator +(AirlineServiceClassReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Code = db.AirlineServiceClasses.Where(a => a.Id == id).Select(a => a.Code).FirstOrDefault();

			return reference;
		}

		public static implicit operator AirlineServiceClassReference(AirlineServiceClass entity)
		{
			return entity == null ? null : new AirlineServiceClassReference { Id = entity.Id, Code = entity.Code };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AirlineServiceClassReference OneRef(this IQueryable<AirlineServiceClass> query)
		{
			return query?
				.Select(a => new AirlineServiceClassReference { Id = a.Id, Code = a.Code })
				.FirstOrDefault();
		}
	}


	public partial class AirlineServiceClassLookup : INameContainer
	{

		public string Id { get; set; }

		public string Code { get; set; }

		public string GetName()	=> Code;

		static partial void SelectAndOrderByName(IQueryable<AirlineServiceClass> query, ref IEnumerable<AirlineServiceClassLookup> lookupList);

		public static IEnumerable<AirlineServiceClassLookup> SelectAndOrderByName(IQueryable<AirlineServiceClass> query)
		{
			IEnumerable<AirlineServiceClassLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AirlineServiceClassLookup { Id = a.Id, Code = a.Code })
				.OrderBy(a => a.Code);
		}

		public static IEnumerable<AirlineServiceClassLookup> DefaultLookup(LookupParams<AirlineServiceClass, AirlineServiceClassLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Code.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Airport

	partial class Airport
	{
	//1

		[ForeignKey("Country")]
		public string CountryId
		{ 
			get { return _CountryId; }
			set 
			{
				if (_CountryId == value) return;
				_CountryId = value;
				_Country = null;
			}
		}
		private string _CountryId;

		public virtual Country Country
		{
			get 
			{ 
				return _Country ?? (_Country = db?.Countries.ById(_CountryId)); 
			}
			set
			{
				_Country = value;
				_CountryId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Airport);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Airports;

		internal Airport n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Airport)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Airport)value;
		}

		public class Lazy<TValue> : Lazy<Airport, TValue>
		{
			public Lazy(Func<Airport, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Airport, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Airport, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Airport operator +(Airport r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Airport operator |(Airport r1, Airport r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Airport
			{
				Code = e.Property(a => a.Code).OriginalValue,
				Settlement = e.Property(a => a.Settlement).OriginalValue,
				LocalizedSettlement = e.Property(a => a.LocalizedSettlement).OriginalValue,
				Latitude = e.Property(a => a.Latitude).OriginalValue,
				Longitude = e.Property(a => a.Longitude).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Airport).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Airport> entity);
		public static void Config(Domain.EntityConfiguration<Airport> entity) => Config_(entity);

	}


	[Localization(typeof(Airport))]
	public class AirportReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public AirportReference() { }

		public AirportReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AirportReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AirportReference operator +(AirportReference reference, DbSet<Airport> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AirportReference operator +(AirportReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Airports.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator AirportReference(Airport entity)
		{
			return entity == null ? null : new AirportReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AirportReference OneRef(this IQueryable<Airport> query)
		{
			return query?
				.Select(a => new AirportReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AirportLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Airport> query, ref IEnumerable<AirportLookup> lookupList);

		public static IEnumerable<AirportLookup> SelectAndOrderByName(IQueryable<Airport> query)
		{
			IEnumerable<AirportLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AirportLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AirportLookup> DefaultLookup(LookupParams<Airport, AirportLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region AviaDocument

	partial class AviaDocument
	{
	//1


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).AviaDocuments;

		internal AviaDocument n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (AviaDocument)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (AviaDocument)value;
		}

		public class Lazy<TValue> : Lazy<AviaDocument, TValue>
		{
			public Lazy(Func<AviaDocument, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<AviaDocument, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<AviaDocument, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static AviaDocument operator +(AviaDocument r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static AviaDocument operator |(AviaDocument r1, AviaDocument r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<AviaDocument> entity);
		public static void Config(Domain.EntityConfiguration<AviaDocument> entity) => Config_(entity);

	}


	[Localization(typeof(AviaDocument))]
	public class AviaDocumentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }
			
		public AviaDocumentReference() { }

		public AviaDocumentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AviaDocumentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AviaDocumentReference operator +(AviaDocumentReference reference, DbSet<AviaDocument> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AviaDocumentReference operator +(AviaDocumentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.AviaDocuments.Where(a => a.Id == id).Select(a => new { a.Name, a.Type }).FirstOrDefault().Do(a =>
			{
				reference.Name = a.Name;
				reference._Type = a.Type.AsString();
			});

			return reference;
		}

		public static implicit operator AviaDocumentReference(AviaDocument entity)
		{
			return entity == null ? null : new AviaDocumentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AviaDocumentReference OneRef(this IQueryable<AviaDocument> query)
		{
			return query?
				.Select(a => new AviaDocumentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AviaDocumentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<AviaDocument> query, ref IEnumerable<AviaDocumentLookup> lookupList);

		public static IEnumerable<AviaDocumentLookup> SelectAndOrderByName(IQueryable<AviaDocument> query)
		{
			IEnumerable<AviaDocumentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AviaDocumentLookup { Id = a.Id, Name = a.Name, _Type = a.Type.ToString() })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AviaDocumentLookup> DefaultLookup(LookupParams<AviaDocument, AviaDocumentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region AviaMco

	partial class AviaMco
	{
	//1

		[ForeignKey("InConnectionWith")]
		public string InConnectionWithId
		{ 
			get { return _InConnectionWithId; }
			set 
			{
				if (_InConnectionWithId == value) return;
				_InConnectionWithId = value;
				_InConnectionWith = null;
			}
		}
		private string _InConnectionWithId;

		public virtual AviaDocument InConnectionWith
		{
			get 
			{ 
				return _InConnectionWith ?? (_InConnectionWith = db?.AviaDocuments.ById(_InConnectionWithId)); 
			}
			set
			{
				_InConnectionWith = value;
				_InConnectionWithId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(AviaMco);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).AviaMcos;

		internal AviaMco n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (AviaMco)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (AviaMco)value;
		}

		public class Lazy<TValue> : Lazy<AviaMco, TValue>
		{
			public Lazy(Func<AviaMco, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<AviaMco, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<AviaMco, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static AviaMco operator +(AviaMco r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static AviaMco operator |(AviaMco r1, AviaMco r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new AviaMco
			{
				Description = e.Property(a => a.Description).OriginalValue,
				InConnectionWithId = e.Property(a => a.InConnectionWithId).OriginalValue,
				AirlineIataCode = e.Property(a => a.AirlineIataCode).OriginalValue,
				AirlinePrefixCode = e.Property(a => a.AirlinePrefixCode).OriginalValue,
				AirlineName = e.Property(a => a.AirlineName).OriginalValue,
				Number = e.Property(a => a.Number).OriginalValue,
				ConjunctionNumbers = e.Property(a => a.ConjunctionNumbers).OriginalValue,
				GdsPassportStatus = e.Property(a => a.GdsPassportStatus).OriginalValue,
				GdsPassport = e.Property(a => a.GdsPassport).OriginalValue,
				PaymentForm = e.Property(a => a.PaymentForm).OriginalValue,
				PaymentDetails = e.Property(a => a.PaymentDetails).OriginalValue,
				AirlinePnrCode = e.Property(a => a.AirlinePnrCode).OriginalValue,
				Remarks = e.Property(a => a.Remarks).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(AviaMco).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<AviaMco> entity);
		public static void Config(Domain.EntityConfiguration<AviaMco> entity) => Config_(entity);

	}


	[Localization(typeof(AviaMco))]
	public class AviaMcoReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public AviaMcoReference() { }

		public AviaMcoReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AviaMcoReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AviaMcoReference operator +(AviaMcoReference reference, DbSet<AviaMco> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AviaMcoReference operator +(AviaMcoReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.AviaMcos.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator AviaMcoReference(AviaMco entity)
		{
			return entity == null ? null : new AviaMcoReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AviaMcoReference OneRef(this IQueryable<AviaMco> query)
		{
			return query?
				.Select(a => new AviaMcoReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AviaMcoLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<AviaMco> query, ref IEnumerable<AviaMcoLookup> lookupList);

		public static IEnumerable<AviaMcoLookup> SelectAndOrderByName(IQueryable<AviaMco> query)
		{
			IEnumerable<AviaMcoLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AviaMcoLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AviaMcoLookup> DefaultLookup(LookupParams<AviaMco, AviaMcoLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region AviaRefund

	partial class AviaRefund
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(AviaRefund);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).AviaRefunds;

		internal AviaRefund n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (AviaRefund)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (AviaRefund)value;
		}

		public class Lazy<TValue> : Lazy<AviaRefund, TValue>
		{
			public Lazy(Func<AviaRefund, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<AviaRefund, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<AviaRefund, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static AviaRefund operator +(AviaRefund r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static AviaRefund operator |(AviaRefund r1, AviaRefund r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new AviaRefund
			{
				AirlineIataCode = e.Property(a => a.AirlineIataCode).OriginalValue,
				AirlinePrefixCode = e.Property(a => a.AirlinePrefixCode).OriginalValue,
				AirlineName = e.Property(a => a.AirlineName).OriginalValue,
				Number = e.Property(a => a.Number).OriginalValue,
				ConjunctionNumbers = e.Property(a => a.ConjunctionNumbers).OriginalValue,
				GdsPassportStatus = e.Property(a => a.GdsPassportStatus).OriginalValue,
				GdsPassport = e.Property(a => a.GdsPassport).OriginalValue,
				PaymentForm = e.Property(a => a.PaymentForm).OriginalValue,
				PaymentDetails = e.Property(a => a.PaymentDetails).OriginalValue,
				AirlinePnrCode = e.Property(a => a.AirlinePnrCode).OriginalValue,
				Remarks = e.Property(a => a.Remarks).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(AviaRefund).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<AviaRefund> entity);
		public static void Config(Domain.EntityConfiguration<AviaRefund> entity) => Config_(entity);

	}


	[Localization(typeof(AviaRefund))]
	public class AviaRefundReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public AviaRefundReference() { }

		public AviaRefundReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AviaRefundReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AviaRefundReference operator +(AviaRefundReference reference, DbSet<AviaRefund> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AviaRefundReference operator +(AviaRefundReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.AviaRefunds.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator AviaRefundReference(AviaRefund entity)
		{
			return entity == null ? null : new AviaRefundReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AviaRefundReference OneRef(this IQueryable<AviaRefund> query)
		{
			return query?
				.Select(a => new AviaRefundReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AviaRefundLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<AviaRefund> query, ref IEnumerable<AviaRefundLookup> lookupList);

		public static IEnumerable<AviaRefundLookup> SelectAndOrderByName(IQueryable<AviaRefund> query)
		{
			IEnumerable<AviaRefundLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AviaRefundLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AviaRefundLookup> DefaultLookup(LookupParams<AviaRefund, AviaRefundLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region AviaTicket

	partial class AviaTicket
	{
	//1
		public AviaTicket()
		{
			FareTotal = new Money();
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(AviaTicket);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).AviaTickets;

		internal AviaTicket n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (AviaTicket)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (AviaTicket)value;
		}

		public class Lazy<TValue> : Lazy<AviaTicket, TValue>
		{
			public Lazy(Func<AviaTicket, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<AviaTicket, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<AviaTicket, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static AviaTicket operator +(AviaTicket r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static AviaTicket operator |(AviaTicket r1, AviaTicket r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new AviaTicket
			{
				Departure = e.Property(a => a.Departure).OriginalValue,
				Domestic = e.Property(a => a.Domestic).OriginalValue,
				Interline = e.Property(a => a.Interline).OriginalValue,
				SegmentClasses = e.Property(a => a.SegmentClasses).OriginalValue,
				Endorsement = e.Property(a => a.Endorsement).OriginalValue,
				FareTotal = e.Property(a => a.FareTotal).OriginalValue,
				AirlineIataCode = e.Property(a => a.AirlineIataCode).OriginalValue,
				AirlinePrefixCode = e.Property(a => a.AirlinePrefixCode).OriginalValue,
				AirlineName = e.Property(a => a.AirlineName).OriginalValue,
				Number = e.Property(a => a.Number).OriginalValue,
				ConjunctionNumbers = e.Property(a => a.ConjunctionNumbers).OriginalValue,
				GdsPassportStatus = e.Property(a => a.GdsPassportStatus).OriginalValue,
				GdsPassport = e.Property(a => a.GdsPassport).OriginalValue,
				PaymentForm = e.Property(a => a.PaymentForm).OriginalValue,
				PaymentDetails = e.Property(a => a.PaymentDetails).OriginalValue,
				AirlinePnrCode = e.Property(a => a.AirlinePnrCode).OriginalValue,
				Remarks = e.Property(a => a.Remarks).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(AviaTicket).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<AviaTicket> entity);
		public static void Config(Domain.EntityConfiguration<AviaTicket> entity) => Config_(entity);

	}


	[Localization(typeof(AviaTicket))]
	public class AviaTicketReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public AviaTicketReference() { }

		public AviaTicketReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(AviaTicketReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static AviaTicketReference operator +(AviaTicketReference reference, DbSet<AviaTicket> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static AviaTicketReference operator +(AviaTicketReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.AviaTickets.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator AviaTicketReference(AviaTicket entity)
		{
			return entity == null ? null : new AviaTicketReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static AviaTicketReference OneRef(this IQueryable<AviaTicket> query)
		{
			return query?
				.Select(a => new AviaTicketReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class AviaTicketLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<AviaTicket> query, ref IEnumerable<AviaTicketLookup> lookupList);

		public static IEnumerable<AviaTicketLookup> SelectAndOrderByName(IQueryable<AviaTicket> query)
		{
			IEnumerable<AviaTicketLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new AviaTicketLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<AviaTicketLookup> DefaultLookup(LookupParams<AviaTicket, AviaTicketLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region BankAccount

	partial class BankAccount
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(BankAccount);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).BankAccounts;

		internal BankAccount n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (BankAccount)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (BankAccount)value;
		}

		public class Lazy<TValue> : Lazy<BankAccount, TValue>
		{
			public Lazy(Func<BankAccount, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<BankAccount, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<BankAccount, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static BankAccount operator +(BankAccount r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static BankAccount operator |(BankAccount r1, BankAccount r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new BankAccount
			{
				IsDefault = e.Property(a => a.IsDefault).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Description = e.Property(a => a.Description).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(BankAccount).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<BankAccount> entity);
		public static void Config(Domain.EntityConfiguration<BankAccount> entity) => Config_(entity);

	}


	[Localization(typeof(BankAccount))]
	public class BankAccountReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public BankAccountReference() { }

		public BankAccountReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(BankAccountReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static BankAccountReference operator +(BankAccountReference reference, DbSet<BankAccount> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static BankAccountReference operator +(BankAccountReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.BankAccounts.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator BankAccountReference(BankAccount entity)
		{
			return entity == null ? null : new BankAccountReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static BankAccountReference OneRef(this IQueryable<BankAccount> query)
		{
			return query?
				.Select(a => new BankAccountReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class BankAccountLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<BankAccount> query, ref IEnumerable<BankAccountLookup> lookupList);

		public static IEnumerable<BankAccountLookup> SelectAndOrderByName(IQueryable<BankAccount> query)
		{
			IEnumerable<BankAccountLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new BankAccountLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<BankAccountLookup> DefaultLookup(LookupParams<BankAccount, BankAccountLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region BusDocument

	partial class BusDocument
	{
	//1


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).BusDocuments;

		internal BusDocument n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (BusDocument)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (BusDocument)value;
		}

		public class Lazy<TValue> : Lazy<BusDocument, TValue>
		{
			public Lazy(Func<BusDocument, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<BusDocument, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<BusDocument, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static BusDocument operator +(BusDocument r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static BusDocument operator |(BusDocument r1, BusDocument r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<BusDocument> entity);
		public static void Config(Domain.EntityConfiguration<BusDocument> entity) => Config_(entity);

	}


	[Localization(typeof(BusDocument))]
	public class BusDocumentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }
			
		public BusDocumentReference() { }

		public BusDocumentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(BusDocumentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static BusDocumentReference operator +(BusDocumentReference reference, DbSet<BusDocument> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static BusDocumentReference operator +(BusDocumentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.BusDocuments.Where(a => a.Id == id).Select(a => new { a.Name, a.Type }).FirstOrDefault().Do(a =>
			{
				reference.Name = a.Name;
				reference._Type = a.Type.AsString();
			});

			return reference;
		}

		public static implicit operator BusDocumentReference(BusDocument entity)
		{
			return entity == null ? null : new BusDocumentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static BusDocumentReference OneRef(this IQueryable<BusDocument> query)
		{
			return query?
				.Select(a => new BusDocumentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class BusDocumentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<BusDocument> query, ref IEnumerable<BusDocumentLookup> lookupList);

		public static IEnumerable<BusDocumentLookup> SelectAndOrderByName(IQueryable<BusDocument> query)
		{
			IEnumerable<BusDocumentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new BusDocumentLookup { Id = a.Id, Name = a.Name, _Type = a.Type.ToString() })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<BusDocumentLookup> DefaultLookup(LookupParams<BusDocument, BusDocumentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region BusTicket

	partial class BusTicket
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(BusTicket);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).BusTickets;

		internal BusTicket n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (BusTicket)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (BusTicket)value;
		}

		public class Lazy<TValue> : Lazy<BusTicket, TValue>
		{
			public Lazy(Func<BusTicket, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<BusTicket, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<BusTicket, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static BusTicket operator +(BusTicket r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static BusTicket operator |(BusTicket r1, BusTicket r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new BusTicket
			{
				Number = e.Property(a => a.Number).OriginalValue,
				DeparturePlace = e.Property(a => a.DeparturePlace).OriginalValue,
				DepartureDate = e.Property(a => a.DepartureDate).OriginalValue,
				DepartureTime = e.Property(a => a.DepartureTime).OriginalValue,
				ArrivalPlace = e.Property(a => a.ArrivalPlace).OriginalValue,
				ArrivalDate = e.Property(a => a.ArrivalDate).OriginalValue,
				ArrivalTime = e.Property(a => a.ArrivalTime).OriginalValue,
				SeatNumber = e.Property(a => a.SeatNumber).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(BusTicket).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<BusTicket> entity);
		public static void Config(Domain.EntityConfiguration<BusTicket> entity) => Config_(entity);

	}


	[Localization(typeof(BusTicket))]
	public class BusTicketReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public BusTicketReference() { }

		public BusTicketReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(BusTicketReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static BusTicketReference operator +(BusTicketReference reference, DbSet<BusTicket> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static BusTicketReference operator +(BusTicketReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.BusTickets.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator BusTicketReference(BusTicket entity)
		{
			return entity == null ? null : new BusTicketReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static BusTicketReference OneRef(this IQueryable<BusTicket> query)
		{
			return query?
				.Select(a => new BusTicketReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class BusTicketLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<BusTicket> query, ref IEnumerable<BusTicketLookup> lookupList);

		public static IEnumerable<BusTicketLookup> SelectAndOrderByName(IQueryable<BusTicket> query)
		{
			IEnumerable<BusTicketLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new BusTicketLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<BusTicketLookup> DefaultLookup(LookupParams<BusTicket, BusTicketLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region BusTicketRefund

	partial class BusTicketRefund
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(BusTicketRefund);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).BusTicketRefunds;

		internal BusTicketRefund n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (BusTicketRefund)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (BusTicketRefund)value;
		}

		public class Lazy<TValue> : Lazy<BusTicketRefund, TValue>
		{
			public Lazy(Func<BusTicketRefund, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<BusTicketRefund, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<BusTicketRefund, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static BusTicketRefund operator +(BusTicketRefund r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static BusTicketRefund operator |(BusTicketRefund r1, BusTicketRefund r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new BusTicketRefund
			{
				Number = e.Property(a => a.Number).OriginalValue,
				DeparturePlace = e.Property(a => a.DeparturePlace).OriginalValue,
				DepartureDate = e.Property(a => a.DepartureDate).OriginalValue,
				DepartureTime = e.Property(a => a.DepartureTime).OriginalValue,
				ArrivalPlace = e.Property(a => a.ArrivalPlace).OriginalValue,
				ArrivalDate = e.Property(a => a.ArrivalDate).OriginalValue,
				ArrivalTime = e.Property(a => a.ArrivalTime).OriginalValue,
				SeatNumber = e.Property(a => a.SeatNumber).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(BusTicketRefund).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<BusTicketRefund> entity);
		public static void Config(Domain.EntityConfiguration<BusTicketRefund> entity) => Config_(entity);

	}


	[Localization(typeof(BusTicketRefund))]
	public class BusTicketRefundReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public BusTicketRefundReference() { }

		public BusTicketRefundReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(BusTicketRefundReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static BusTicketRefundReference operator +(BusTicketRefundReference reference, DbSet<BusTicketRefund> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static BusTicketRefundReference operator +(BusTicketRefundReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.BusTicketRefunds.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator BusTicketRefundReference(BusTicketRefund entity)
		{
			return entity == null ? null : new BusTicketRefundReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static BusTicketRefundReference OneRef(this IQueryable<BusTicketRefund> query)
		{
			return query?
				.Select(a => new BusTicketRefundReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class BusTicketRefundLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<BusTicketRefund> query, ref IEnumerable<BusTicketRefundLookup> lookupList);

		public static IEnumerable<BusTicketRefundLookup> SelectAndOrderByName(IQueryable<BusTicketRefund> query)
		{
			IEnumerable<BusTicketRefundLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new BusTicketRefundLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<BusTicketRefundLookup> DefaultLookup(LookupParams<BusTicketRefund, BusTicketRefundLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region CarRental

	partial class CarRental
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(CarRental);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).CarRentals;

		internal CarRental n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (CarRental)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (CarRental)value;
		}

		public class Lazy<TValue> : Lazy<CarRental, TValue>
		{
			public Lazy(Func<CarRental, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<CarRental, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<CarRental, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static CarRental operator +(CarRental r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static CarRental operator |(CarRental r1, CarRental r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new CarRental
			{
				CarBrand = e.Property(a => a.CarBrand).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(CarRental).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<CarRental> entity);
		public static void Config(Domain.EntityConfiguration<CarRental> entity) => Config_(entity);

	}


	[Localization(typeof(CarRental))]
	public class CarRentalReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public CarRentalReference() { }

		public CarRentalReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(CarRentalReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static CarRentalReference operator +(CarRentalReference reference, DbSet<CarRental> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static CarRentalReference operator +(CarRentalReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.CarRentals.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator CarRentalReference(CarRental entity)
		{
			return entity == null ? null : new CarRentalReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static CarRentalReference OneRef(this IQueryable<CarRental> query)
		{
			return query?
				.Select(a => new CarRentalReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class CarRentalLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<CarRental> query, ref IEnumerable<CarRentalLookup> lookupList);

		public static IEnumerable<CarRentalLookup> SelectAndOrderByName(IQueryable<CarRental> query)
		{
			IEnumerable<CarRentalLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new CarRentalLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<CarRentalLookup> DefaultLookup(LookupParams<CarRental, CarRentalLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region CashInOrderPayment

	partial class CashInOrderPayment
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(CashInOrderPayment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).CashInOrderPayments;

		internal CashInOrderPayment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (CashInOrderPayment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (CashInOrderPayment)value;
		}

		public class Lazy<TValue> : Lazy<CashInOrderPayment, TValue>
		{
			public Lazy(Func<CashInOrderPayment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<CashInOrderPayment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<CashInOrderPayment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static CashInOrderPayment operator +(CashInOrderPayment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static CashInOrderPayment operator |(CashInOrderPayment r1, CashInOrderPayment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new CashInOrderPayment
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				DocumentNumber = e.Property(a => a.DocumentNumber).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ReceivedFrom = e.Property(a => a.ReceivedFrom).OriginalValue,
				PostedOn = e.Property(a => a.PostedOn).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				PrintedDocument = e.Property(a => a.PrintedDocument).OriginalValue,
				PayerId = e.Property(a => a.PayerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				InvoiceId = e.Property(a => a.InvoiceId).OriginalValue,
				AssignedToId = e.Property(a => a.AssignedToId).OriginalValue,
				RegisteredById = e.Property(a => a.RegisteredById).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				PaymentSystemId = e.Property(a => a.PaymentSystemId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(CashInOrderPayment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<CashInOrderPayment> entity);
		public static void Config(Domain.EntityConfiguration<CashInOrderPayment> entity) => Config_(entity);

	}


	[Localization(typeof(CashInOrderPayment))]
	public class CashInOrderPaymentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public CashInOrderPaymentReference() { }

		public CashInOrderPaymentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(CashInOrderPaymentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static CashInOrderPaymentReference operator +(CashInOrderPaymentReference reference, DbSet<CashInOrderPayment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static CashInOrderPaymentReference operator +(CashInOrderPaymentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.CashInOrderPayments.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator CashInOrderPaymentReference(CashInOrderPayment entity)
		{
			return entity == null ? null : new CashInOrderPaymentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static CashInOrderPaymentReference OneRef(this IQueryable<CashInOrderPayment> query)
		{
			return query?
				.Select(a => new CashInOrderPaymentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class CashInOrderPaymentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<CashInOrderPayment> query, ref IEnumerable<CashInOrderPaymentLookup> lookupList);

		public static IEnumerable<CashInOrderPaymentLookup> SelectAndOrderByName(IQueryable<CashInOrderPayment> query)
		{
			IEnumerable<CashInOrderPaymentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new CashInOrderPaymentLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<CashInOrderPaymentLookup> DefaultLookup(LookupParams<CashInOrderPayment, CashInOrderPaymentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region CashOutOrderPayment

	partial class CashOutOrderPayment
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(CashOutOrderPayment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).CashOutOrderPayments;

		internal CashOutOrderPayment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (CashOutOrderPayment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (CashOutOrderPayment)value;
		}

		public class Lazy<TValue> : Lazy<CashOutOrderPayment, TValue>
		{
			public Lazy(Func<CashOutOrderPayment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<CashOutOrderPayment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<CashOutOrderPayment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static CashOutOrderPayment operator +(CashOutOrderPayment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static CashOutOrderPayment operator |(CashOutOrderPayment r1, CashOutOrderPayment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new CashOutOrderPayment
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				DocumentNumber = e.Property(a => a.DocumentNumber).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ReceivedFrom = e.Property(a => a.ReceivedFrom).OriginalValue,
				PostedOn = e.Property(a => a.PostedOn).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				PrintedDocument = e.Property(a => a.PrintedDocument).OriginalValue,
				PayerId = e.Property(a => a.PayerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				InvoiceId = e.Property(a => a.InvoiceId).OriginalValue,
				AssignedToId = e.Property(a => a.AssignedToId).OriginalValue,
				RegisteredById = e.Property(a => a.RegisteredById).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				PaymentSystemId = e.Property(a => a.PaymentSystemId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(CashOutOrderPayment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<CashOutOrderPayment> entity);
		public static void Config(Domain.EntityConfiguration<CashOutOrderPayment> entity) => Config_(entity);

	}


	[Localization(typeof(CashOutOrderPayment))]
	public class CashOutOrderPaymentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public CashOutOrderPaymentReference() { }

		public CashOutOrderPaymentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(CashOutOrderPaymentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static CashOutOrderPaymentReference operator +(CashOutOrderPaymentReference reference, DbSet<CashOutOrderPayment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static CashOutOrderPaymentReference operator +(CashOutOrderPaymentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.CashOutOrderPayments.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator CashOutOrderPaymentReference(CashOutOrderPayment entity)
		{
			return entity == null ? null : new CashOutOrderPaymentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static CashOutOrderPaymentReference OneRef(this IQueryable<CashOutOrderPayment> query)
		{
			return query?
				.Select(a => new CashOutOrderPaymentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class CashOutOrderPaymentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<CashOutOrderPayment> query, ref IEnumerable<CashOutOrderPaymentLookup> lookupList);

		public static IEnumerable<CashOutOrderPaymentLookup> SelectAndOrderByName(IQueryable<CashOutOrderPayment> query)
		{
			IEnumerable<CashOutOrderPaymentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new CashOutOrderPaymentLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<CashOutOrderPaymentLookup> DefaultLookup(LookupParams<CashOutOrderPayment, CashOutOrderPaymentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region CateringType

	partial class CateringType
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(CateringType);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).CateringTypes;

		internal CateringType n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (CateringType)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (CateringType)value;
		}

		public class Lazy<TValue> : Lazy<CateringType, TValue>
		{
			public Lazy(Func<CateringType, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<CateringType, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<CateringType, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static CateringType operator +(CateringType r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static CateringType operator |(CateringType r1, CateringType r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new CateringType
			{
				Description = e.Property(a => a.Description).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(CateringType).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<CateringType> entity);
		public static void Config(Domain.EntityConfiguration<CateringType> entity) => Config_(entity);

	}


	[Localization(typeof(CateringType))]
	public class CateringTypeReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public CateringTypeReference() { }

		public CateringTypeReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(CateringTypeReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static CateringTypeReference operator +(CateringTypeReference reference, DbSet<CateringType> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static CateringTypeReference operator +(CateringTypeReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.CateringTypes.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator CateringTypeReference(CateringType entity)
		{
			return entity == null ? null : new CateringTypeReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static CateringTypeReference OneRef(this IQueryable<CateringType> query)
		{
			return query?
				.Select(a => new CateringTypeReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class CateringTypeLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<CateringType> query, ref IEnumerable<CateringTypeLookup> lookupList);

		public static IEnumerable<CateringTypeLookup> SelectAndOrderByName(IQueryable<CateringType> query)
		{
			IEnumerable<CateringTypeLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new CateringTypeLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<CateringTypeLookup> DefaultLookup(LookupParams<CateringType, CateringTypeLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region CheckPayment

	partial class CheckPayment
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(CheckPayment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).CheckPayments;

		internal CheckPayment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (CheckPayment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (CheckPayment)value;
		}

		public class Lazy<TValue> : Lazy<CheckPayment, TValue>
		{
			public Lazy(Func<CheckPayment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<CheckPayment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<CheckPayment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static CheckPayment operator +(CheckPayment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static CheckPayment operator |(CheckPayment r1, CheckPayment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new CheckPayment
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				DocumentNumber = e.Property(a => a.DocumentNumber).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ReceivedFrom = e.Property(a => a.ReceivedFrom).OriginalValue,
				PostedOn = e.Property(a => a.PostedOn).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				PrintedDocument = e.Property(a => a.PrintedDocument).OriginalValue,
				PayerId = e.Property(a => a.PayerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				InvoiceId = e.Property(a => a.InvoiceId).OriginalValue,
				AssignedToId = e.Property(a => a.AssignedToId).OriginalValue,
				RegisteredById = e.Property(a => a.RegisteredById).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				PaymentSystemId = e.Property(a => a.PaymentSystemId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(CheckPayment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<CheckPayment> entity);
		public static void Config(Domain.EntityConfiguration<CheckPayment> entity) => Config_(entity);

	}


	[Localization(typeof(CheckPayment))]
	public class CheckPaymentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public CheckPaymentReference() { }

		public CheckPaymentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(CheckPaymentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static CheckPaymentReference operator +(CheckPaymentReference reference, DbSet<CheckPayment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static CheckPaymentReference operator +(CheckPaymentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.CheckPayments.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator CheckPaymentReference(CheckPayment entity)
		{
			return entity == null ? null : new CheckPaymentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static CheckPaymentReference OneRef(this IQueryable<CheckPayment> query)
		{
			return query?
				.Select(a => new CheckPaymentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class CheckPaymentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<CheckPayment> query, ref IEnumerable<CheckPaymentLookup> lookupList);

		public static IEnumerable<CheckPaymentLookup> SelectAndOrderByName(IQueryable<CheckPayment> query)
		{
			IEnumerable<CheckPaymentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new CheckPaymentLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<CheckPaymentLookup> DefaultLookup(LookupParams<CheckPayment, CheckPaymentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Consignment

	partial class Consignment
	{
	//1
		public Consignment()
		{
			GrandTotal = new Money();
			Vat = new Money();
			Discount = new Money();
		}

		[ForeignKey("Supplier")]
		public string SupplierId
		{ 
			get { return _SupplierId; }
			set 
			{
				if (_SupplierId == value) return;
				_SupplierId = value;
				_Supplier = null;
			}
		}
		private string _SupplierId;

		public virtual Party Supplier
		{
			get 
			{ 
				return _Supplier ?? (_Supplier = db?.Parties.ById(_SupplierId)); 
			}
			set
			{
				_Supplier = value;
				_SupplierId = value?.Id;
			}
		}

		[ForeignKey("Acquirer")]
		public string AcquirerId
		{ 
			get { return _AcquirerId; }
			set 
			{
				if (_AcquirerId == value) return;
				_AcquirerId = value;
				_Acquirer = null;
			}
		}
		private string _AcquirerId;

		public virtual Party Acquirer
		{
			get 
			{ 
				return _Acquirer ?? (_Acquirer = db?.Parties.ById(_AcquirerId)); 
			}
			set
			{
				_Acquirer = value;
				_AcquirerId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Consignment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Consignments;

		internal Consignment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Consignment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Consignment)value;
		}

		public class Lazy<TValue> : Lazy<Consignment, TValue>
		{
			public Lazy(Func<Consignment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Consignment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Consignment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Consignment operator +(Consignment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Consignment operator |(Consignment r1, Consignment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Consignment
			{
				Number = e.Property(a => a.Number).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				TotalSupplied = e.Property(a => a.TotalSupplied).OriginalValue,
				SupplierId = e.Property(a => a.SupplierId).OriginalValue,
				AcquirerId = e.Property(a => a.AcquirerId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Consignment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Consignment> entity);
		public static void Config(Domain.EntityConfiguration<Consignment> entity) => Config_(entity);

	}


	[Localization(typeof(Consignment))]
	public class ConsignmentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public ConsignmentReference() { }

		public ConsignmentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(ConsignmentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static ConsignmentReference operator +(ConsignmentReference reference, DbSet<Consignment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static ConsignmentReference operator +(ConsignmentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.Consignments.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator ConsignmentReference(Consignment entity)
		{
			return entity == null ? null : new ConsignmentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static ConsignmentReference OneRef(this IQueryable<Consignment> query)
		{
			return query?
				.Select(a => new ConsignmentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class ConsignmentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<Consignment> query, ref IEnumerable<ConsignmentLookup> lookupList);

		public static IEnumerable<ConsignmentLookup> SelectAndOrderByName(IQueryable<Consignment> query)
		{
			IEnumerable<ConsignmentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new ConsignmentLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<ConsignmentLookup> DefaultLookup(LookupParams<Consignment, ConsignmentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Country

	partial class Country
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Country);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Countries;

		internal Country n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Country)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Country)value;
		}

		public class Lazy<TValue> : Lazy<Country, TValue>
		{
			public Lazy(Func<Country, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Country, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Country, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Country operator +(Country r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Country operator |(Country r1, Country r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Country
			{
				TwoCharCode = e.Property(a => a.TwoCharCode).OriginalValue,
				ThreeCharCode = e.Property(a => a.ThreeCharCode).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Country).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Country> entity);
		public static void Config(Domain.EntityConfiguration<Country> entity) => Config_(entity);

	}


	[Localization(typeof(Country))]
	public class CountryReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public CountryReference() { }

		public CountryReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(CountryReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static CountryReference operator +(CountryReference reference, DbSet<Country> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static CountryReference operator +(CountryReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Countries.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator CountryReference(Country entity)
		{
			return entity == null ? null : new CountryReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static CountryReference OneRef(this IQueryable<Country> query)
		{
			return query?
				.Select(a => new CountryReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class CountryLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Country> query, ref IEnumerable<CountryLookup> lookupList);

		public static IEnumerable<CountryLookup> SelectAndOrderByName(IQueryable<Country> query)
		{
			IEnumerable<CountryLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new CountryLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<CountryLookup> DefaultLookup(LookupParams<Country, CountryLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region CurrencyDailyRate

	partial class CurrencyDailyRate
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(CurrencyDailyRate);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).CurrencyDailyRates;

		internal CurrencyDailyRate n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (CurrencyDailyRate)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (CurrencyDailyRate)value;
		}

		public class Lazy<TValue> : Lazy<CurrencyDailyRate, TValue>
		{
			public Lazy(Func<CurrencyDailyRate, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<CurrencyDailyRate, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<CurrencyDailyRate, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static CurrencyDailyRate operator +(CurrencyDailyRate r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static CurrencyDailyRate operator |(CurrencyDailyRate r1, CurrencyDailyRate r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new CurrencyDailyRate
			{
				Date = e.Property(a => a.Date).OriginalValue,
				UAH_EUR = e.Property(a => a.UAH_EUR).OriginalValue,
				UAH_RUB = e.Property(a => a.UAH_RUB).OriginalValue,
				UAH_USD = e.Property(a => a.UAH_USD).OriginalValue,
				RUB_EUR = e.Property(a => a.RUB_EUR).OriginalValue,
				RUB_USD = e.Property(a => a.RUB_USD).OriginalValue,
				EUR_USD = e.Property(a => a.EUR_USD).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(CurrencyDailyRate).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<CurrencyDailyRate> entity);
		public static void Config(Domain.EntityConfiguration<CurrencyDailyRate> entity) => Config_(entity);

	}

	#endregion
	

	#region Department

	partial class Department
	{
	//1

		[ForeignKey("Organization")]
		public string OrganizationId
		{ 
			get { return _OrganizationId; }
			set 
			{
				if (_OrganizationId == value) return;
				_OrganizationId = value;
				_Organization = null;
			}
		}
		private string _OrganizationId;

		public virtual Organization Organization
		{
			get 
			{ 
				return _Organization ?? (_Organization = db?.Organizations.ById(_OrganizationId)); 
			}
			set
			{
				_Organization = value;
				_OrganizationId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Department);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Departments;

		internal Department n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Department)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Department)value;
		}

		public class Lazy<TValue> : Lazy<Department, TValue>
		{
			public Lazy(Func<Department, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Department, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Department, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Department operator +(Department r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Department operator |(Department r1, Department r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Department
			{
				OrganizationId = e.Property(a => a.OrganizationId).OriginalValue,
				LegalName = e.Property(a => a.LegalName).OriginalValue,
				Code = e.Property(a => a.Code).OriginalValue,
				Phone1 = e.Property(a => a.Phone1).OriginalValue,
				Phone2 = e.Property(a => a.Phone2).OriginalValue,
				Fax = e.Property(a => a.Fax).OriginalValue,
				Email1 = e.Property(a => a.Email1).OriginalValue,
				Email2 = e.Property(a => a.Email2).OriginalValue,
				WebAddress = e.Property(a => a.WebAddress).OriginalValue,
				IsCustomer = e.Property(a => a.IsCustomer).OriginalValue,
				IsSupplier = e.Property(a => a.IsSupplier).OriginalValue,
				Details = e.Property(a => a.Details).OriginalValue,
				LegalAddress = e.Property(a => a.LegalAddress).OriginalValue,
				ActualAddress = e.Property(a => a.ActualAddress).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				ReportsToId = e.Property(a => a.ReportsToId).OriginalValue,
				DefaultBankAccountId = e.Property(a => a.DefaultBankAccountId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Department).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Department> entity);
		public static void Config(Domain.EntityConfiguration<Department> entity) => Config_(entity);

	}


	[Localization(typeof(Department))]
	public class DepartmentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public DepartmentReference() { }

		public DepartmentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(DepartmentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static DepartmentReference operator +(DepartmentReference reference, DbSet<Department> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static DepartmentReference operator +(DepartmentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Departments.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator DepartmentReference(Department entity)
		{
			return entity == null ? null : new DepartmentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static DepartmentReference OneRef(this IQueryable<Department> query)
		{
			return query?
				.Select(a => new DepartmentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class DepartmentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Department> query, ref IEnumerable<DepartmentLookup> lookupList);

		public static IEnumerable<DepartmentLookup> SelectAndOrderByName(IQueryable<Department> query)
		{
			IEnumerable<DepartmentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new DepartmentLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<DepartmentLookup> DefaultLookup(LookupParams<Department, DepartmentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region DocumentAccess

	partial class DocumentAccess
	{
	//1

		[ForeignKey("Person")]
		public string PersonId
		{ 
			get { return _PersonId; }
			set 
			{
				if (_PersonId == value) return;
				_PersonId = value;
				_Person = null;
			}
		}
		private string _PersonId;

		public virtual Person Person
		{
			get 
			{ 
				return _Person ?? (_Person = db?.Persons.ById(_PersonId)); 
			}
			set
			{
				_Person = value;
				_PersonId = value?.Id;
			}
		}

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Party Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Parties.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(DocumentAccess);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).DocumentAccesses;

		internal DocumentAccess n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (DocumentAccess)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (DocumentAccess)value;
		}

		public class Lazy<TValue> : Lazy<DocumentAccess, TValue>
		{
			public Lazy(Func<DocumentAccess, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<DocumentAccess, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<DocumentAccess, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static DocumentAccess operator +(DocumentAccess r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static DocumentAccess operator |(DocumentAccess r1, DocumentAccess r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new DocumentAccess
			{
				FullDocumentControl = e.Property(a => a.FullDocumentControl).OriginalValue,
				PersonId = e.Property(a => a.PersonId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(DocumentAccess).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<DocumentAccess> entity);
		public static void Config(Domain.EntityConfiguration<DocumentAccess> entity) => Config_(entity);

	}

	#endregion
	

	#region DocumentOwner

	partial class DocumentOwner
	{
	//1

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Party Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Parties.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(DocumentOwner);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).DocumentOwners;

		internal DocumentOwner n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (DocumentOwner)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (DocumentOwner)value;
		}

		public class Lazy<TValue> : Lazy<DocumentOwner, TValue>
		{
			public Lazy(Func<DocumentOwner, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<DocumentOwner, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<DocumentOwner, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static DocumentOwner operator +(DocumentOwner r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static DocumentOwner operator |(DocumentOwner r1, DocumentOwner r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new DocumentOwner
			{
				IsActive = e.Property(a => a.IsActive).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(DocumentOwner).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<DocumentOwner> entity);
		public static void Config(Domain.EntityConfiguration<DocumentOwner> entity) => Config_(entity);

	}

	#endregion
	

	#region ElectronicPayment

	partial class ElectronicPayment
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(ElectronicPayment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).ElectronicPayments;

		internal ElectronicPayment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (ElectronicPayment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (ElectronicPayment)value;
		}

		public class Lazy<TValue> : Lazy<ElectronicPayment, TValue>
		{
			public Lazy(Func<ElectronicPayment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<ElectronicPayment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<ElectronicPayment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static ElectronicPayment operator +(ElectronicPayment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static ElectronicPayment operator |(ElectronicPayment r1, ElectronicPayment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new ElectronicPayment
			{
				AuthorizationCode = e.Property(a => a.AuthorizationCode).OriginalValue,
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				DocumentNumber = e.Property(a => a.DocumentNumber).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ReceivedFrom = e.Property(a => a.ReceivedFrom).OriginalValue,
				PostedOn = e.Property(a => a.PostedOn).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				PrintedDocument = e.Property(a => a.PrintedDocument).OriginalValue,
				PayerId = e.Property(a => a.PayerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				InvoiceId = e.Property(a => a.InvoiceId).OriginalValue,
				AssignedToId = e.Property(a => a.AssignedToId).OriginalValue,
				RegisteredById = e.Property(a => a.RegisteredById).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				PaymentSystemId = e.Property(a => a.PaymentSystemId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(ElectronicPayment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<ElectronicPayment> entity);
		public static void Config(Domain.EntityConfiguration<ElectronicPayment> entity) => Config_(entity);

	}


	[Localization(typeof(ElectronicPayment))]
	public class ElectronicPaymentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public ElectronicPaymentReference() { }

		public ElectronicPaymentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(ElectronicPaymentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static ElectronicPaymentReference operator +(ElectronicPaymentReference reference, DbSet<ElectronicPayment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static ElectronicPaymentReference operator +(ElectronicPaymentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.ElectronicPayments.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator ElectronicPaymentReference(ElectronicPayment entity)
		{
			return entity == null ? null : new ElectronicPaymentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static ElectronicPaymentReference OneRef(this IQueryable<ElectronicPayment> query)
		{
			return query?
				.Select(a => new ElectronicPaymentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class ElectronicPaymentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<ElectronicPayment> query, ref IEnumerable<ElectronicPaymentLookup> lookupList);

		public static IEnumerable<ElectronicPaymentLookup> SelectAndOrderByName(IQueryable<ElectronicPayment> query)
		{
			IEnumerable<ElectronicPaymentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new ElectronicPaymentLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<ElectronicPaymentLookup> DefaultLookup(LookupParams<ElectronicPayment, ElectronicPaymentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Excursion

	partial class Excursion
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Excursion);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Excursions;

		internal Excursion n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Excursion)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Excursion)value;
		}

		public class Lazy<TValue> : Lazy<Excursion, TValue>
		{
			public Lazy(Func<Excursion, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Excursion, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Excursion, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Excursion operator +(Excursion r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Excursion operator |(Excursion r1, Excursion r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Excursion
			{
				TourName = e.Property(a => a.TourName).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Excursion).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Excursion> entity);
		public static void Config(Domain.EntityConfiguration<Excursion> entity) => Config_(entity);

	}


	[Localization(typeof(Excursion))]
	public class ExcursionReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public ExcursionReference() { }

		public ExcursionReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(ExcursionReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static ExcursionReference operator +(ExcursionReference reference, DbSet<Excursion> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static ExcursionReference operator +(ExcursionReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Excursions.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator ExcursionReference(Excursion entity)
		{
			return entity == null ? null : new ExcursionReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static ExcursionReference OneRef(this IQueryable<Excursion> query)
		{
			return query?
				.Select(a => new ExcursionReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class ExcursionLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Excursion> query, ref IEnumerable<ExcursionLookup> lookupList);

		public static IEnumerable<ExcursionLookup> SelectAndOrderByName(IQueryable<Excursion> query)
		{
			IEnumerable<ExcursionLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new ExcursionLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<ExcursionLookup> DefaultLookup(LookupParams<Excursion, ExcursionLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region File

	partial class File
	{
	//1

		[ForeignKey("Party")]
		public string PartyId
		{ 
			get { return _PartyId; }
			set 
			{
				if (_PartyId == value) return;
				_PartyId = value;
				_Party = null;
			}
		}
		private string _PartyId;

		public virtual Party Party
		{
			get 
			{ 
				return _Party ?? (_Party = db?.Parties.ById(_PartyId)); 
			}
			set
			{
				_Party = value;
				_PartyId = value?.Id;
			}
		}

		[ForeignKey("UploadedBy")]
		public string UploadedById
		{ 
			get { return _UploadedById; }
			set 
			{
				if (_UploadedById == value) return;
				_UploadedById = value;
				_UploadedBy = null;
			}
		}
		private string _UploadedById;

		public virtual Person UploadedBy
		{
			get 
			{ 
				return _UploadedBy ?? (_UploadedBy = db?.Persons.ById(_UploadedById)); 
			}
			set
			{
				_UploadedBy = value;
				_UploadedById = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(File);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Files;

		internal File n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (File)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (File)value;
		}

		public class Lazy<TValue> : Lazy<File, TValue>
		{
			public Lazy(Func<File, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<File, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<File, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static File operator +(File r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static File operator |(File r1, File r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new File
			{
				FileName = e.Property(a => a.FileName).OriginalValue,
				TimeStamp = e.Property(a => a.TimeStamp).OriginalValue,
				Content = e.Property(a => a.Content).OriginalValue,
				PartyId = e.Property(a => a.PartyId).OriginalValue,
				UploadedById = e.Property(a => a.UploadedById).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(File).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<File> entity);
		public static void Config(Domain.EntityConfiguration<File> entity) => Config_(entity);

	}

	#endregion
	

	#region FlightSegment

	partial class FlightSegment
	{
	//1
		public FlightSegment()
		{
			Amount = new Money();
			CouponAmount = new Money();
		}

		[ForeignKey("Ticket")]
		public string TicketId
		{ 
			get { return _TicketId; }
			set 
			{
				if (_TicketId == value) return;
				_TicketId = value;
				_Ticket = null;
			}
		}
		private string _TicketId;

		public virtual AviaTicket Ticket
		{
			get 
			{ 
				return _Ticket ?? (_Ticket = db?.AviaTickets.ById(_TicketId)); 
			}
			set
			{
				_Ticket = value;
				_TicketId = value?.Id;
			}
		}

		[ForeignKey("FromAirport")]
		public string FromAirportId
		{ 
			get { return _FromAirportId; }
			set 
			{
				if (_FromAirportId == value) return;
				_FromAirportId = value;
				_FromAirport = null;
			}
		}
		private string _FromAirportId;

		public virtual Airport FromAirport
		{
			get 
			{ 
				return _FromAirport ?? (_FromAirport = db?.Airports.ById(_FromAirportId)); 
			}
			set
			{
				_FromAirport = value;
				_FromAirportId = value?.Id;
			}
		}

		[ForeignKey("ToAirport")]
		public string ToAirportId
		{ 
			get { return _ToAirportId; }
			set 
			{
				if (_ToAirportId == value) return;
				_ToAirportId = value;
				_ToAirport = null;
			}
		}
		private string _ToAirportId;

		public virtual Airport ToAirport
		{
			get 
			{ 
				return _ToAirport ?? (_ToAirport = db?.Airports.ById(_ToAirportId)); 
			}
			set
			{
				_ToAirport = value;
				_ToAirportId = value?.Id;
			}
		}

		[ForeignKey("Carrier")]
		public string CarrierId
		{ 
			get { return _CarrierId; }
			set 
			{
				if (_CarrierId == value) return;
				_CarrierId = value;
				_Carrier = null;
			}
		}
		private string _CarrierId;

		public virtual Organization Carrier
		{
			get 
			{ 
				return _Carrier ?? (_Carrier = db?.Organizations.ById(_CarrierId)); 
			}
			set
			{
				_Carrier = value;
				_CarrierId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(FlightSegment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).FlightSegments;

		internal FlightSegment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (FlightSegment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (FlightSegment)value;
		}

		public class Lazy<TValue> : Lazy<FlightSegment, TValue>
		{
			public Lazy(Func<FlightSegment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<FlightSegment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<FlightSegment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static FlightSegment operator +(FlightSegment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static FlightSegment operator |(FlightSegment r1, FlightSegment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new FlightSegment
			{
				Position = e.Property(a => a.Position).OriginalValue,
				Type = e.Property(a => a.Type).OriginalValue,
				FromAirportCode = e.Property(a => a.FromAirportCode).OriginalValue,
				FromAirportName = e.Property(a => a.FromAirportName).OriginalValue,
				ToAirportCode = e.Property(a => a.ToAirportCode).OriginalValue,
				ToAirportName = e.Property(a => a.ToAirportName).OriginalValue,
				CarrierIataCode = e.Property(a => a.CarrierIataCode).OriginalValue,
				CarrierPrefixCode = e.Property(a => a.CarrierPrefixCode).OriginalValue,
				CarrierName = e.Property(a => a.CarrierName).OriginalValue,
				FlightNumber = e.Property(a => a.FlightNumber).OriginalValue,
				ServiceClassCode = e.Property(a => a.ServiceClassCode).OriginalValue,
				ServiceClass = e.Property(a => a.ServiceClass).OriginalValue,
				DepartureTime = e.Property(a => a.DepartureTime).OriginalValue,
				ArrivalTime = e.Property(a => a.ArrivalTime).OriginalValue,
				MealCodes = e.Property(a => a.MealCodes).OriginalValue,
				MealTypes = e.Property(a => a.MealTypes).OriginalValue,
				NumberOfStops = e.Property(a => a.NumberOfStops).OriginalValue,
				Luggage = e.Property(a => a.Luggage).OriginalValue,
				CheckInTerminal = e.Property(a => a.CheckInTerminal).OriginalValue,
				CheckInTime = e.Property(a => a.CheckInTime).OriginalValue,
				Duration = e.Property(a => a.Duration).OriginalValue,
				ArrivalTerminal = e.Property(a => a.ArrivalTerminal).OriginalValue,
				Seat = e.Property(a => a.Seat).OriginalValue,
				FareBasis = e.Property(a => a.FareBasis).OriginalValue,
				Stopover = e.Property(a => a.Stopover).OriginalValue,
				Surcharges = e.Property(a => a.Surcharges).OriginalValue,
				IsInclusive = e.Property(a => a.IsInclusive).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				StopoverOrTransferCharge = e.Property(a => a.StopoverOrTransferCharge).OriginalValue,
				IsSideTrip = e.Property(a => a.IsSideTrip).OriginalValue,
				Distance = e.Property(a => a.Distance).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				CouponAmount = e.Property(a => a.CouponAmount).OriginalValue,
				TicketId = e.Property(a => a.TicketId).OriginalValue,
				FromAirportId = e.Property(a => a.FromAirportId).OriginalValue,
				ToAirportId = e.Property(a => a.ToAirportId).OriginalValue,
				CarrierId = e.Property(a => a.CarrierId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(FlightSegment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<FlightSegment> entity);
		public static void Config(Domain.EntityConfiguration<FlightSegment> entity) => Config_(entity);

	}


	[Localization(typeof(FlightSegment))]
	public class FlightSegmentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public FlightSegmentReference() { }

		public FlightSegmentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(FlightSegmentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static FlightSegmentReference operator +(FlightSegmentReference reference, DbSet<FlightSegment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static FlightSegmentReference operator +(FlightSegmentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.FlightSegments.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator FlightSegmentReference(FlightSegment entity)
		{
			return entity == null ? null : new FlightSegmentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static FlightSegmentReference OneRef(this IQueryable<FlightSegment> query)
		{
			return query?
				.Select(a => new FlightSegmentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class FlightSegmentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<FlightSegment> query, ref IEnumerable<FlightSegmentLookup> lookupList);

		public static IEnumerable<FlightSegmentLookup> SelectAndOrderByName(IQueryable<FlightSegment> query)
		{
			IEnumerable<FlightSegmentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new FlightSegmentLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<FlightSegmentLookup> DefaultLookup(LookupParams<FlightSegment, FlightSegmentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region GdsAgent

	partial class GdsAgent
	{
	//1

		[ForeignKey("Person")]
		public string PersonId
		{ 
			get { return _PersonId; }
			set 
			{
				if (_PersonId == value) return;
				_PersonId = value;
				_Person = null;
			}
		}
		private string _PersonId;

		public virtual Person Person
		{
			get 
			{ 
				return _Person ?? (_Person = db?.Persons.ById(_PersonId)); 
			}
			set
			{
				_Person = value;
				_PersonId = value?.Id;
			}
		}

		[ForeignKey("Office")]
		public string OfficeId
		{ 
			get { return _OfficeId; }
			set 
			{
				if (_OfficeId == value) return;
				_OfficeId = value;
				_Office = null;
			}
		}
		private string _OfficeId;

		public virtual Party Office
		{
			get 
			{ 
				return _Office ?? (_Office = db?.Parties.ById(_OfficeId)); 
			}
			set
			{
				_Office = value;
				_OfficeId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(GdsAgent);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).GdsAgents;

		internal GdsAgent n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (GdsAgent)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (GdsAgent)value;
		}

		public class Lazy<TValue> : Lazy<GdsAgent, TValue>
		{
			public Lazy(Func<GdsAgent, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<GdsAgent, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<GdsAgent, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static GdsAgent operator +(GdsAgent r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static GdsAgent operator |(GdsAgent r1, GdsAgent r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new GdsAgent
			{
				Origin = e.Property(a => a.Origin).OriginalValue,
				Code = e.Property(a => a.Code).OriginalValue,
				OfficeCode = e.Property(a => a.OfficeCode).OriginalValue,
				PersonId = e.Property(a => a.PersonId).OriginalValue,
				OfficeId = e.Property(a => a.OfficeId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(GdsAgent).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<GdsAgent> entity);
		public static void Config(Domain.EntityConfiguration<GdsAgent> entity) => Config_(entity);

	}


	[Localization(typeof(GdsAgent))]
	public class GdsAgentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public GdsAgentReference() { }

		public GdsAgentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(GdsAgentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static GdsAgentReference operator +(GdsAgentReference reference, DbSet<GdsAgent> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static GdsAgentReference operator +(GdsAgentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.GdsAgents.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator GdsAgentReference(GdsAgent entity)
		{
			return entity == null ? null : new GdsAgentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static GdsAgentReference OneRef(this IQueryable<GdsAgent> query)
		{
			return query?
				.Select(a => new GdsAgentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class GdsAgentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<GdsAgent> query, ref IEnumerable<GdsAgentLookup> lookupList);

		public static IEnumerable<GdsAgentLookup> SelectAndOrderByName(IQueryable<GdsAgent> query)
		{
			IEnumerable<GdsAgentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new GdsAgentLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<GdsAgentLookup> DefaultLookup(LookupParams<GdsAgent, GdsAgentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region GdsFile

	partial class GdsFile
	{
	//1


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).GdsFiles;

		internal GdsFile n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (GdsFile)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (GdsFile)value;
		}

		public class Lazy<TValue> : Lazy<GdsFile, TValue>
		{
			public Lazy(Func<GdsFile, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<GdsFile, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<GdsFile, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static GdsFile operator +(GdsFile r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static GdsFile operator |(GdsFile r1, GdsFile r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<GdsFile> entity);
		public static void Config(Domain.EntityConfiguration<GdsFile> entity) => Config_(entity);

	}


	[Localization(typeof(GdsFile))]
	public class GdsFileReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public GdsFileReference() { }

		public GdsFileReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(GdsFileReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static GdsFileReference operator +(GdsFileReference reference, DbSet<GdsFile> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static GdsFileReference operator +(GdsFileReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.GdsFiles.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator GdsFileReference(GdsFile entity)
		{
			return entity == null ? null : new GdsFileReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static GdsFileReference OneRef(this IQueryable<GdsFile> query)
		{
			return query?
				.Select(a => new GdsFileReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class GdsFileLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<GdsFile> query, ref IEnumerable<GdsFileLookup> lookupList);

		public static IEnumerable<GdsFileLookup> SelectAndOrderByName(IQueryable<GdsFile> query)
		{
			IEnumerable<GdsFileLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new GdsFileLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<GdsFileLookup> DefaultLookup(LookupParams<GdsFile, GdsFileLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region GenericProduct

	partial class GenericProduct
	{
	//1

		[ForeignKey("GenericType")]
		public string GenericTypeId
		{ 
			get { return _GenericTypeId; }
			set 
			{
				if (_GenericTypeId == value) return;
				_GenericTypeId = value;
				_GenericType = null;
			}
		}
		private string _GenericTypeId;

		public virtual GenericProductType GenericType
		{
			get 
			{ 
				return _GenericType ?? (_GenericType = db?.GenericProductTypes.ById(_GenericTypeId)); 
			}
			set
			{
				_GenericType = value;
				_GenericTypeId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(GenericProduct);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).GenericProducts;

		internal GenericProduct n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (GenericProduct)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (GenericProduct)value;
		}

		public class Lazy<TValue> : Lazy<GenericProduct, TValue>
		{
			public Lazy(Func<GenericProduct, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<GenericProduct, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<GenericProduct, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static GenericProduct operator +(GenericProduct r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static GenericProduct operator |(GenericProduct r1, GenericProduct r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new GenericProduct
			{
				Number = e.Property(a => a.Number).OriginalValue,
				GenericTypeId = e.Property(a => a.GenericTypeId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(GenericProduct).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<GenericProduct> entity);
		public static void Config(Domain.EntityConfiguration<GenericProduct> entity) => Config_(entity);

	}


	[Localization(typeof(GenericProduct))]
	public class GenericProductReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public GenericProductReference() { }

		public GenericProductReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(GenericProductReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static GenericProductReference operator +(GenericProductReference reference, DbSet<GenericProduct> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static GenericProductReference operator +(GenericProductReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.GenericProducts.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator GenericProductReference(GenericProduct entity)
		{
			return entity == null ? null : new GenericProductReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static GenericProductReference OneRef(this IQueryable<GenericProduct> query)
		{
			return query?
				.Select(a => new GenericProductReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class GenericProductLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<GenericProduct> query, ref IEnumerable<GenericProductLookup> lookupList);

		public static IEnumerable<GenericProductLookup> SelectAndOrderByName(IQueryable<GenericProduct> query)
		{
			IEnumerable<GenericProductLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new GenericProductLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<GenericProductLookup> DefaultLookup(LookupParams<GenericProduct, GenericProductLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region GenericProductType

	partial class GenericProductType
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(GenericProductType);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).GenericProductTypes;

		internal GenericProductType n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (GenericProductType)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (GenericProductType)value;
		}

		public class Lazy<TValue> : Lazy<GenericProductType, TValue>
		{
			public Lazy(Func<GenericProductType, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<GenericProductType, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<GenericProductType, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static GenericProductType operator +(GenericProductType r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static GenericProductType operator |(GenericProductType r1, GenericProductType r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new GenericProductType
			{
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(GenericProductType).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<GenericProductType> entity);
		public static void Config(Domain.EntityConfiguration<GenericProductType> entity) => Config_(entity);

	}


	[Localization(typeof(GenericProductType))]
	public class GenericProductTypeReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public GenericProductTypeReference() { }

		public GenericProductTypeReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(GenericProductTypeReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static GenericProductTypeReference operator +(GenericProductTypeReference reference, DbSet<GenericProductType> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static GenericProductTypeReference operator +(GenericProductTypeReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.GenericProductTypes.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator GenericProductTypeReference(GenericProductType entity)
		{
			return entity == null ? null : new GenericProductTypeReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static GenericProductTypeReference OneRef(this IQueryable<GenericProductType> query)
		{
			return query?
				.Select(a => new GenericProductTypeReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class GenericProductTypeLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<GenericProductType> query, ref IEnumerable<GenericProductTypeLookup> lookupList);

		public static IEnumerable<GenericProductTypeLookup> SelectAndOrderByName(IQueryable<GenericProductType> query)
		{
			IEnumerable<GenericProductTypeLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new GenericProductTypeLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<GenericProductTypeLookup> DefaultLookup(LookupParams<GenericProductType, GenericProductTypeLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Identity

	partial class Identity
	{
	//1


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Identities;

		internal Identity n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Identity)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Identity)value;
		}

		public class Lazy<TValue> : Lazy<Identity, TValue>
		{
			public Lazy(Func<Identity, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Identity, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Identity, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Identity operator +(Identity r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Identity operator |(Identity r1, Identity r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<Identity> entity);
		public static void Config(Domain.EntityConfiguration<Identity> entity) => Config_(entity);

	}


	[Localization(typeof(Identity))]
	public class IdentityReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public IdentityReference() { }

		public IdentityReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(IdentityReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static IdentityReference operator +(IdentityReference reference, DbSet<Identity> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static IdentityReference operator +(IdentityReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Identities.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator IdentityReference(Identity entity)
		{
			return entity == null ? null : new IdentityReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static IdentityReference OneRef(this IQueryable<Identity> query)
		{
			return query?
				.Select(a => new IdentityReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class IdentityLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Identity> query, ref IEnumerable<IdentityLookup> lookupList);

		public static IEnumerable<IdentityLookup> SelectAndOrderByName(IQueryable<Identity> query)
		{
			IEnumerable<IdentityLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new IdentityLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<IdentityLookup> DefaultLookup(LookupParams<Identity, IdentityLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Insurance

	partial class Insurance
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Insurance);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Insurances;

		internal Insurance n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Insurance)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Insurance)value;
		}

		public class Lazy<TValue> : Lazy<Insurance, TValue>
		{
			public Lazy(Func<Insurance, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Insurance, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Insurance, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Insurance operator +(Insurance r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Insurance operator |(Insurance r1, Insurance r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Insurance
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Insurance).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Insurance> entity);
		public static void Config(Domain.EntityConfiguration<Insurance> entity) => Config_(entity);

	}


	[Localization(typeof(Insurance))]
	public class InsuranceReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public InsuranceReference() { }

		public InsuranceReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(InsuranceReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static InsuranceReference operator +(InsuranceReference reference, DbSet<Insurance> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static InsuranceReference operator +(InsuranceReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Insurances.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator InsuranceReference(Insurance entity)
		{
			return entity == null ? null : new InsuranceReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static InsuranceReference OneRef(this IQueryable<Insurance> query)
		{
			return query?
				.Select(a => new InsuranceReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class InsuranceLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Insurance> query, ref IEnumerable<InsuranceLookup> lookupList);

		public static IEnumerable<InsuranceLookup> SelectAndOrderByName(IQueryable<Insurance> query)
		{
			IEnumerable<InsuranceLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new InsuranceLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<InsuranceLookup> DefaultLookup(LookupParams<Insurance, InsuranceLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region InsuranceDocument

	partial class InsuranceDocument
	{
	//1


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).InsuranceDocuments;

		internal InsuranceDocument n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (InsuranceDocument)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (InsuranceDocument)value;
		}

		public class Lazy<TValue> : Lazy<InsuranceDocument, TValue>
		{
			public Lazy(Func<InsuranceDocument, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<InsuranceDocument, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<InsuranceDocument, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static InsuranceDocument operator +(InsuranceDocument r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static InsuranceDocument operator |(InsuranceDocument r1, InsuranceDocument r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<InsuranceDocument> entity);
		public static void Config(Domain.EntityConfiguration<InsuranceDocument> entity) => Config_(entity);

	}


	[Localization(typeof(InsuranceDocument))]
	public class InsuranceDocumentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }
			
		public InsuranceDocumentReference() { }

		public InsuranceDocumentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(InsuranceDocumentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static InsuranceDocumentReference operator +(InsuranceDocumentReference reference, DbSet<InsuranceDocument> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static InsuranceDocumentReference operator +(InsuranceDocumentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.InsuranceDocuments.Where(a => a.Id == id).Select(a => new { a.Name, a.Type }).FirstOrDefault().Do(a =>
			{
				reference.Name = a.Name;
				reference._Type = a.Type.AsString();
			});

			return reference;
		}

		public static implicit operator InsuranceDocumentReference(InsuranceDocument entity)
		{
			return entity == null ? null : new InsuranceDocumentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static InsuranceDocumentReference OneRef(this IQueryable<InsuranceDocument> query)
		{
			return query?
				.Select(a => new InsuranceDocumentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class InsuranceDocumentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<InsuranceDocument> query, ref IEnumerable<InsuranceDocumentLookup> lookupList);

		public static IEnumerable<InsuranceDocumentLookup> SelectAndOrderByName(IQueryable<InsuranceDocument> query)
		{
			IEnumerable<InsuranceDocumentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new InsuranceDocumentLookup { Id = a.Id, Name = a.Name, _Type = a.Type.ToString() })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<InsuranceDocumentLookup> DefaultLookup(LookupParams<InsuranceDocument, InsuranceDocumentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region InsuranceRefund

	partial class InsuranceRefund
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(InsuranceRefund);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).InsuranceRefunds;

		internal InsuranceRefund n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (InsuranceRefund)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (InsuranceRefund)value;
		}

		public class Lazy<TValue> : Lazy<InsuranceRefund, TValue>
		{
			public Lazy(Func<InsuranceRefund, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<InsuranceRefund, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<InsuranceRefund, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static InsuranceRefund operator +(InsuranceRefund r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static InsuranceRefund operator |(InsuranceRefund r1, InsuranceRefund r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new InsuranceRefund
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(InsuranceRefund).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<InsuranceRefund> entity);
		public static void Config(Domain.EntityConfiguration<InsuranceRefund> entity) => Config_(entity);

	}


	[Localization(typeof(InsuranceRefund))]
	public class InsuranceRefundReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public InsuranceRefundReference() { }

		public InsuranceRefundReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(InsuranceRefundReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static InsuranceRefundReference operator +(InsuranceRefundReference reference, DbSet<InsuranceRefund> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static InsuranceRefundReference operator +(InsuranceRefundReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.InsuranceRefunds.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator InsuranceRefundReference(InsuranceRefund entity)
		{
			return entity == null ? null : new InsuranceRefundReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static InsuranceRefundReference OneRef(this IQueryable<InsuranceRefund> query)
		{
			return query?
				.Select(a => new InsuranceRefundReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class InsuranceRefundLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<InsuranceRefund> query, ref IEnumerable<InsuranceRefundLookup> lookupList);

		public static IEnumerable<InsuranceRefundLookup> SelectAndOrderByName(IQueryable<InsuranceRefund> query)
		{
			IEnumerable<InsuranceRefundLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new InsuranceRefundLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<InsuranceRefundLookup> DefaultLookup(LookupParams<InsuranceRefund, InsuranceRefundLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region InternalIdentity

	partial class InternalIdentity
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(InternalIdentity);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).InternalIdentities;

		internal InternalIdentity n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (InternalIdentity)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (InternalIdentity)value;
		}

		public class Lazy<TValue> : Lazy<InternalIdentity, TValue>
		{
			public Lazy(Func<InternalIdentity, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<InternalIdentity, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<InternalIdentity, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static InternalIdentity operator +(InternalIdentity r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static InternalIdentity operator |(InternalIdentity r1, InternalIdentity r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new InternalIdentity
			{
				Description = e.Property(a => a.Description).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(InternalIdentity).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<InternalIdentity> entity);
		public static void Config(Domain.EntityConfiguration<InternalIdentity> entity) => Config_(entity);

	}


	[Localization(typeof(InternalIdentity))]
	public class InternalIdentityReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public InternalIdentityReference() { }

		public InternalIdentityReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(InternalIdentityReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static InternalIdentityReference operator +(InternalIdentityReference reference, DbSet<InternalIdentity> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static InternalIdentityReference operator +(InternalIdentityReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.InternalIdentities.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator InternalIdentityReference(InternalIdentity entity)
		{
			return entity == null ? null : new InternalIdentityReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static InternalIdentityReference OneRef(this IQueryable<InternalIdentity> query)
		{
			return query?
				.Select(a => new InternalIdentityReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class InternalIdentityLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<InternalIdentity> query, ref IEnumerable<InternalIdentityLookup> lookupList);

		public static IEnumerable<InternalIdentityLookup> SelectAndOrderByName(IQueryable<InternalIdentity> query)
		{
			IEnumerable<InternalIdentityLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new InternalIdentityLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<InternalIdentityLookup> DefaultLookup(LookupParams<InternalIdentity, InternalIdentityLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region InternalTransfer

	partial class InternalTransfer
	{
	//1

		[ForeignKey("FromOrder")]
		public string FromOrderId
		{ 
			get { return _FromOrderId; }
			set 
			{
				if (_FromOrderId == value) return;
				_FromOrderId = value;
				_FromOrder = null;
			}
		}
		private string _FromOrderId;

		public virtual Order FromOrder
		{
			get 
			{ 
				return _FromOrder ?? (_FromOrder = db?.Orders.ById(_FromOrderId)); 
			}
			set
			{
				_FromOrder = value;
				_FromOrderId = value?.Id;
			}
		}

		[ForeignKey("FromParty")]
		public string FromPartyId
		{ 
			get { return _FromPartyId; }
			set 
			{
				if (_FromPartyId == value) return;
				_FromPartyId = value;
				_FromParty = null;
			}
		}
		private string _FromPartyId;

		public virtual Party FromParty
		{
			get 
			{ 
				return _FromParty ?? (_FromParty = db?.Parties.ById(_FromPartyId)); 
			}
			set
			{
				_FromParty = value;
				_FromPartyId = value?.Id;
			}
		}

		[ForeignKey("ToOrder")]
		public string ToOrderId
		{ 
			get { return _ToOrderId; }
			set 
			{
				if (_ToOrderId == value) return;
				_ToOrderId = value;
				_ToOrder = null;
			}
		}
		private string _ToOrderId;

		public virtual Order ToOrder
		{
			get 
			{ 
				return _ToOrder ?? (_ToOrder = db?.Orders.ById(_ToOrderId)); 
			}
			set
			{
				_ToOrder = value;
				_ToOrderId = value?.Id;
			}
		}

		[ForeignKey("ToParty")]
		public string ToPartyId
		{ 
			get { return _ToPartyId; }
			set 
			{
				if (_ToPartyId == value) return;
				_ToPartyId = value;
				_ToParty = null;
			}
		}
		private string _ToPartyId;

		public virtual Party ToParty
		{
			get 
			{ 
				return _ToParty ?? (_ToParty = db?.Parties.ById(_ToPartyId)); 
			}
			set
			{
				_ToParty = value;
				_ToPartyId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(InternalTransfer);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).InternalTransfers;

		internal InternalTransfer n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (InternalTransfer)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (InternalTransfer)value;
		}

		public class Lazy<TValue> : Lazy<InternalTransfer, TValue>
		{
			public Lazy(Func<InternalTransfer, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<InternalTransfer, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<InternalTransfer, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static InternalTransfer operator +(InternalTransfer r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static InternalTransfer operator |(InternalTransfer r1, InternalTransfer r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new InternalTransfer
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				FromOrderId = e.Property(a => a.FromOrderId).OriginalValue,
				FromPartyId = e.Property(a => a.FromPartyId).OriginalValue,
				ToOrderId = e.Property(a => a.ToOrderId).OriginalValue,
				ToPartyId = e.Property(a => a.ToPartyId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(InternalTransfer).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<InternalTransfer> entity);
		public static void Config(Domain.EntityConfiguration<InternalTransfer> entity) => Config_(entity);

	}


	[Localization(typeof(InternalTransfer))]
	public class InternalTransferReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public InternalTransferReference() { }

		public InternalTransferReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(InternalTransferReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static InternalTransferReference operator +(InternalTransferReference reference, DbSet<InternalTransfer> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static InternalTransferReference operator +(InternalTransferReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.InternalTransfers.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator InternalTransferReference(InternalTransfer entity)
		{
			return entity == null ? null : new InternalTransferReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static InternalTransferReference OneRef(this IQueryable<InternalTransfer> query)
		{
			return query?
				.Select(a => new InternalTransferReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class InternalTransferLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<InternalTransfer> query, ref IEnumerable<InternalTransferLookup> lookupList);

		public static IEnumerable<InternalTransferLookup> SelectAndOrderByName(IQueryable<InternalTransfer> query)
		{
			IEnumerable<InternalTransferLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new InternalTransferLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<InternalTransferLookup> DefaultLookup(LookupParams<InternalTransfer, InternalTransferLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Invoice

	partial class Invoice
	{
	//1
		public Invoice()
		{
			Total = new Money();
			Vat = new Money();
		}

		[ForeignKey("Order")]
		public string OrderId
		{ 
			get { return _OrderId; }
			set 
			{
				if (_OrderId == value) return;
				_OrderId = value;
				_Order = null;
			}
		}
		private string _OrderId;

		public virtual Order Order
		{
			get 
			{ 
				return _Order ?? (_Order = db?.Orders.ById(_OrderId)); 
			}
			set
			{
				_Order = value;
				_OrderId = value?.Id;
			}
		}

		[ForeignKey("IssuedBy")]
		public string IssuedById
		{ 
			get { return _IssuedById; }
			set 
			{
				if (_IssuedById == value) return;
				_IssuedById = value;
				_IssuedBy = null;
			}
		}
		private string _IssuedById;

		public virtual Person IssuedBy
		{
			get 
			{ 
				return _IssuedBy ?? (_IssuedBy = db?.Persons.ById(_IssuedById)); 
			}
			set
			{
				_IssuedBy = value;
				_IssuedById = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Invoice);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Invoices;

		internal Invoice n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Invoice)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Invoice)value;
		}

		public class Lazy<TValue> : Lazy<Invoice, TValue>
		{
			public Lazy(Func<Invoice, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Invoice, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Invoice, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Invoice operator +(Invoice r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Invoice operator |(Invoice r1, Invoice r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Invoice
			{
				Type = e.Property(a => a.Type).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				Number = e.Property(a => a.Number).OriginalValue,
				Agreement = e.Property(a => a.Agreement).OriginalValue,
				TimeStamp = e.Property(a => a.TimeStamp).OriginalValue,
				Content = e.Property(a => a.Content).OriginalValue,
				Total = e.Property(a => a.Total).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IssuedById = e.Property(a => a.IssuedById).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Invoice).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Invoice> entity);
		public static void Config(Domain.EntityConfiguration<Invoice> entity) => Config_(entity);

	}


	[Localization(typeof(Invoice))]
	public class InvoiceReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public InvoiceReference() { }

		public InvoiceReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(InvoiceReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static InvoiceReference operator +(InvoiceReference reference, DbSet<Invoice> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static InvoiceReference operator +(InvoiceReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.Invoices.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator InvoiceReference(Invoice entity)
		{
			return entity == null ? null : new InvoiceReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static InvoiceReference OneRef(this IQueryable<Invoice> query)
		{
			return query?
				.Select(a => new InvoiceReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class InvoiceLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<Invoice> query, ref IEnumerable<InvoiceLookup> lookupList);

		public static IEnumerable<InvoiceLookup> SelectAndOrderByName(IQueryable<Invoice> query)
		{
			IEnumerable<InvoiceLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new InvoiceLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<InvoiceLookup> DefaultLookup(LookupParams<Invoice, InvoiceLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Isic

	partial class Isic
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Isic);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Isics;

		internal Isic n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Isic)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Isic)value;
		}

		public class Lazy<TValue> : Lazy<Isic, TValue>
		{
			public Lazy(Func<Isic, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Isic, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Isic, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Isic operator +(Isic r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Isic operator |(Isic r1, Isic r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Isic
			{
				CardType = e.Property(a => a.CardType).OriginalValue,
				Number1 = e.Property(a => a.Number1).OriginalValue,
				Number2 = e.Property(a => a.Number2).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Isic).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Isic> entity);
		public static void Config(Domain.EntityConfiguration<Isic> entity) => Config_(entity);

	}


	[Localization(typeof(Isic))]
	public class IsicReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public IsicReference() { }

		public IsicReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(IsicReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static IsicReference operator +(IsicReference reference, DbSet<Isic> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static IsicReference operator +(IsicReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Isics.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator IsicReference(Isic entity)
		{
			return entity == null ? null : new IsicReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static IsicReference OneRef(this IQueryable<Isic> query)
		{
			return query?
				.Select(a => new IsicReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class IsicLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Isic> query, ref IEnumerable<IsicLookup> lookupList);

		public static IEnumerable<IsicLookup> SelectAndOrderByName(IQueryable<Isic> query)
		{
			IEnumerable<IsicLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new IsicLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<IsicLookup> DefaultLookup(LookupParams<Isic, IsicLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region IssuedConsignment

	partial class IssuedConsignment
	{
	//1

		[ForeignKey("Consignment")]
		public string ConsignmentId
		{ 
			get { return _ConsignmentId; }
			set 
			{
				if (_ConsignmentId == value) return;
				_ConsignmentId = value;
				_Consignment = null;
			}
		}
		private string _ConsignmentId;

		public virtual Consignment Consignment
		{
			get 
			{ 
				return _Consignment ?? (_Consignment = db?.Consignments.ById(_ConsignmentId)); 
			}
			set
			{
				_Consignment = value;
				_ConsignmentId = value?.Id;
			}
		}

		[ForeignKey("IssuedBy")]
		public string IssuedById
		{ 
			get { return _IssuedById; }
			set 
			{
				if (_IssuedById == value) return;
				_IssuedById = value;
				_IssuedBy = null;
			}
		}
		private string _IssuedById;

		public virtual Person IssuedBy
		{
			get 
			{ 
				return _IssuedBy ?? (_IssuedBy = db?.Persons.ById(_IssuedById)); 
			}
			set
			{
				_IssuedBy = value;
				_IssuedById = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(IssuedConsignment);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).IssuedConsignments;

		internal IssuedConsignment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (IssuedConsignment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (IssuedConsignment)value;
		}

		public class Lazy<TValue> : Lazy<IssuedConsignment, TValue>
		{
			public Lazy(Func<IssuedConsignment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<IssuedConsignment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<IssuedConsignment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static IssuedConsignment operator +(IssuedConsignment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static IssuedConsignment operator |(IssuedConsignment r1, IssuedConsignment r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new IssuedConsignment
			{
				Number = e.Property(a => a.Number).OriginalValue,
				TimeStamp = e.Property(a => a.TimeStamp).OriginalValue,
				Content = e.Property(a => a.Content).OriginalValue,
				ConsignmentId = e.Property(a => a.ConsignmentId).OriginalValue,
				IssuedById = e.Property(a => a.IssuedById).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(IssuedConsignment).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<IssuedConsignment> entity);
		public static void Config(Domain.EntityConfiguration<IssuedConsignment> entity) => Config_(entity);

	}


	[Localization(typeof(IssuedConsignment))]
	public class IssuedConsignmentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public IssuedConsignmentReference() { }

		public IssuedConsignmentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(IssuedConsignmentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static IssuedConsignmentReference operator +(IssuedConsignmentReference reference, DbSet<IssuedConsignment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static IssuedConsignmentReference operator +(IssuedConsignmentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.IssuedConsignments.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator IssuedConsignmentReference(IssuedConsignment entity)
		{
			return entity == null ? null : new IssuedConsignmentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static IssuedConsignmentReference OneRef(this IQueryable<IssuedConsignment> query)
		{
			return query?
				.Select(a => new IssuedConsignmentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class IssuedConsignmentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<IssuedConsignment> query, ref IEnumerable<IssuedConsignmentLookup> lookupList);

		public static IEnumerable<IssuedConsignmentLookup> SelectAndOrderByName(IQueryable<IssuedConsignment> query)
		{
			IEnumerable<IssuedConsignmentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new IssuedConsignmentLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<IssuedConsignmentLookup> DefaultLookup(LookupParams<IssuedConsignment, IssuedConsignmentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region MilesCard

	partial class MilesCard
	{
	//1

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Person Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Persons.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}

		[ForeignKey("Organization")]
		public string OrganizationId
		{ 
			get { return _OrganizationId; }
			set 
			{
				if (_OrganizationId == value) return;
				_OrganizationId = value;
				_Organization = null;
			}
		}
		private string _OrganizationId;

		public virtual Organization Organization
		{
			get 
			{ 
				return _Organization ?? (_Organization = db?.Organizations.ById(_OrganizationId)); 
			}
			set
			{
				_Organization = value;
				_OrganizationId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(MilesCard);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).MilesCards;

		internal MilesCard n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (MilesCard)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (MilesCard)value;
		}

		public class Lazy<TValue> : Lazy<MilesCard, TValue>
		{
			public Lazy(Func<MilesCard, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<MilesCard, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<MilesCard, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static MilesCard operator +(MilesCard r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static MilesCard operator |(MilesCard r1, MilesCard r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new MilesCard
			{
				Number = e.Property(a => a.Number).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OrganizationId = e.Property(a => a.OrganizationId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(MilesCard).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<MilesCard> entity);
		public static void Config(Domain.EntityConfiguration<MilesCard> entity) => Config_(entity);

	}


	[Localization(typeof(MilesCard))]
	public class MilesCardReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public MilesCardReference() { }

		public MilesCardReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(MilesCardReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static MilesCardReference operator +(MilesCardReference reference, DbSet<MilesCard> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static MilesCardReference operator +(MilesCardReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.MilesCards.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator MilesCardReference(MilesCard entity)
		{
			return entity == null ? null : new MilesCardReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static MilesCardReference OneRef(this IQueryable<MilesCard> query)
		{
			return query?
				.Select(a => new MilesCardReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class MilesCardLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<MilesCard> query, ref IEnumerable<MilesCardLookup> lookupList);

		public static IEnumerable<MilesCardLookup> SelectAndOrderByName(IQueryable<MilesCard> query)
		{
			IEnumerable<MilesCardLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new MilesCardLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<MilesCardLookup> DefaultLookup(LookupParams<MilesCard, MilesCardLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region OpeningBalance

	partial class OpeningBalance
	{
	//1

		[ForeignKey("Party")]
		public string PartyId
		{ 
			get { return _PartyId; }
			set 
			{
				if (_PartyId == value) return;
				_PartyId = value;
				_Party = null;
			}
		}
		private string _PartyId;

		public virtual Party Party
		{
			get 
			{ 
				return _Party ?? (_Party = db?.Parties.ById(_PartyId)); 
			}
			set
			{
				_Party = value;
				_PartyId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(OpeningBalance);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).OpeningBalances;

		internal OpeningBalance n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (OpeningBalance)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (OpeningBalance)value;
		}

		public class Lazy<TValue> : Lazy<OpeningBalance, TValue>
		{
			public Lazy(Func<OpeningBalance, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<OpeningBalance, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<OpeningBalance, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static OpeningBalance operator +(OpeningBalance r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static OpeningBalance operator |(OpeningBalance r1, OpeningBalance r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new OpeningBalance
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				Balance = e.Property(a => a.Balance).OriginalValue,
				PartyId = e.Property(a => a.PartyId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(OpeningBalance).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<OpeningBalance> entity);
		public static void Config(Domain.EntityConfiguration<OpeningBalance> entity) => Config_(entity);

	}


	[Localization(typeof(OpeningBalance))]
	public class OpeningBalanceReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public OpeningBalanceReference() { }

		public OpeningBalanceReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(OpeningBalanceReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static OpeningBalanceReference operator +(OpeningBalanceReference reference, DbSet<OpeningBalance> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static OpeningBalanceReference operator +(OpeningBalanceReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.OpeningBalances.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator OpeningBalanceReference(OpeningBalance entity)
		{
			return entity == null ? null : new OpeningBalanceReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static OpeningBalanceReference OneRef(this IQueryable<OpeningBalance> query)
		{
			return query?
				.Select(a => new OpeningBalanceReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class OpeningBalanceLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<OpeningBalance> query, ref IEnumerable<OpeningBalanceLookup> lookupList);

		public static IEnumerable<OpeningBalanceLookup> SelectAndOrderByName(IQueryable<OpeningBalance> query)
		{
			IEnumerable<OpeningBalanceLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new OpeningBalanceLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<OpeningBalanceLookup> DefaultLookup(LookupParams<OpeningBalance, OpeningBalanceLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Order

	partial class Order
	{
	//1
		public Order()
		{
			Discount = new Money();
			Total = new Money();
			Vat = new Money();
			Paid = new Money();
			TotalDue = new Money();
			VatDue = new Money();
		}

		[ForeignKey("Customer")]
		public string CustomerId
		{ 
			get { return _CustomerId; }
			set 
			{
				if (_CustomerId == value) return;
				_CustomerId = value;
				_Customer = null;
			}
		}
		private string _CustomerId;

		public virtual Party Customer
		{
			get 
			{ 
				return _Customer ?? (_Customer = db?.Parties.ById(_CustomerId)); 
			}
			set
			{
				_Customer = value;
				_CustomerId = value?.Id;
			}
		}

		[ForeignKey("BillTo")]
		public string BillToId
		{ 
			get { return _BillToId; }
			set 
			{
				if (_BillToId == value) return;
				_BillToId = value;
				_BillTo = null;
			}
		}
		private string _BillToId;

		public virtual Party BillTo
		{
			get 
			{ 
				return _BillTo ?? (_BillTo = db?.Parties.ById(_BillToId)); 
			}
			set
			{
				_BillTo = value;
				_BillToId = value?.Id;
			}
		}

		[ForeignKey("ShipTo")]
		public string ShipToId
		{ 
			get { return _ShipToId; }
			set 
			{
				if (_ShipToId == value) return;
				_ShipToId = value;
				_ShipTo = null;
			}
		}
		private string _ShipToId;

		public virtual Party ShipTo
		{
			get 
			{ 
				return _ShipTo ?? (_ShipTo = db?.Parties.ById(_ShipToId)); 
			}
			set
			{
				_ShipTo = value;
				_ShipToId = value?.Id;
			}
		}

		[ForeignKey("AssignedTo")]
		public string AssignedToId
		{ 
			get { return _AssignedToId; }
			set 
			{
				if (_AssignedToId == value) return;
				_AssignedToId = value;
				_AssignedTo = null;
			}
		}
		private string _AssignedToId;

		public virtual Person AssignedTo
		{
			get 
			{ 
				return _AssignedTo ?? (_AssignedTo = db?.Persons.ById(_AssignedToId)); 
			}
			set
			{
				_AssignedTo = value;
				_AssignedToId = value?.Id;
			}
		}

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Party Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Parties.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}

		[ForeignKey("BankAccount")]
		public string BankAccountId
		{ 
			get { return _BankAccountId; }
			set 
			{
				if (_BankAccountId == value) return;
				_BankAccountId = value;
				_BankAccount = null;
			}
		}
		private string _BankAccountId;

		public virtual BankAccount BankAccount
		{
			get 
			{ 
				return _BankAccount ?? (_BankAccount = db?.BankAccounts.ById(_BankAccountId)); 
			}
			set
			{
				_BankAccount = value;
				_BankAccountId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Order);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Orders;

		internal Order n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Order)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Order)value;
		}

		public class Lazy<TValue> : Lazy<Order, TValue>
		{
			public Lazy(Func<Order, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Order, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Order, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Order operator +(Order r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Order operator |(Order r1, Order r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Order
			{
				Number = e.Property(a => a.Number).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				BillToName = e.Property(a => a.BillToName).OriginalValue,
				IsPublic = e.Property(a => a.IsPublic).OriginalValue,
				IsSubjectOfPaymentsControl = e.Property(a => a.IsSubjectOfPaymentsControl).OriginalValue,
				SeparateServiceFee = e.Property(a => a.SeparateServiceFee).OriginalValue,
				UseServiceFeeOnlyInVat = e.Property(a => a.UseServiceFeeOnlyInVat).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				InvoiceLastIndex = e.Property(a => a.InvoiceLastIndex).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				BillToId = e.Property(a => a.BillToId).OriginalValue,
				ShipToId = e.Property(a => a.ShipToId).OriginalValue,
				AssignedToId = e.Property(a => a.AssignedToId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				BankAccountId = e.Property(a => a.BankAccountId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Order).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Order> entity);
		public static void Config(Domain.EntityConfiguration<Order> entity) => Config_(entity);

	}


	[Localization(typeof(Order))]
	public class OrderReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public OrderReference() { }

		public OrderReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(OrderReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static OrderReference operator +(OrderReference reference, DbSet<Order> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static OrderReference operator +(OrderReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.Orders.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator OrderReference(Order entity)
		{
			return entity == null ? null : new OrderReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static OrderReference OneRef(this IQueryable<Order> query)
		{
			return query?
				.Select(a => new OrderReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class OrderLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<Order> query, ref IEnumerable<OrderLookup> lookupList);

		public static IEnumerable<OrderLookup> SelectAndOrderByName(IQueryable<Order> query)
		{
			IEnumerable<OrderLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new OrderLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<OrderLookup> DefaultLookup(LookupParams<Order, OrderLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region OrderCheck

	partial class OrderCheck
	{
	//1

		[ForeignKey("Order")]
		public string OrderId
		{ 
			get { return _OrderId; }
			set 
			{
				if (_OrderId == value) return;
				_OrderId = value;
				_Order = null;
			}
		}
		private string _OrderId;

		public virtual Order Order
		{
			get 
			{ 
				return _Order ?? (_Order = db?.Orders.ById(_OrderId)); 
			}
			set
			{
				_Order = value;
				_OrderId = value?.Id;
			}
		}

		[ForeignKey("Person")]
		public string PersonId
		{ 
			get { return _PersonId; }
			set 
			{
				if (_PersonId == value) return;
				_PersonId = value;
				_Person = null;
			}
		}
		private string _PersonId;

		public virtual Person Person
		{
			get 
			{ 
				return _Person ?? (_Person = db?.Persons.ById(_PersonId)); 
			}
			set
			{
				_Person = value;
				_PersonId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(OrderCheck);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).OrderChecks;

		internal OrderCheck n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (OrderCheck)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (OrderCheck)value;
		}

		public class Lazy<TValue> : Lazy<OrderCheck, TValue>
		{
			public Lazy(Func<OrderCheck, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<OrderCheck, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<OrderCheck, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static OrderCheck operator +(OrderCheck r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static OrderCheck operator |(OrderCheck r1, OrderCheck r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new OrderCheck
			{
				Date = e.Property(a => a.Date).OriginalValue,
				CheckType = e.Property(a => a.CheckType).OriginalValue,
				CheckNumber = e.Property(a => a.CheckNumber).OriginalValue,
				Currency = e.Property(a => a.Currency).OriginalValue,
				CheckAmount = e.Property(a => a.CheckAmount).OriginalValue,
				CheckVat = e.Property(a => a.CheckVat).OriginalValue,
				PayAmount = e.Property(a => a.PayAmount).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				Description = e.Property(a => a.Description).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				PersonId = e.Property(a => a.PersonId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(OrderCheck).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<OrderCheck> entity);
		public static void Config(Domain.EntityConfiguration<OrderCheck> entity) => Config_(entity);

	}


	[Localization(typeof(OrderCheck))]
	public class OrderCheckReference : INameContainer
	{

		public string Id { get; set; }

		public string CheckNumber { get; set; }
			
		public OrderCheckReference() { }

		public OrderCheckReference(string id, string name)
		{
			Id = id;
			CheckNumber = name;
		}

		public string GetName() => CheckNumber;

		public static implicit operator bool(OrderCheckReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static OrderCheckReference operator +(OrderCheckReference reference, DbSet<OrderCheck> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.CheckNumber = set.Where(a => a.Id == id).Select(a => a.CheckNumber).FirstOrDefault();


			return reference;
		}

		public static OrderCheckReference operator +(OrderCheckReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.CheckNumber = db.OrderChecks.Where(a => a.Id == id).Select(a => a.CheckNumber).FirstOrDefault();

			return reference;
		}

		public static implicit operator OrderCheckReference(OrderCheck entity)
		{
			return entity == null ? null : new OrderCheckReference { Id = entity.Id, CheckNumber = entity.CheckNumber };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static OrderCheckReference OneRef(this IQueryable<OrderCheck> query)
		{
			return query?
				.Select(a => new OrderCheckReference { Id = a.Id, CheckNumber = a.CheckNumber })
				.FirstOrDefault();
		}
	}


	public partial class OrderCheckLookup : INameContainer
	{

		public string Id { get; set; }

		public string CheckNumber { get; set; }

		public string GetName()	=> CheckNumber;

		static partial void SelectAndOrderByName(IQueryable<OrderCheck> query, ref IEnumerable<OrderCheckLookup> lookupList);

		public static IEnumerable<OrderCheckLookup> SelectAndOrderByName(IQueryable<OrderCheck> query)
		{
			IEnumerable<OrderCheckLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new OrderCheckLookup { Id = a.Id, CheckNumber = a.CheckNumber })
				.OrderBy(a => a.CheckNumber);
		}

		public static IEnumerable<OrderCheckLookup> DefaultLookup(LookupParams<OrderCheck, OrderCheckLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.CheckNumber.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region OrderItem

	partial class OrderItem
	{
	//1
		public OrderItem()
		{
			Price = new Money();
			Discount = new Money();
			GrandTotal = new Money();
			GivenVat = new Money();
			TaxedTotal = new Money();
		}

		[ForeignKey("Order")]
		public string OrderId
		{ 
			get { return _OrderId; }
			set 
			{
				if (_OrderId == value) return;
				_OrderId = value;
				_Order = null;
			}
		}
		private string _OrderId;

		public virtual Order Order
		{
			get 
			{ 
				return _Order ?? (_Order = db?.Orders.ById(_OrderId)); 
			}
			set
			{
				_Order = value;
				_OrderId = value?.Id;
			}
		}

		[ForeignKey("Product")]
		public string ProductId
		{ 
			get { return _ProductId; }
			set 
			{
				if (_ProductId == value) return;
				_ProductId = value;
				_Product = null;
			}
		}
		private string _ProductId;

		public virtual Product Product
		{
			get 
			{ 
				return _Product ?? (_Product = db?.Products.ById(_ProductId)); 
			}
			set
			{
				_Product = value;
				_ProductId = value?.Id;
			}
		}

		[ForeignKey("Consignment")]
		public string ConsignmentId
		{ 
			get { return _ConsignmentId; }
			set 
			{
				if (_ConsignmentId == value) return;
				_ConsignmentId = value;
				_Consignment = null;
			}
		}
		private string _ConsignmentId;

		public virtual Consignment Consignment
		{
			get 
			{ 
				return _Consignment ?? (_Consignment = db?.Consignments.ById(_ConsignmentId)); 
			}
			set
			{
				_Consignment = value;
				_ConsignmentId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(OrderItem);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).OrderItems;

		internal OrderItem n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (OrderItem)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (OrderItem)value;
		}

		public class Lazy<TValue> : Lazy<OrderItem, TValue>
		{
			public Lazy(Func<OrderItem, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<OrderItem, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<OrderItem, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static OrderItem operator +(OrderItem r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static OrderItem operator |(OrderItem r1, OrderItem r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new OrderItem
			{
				Position = e.Property(a => a.Position).OriginalValue,
				Text = e.Property(a => a.Text).OriginalValue,
				LinkType = e.Property(a => a.LinkType).OriginalValue,
				Price = e.Property(a => a.Price).OriginalValue,
				Quantity = e.Property(a => a.Quantity).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				HasVat = e.Property(a => a.HasVat).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				ProductId = e.Property(a => a.ProductId).OriginalValue,
				ConsignmentId = e.Property(a => a.ConsignmentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(OrderItem).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<OrderItem> entity);
		public static void Config(Domain.EntityConfiguration<OrderItem> entity) => Config_(entity);

	}


	[Localization(typeof(OrderItem))]
	public class OrderItemReference : INameContainer
	{

		public string Id { get; set; }

		public string Text { get; set; }
			
		public OrderItemReference() { }

		public OrderItemReference(string id, string name)
		{
			Id = id;
			Text = name;
		}

		public string GetName() => Text;

		public static implicit operator bool(OrderItemReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static OrderItemReference operator +(OrderItemReference reference, DbSet<OrderItem> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Text = set.Where(a => a.Id == id).Select(a => a.Text).FirstOrDefault();


			return reference;
		}

		public static OrderItemReference operator +(OrderItemReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Text = db.OrderItems.Where(a => a.Id == id).Select(a => a.Text).FirstOrDefault();

			return reference;
		}

		public static implicit operator OrderItemReference(OrderItem entity)
		{
			return entity == null ? null : new OrderItemReference { Id = entity.Id, Text = entity.Text };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static OrderItemReference OneRef(this IQueryable<OrderItem> query)
		{
			return query?
				.Select(a => new OrderItemReference { Id = a.Id, Text = a.Text })
				.FirstOrDefault();
		}
	}


	public partial class OrderItemLookup : INameContainer
	{

		public string Id { get; set; }

		public string Text { get; set; }

		public string GetName()	=> Text;

		static partial void SelectAndOrderByName(IQueryable<OrderItem> query, ref IEnumerable<OrderItemLookup> lookupList);

		public static IEnumerable<OrderItemLookup> SelectAndOrderByName(IQueryable<OrderItem> query)
		{
			IEnumerable<OrderItemLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new OrderItemLookup { Id = a.Id, Text = a.Text })
				.OrderBy(a => a.Text);
		}

		public static IEnumerable<OrderItemLookup> DefaultLookup(LookupParams<OrderItem, OrderItemLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Text.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Organization

	partial class Organization
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Organization);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Organizations;

		internal Organization n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Organization)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Organization)value;
		}

		public class Lazy<TValue> : Lazy<Organization, TValue>
		{
			public Lazy(Func<Organization, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Organization, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Organization, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Organization operator +(Organization r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Organization operator |(Organization r1, Organization r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Organization
			{
				IsAirline = e.Property(a => a.IsAirline).OriginalValue,
				AirlineIataCode = e.Property(a => a.AirlineIataCode).OriginalValue,
				AirlinePrefixCode = e.Property(a => a.AirlinePrefixCode).OriginalValue,
				AirlinePassportRequirement = e.Property(a => a.AirlinePassportRequirement).OriginalValue,
				IsAccommodationProvider = e.Property(a => a.IsAccommodationProvider).OriginalValue,
				IsBusTicketProvider = e.Property(a => a.IsBusTicketProvider).OriginalValue,
				IsCarRentalProvider = e.Property(a => a.IsCarRentalProvider).OriginalValue,
				IsPasteboardProvider = e.Property(a => a.IsPasteboardProvider).OriginalValue,
				IsTourProvider = e.Property(a => a.IsTourProvider).OriginalValue,
				IsTransferProvider = e.Property(a => a.IsTransferProvider).OriginalValue,
				IsGenericProductProvider = e.Property(a => a.IsGenericProductProvider).OriginalValue,
				IsProvider = e.Property(a => a.IsProvider).OriginalValue,
				IsInsuranceCompany = e.Property(a => a.IsInsuranceCompany).OriginalValue,
				IsRoamingOperator = e.Property(a => a.IsRoamingOperator).OriginalValue,
				LegalName = e.Property(a => a.LegalName).OriginalValue,
				Code = e.Property(a => a.Code).OriginalValue,
				Phone1 = e.Property(a => a.Phone1).OriginalValue,
				Phone2 = e.Property(a => a.Phone2).OriginalValue,
				Fax = e.Property(a => a.Fax).OriginalValue,
				Email1 = e.Property(a => a.Email1).OriginalValue,
				Email2 = e.Property(a => a.Email2).OriginalValue,
				WebAddress = e.Property(a => a.WebAddress).OriginalValue,
				IsCustomer = e.Property(a => a.IsCustomer).OriginalValue,
				IsSupplier = e.Property(a => a.IsSupplier).OriginalValue,
				Details = e.Property(a => a.Details).OriginalValue,
				LegalAddress = e.Property(a => a.LegalAddress).OriginalValue,
				ActualAddress = e.Property(a => a.ActualAddress).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				ReportsToId = e.Property(a => a.ReportsToId).OriginalValue,
				DefaultBankAccountId = e.Property(a => a.DefaultBankAccountId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Organization).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Organization> entity);
		public static void Config(Domain.EntityConfiguration<Organization> entity) => Config_(entity);

	}


	[Localization(typeof(Organization))]
	public class OrganizationReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public OrganizationReference() { }

		public OrganizationReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(OrganizationReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static OrganizationReference operator +(OrganizationReference reference, DbSet<Organization> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static OrganizationReference operator +(OrganizationReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Organizations.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator OrganizationReference(Organization entity)
		{
			return entity == null ? null : new OrganizationReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static OrganizationReference OneRef(this IQueryable<Organization> query)
		{
			return query?
				.Select(a => new OrganizationReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class OrganizationLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Organization> query, ref IEnumerable<OrganizationLookup> lookupList);

		public static IEnumerable<OrganizationLookup> SelectAndOrderByName(IQueryable<Organization> query)
		{
			IEnumerable<OrganizationLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new OrganizationLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<OrganizationLookup> DefaultLookup(LookupParams<Organization, OrganizationLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Party

	partial class Party
	{
	//1

		[ForeignKey("ReportsTo")]
		public string ReportsToId
		{ 
			get { return _ReportsToId; }
			set 
			{
				if (_ReportsToId == value) return;
				_ReportsToId = value;
				_ReportsTo = null;
			}
		}
		private string _ReportsToId;

		public virtual Party ReportsTo
		{
			get 
			{ 
				return _ReportsTo ?? (_ReportsTo = db?.Parties.ById(_ReportsToId)); 
			}
			set
			{
				_ReportsTo = value;
				_ReportsToId = value?.Id;
			}
		}

		[ForeignKey("DefaultBankAccount")]
		public string DefaultBankAccountId
		{ 
			get { return _DefaultBankAccountId; }
			set 
			{
				if (_DefaultBankAccountId == value) return;
				_DefaultBankAccountId = value;
				_DefaultBankAccount = null;
			}
		}
		private string _DefaultBankAccountId;

		public virtual BankAccount DefaultBankAccount
		{
			get 
			{ 
				return _DefaultBankAccount ?? (_DefaultBankAccount = db?.BankAccounts.ById(_DefaultBankAccountId)); 
			}
			set
			{
				_DefaultBankAccount = value;
				_DefaultBankAccountId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Parties;

		internal Party n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Party)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Party)value;
		}

		public class Lazy<TValue> : Lazy<Party, TValue>
		{
			public Lazy(Func<Party, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Party, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Party, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Party operator +(Party r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Party operator |(Party r1, Party r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<Party> entity);
		public static void Config(Domain.EntityConfiguration<Party> entity) => Config_(entity);

	}


	[Localization(typeof(Party))]
	public class PartyReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }
			
		public PartyReference() { }

		public PartyReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(PartyReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PartyReference operator +(PartyReference reference, DbSet<Party> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static PartyReference operator +(PartyReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.Parties.Where(a => a.Id == id).Select(a => new { a.Name, a.Type }).FirstOrDefault().Do(a =>
			{
				reference.Name = a.Name;
				reference._Type = a.Type.AsString();
			});

			return reference;
		}

		public static implicit operator PartyReference(Party entity)
		{
			return entity == null ? null : new PartyReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PartyReference OneRef(this IQueryable<Party> query)
		{
			return query?
				.Select(a => new PartyReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class PartyLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Party> query, ref IEnumerable<PartyLookup> lookupList);

		public static IEnumerable<PartyLookup> SelectAndOrderByName(IQueryable<Party> query)
		{
			IEnumerable<PartyLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PartyLookup { Id = a.Id, Name = a.Name, _Type = a.Type.ToString() })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<PartyLookup> DefaultLookup(LookupParams<Party, PartyLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Passport

	partial class Passport
	{
	//1

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Person Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Persons.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}

		[ForeignKey("Citizenship")]
		public string CitizenshipId
		{ 
			get { return _CitizenshipId; }
			set 
			{
				if (_CitizenshipId == value) return;
				_CitizenshipId = value;
				_Citizenship = null;
			}
		}
		private string _CitizenshipId;

		public virtual Country Citizenship
		{
			get 
			{ 
				return _Citizenship ?? (_Citizenship = db?.Countries.ById(_CitizenshipId)); 
			}
			set
			{
				_Citizenship = value;
				_CitizenshipId = value?.Id;
			}
		}

		[ForeignKey("IssuedBy")]
		public string IssuedById
		{ 
			get { return _IssuedById; }
			set 
			{
				if (_IssuedById == value) return;
				_IssuedById = value;
				_IssuedBy = null;
			}
		}
		private string _IssuedById;

		public virtual Country IssuedBy
		{
			get 
			{ 
				return _IssuedBy ?? (_IssuedBy = db?.Countries.ById(_IssuedById)); 
			}
			set
			{
				_IssuedBy = value;
				_IssuedById = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Passport);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Passports;

		internal Passport n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Passport)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Passport)value;
		}

		public class Lazy<TValue> : Lazy<Passport, TValue>
		{
			public Lazy(Func<Passport, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Passport, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Passport, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Passport operator +(Passport r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Passport operator |(Passport r1, Passport r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Passport
			{
				Number = e.Property(a => a.Number).OriginalValue,
				FirstName = e.Property(a => a.FirstName).OriginalValue,
				MiddleName = e.Property(a => a.MiddleName).OriginalValue,
				LastName = e.Property(a => a.LastName).OriginalValue,
				Birthday = e.Property(a => a.Birthday).OriginalValue,
				Gender = e.Property(a => a.Gender).OriginalValue,
				ExpiredOn = e.Property(a => a.ExpiredOn).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				CitizenshipId = e.Property(a => a.CitizenshipId).OriginalValue,
				IssuedById = e.Property(a => a.IssuedById).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Passport).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Passport> entity);
		public static void Config(Domain.EntityConfiguration<Passport> entity) => Config_(entity);

	}


	[Localization(typeof(Passport))]
	public class PassportReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public PassportReference() { }

		public PassportReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(PassportReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PassportReference operator +(PassportReference reference, DbSet<Passport> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static PassportReference operator +(PassportReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.Passports.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator PassportReference(Passport entity)
		{
			return entity == null ? null : new PassportReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PassportReference OneRef(this IQueryable<Passport> query)
		{
			return query?
				.Select(a => new PassportReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class PassportLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<Passport> query, ref IEnumerable<PassportLookup> lookupList);

		public static IEnumerable<PassportLookup> SelectAndOrderByName(IQueryable<Passport> query)
		{
			IEnumerable<PassportLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PassportLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<PassportLookup> DefaultLookup(LookupParams<Passport, PassportLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Pasteboard

	partial class Pasteboard
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Pasteboard);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Pasteboards;

		internal Pasteboard n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Pasteboard)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Pasteboard)value;
		}

		public class Lazy<TValue> : Lazy<Pasteboard, TValue>
		{
			public Lazy(Func<Pasteboard, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Pasteboard, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Pasteboard, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Pasteboard operator +(Pasteboard r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Pasteboard operator |(Pasteboard r1, Pasteboard r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Pasteboard
			{
				Number = e.Property(a => a.Number).OriginalValue,
				DeparturePlace = e.Property(a => a.DeparturePlace).OriginalValue,
				DepartureDate = e.Property(a => a.DepartureDate).OriginalValue,
				DepartureTime = e.Property(a => a.DepartureTime).OriginalValue,
				ArrivalPlace = e.Property(a => a.ArrivalPlace).OriginalValue,
				ArrivalDate = e.Property(a => a.ArrivalDate).OriginalValue,
				ArrivalTime = e.Property(a => a.ArrivalTime).OriginalValue,
				TrainNumber = e.Property(a => a.TrainNumber).OriginalValue,
				CarNumber = e.Property(a => a.CarNumber).OriginalValue,
				SeatNumber = e.Property(a => a.SeatNumber).OriginalValue,
				ServiceClass = e.Property(a => a.ServiceClass).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Pasteboard).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Pasteboard> entity);
		public static void Config(Domain.EntityConfiguration<Pasteboard> entity) => Config_(entity);

	}


	[Localization(typeof(Pasteboard))]
	public class PasteboardReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public PasteboardReference() { }

		public PasteboardReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(PasteboardReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PasteboardReference operator +(PasteboardReference reference, DbSet<Pasteboard> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static PasteboardReference operator +(PasteboardReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Pasteboards.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator PasteboardReference(Pasteboard entity)
		{
			return entity == null ? null : new PasteboardReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PasteboardReference OneRef(this IQueryable<Pasteboard> query)
		{
			return query?
				.Select(a => new PasteboardReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class PasteboardLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Pasteboard> query, ref IEnumerable<PasteboardLookup> lookupList);

		public static IEnumerable<PasteboardLookup> SelectAndOrderByName(IQueryable<Pasteboard> query)
		{
			IEnumerable<PasteboardLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PasteboardLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<PasteboardLookup> DefaultLookup(LookupParams<Pasteboard, PasteboardLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region PasteboardRefund

	partial class PasteboardRefund
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(PasteboardRefund);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).PasteboardRefunds;

		internal PasteboardRefund n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (PasteboardRefund)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (PasteboardRefund)value;
		}

		public class Lazy<TValue> : Lazy<PasteboardRefund, TValue>
		{
			public Lazy(Func<PasteboardRefund, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<PasteboardRefund, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<PasteboardRefund, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static PasteboardRefund operator +(PasteboardRefund r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static PasteboardRefund operator |(PasteboardRefund r1, PasteboardRefund r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new PasteboardRefund
			{
				Number = e.Property(a => a.Number).OriginalValue,
				DeparturePlace = e.Property(a => a.DeparturePlace).OriginalValue,
				DepartureDate = e.Property(a => a.DepartureDate).OriginalValue,
				DepartureTime = e.Property(a => a.DepartureTime).OriginalValue,
				ArrivalPlace = e.Property(a => a.ArrivalPlace).OriginalValue,
				ArrivalDate = e.Property(a => a.ArrivalDate).OriginalValue,
				ArrivalTime = e.Property(a => a.ArrivalTime).OriginalValue,
				TrainNumber = e.Property(a => a.TrainNumber).OriginalValue,
				CarNumber = e.Property(a => a.CarNumber).OriginalValue,
				SeatNumber = e.Property(a => a.SeatNumber).OriginalValue,
				ServiceClass = e.Property(a => a.ServiceClass).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(PasteboardRefund).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<PasteboardRefund> entity);
		public static void Config(Domain.EntityConfiguration<PasteboardRefund> entity) => Config_(entity);

	}


	[Localization(typeof(PasteboardRefund))]
	public class PasteboardRefundReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public PasteboardRefundReference() { }

		public PasteboardRefundReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(PasteboardRefundReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PasteboardRefundReference operator +(PasteboardRefundReference reference, DbSet<PasteboardRefund> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static PasteboardRefundReference operator +(PasteboardRefundReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.PasteboardRefunds.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator PasteboardRefundReference(PasteboardRefund entity)
		{
			return entity == null ? null : new PasteboardRefundReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PasteboardRefundReference OneRef(this IQueryable<PasteboardRefund> query)
		{
			return query?
				.Select(a => new PasteboardRefundReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class PasteboardRefundLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<PasteboardRefund> query, ref IEnumerable<PasteboardRefundLookup> lookupList);

		public static IEnumerable<PasteboardRefundLookup> SelectAndOrderByName(IQueryable<PasteboardRefund> query)
		{
			IEnumerable<PasteboardRefundLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PasteboardRefundLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<PasteboardRefundLookup> DefaultLookup(LookupParams<PasteboardRefund, PasteboardRefundLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Payment

	partial class Payment
	{
	//1
		public Payment()
		{
			Amount = new Money();
			Vat = new Money();
		}

		[ForeignKey("Payer")]
		public string PayerId
		{ 
			get { return _PayerId; }
			set 
			{
				if (_PayerId == value) return;
				_PayerId = value;
				_Payer = null;
			}
		}
		private string _PayerId;

		public virtual Party Payer
		{
			get 
			{ 
				return _Payer ?? (_Payer = db?.Parties.ById(_PayerId)); 
			}
			set
			{
				_Payer = value;
				_PayerId = value?.Id;
			}
		}

		[ForeignKey("Order")]
		public string OrderId
		{ 
			get { return _OrderId; }
			set 
			{
				if (_OrderId == value) return;
				_OrderId = value;
				_Order = null;
			}
		}
		private string _OrderId;

		public virtual Order Order
		{
			get 
			{ 
				return _Order ?? (_Order = db?.Orders.ById(_OrderId)); 
			}
			set
			{
				_Order = value;
				_OrderId = value?.Id;
			}
		}

		[ForeignKey("Invoice")]
		public string InvoiceId
		{ 
			get { return _InvoiceId; }
			set 
			{
				if (_InvoiceId == value) return;
				_InvoiceId = value;
				_Invoice = null;
			}
		}
		private string _InvoiceId;

		public virtual Invoice Invoice
		{
			get 
			{ 
				return _Invoice ?? (_Invoice = db?.Invoices.ById(_InvoiceId)); 
			}
			set
			{
				_Invoice = value;
				_InvoiceId = value?.Id;
			}
		}

		[ForeignKey("AssignedTo")]
		public string AssignedToId
		{ 
			get { return _AssignedToId; }
			set 
			{
				if (_AssignedToId == value) return;
				_AssignedToId = value;
				_AssignedTo = null;
			}
		}
		private string _AssignedToId;

		public virtual Person AssignedTo
		{
			get 
			{ 
				return _AssignedTo ?? (_AssignedTo = db?.Persons.ById(_AssignedToId)); 
			}
			set
			{
				_AssignedTo = value;
				_AssignedToId = value?.Id;
			}
		}

		[ForeignKey("RegisteredBy")]
		public string RegisteredById
		{ 
			get { return _RegisteredById; }
			set 
			{
				if (_RegisteredById == value) return;
				_RegisteredById = value;
				_RegisteredBy = null;
			}
		}
		private string _RegisteredById;

		public virtual Person RegisteredBy
		{
			get 
			{ 
				return _RegisteredBy ?? (_RegisteredBy = db?.Persons.ById(_RegisteredById)); 
			}
			set
			{
				_RegisteredBy = value;
				_RegisteredById = value?.Id;
			}
		}

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Party Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Parties.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}

		[ForeignKey("PaymentSystem")]
		public string PaymentSystemId
		{ 
			get { return _PaymentSystemId; }
			set 
			{
				if (_PaymentSystemId == value) return;
				_PaymentSystemId = value;
				_PaymentSystem = null;
			}
		}
		private string _PaymentSystemId;

		public virtual PaymentSystem PaymentSystem
		{
			get 
			{ 
				return _PaymentSystem ?? (_PaymentSystem = db?.PaymentSystems.ById(_PaymentSystemId)); 
			}
			set
			{
				_PaymentSystem = value;
				_PaymentSystemId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Payments;

		internal Payment n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Payment)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Payment)value;
		}

		public class Lazy<TValue> : Lazy<Payment, TValue>
		{
			public Lazy(Func<Payment, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Payment, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Payment, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Payment operator +(Payment r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Payment operator |(Payment r1, Payment r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<Payment> entity);
		public static void Config(Domain.EntityConfiguration<Payment> entity) => Config_(entity);

	}


	[Localization(typeof(Payment))]
	public class PaymentReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string _Type { get; set; }
			
		public PaymentReference() { }

		public PaymentReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(PaymentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PaymentReference operator +(PaymentReference reference, DbSet<Payment> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static PaymentReference operator +(PaymentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.Payments.Where(a => a.Id == id).Select(a => new { a.Number, a.PaymentForm }).FirstOrDefault().Do(a =>
			{
				reference.Number = a.Number;
				reference._Type = a.PaymentForm.AsString();
			});

			return reference;
		}

		public static implicit operator PaymentReference(Payment entity)
		{
			return entity == null ? null : new PaymentReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PaymentReference OneRef(this IQueryable<Payment> query)
		{
			return query?
				.Select(a => new PaymentReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class PaymentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<Payment> query, ref IEnumerable<PaymentLookup> lookupList);

		public static IEnumerable<PaymentLookup> SelectAndOrderByName(IQueryable<Payment> query)
		{
			IEnumerable<PaymentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PaymentLookup { Id = a.Id, Number = a.Number, _Type = a.PaymentForm.ToString() })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<PaymentLookup> DefaultLookup(LookupParams<Payment, PaymentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region PaymentSystem

	partial class PaymentSystem
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(PaymentSystem);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).PaymentSystems;

		internal PaymentSystem n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (PaymentSystem)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (PaymentSystem)value;
		}

		public class Lazy<TValue> : Lazy<PaymentSystem, TValue>
		{
			public Lazy(Func<PaymentSystem, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<PaymentSystem, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<PaymentSystem, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static PaymentSystem operator +(PaymentSystem r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static PaymentSystem operator |(PaymentSystem r1, PaymentSystem r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new PaymentSystem
			{
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(PaymentSystem).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<PaymentSystem> entity);
		public static void Config(Domain.EntityConfiguration<PaymentSystem> entity) => Config_(entity);

	}


	[Localization(typeof(PaymentSystem))]
	public class PaymentSystemReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public PaymentSystemReference() { }

		public PaymentSystemReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(PaymentSystemReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PaymentSystemReference operator +(PaymentSystemReference reference, DbSet<PaymentSystem> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static PaymentSystemReference operator +(PaymentSystemReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.PaymentSystems.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator PaymentSystemReference(PaymentSystem entity)
		{
			return entity == null ? null : new PaymentSystemReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PaymentSystemReference OneRef(this IQueryable<PaymentSystem> query)
		{
			return query?
				.Select(a => new PaymentSystemReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class PaymentSystemLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<PaymentSystem> query, ref IEnumerable<PaymentSystemLookup> lookupList);

		public static IEnumerable<PaymentSystemLookup> SelectAndOrderByName(IQueryable<PaymentSystem> query)
		{
			IEnumerable<PaymentSystemLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PaymentSystemLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<PaymentSystemLookup> DefaultLookup(LookupParams<PaymentSystem, PaymentSystemLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Person

	partial class Person
	{
	//1

		[ForeignKey("Organization")]
		public string OrganizationId
		{ 
			get { return _OrganizationId; }
			set 
			{
				if (_OrganizationId == value) return;
				_OrganizationId = value;
				_Organization = null;
			}
		}
		private string _OrganizationId;

		public virtual Organization Organization
		{
			get 
			{ 
				return _Organization ?? (_Organization = db?.Organizations.ById(_OrganizationId)); 
			}
			set
			{
				_Organization = value;
				_OrganizationId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Person);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Persons;

		internal Person n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Person)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Person)value;
		}

		public class Lazy<TValue> : Lazy<Person, TValue>
		{
			public Lazy(Func<Person, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Person, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Person, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Person operator +(Person r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Person operator |(Person r1, Person r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Person
			{
				MilesCardsString = e.Property(a => a.MilesCardsString).OriginalValue,
				Birthday = e.Property(a => a.Birthday).OriginalValue,
				Title = e.Property(a => a.Title).OriginalValue,
				BonusCardNumber = e.Property(a => a.BonusCardNumber).OriginalValue,
				OrganizationId = e.Property(a => a.OrganizationId).OriginalValue,
				LegalName = e.Property(a => a.LegalName).OriginalValue,
				Code = e.Property(a => a.Code).OriginalValue,
				Phone1 = e.Property(a => a.Phone1).OriginalValue,
				Phone2 = e.Property(a => a.Phone2).OriginalValue,
				Fax = e.Property(a => a.Fax).OriginalValue,
				Email1 = e.Property(a => a.Email1).OriginalValue,
				Email2 = e.Property(a => a.Email2).OriginalValue,
				WebAddress = e.Property(a => a.WebAddress).OriginalValue,
				IsCustomer = e.Property(a => a.IsCustomer).OriginalValue,
				IsSupplier = e.Property(a => a.IsSupplier).OriginalValue,
				Details = e.Property(a => a.Details).OriginalValue,
				LegalAddress = e.Property(a => a.LegalAddress).OriginalValue,
				ActualAddress = e.Property(a => a.ActualAddress).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				ReportsToId = e.Property(a => a.ReportsToId).OriginalValue,
				DefaultBankAccountId = e.Property(a => a.DefaultBankAccountId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Person).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Person> entity);
		public static void Config(Domain.EntityConfiguration<Person> entity) => Config_(entity);

	}


	[Localization(typeof(Person))]
	public class PersonReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public PersonReference() { }

		public PersonReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(PersonReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static PersonReference operator +(PersonReference reference, DbSet<Person> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static PersonReference operator +(PersonReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Persons.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator PersonReference(Person entity)
		{
			return entity == null ? null : new PersonReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static PersonReference OneRef(this IQueryable<Person> query)
		{
			return query?
				.Select(a => new PersonReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class PersonLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Person> query, ref IEnumerable<PersonLookup> lookupList);

		public static IEnumerable<PersonLookup> SelectAndOrderByName(IQueryable<Person> query)
		{
			IEnumerable<PersonLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new PersonLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<PersonLookup> DefaultLookup(LookupParams<Person, PersonLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Product

	partial class Product
	{
	//1
		public Product()
		{
			Fare = new Money();
			EqualFare = new Money();
			FeesTotal = new Money();
			CancelFee = new Money();
			Total = new Money();
			Vat = new Money();
			ServiceFee = new Money();
			ServiceFeePenalty = new Money();
			Handling = new Money();
			Commission = new Money();
			CommissionDiscount = new Money();
			Discount = new Money();
			BonusDiscount = new Money();
			BonusAccumulation = new Money();
			RefundServiceFee = new Money();
			GrandTotal = new Money();
			CancelCommission = new Money();
		}

		[ForeignKey("Producer")]
		public string ProducerId
		{ 
			get { return _ProducerId; }
			set 
			{
				if (_ProducerId == value) return;
				_ProducerId = value;
				_Producer = null;
			}
		}
		private string _ProducerId;

		public virtual Organization Producer
		{
			get 
			{ 
				return _Producer ?? (_Producer = db?.Organizations.ById(_ProducerId)); 
			}
			set
			{
				_Producer = value;
				_ProducerId = value?.Id;
			}
		}

		[ForeignKey("Provider")]
		public string ProviderId
		{ 
			get { return _ProviderId; }
			set 
			{
				if (_ProviderId == value) return;
				_ProviderId = value;
				_Provider = null;
			}
		}
		private string _ProviderId;

		public virtual Organization Provider
		{
			get 
			{ 
				return _Provider ?? (_Provider = db?.Organizations.ById(_ProviderId)); 
			}
			set
			{
				_Provider = value;
				_ProviderId = value?.Id;
			}
		}

		[ForeignKey("ReissueFor")]
		public string ReissueForId
		{ 
			get { return _ReissueForId; }
			set 
			{
				if (_ReissueForId == value) return;
				_ReissueForId = value;
				_ReissueFor = null;
			}
		}
		private string _ReissueForId;

		public virtual Product ReissueFor
		{
			get 
			{ 
				return _ReissueFor ?? (_ReissueFor = db?.Products.ById(_ReissueForId)); 
			}
			set
			{
				_ReissueFor = value;
				_ReissueForId = value?.Id;
			}
		}

		[ForeignKey("RefundedProduct")]
		public string RefundedProductId
		{ 
			get { return _RefundedProductId; }
			set 
			{
				if (_RefundedProductId == value) return;
				_RefundedProductId = value;
				_RefundedProduct = null;
			}
		}
		private string _RefundedProductId;

		public virtual Product RefundedProduct
		{
			get 
			{ 
				return _RefundedProduct ?? (_RefundedProduct = db?.Products.ById(_RefundedProductId)); 
			}
			set
			{
				_RefundedProduct = value;
				_RefundedProductId = value?.Id;
			}
		}

		[ForeignKey("Customer")]
		public string CustomerId
		{ 
			get { return _CustomerId; }
			set 
			{
				if (_CustomerId == value) return;
				_CustomerId = value;
				_Customer = null;
			}
		}
		private string _CustomerId;

		public virtual Party Customer
		{
			get 
			{ 
				return _Customer ?? (_Customer = db?.Parties.ById(_CustomerId)); 
			}
			set
			{
				_Customer = value;
				_CustomerId = value?.Id;
			}
		}

		[ForeignKey("Order")]
		public string OrderId
		{ 
			get { return _OrderId; }
			set 
			{
				if (_OrderId == value) return;
				_OrderId = value;
				_Order = null;
			}
		}
		private string _OrderId;

		public virtual Order Order
		{
			get 
			{ 
				return _Order ?? (_Order = db?.Orders.ById(_OrderId)); 
			}
			set
			{
				_Order = value;
				_OrderId = value?.Id;
			}
		}

		[ForeignKey("Intermediary")]
		public string IntermediaryId
		{ 
			get { return _IntermediaryId; }
			set 
			{
				if (_IntermediaryId == value) return;
				_IntermediaryId = value;
				_Intermediary = null;
			}
		}
		private string _IntermediaryId;

		public virtual Party Intermediary
		{
			get 
			{ 
				return _Intermediary ?? (_Intermediary = db?.Parties.ById(_IntermediaryId)); 
			}
			set
			{
				_Intermediary = value;
				_IntermediaryId = value?.Id;
			}
		}

		[ForeignKey("Country")]
		public string CountryId
		{ 
			get { return _CountryId; }
			set 
			{
				if (_CountryId == value) return;
				_CountryId = value;
				_Country = null;
			}
		}
		private string _CountryId;

		public virtual Country Country
		{
			get 
			{ 
				return _Country ?? (_Country = db?.Countries.ById(_CountryId)); 
			}
			set
			{
				_Country = value;
				_CountryId = value?.Id;
			}
		}

		[ForeignKey("Booker")]
		public string BookerId
		{ 
			get { return _BookerId; }
			set 
			{
				if (_BookerId == value) return;
				_BookerId = value;
				_Booker = null;
			}
		}
		private string _BookerId;

		public virtual Person Booker
		{
			get 
			{ 
				return _Booker ?? (_Booker = db?.Persons.ById(_BookerId)); 
			}
			set
			{
				_Booker = value;
				_BookerId = value?.Id;
			}
		}

		[ForeignKey("Ticketer")]
		public string TicketerId
		{ 
			get { return _TicketerId; }
			set 
			{
				if (_TicketerId == value) return;
				_TicketerId = value;
				_Ticketer = null;
			}
		}
		private string _TicketerId;

		public virtual Person Ticketer
		{
			get 
			{ 
				return _Ticketer ?? (_Ticketer = db?.Persons.ById(_TicketerId)); 
			}
			set
			{
				_Ticketer = value;
				_TicketerId = value?.Id;
			}
		}

		[ForeignKey("Seller")]
		public string SellerId
		{ 
			get { return _SellerId; }
			set 
			{
				if (_SellerId == value) return;
				_SellerId = value;
				_Seller = null;
			}
		}
		private string _SellerId;

		public virtual Person Seller
		{
			get 
			{ 
				return _Seller ?? (_Seller = db?.Persons.ById(_SellerId)); 
			}
			set
			{
				_Seller = value;
				_SellerId = value?.Id;
			}
		}

		[ForeignKey("Owner")]
		public string OwnerId
		{ 
			get { return _OwnerId; }
			set 
			{
				if (_OwnerId == value) return;
				_OwnerId = value;
				_Owner = null;
			}
		}
		private string _OwnerId;

		public virtual Party Owner
		{
			get 
			{ 
				return _Owner ?? (_Owner = db?.Parties.ById(_OwnerId)); 
			}
			set
			{
				_Owner = value;
				_OwnerId = value?.Id;
			}
		}

		[ForeignKey("OriginalDocument")]
		public string OriginalDocumentId
		{ 
			get { return _OriginalDocumentId; }
			set 
			{
				if (_OriginalDocumentId == value) return;
				_OriginalDocumentId = value;
				_OriginalDocument = null;
			}
		}
		private string _OriginalDocumentId;

		public virtual GdsFile OriginalDocument
		{
			get 
			{ 
				return _OriginalDocument ?? (_OriginalDocument = db?.GdsFiles.ById(_OriginalDocumentId)); 
			}
			set
			{
				_OriginalDocument = value;
				_OriginalDocumentId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Products;

		internal Product n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Product)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Product)value;
		}

		public class Lazy<TValue> : Lazy<Product, TValue>
		{
			public Lazy(Func<Product, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Product, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Product, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Product operator +(Product r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Product operator |(Product r1, Product r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<Product> entity);
		public static void Config(Domain.EntityConfiguration<Product> entity) => Config_(entity);

	}


	[Localization(typeof(Product))]
	public class ProductReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }
			
		public ProductReference() { }

		public ProductReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(ProductReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static ProductReference operator +(ProductReference reference, DbSet<Product> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static ProductReference operator +(ProductReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.Products.Where(a => a.Id == id).Select(a => new { a.Name, a.Type }).FirstOrDefault().Do(a =>
			{
				reference.Name = a.Name;
				reference._Type = a.Type.AsString();
			});

			return reference;
		}

		public static implicit operator ProductReference(Product entity)
		{
			return entity == null ? null : new ProductReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static ProductReference OneRef(this IQueryable<Product> query)
		{
			return query?
				.Select(a => new ProductReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class ProductLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Product> query, ref IEnumerable<ProductLookup> lookupList);

		public static IEnumerable<ProductLookup> SelectAndOrderByName(IQueryable<Product> query)
		{
			IEnumerable<ProductLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new ProductLookup { Id = a.Id, Name = a.Name, _Type = a.Type.ToString() })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<ProductLookup> DefaultLookup(LookupParams<Product, ProductLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region ProductPassenger

	partial class ProductPassenger
	{
	//1

		[ForeignKey("Product")]
		public string ProductId
		{ 
			get { return _ProductId; }
			set 
			{
				if (_ProductId == value) return;
				_ProductId = value;
				_Product = null;
			}
		}
		private string _ProductId;

		public virtual Product Product
		{
			get 
			{ 
				return _Product ?? (_Product = db?.Products.ById(_ProductId)); 
			}
			set
			{
				_Product = value;
				_ProductId = value?.Id;
			}
		}

		[ForeignKey("Passenger")]
		public string PassengerId
		{ 
			get { return _PassengerId; }
			set 
			{
				if (_PassengerId == value) return;
				_PassengerId = value;
				_Passenger = null;
			}
		}
		private string _PassengerId;

		public virtual Person Passenger
		{
			get 
			{ 
				return _Passenger ?? (_Passenger = db?.Persons.ById(_PassengerId)); 
			}
			set
			{
				_Passenger = value;
				_PassengerId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(ProductPassenger);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).ProductPassengers;

		internal ProductPassenger n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (ProductPassenger)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (ProductPassenger)value;
		}

		public class Lazy<TValue> : Lazy<ProductPassenger, TValue>
		{
			public Lazy(Func<ProductPassenger, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<ProductPassenger, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<ProductPassenger, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static ProductPassenger operator +(ProductPassenger r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static ProductPassenger operator |(ProductPassenger r1, ProductPassenger r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new ProductPassenger
			{
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProductId = e.Property(a => a.ProductId).OriginalValue,
				PassengerId = e.Property(a => a.PassengerId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(ProductPassenger).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<ProductPassenger> entity);
		public static void Config(Domain.EntityConfiguration<ProductPassenger> entity) => Config_(entity);

	}

	#endregion
	

	#region RailwayDocument

	partial class RailwayDocument
	{
	//1


		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).RailwayDocuments;

		internal RailwayDocument n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (RailwayDocument)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (RailwayDocument)value;
		}

		public class Lazy<TValue> : Lazy<RailwayDocument, TValue>
		{
			public Lazy(Func<RailwayDocument, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<RailwayDocument, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<RailwayDocument, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static RailwayDocument operator +(RailwayDocument r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static RailwayDocument operator |(RailwayDocument r1, RailwayDocument r2) => r1 ?? r2;


		static partial void Config_(Domain.EntityConfiguration<RailwayDocument> entity);
		public static void Config(Domain.EntityConfiguration<RailwayDocument> entity) => Config_(entity);

	}


	[Localization(typeof(RailwayDocument))]
	public class RailwayDocumentReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }
			
		public RailwayDocumentReference() { }

		public RailwayDocumentReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(RailwayDocumentReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static RailwayDocumentReference operator +(RailwayDocumentReference reference, DbSet<RailwayDocument> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static RailwayDocumentReference operator +(RailwayDocumentReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			db.RailwayDocuments.Where(a => a.Id == id).Select(a => new { a.Name, a.Type }).FirstOrDefault().Do(a =>
			{
				reference.Name = a.Name;
				reference._Type = a.Type.AsString();
			});

			return reference;
		}

		public static implicit operator RailwayDocumentReference(RailwayDocument entity)
		{
			return entity == null ? null : new RailwayDocumentReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static RailwayDocumentReference OneRef(this IQueryable<RailwayDocument> query)
		{
			return query?
				.Select(a => new RailwayDocumentReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class RailwayDocumentLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string _Type { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<RailwayDocument> query, ref IEnumerable<RailwayDocumentLookup> lookupList);

		public static IEnumerable<RailwayDocumentLookup> SelectAndOrderByName(IQueryable<RailwayDocument> query)
		{
			IEnumerable<RailwayDocumentLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new RailwayDocumentLookup { Id = a.Id, Name = a.Name, _Type = a.Type.ToString() })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<RailwayDocumentLookup> DefaultLookup(LookupParams<RailwayDocument, RailwayDocumentLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Sequence

	partial class Sequence
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Sequence);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Sequences;

		internal Sequence n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Sequence)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Sequence)value;
		}

		public class Lazy<TValue> : Lazy<Sequence, TValue>
		{
			public Lazy(Func<Sequence, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Sequence, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Sequence, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Sequence operator +(Sequence r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Sequence operator |(Sequence r1, Sequence r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Sequence
			{
				Name = e.Property(a => a.Name).OriginalValue,
				Discriminator = e.Property(a => a.Discriminator).OriginalValue,
				Current = e.Property(a => a.Current).OriginalValue,
				Format = e.Property(a => a.Format).OriginalValue,
				Timestamp = e.Property(a => a.Timestamp).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Sequence).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Sequence> entity);
		public static void Config(Domain.EntityConfiguration<Sequence> entity) => Config_(entity);

	}


	[Localization(typeof(Sequence))]
	public class SequenceReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public SequenceReference() { }

		public SequenceReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(SequenceReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static SequenceReference operator +(SequenceReference reference, DbSet<Sequence> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static SequenceReference operator +(SequenceReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Sequences.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator SequenceReference(Sequence entity)
		{
			return entity == null ? null : new SequenceReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static SequenceReference OneRef(this IQueryable<Sequence> query)
		{
			return query?
				.Select(a => new SequenceReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class SequenceLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Sequence> query, ref IEnumerable<SequenceLookup> lookupList);

		public static IEnumerable<SequenceLookup> SelectAndOrderByName(IQueryable<Sequence> query)
		{
			IEnumerable<SequenceLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new SequenceLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<SequenceLookup> DefaultLookup(LookupParams<Sequence, SequenceLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region SimCard

	partial class SimCard
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(SimCard);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).SimCards;

		internal SimCard n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (SimCard)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (SimCard)value;
		}

		public class Lazy<TValue> : Lazy<SimCard, TValue>
		{
			public Lazy(Func<SimCard, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<SimCard, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<SimCard, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static SimCard operator +(SimCard r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static SimCard operator |(SimCard r1, SimCard r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new SimCard
			{
				Type = e.Property(a => a.Type).OriginalValue,
				Number = e.Property(a => a.Number).OriginalValue,
				IsSale = e.Property(a => a.IsSale).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(SimCard).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<SimCard> entity);
		public static void Config(Domain.EntityConfiguration<SimCard> entity) => Config_(entity);

	}


	[Localization(typeof(SimCard))]
	public class SimCardReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public SimCardReference() { }

		public SimCardReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(SimCardReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static SimCardReference operator +(SimCardReference reference, DbSet<SimCard> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static SimCardReference operator +(SimCardReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.SimCards.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator SimCardReference(SimCard entity)
		{
			return entity == null ? null : new SimCardReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static SimCardReference OneRef(this IQueryable<SimCard> query)
		{
			return query?
				.Select(a => new SimCardReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class SimCardLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<SimCard> query, ref IEnumerable<SimCardLookup> lookupList);

		public static IEnumerable<SimCardLookup> SelectAndOrderByName(IQueryable<SimCard> query)
		{
			IEnumerable<SimCardLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new SimCardLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<SimCardLookup> DefaultLookup(LookupParams<SimCard, SimCardLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region SystemConfiguration

	partial class SystemConfiguration
	{
	//1

		[ForeignKey("Company")]
		public string CompanyId
		{ 
			get { return _CompanyId; }
			set 
			{
				if (_CompanyId == value) return;
				_CompanyId = value;
				_Company = null;
			}
		}
		private string _CompanyId;

		public virtual Organization Company
		{
			get 
			{ 
				return _Company ?? (_Company = db?.Organizations.ById(_CompanyId)); 
			}
			set
			{
				_Company = value;
				_CompanyId = value?.Id;
			}
		}

		[ForeignKey("Country")]
		public string CountryId
		{ 
			get { return _CountryId; }
			set 
			{
				if (_CountryId == value) return;
				_CountryId = value;
				_Country = null;
			}
		}
		private string _CountryId;

		public virtual Country Country
		{
			get 
			{ 
				return _Country ?? (_Country = db?.Countries.ById(_CountryId)); 
			}
			set
			{
				_Country = value;
				_CountryId = value?.Id;
			}
		}

		[ForeignKey("BirthdayTaskResponsible")]
		public string BirthdayTaskResponsibleId
		{ 
			get { return _BirthdayTaskResponsibleId; }
			set 
			{
				if (_BirthdayTaskResponsibleId == value) return;
				_BirthdayTaskResponsibleId = value;
				_BirthdayTaskResponsible = null;
			}
		}
		private string _BirthdayTaskResponsibleId;

		public virtual Person BirthdayTaskResponsible
		{
			get 
			{ 
				return _BirthdayTaskResponsible ?? (_BirthdayTaskResponsible = db?.Persons.ById(_BirthdayTaskResponsibleId)); 
			}
			set
			{
				_BirthdayTaskResponsible = value;
				_BirthdayTaskResponsibleId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(SystemConfiguration);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).SystemConfigurations;

		internal SystemConfiguration n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (SystemConfiguration)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (SystemConfiguration)value;
		}

		public class Lazy<TValue> : Lazy<SystemConfiguration, TValue>
		{
			public Lazy(Func<SystemConfiguration, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<SystemConfiguration, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<SystemConfiguration, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static SystemConfiguration operator +(SystemConfiguration r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static SystemConfiguration operator |(SystemConfiguration r1, SystemConfiguration r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new SystemConfiguration
			{
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				CompanyDetails = e.Property(a => a.CompanyDetails).OriginalValue,
				DefaultCurrency = e.Property(a => a.DefaultCurrency).OriginalValue,
				UseDefaultCurrencyForInput = e.Property(a => a.UseDefaultCurrencyForInput).OriginalValue,
				VatRate = e.Property(a => a.VatRate).OriginalValue,
				AmadeusRizUsingMode = e.Property(a => a.AmadeusRizUsingMode).OriginalValue,
				IsPassengerPassportRequired = e.Property(a => a.IsPassengerPassportRequired).OriginalValue,
				AviaOrderItemGenerationOption = e.Property(a => a.AviaOrderItemGenerationOption).OriginalValue,
				AllowAgentSetOrderVat = e.Property(a => a.AllowAgentSetOrderVat).OriginalValue,
				UseAviaDocumentVatInOrder = e.Property(a => a.UseAviaDocumentVatInOrder).OriginalValue,
				AviaDocumentVatOptions = e.Property(a => a.AviaDocumentVatOptions).OriginalValue,
				AccountantDisplayString = e.Property(a => a.AccountantDisplayString).OriginalValue,
				IncomingCashOrderCorrespondentAccount = e.Property(a => a.IncomingCashOrderCorrespondentAccount).OriginalValue,
				SeparateDocumentAccess = e.Property(a => a.SeparateDocumentAccess).OriginalValue,
				IsOrganizationCodeRequired = e.Property(a => a.IsOrganizationCodeRequired).OriginalValue,
				UseAviaHandling = e.Property(a => a.UseAviaHandling).OriginalValue,
				DaysBeforeDeparture = e.Property(a => a.DaysBeforeDeparture).OriginalValue,
				IsOrderRequiredForProcessedDocument = e.Property(a => a.IsOrderRequiredForProcessedDocument).OriginalValue,
				MetricsFromDate = e.Property(a => a.MetricsFromDate).OriginalValue,
				ReservationsInOfficeMetrics = e.Property(a => a.ReservationsInOfficeMetrics).OriginalValue,
				McoRequiresDescription = e.Property(a => a.McoRequiresDescription).OriginalValue,
				NeutralAirlineCode = e.Property(a => a.NeutralAirlineCode).OriginalValue,
				Order_UseServiceFeeOnlyInVat = e.Property(a => a.Order_UseServiceFeeOnlyInVat).OriginalValue,
				Invoice_NumberMode = e.Property(a => a.Invoice_NumberMode).OriginalValue,
				InvoicePrinter_FooterDetails = e.Property(a => a.InvoicePrinter_FooterDetails).OriginalValue,
				GalileoWebService_LoadedOn = e.Property(a => a.GalileoWebService_LoadedOn).OriginalValue,
				CompanyId = e.Property(a => a.CompanyId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BirthdayTaskResponsibleId = e.Property(a => a.BirthdayTaskResponsibleId).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(SystemConfiguration).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<SystemConfiguration> entity);
		public static void Config(Domain.EntityConfiguration<SystemConfiguration> entity) => Config_(entity);

	}

	#endregion
	

	#region Tour

	partial class Tour
	{
	//1

		[ForeignKey("AccommodationType")]
		public string AccommodationTypeId
		{ 
			get { return _AccommodationTypeId; }
			set 
			{
				if (_AccommodationTypeId == value) return;
				_AccommodationTypeId = value;
				_AccommodationType = null;
			}
		}
		private string _AccommodationTypeId;

		public virtual AccommodationType AccommodationType
		{
			get 
			{ 
				return _AccommodationType ?? (_AccommodationType = db?.AccommodationTypes.ById(_AccommodationTypeId)); 
			}
			set
			{
				_AccommodationType = value;
				_AccommodationTypeId = value?.Id;
			}
		}

		[ForeignKey("CateringType")]
		public string CateringTypeId
		{ 
			get { return _CateringTypeId; }
			set 
			{
				if (_CateringTypeId == value) return;
				_CateringTypeId = value;
				_CateringType = null;
			}
		}
		private string _CateringTypeId;

		public virtual CateringType CateringType
		{
			get 
			{ 
				return _CateringType ?? (_CateringType = db?.CateringTypes.ById(_CateringTypeId)); 
			}
			set
			{
				_CateringType = value;
				_CateringTypeId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Tour);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Tours;

		internal Tour n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Tour)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Tour)value;
		}

		public class Lazy<TValue> : Lazy<Tour, TValue>
		{
			public Lazy(Func<Tour, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Tour, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Tour, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Tour operator +(Tour r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Tour operator |(Tour r1, Tour r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Tour
			{
				HotelName = e.Property(a => a.HotelName).OriginalValue,
				HotelOffice = e.Property(a => a.HotelOffice).OriginalValue,
				HotelCode = e.Property(a => a.HotelCode).OriginalValue,
				PlacementName = e.Property(a => a.PlacementName).OriginalValue,
				PlacementOffice = e.Property(a => a.PlacementOffice).OriginalValue,
				PlacementCode = e.Property(a => a.PlacementCode).OriginalValue,
				AviaDescription = e.Property(a => a.AviaDescription).OriginalValue,
				TransferDescription = e.Property(a => a.TransferDescription).OriginalValue,
				AccommodationTypeId = e.Property(a => a.AccommodationTypeId).OriginalValue,
				CateringTypeId = e.Property(a => a.CateringTypeId).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Tour).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Tour> entity);
		public static void Config(Domain.EntityConfiguration<Tour> entity) => Config_(entity);

	}


	[Localization(typeof(Tour))]
	public class TourReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public TourReference() { }

		public TourReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(TourReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static TourReference operator +(TourReference reference, DbSet<Tour> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static TourReference operator +(TourReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Tours.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator TourReference(Tour entity)
		{
			return entity == null ? null : new TourReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static TourReference OneRef(this IQueryable<Tour> query)
		{
			return query?
				.Select(a => new TourReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class TourLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Tour> query, ref IEnumerable<TourLookup> lookupList);

		public static IEnumerable<TourLookup> SelectAndOrderByName(IQueryable<Tour> query)
		{
			IEnumerable<TourLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new TourLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<TourLookup> DefaultLookup(LookupParams<Tour, TourLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region Transfer

	partial class Transfer
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(Transfer);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Transfers;

		internal Transfer n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (Transfer)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (Transfer)value;
		}

		public class Lazy<TValue> : Lazy<Transfer, TValue>
		{
			public Lazy(Func<Transfer, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<Transfer, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<Transfer, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static Transfer operator +(Transfer r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static Transfer operator |(Transfer r1, Transfer r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new Transfer
			{
				Name = e.Property(a => a.Name).OriginalValue,
				IssueDate = e.Property(a => a.IssueDate).OriginalValue,
				IsRefund = e.Property(a => a.IsRefund).OriginalValue,
				IsReservation = e.Property(a => a.IsReservation).OriginalValue,
				IsProcessed = e.Property(a => a.IsProcessed).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				RequiresProcessing = e.Property(a => a.RequiresProcessing).OriginalValue,
				Itinerary = e.Property(a => a.Itinerary).OriginalValue,
				StartDate = e.Property(a => a.StartDate).OriginalValue,
				FinishDate = e.Property(a => a.FinishDate).OriginalValue,
				PnrCode = e.Property(a => a.PnrCode).OriginalValue,
				TourCode = e.Property(a => a.TourCode).OriginalValue,
				BookerOffice = e.Property(a => a.BookerOffice).OriginalValue,
				BookerCode = e.Property(a => a.BookerCode).OriginalValue,
				TicketerOffice = e.Property(a => a.TicketerOffice).OriginalValue,
				TicketerCode = e.Property(a => a.TicketerCode).OriginalValue,
				TicketingIataOffice = e.Property(a => a.TicketingIataOffice).OriginalValue,
				IsTicketerRobot = e.Property(a => a.IsTicketerRobot).OriginalValue,
				Fare = e.Property(a => a.Fare).OriginalValue,
				EqualFare = e.Property(a => a.EqualFare).OriginalValue,
				FeesTotal = e.Property(a => a.FeesTotal).OriginalValue,
				CancelFee = e.Property(a => a.CancelFee).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ServiceFee = e.Property(a => a.ServiceFee).OriginalValue,
				ServiceFeePenalty = e.Property(a => a.ServiceFeePenalty).OriginalValue,
				Handling = e.Property(a => a.Handling).OriginalValue,
				Commission = e.Property(a => a.Commission).OriginalValue,
				CommissionDiscount = e.Property(a => a.CommissionDiscount).OriginalValue,
				Discount = e.Property(a => a.Discount).OriginalValue,
				BonusDiscount = e.Property(a => a.BonusDiscount).OriginalValue,
				BonusAccumulation = e.Property(a => a.BonusAccumulation).OriginalValue,
				RefundServiceFee = e.Property(a => a.RefundServiceFee).OriginalValue,
				GrandTotal = e.Property(a => a.GrandTotal).OriginalValue,
				CancelCommissionPercent = e.Property(a => a.CancelCommissionPercent).OriginalValue,
				CancelCommission = e.Property(a => a.CancelCommission).OriginalValue,
				CommissionPercent = e.Property(a => a.CommissionPercent).OriginalValue,
				PaymentType = e.Property(a => a.PaymentType).OriginalValue,
				TaxRateOfProduct = e.Property(a => a.TaxRateOfProduct).OriginalValue,
				TaxRateOfServiceFee = e.Property(a => a.TaxRateOfServiceFee).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				Originator = e.Property(a => a.Originator).OriginalValue,
				Origin = e.Property(a => a.Origin).OriginalValue,
				PassengerName = e.Property(a => a.PassengerName).OriginalValue,
				ProducerId = e.Property(a => a.ProducerId).OriginalValue,
				ProviderId = e.Property(a => a.ProviderId).OriginalValue,
				ReissueForId = e.Property(a => a.ReissueForId).OriginalValue,
				RefundedProductId = e.Property(a => a.RefundedProductId).OriginalValue,
				CustomerId = e.Property(a => a.CustomerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				IntermediaryId = e.Property(a => a.IntermediaryId).OriginalValue,
				CountryId = e.Property(a => a.CountryId).OriginalValue,
				BookerId = e.Property(a => a.BookerId).OriginalValue,
				TicketerId = e.Property(a => a.TicketerId).OriginalValue,
				SellerId = e.Property(a => a.SellerId).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				OriginalDocumentId = e.Property(a => a.OriginalDocumentId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(Transfer).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<Transfer> entity);
		public static void Config(Domain.EntityConfiguration<Transfer> entity) => Config_(entity);

	}


	[Localization(typeof(Transfer))]
	public class TransferReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public TransferReference() { }

		public TransferReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(TransferReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static TransferReference operator +(TransferReference reference, DbSet<Transfer> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static TransferReference operator +(TransferReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Transfers.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator TransferReference(Transfer entity)
		{
			return entity == null ? null : new TransferReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static TransferReference OneRef(this IQueryable<Transfer> query)
		{
			return query?
				.Select(a => new TransferReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class TransferLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<Transfer> query, ref IEnumerable<TransferLookup> lookupList);

		public static IEnumerable<TransferLookup> SelectAndOrderByName(IQueryable<Transfer> query)
		{
			IEnumerable<TransferLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new TransferLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<TransferLookup> DefaultLookup(LookupParams<Transfer, TransferLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region User

	partial class User
	{
	//1

		[ForeignKey("Person")]
		public string PersonId
		{ 
			get { return _PersonId; }
			set 
			{
				if (_PersonId == value) return;
				_PersonId = value;
				_Person = null;
			}
		}
		private string _PersonId;

		public virtual Person Person
		{
			get 
			{ 
				return _Person ?? (_Person = db?.Persons.ById(_PersonId)); 
			}
			set
			{
				_Person = value;
				_PersonId = value?.Id;
			}
		}


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(User);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).Users;

		internal User n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (User)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (User)value;
		}

		public class Lazy<TValue> : Lazy<User, TValue>
		{
			public Lazy(Func<User, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<User, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<User, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static User operator +(User r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static User operator |(User r1, User r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new User
			{
				Password = e.Property(a => a.Password).OriginalValue,
				Active = e.Property(a => a.Active).OriginalValue,
				IsAdministrator = e.Property(a => a.IsAdministrator).OriginalValue,
				IsSupervisor = e.Property(a => a.IsSupervisor).OriginalValue,
				IsAgent = e.Property(a => a.IsAgent).OriginalValue,
				IsCashier = e.Property(a => a.IsCashier).OriginalValue,
				IsAnalyst = e.Property(a => a.IsAnalyst).OriginalValue,
				IsSubAgent = e.Property(a => a.IsSubAgent).OriginalValue,
				SessionId = e.Property(a => a.SessionId).OriginalValue,
				PersonId = e.Property(a => a.PersonId).OriginalValue,
				Description = e.Property(a => a.Description).OriginalValue,
				Name = e.Property(a => a.Name).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(User).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<User> entity);
		public static void Config(Domain.EntityConfiguration<User> entity) => Config_(entity);

	}


	[Localization(typeof(User))]
	public class UserReference : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }
			
		public UserReference() { }

		public UserReference(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public string GetName() => Name;

		public static implicit operator bool(UserReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static UserReference operator +(UserReference reference, DbSet<User> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Name = set.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();


			return reference;
		}

		public static UserReference operator +(UserReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Name = db.Users.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();

			return reference;
		}

		public static implicit operator UserReference(User entity)
		{
			return entity == null ? null : new UserReference { Id = entity.Id, Name = entity.Name };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static UserReference OneRef(this IQueryable<User> query)
		{
			return query?
				.Select(a => new UserReference { Id = a.Id, Name = a.Name })
				.FirstOrDefault();
		}
	}


	public partial class UserLookup : INameContainer
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string GetName()	=> Name;

		static partial void SelectAndOrderByName(IQueryable<User> query, ref IEnumerable<UserLookup> lookupList);

		public static IEnumerable<UserLookup> SelectAndOrderByName(IQueryable<User> query)
		{
			IEnumerable<UserLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new UserLookup { Id = a.Id, Name = a.Name })
				.OrderBy(a => a.Name);
		}

		public static IEnumerable<UserLookup> DefaultLookup(LookupParams<User, UserLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Name.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region WireTransfer

	partial class WireTransfer
	{
	//1


		[DebuggerStepThrough]
		public override Type GetClass() => typeof(WireTransfer);

		[DebuggerStepThrough]
		public override DbSet GetDbSet(Domain domain = null) => (domain ?? db).WireTransfers;

		internal WireTransfer n, o;

		protected override Domain.Entity GetNew() => n;
		protected override void SetNew(Domain.Entity value)
		{
			base.SetNew(value);
			n = (WireTransfer)value;
		}

		protected override Domain.Entity GetOld() => o;
		protected override void SetOld(Domain.Entity value)
		{
			base.SetOld(value);
			o = (WireTransfer)value;
		}

		public class Lazy<TValue> : Lazy<WireTransfer, TValue>
		{
			public Lazy(Func<WireTransfer, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<WireTransfer, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		public static PropertyInfo Property<TValue>(Expression<Func<WireTransfer, TValue>> property)
		{
			return property.GetProperty();
		}


		[DebuggerStepThrough]
		public static WireTransfer operator +(WireTransfer r, Domain db) => r.Resolve(db);

		[DebuggerStepThrough]
		public static WireTransfer operator |(WireTransfer r1, WireTransfer r2) => r1 ?? r2;

		public override Domain.Entity GetFromDb()
		{
			var e = db.Entry(this);

			var r = new WireTransfer
			{
				Number = e.Property(a => a.Number).OriginalValue,
				Date = e.Property(a => a.Date).OriginalValue,
				DocumentNumber = e.Property(a => a.DocumentNumber).OriginalValue,
				Amount = e.Property(a => a.Amount).OriginalValue,
				Vat = e.Property(a => a.Vat).OriginalValue,
				ReceivedFrom = e.Property(a => a.ReceivedFrom).OriginalValue,
				PostedOn = e.Property(a => a.PostedOn).OriginalValue,
				Note = e.Property(a => a.Note).OriginalValue,
				IsVoid = e.Property(a => a.IsVoid).OriginalValue,
				PrintedDocument = e.Property(a => a.PrintedDocument).OriginalValue,
				PayerId = e.Property(a => a.PayerId).OriginalValue,
				OrderId = e.Property(a => a.OrderId).OriginalValue,
				InvoiceId = e.Property(a => a.InvoiceId).OriginalValue,
				AssignedToId = e.Property(a => a.AssignedToId).OriginalValue,
				RegisteredById = e.Property(a => a.RegisteredById).OriginalValue,
				OwnerId = e.Property(a => a.OwnerId).OriginalValue,
				PaymentSystemId = e.Property(a => a.PaymentSystemId).OriginalValue,
				CreatedOn = e.Property(a => a.CreatedOn).OriginalValue,
				CreatedBy = e.Property(a => a.CreatedBy).OriginalValue,
				ModifiedOn = e.Property(a => a.ModifiedOn).OriginalValue,
				ModifiedBy = e.Property(a => a.ModifiedBy).OriginalValue,
				Version = e.Property(a => a.Version).OriginalValue,
				Id = e.Property(a => a.Id).OriginalValue,
			};

			return r.Domain(db);
		}


		public override LocalizationText Localization(string lang) => Localization()[lang];
		
		public static LocalizationAttribute Localization()
		{
			return _localization ?? (_localization = typeof(WireTransfer).Localization(Domain.DefaultLocalizationTypesSource));
		}
		static LocalizationAttribute _localization;


		static partial void Config_(Domain.EntityConfiguration<WireTransfer> entity);
		public static void Config(Domain.EntityConfiguration<WireTransfer> entity) => Config_(entity);

	}


	[Localization(typeof(WireTransfer))]
	public class WireTransferReference : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }
			
		public WireTransferReference() { }

		public WireTransferReference(string id, string name)
		{
			Id = id;
			Number = name;
		}

		public string GetName() => Number;

		public static implicit operator bool(WireTransferReference reference)
		{
			return reference != null && reference.Id.Yes();
		}

		public static WireTransferReference operator +(WireTransferReference reference, DbSet<WireTransfer> set)
		{
			if (reference == null) return null;
			if (set == null) return reference;

			var id = reference.Id;

			reference.Number = set.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();


			return reference;
		}

		public static WireTransferReference operator +(WireTransferReference reference, Domain db)
		{
			if (reference == null) return null;
			if (db == null) return reference;

			var id = reference.Id;

			reference.Number = db.WireTransfers.Where(a => a.Id == id).Select(a => a.Number).FirstOrDefault();

			return reference;
		}

		public static implicit operator WireTransferReference(WireTransfer entity)
		{
			return entity == null ? null : new WireTransferReference { Id = entity.Id, Number = entity.Number };
		}

	}

	
	public static partial class EntityReferenceExtension
	{
		public static WireTransferReference OneRef(this IQueryable<WireTransfer> query)
		{
			return query?
				.Select(a => new WireTransferReference { Id = a.Id, Number = a.Number })
				.FirstOrDefault();
		}
	}


	public partial class WireTransferLookup : INameContainer
	{

		public string Id { get; set; }

		public string Number { get; set; }

		public string GetName()	=> Number;

		static partial void SelectAndOrderByName(IQueryable<WireTransfer> query, ref IEnumerable<WireTransferLookup> lookupList);

		public static IEnumerable<WireTransferLookup> SelectAndOrderByName(IQueryable<WireTransfer> query)
		{
			IEnumerable<WireTransferLookup> lookupList = null;
			SelectAndOrderByName(query, ref lookupList);

			// ReSharper disable once ConstantNullCoalescingCondition
			return lookupList ?? query
				.Select(a => new WireTransferLookup { Id = a.Id, Number = a.Number })
				.OrderBy(a => a.Number);
		}

		public static IEnumerable<WireTransferLookup> DefaultLookup(LookupParams<WireTransfer, WireTransferLookup> p)
		{
			return p.GetList(p.Filter.No() ? null : p.Where(a => a.Number.Contains(p.Filter)));
		}

	}

	#endregion
	

	#region EverydayProfitReportParams

	partial class EverydayProfitReportParams
	{ 
		public string ProductId { get; set; }		
		public string CountryId { get; set; }		
		public string OrderId { get; set; }		
		public string PayerId { get; set; }		
		public string InvoiceId { get; set; }		
		public string CompletionCertificateId { get; set; }		
		public string PaymentId { get; set; }		
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: EverydayProfitReport

		public ProductType ProductType { get; set; }		

		public ProductReference Product { get; set; }		

		public string PassengerName { get; set; }		

		public string Itinerary { get; set; }		

		public DateTimeOffset? StartDate { get; set; }		

		public DateTimeOffset? FinishDate { get; set; }		

		public CountryReference Country { get; set; }		

		public Money Fare { get; set; }		

		public string Currency { get; set; }		

		public decimal? CurrencyRate { get; set; }		

		public decimal? EqualFare { get; set; }		

		public decimal? FeesTotal { get; set; }		

		public decimal? CancelFee { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? Commission { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? Vat { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public OrderReference Order { get; set; }		

		public PartyReference Payer { get; set; }		

		public InvoiceReference Invoice { get; set; }		

		public DateTimeOffset? InvoiceDate { get; set; }		

		public InvoiceReference CompletionCertificate { get; set; }		

		public DateTimeOffset? CompletionCertificateDate { get; set; }		

		public PaymentReference Payment { get; set; }		

		public DateTimeOffset? PaymentDate { get; set; }		
	}

	#endregion


	#region FlownReportParams

	partial class FlownReportParams
	{ 
		public string TicketNumberId { get; set; }		
		public string ClientId { get; set; }		
		public string CheapTicketId { get; set; }		
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: FlownReport

		public DateTimeOffset Date { get; set; }		

		public string Op { get; set; }		

		public string AC { get; set; }		

		public AviaDocumentReference TicketNumber { get; set; }		

		public PartyReference Client { get; set; }		

		public string Passenger { get; set; }		

		public string Route { get; set; }		

		public string Curr { get; set; }		

		public decimal? Fare { get; set; }		

		public decimal? Tax { get; set; }		

		public decimal? Flown1 { get; set; }		

		public decimal? Flown2 { get; set; }		

		public decimal? Flown3 { get; set; }		

		public decimal? Flown4 { get; set; }		

		public decimal? Flown5 { get; set; }		

		public decimal? Flown6 { get; set; }		

		public decimal? Flown7 { get; set; }		

		public decimal? Flown8 { get; set; }		

		public decimal? Flown9 { get; set; }		

		public decimal? Flown10 { get; set; }		

		public decimal? Flown11 { get; set; }		

		public decimal? Flown12 { get; set; }		

		public string TourCode { get; set; }		

		public AviaDocumentReference CheapTicket { get; set; }		
	}

	#endregion


	#region OrderBalanceParams

	partial class OrderBalanceParams
	{ 
		public string OrderId { get; set; }		
		public string CustomerId { get; set; }		
		// entity.BaseType: Object
		// entity.QueryResultType: OrderBalance

		public string Id { get; set; }		

		public OrderReference Order { get; set; }		

		public DateTimeOffset? IssueDate { get; set; }		

		public PartyReference Customer { get; set; }		

		public string Currency { get; set; }		

		public decimal Delivered { get; set; }		

		public decimal Paid { get; set; }		

		public decimal Balance { get; set; }		
	}

	#endregion


	#region ProductFilter

	partial class ProductFilter
	{ 
		public string ProviderId { get; set; }		
		public string CustomerId { get; set; }		
		public string BookerId { get; set; }		
		public string TicketerId { get; set; }		
		public string SellerId { get; set; }		
		public string OwnerId { get; set; }		
	}

	#endregion


	#region ProductSummaryParams

	partial class ProductSummaryParams
	{ 
		public string OrderId { get; set; }		
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductSummary

		public string Itinerary { get; set; }		

		public bool IsRefund { get; set; }		

		public Money Total { get; set; }		

		public Money ServiceFee { get; set; }		

		public Money GrandTotal { get; set; }		

		public OrderReference Order { get; set; }		
	}

	#endregion


	#region ProductTotalByBookerParams

	partial class ProductTotalByBookerParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByBooker

		public int Rank { get; set; }		

		public string BookerName { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByDayParams

	partial class ProductTotalByDayParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByDay

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByMonthParams

	partial class ProductTotalByMonthParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByMonth

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByOwnerParams

	partial class ProductTotalByOwnerParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByOwner

		public int Rank { get; set; }		

		public string OwnerName { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByProviderParams

	partial class ProductTotalByProviderParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByProvider

		public int Rank { get; set; }		

		public string ProviderName { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByQuarterParams

	partial class ProductTotalByQuarterParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByQuarter

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalBySellerParams

	partial class ProductTotalBySellerParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalBySeller

		public int Rank { get; set; }		

		public string SellerName { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByTypeParams

	partial class ProductTotalByTypeParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByType

		public int Rank { get; set; }		

		public string TypeName { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProductTotalByYearParams

	partial class ProductTotalByYearParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProductTotalByYear

		public int Year { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public string Note { get; set; }		
	}

	#endregion


	#region ProfitDistributionByCustomerParams

	partial class ProfitDistributionByCustomerParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProfitDistributionByCustomer

		public int Rank { get; set; }		

		public int SellCount { get; set; }		

		public int RefundCount { get; set; }		

		public int VoidCount { get; set; }		

		public string Currency { get; set; }		

		public decimal? SellGrandTotal { get; set; }		

		public decimal? RefundGrandTotal { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? Commission { get; set; }		

		public decimal? AgentTotal { get; set; }		

		public decimal? Vat { get; set; }		
	}

	#endregion


	#region ProfitDistributionByProviderParams

	partial class ProfitDistributionByProviderParams
	{ 
		// entity.BaseType: ProductFilter
		// entity.QueryResultType: ProfitDistributionByProvider

		public int Rank { get; set; }		

		public int SellCount { get; set; }		

		public int RefundCount { get; set; }		

		public int VoidCount { get; set; }		

		public string Currency { get; set; }		

		public decimal? SellGrandTotal { get; set; }		

		public decimal? RefundGrandTotal { get; set; }		

		public decimal? GrandTotal { get; set; }		

		public decimal? Total { get; set; }		

		public decimal? ServiceFee { get; set; }		

		public decimal? Commission { get; set; }		

		public decimal? AgentTotal { get; set; }		

		public decimal? Vat { get; set; }		
	}

	#endregion


	#region EverydayProfitReport

	partial class EverydayProfitReport
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<EverydayProfitReport> sm)
		{
			sm.Patterns(( a) => new EverydayProfitReport
			{
				Id = a.Id,
				Provider = a.Provider,
				ProductType = a.ProductType,
				Product = a.Product,
				IssueDate = a.IssueDate,
				Seller = a.Seller,
				PassengerName = a.PassengerName,
				Itinerary = a.Itinerary,
				StartDate = a.StartDate,
				FinishDate = a.FinishDate,
				Country = a.Country,
				Fare = a.Fare,
				Currency = a.Currency,
				CurrencyRate = a.CurrencyRate,
				EqualFare = a.EqualFare,
				FeesTotal = a.FeesTotal,
				CancelFee = a.CancelFee,
				Total = a.Total,
				Commission = a.Commission,
				ServiceFee = a.ServiceFee,
				Vat = a.Vat,
				GrandTotal = a.GrandTotal,
				Order = a.Order,
				Payer = a.Payer,
				Invoice = a.Invoice,
				InvoiceDate = a.InvoiceDate,
				CompletionCertificate = a.CompletionCertificate,
				CompletionCertificateDate = a.CompletionCertificateDate,
				Payment = a.Payment,
				PaymentDate = a.PaymentDate,
			});
		}
		*/
	}

	#endregion


	#region FlownReport

	partial class FlownReport
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<FlownReport> sm)
		{
			sm.Patterns(( a) => new FlownReport
			{
				Id = a.Id,
				Date = a.Date,
				Op = a.Op,
				AC = a.AC,
				TicketNumber = a.TicketNumber,
				Client = a.Client,
				Passenger = a.Passenger,
				Route = a.Route,
				Curr = a.Curr,
				Fare = a.Fare,
				Tax = a.Tax,
				Flown1 = a.Flown1,
				Flown2 = a.Flown2,
				Flown3 = a.Flown3,
				Flown4 = a.Flown4,
				Flown5 = a.Flown5,
				Flown6 = a.Flown6,
				Flown7 = a.Flown7,
				Flown8 = a.Flown8,
				Flown9 = a.Flown9,
				Flown10 = a.Flown10,
				Flown11 = a.Flown11,
				Flown12 = a.Flown12,
				TourCode = a.TourCode,
				CheapTicket = a.CheapTicket,
			});
		}
		*/
	}

	#endregion


	#region OrderBalance

	partial class OrderBalance
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<OrderBalance> sm)
		{
			sm.Patterns(( a) => new OrderBalance
			{
				Id = a.Id,
				Order = a.Order,
				IssueDate = a.IssueDate,
				Customer = a.Customer,
				Currency = a.Currency,
				Delivered = a.Delivered,
				Paid = a.Paid,
				Balance = a.Balance,
			});
		}
		*/
	}

	#endregion


	#region ProductSummary

	partial class ProductSummary
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductSummary> sm)
		{
			sm.Patterns(( a) => new ProductSummary
			{
				Id = a.Id,
				IssueDate = a.IssueDate,
				Type = a.Type,
				Name = a.Name,
				Itinerary = a.Itinerary,
				IsRefund = a.IsRefund,
				Total = a.Total,
				ServiceFee = a.ServiceFee,
				GrandTotal = a.GrandTotal,
				Order = a.Order,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByBooker

	partial class ProductTotalByBooker
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByBooker> sm)
		{
			sm.Patterns(( a) => new ProductTotalByBooker
			{
				Rank = a.Rank,
				BookerName = a.BookerName,
				Booker = a.Booker,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByDay

	partial class ProductTotalByDay
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByDay> sm)
		{
			sm.Patterns(( a) => new ProductTotalByDay
			{
				IssueDate = a.IssueDate,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByMonth

	partial class ProductTotalByMonth
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByMonth> sm)
		{
			sm.Patterns(( a) => new ProductTotalByMonth
			{
				IssueDate = a.IssueDate,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByOwner

	partial class ProductTotalByOwner
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByOwner> sm)
		{
			sm.Patterns(( a) => new ProductTotalByOwner
			{
				Rank = a.Rank,
				OwnerName = a.OwnerName,
				Owner = a.Owner,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByProvider

	partial class ProductTotalByProvider
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByProvider> sm)
		{
			sm.Patterns(( a) => new ProductTotalByProvider
			{
				Rank = a.Rank,
				ProviderName = a.ProviderName,
				Provider = a.Provider,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByQuarter

	partial class ProductTotalByQuarter
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByQuarter> sm)
		{
			sm.Patterns(( a) => new ProductTotalByQuarter
			{
				IssueDate = a.IssueDate,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalBySeller

	partial class ProductTotalBySeller
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalBySeller> sm)
		{
			sm.Patterns(( a) => new ProductTotalBySeller
			{
				Rank = a.Rank,
				SellerName = a.SellerName,
				Seller = a.Seller,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByType

	partial class ProductTotalByType
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByType> sm)
		{
			sm.Patterns(( a) => new ProductTotalByType
			{
				Rank = a.Rank,
				Type = a.Type,
				TypeName = a.TypeName,
			});
		}
		*/
	}

	#endregion


	#region ProductTotalByYear

	partial class ProductTotalByYear
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProductTotalByYear> sm)
		{
			sm.Patterns(( a) => new ProductTotalByYear
			{
				Year = a.Year,
			});
		}
		*/
	}

	#endregion


	#region ProfitDistributionByCustomer

	partial class ProfitDistributionByCustomer
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProfitDistributionByCustomer> sm)
		{
			sm.Patterns(( a) => new ProfitDistributionByCustomer
			{
				Customer = a.Customer,
			});
		}
		*/
	}

	#endregion


	#region ProfitDistributionByProvider

	partial class ProfitDistributionByProvider
	{ 
		/*
		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<ProfitDistributionByProvider> sm)
		{
			sm.Patterns(( a) => new ProfitDistributionByProvider
			{
				Provider = a.Provider,
			});
		}
		*/
	}

	#endregion


	#region GdsAgent_ApplyToUnassigned

	partial class GdsAgent_ApplyToUnassigned
	{

		[ForeignKey("GdsAgent")]
		public string GdsAgentId
		{ 
			get { return _GdsAgentId; }
			set 
			{
				if (_GdsAgentId == value) return;
				_GdsAgentId = value;
				_GdsAgent = null;
			}
		}
		private string _GdsAgentId;

		public virtual GdsAgent GdsAgent
		{
			get 
			{ 
				return _GdsAgent ?? (_GdsAgent = db?.GdsAgents.ById(GdsAgentId)); 
			}
			set
			{
				_GdsAgent = value;
				_GdsAgentId = value?.Id;
			}
		}
	}

	#endregion



	partial class Domain
	{

		partial void InitQueries()
		{
			AccommodationProviders = new AccommodationProvider { db = this };
			ActiveOwners = new ActiveOwner { db = this };
			Agents = new Agent { db = this };
			Airlines = new Airline { db = this };
			BusTicketProviders = new BusTicketProvider { db = this };
			CarRentalProviders = new CarRentalProvider { db = this };
			Customers = new Customer { db = this };
			Employees = new Employee { db = this };
			GenericProductProviders = new GenericProductProvider { db = this };
			InsuranceCompanies = new InsuranceCompany { db = this };
			PasteboardProviders = new PasteboardProvider { db = this };
			Receipts = new Receipt { db = this };
			RoamingOperators = new RoamingOperator { db = this };
			TourProviders = new TourProvider { db = this };
			TransferProviders = new TransferProvider { db = this };
		}


		static partial void ConfigEntityInfos_(EntityInfo[] entities)
		{
			Accommodation.Config(entities.Config<Accommodation>());
			AccommodationType.Config(entities.Config<AccommodationType>());
			AirlineServiceClass.Config(entities.Config<AirlineServiceClass>());
			Airport.Config(entities.Config<Airport>());
			AviaDocument.Config(entities.Config<AviaDocument>());
			AviaMco.Config(entities.Config<AviaMco>());
			AviaRefund.Config(entities.Config<AviaRefund>());
			AviaTicket.Config(entities.Config<AviaTicket>());
			BankAccount.Config(entities.Config<BankAccount>());
			BusDocument.Config(entities.Config<BusDocument>());
			BusTicket.Config(entities.Config<BusTicket>());
			BusTicketRefund.Config(entities.Config<BusTicketRefund>());
			CarRental.Config(entities.Config<CarRental>());
			CashInOrderPayment.Config(entities.Config<CashInOrderPayment>());
			CashOutOrderPayment.Config(entities.Config<CashOutOrderPayment>());
			CateringType.Config(entities.Config<CateringType>());
			CheckPayment.Config(entities.Config<CheckPayment>());
			Consignment.Config(entities.Config<Consignment>());
			Country.Config(entities.Config<Country>());
			CurrencyDailyRate.Config(entities.Config<CurrencyDailyRate>());
			Department.Config(entities.Config<Department>());
			DocumentAccess.Config(entities.Config<DocumentAccess>());
			DocumentOwner.Config(entities.Config<DocumentOwner>());
			ElectronicPayment.Config(entities.Config<ElectronicPayment>());
			Excursion.Config(entities.Config<Excursion>());
			File.Config(entities.Config<File>());
			FlightSegment.Config(entities.Config<FlightSegment>());
			GdsAgent.Config(entities.Config<GdsAgent>());
			GdsFile.Config(entities.Config<GdsFile>());
			GenericProduct.Config(entities.Config<GenericProduct>());
			GenericProductType.Config(entities.Config<GenericProductType>());
			Identity.Config(entities.Config<Identity>());
			Insurance.Config(entities.Config<Insurance>());
			InsuranceDocument.Config(entities.Config<InsuranceDocument>());
			InsuranceRefund.Config(entities.Config<InsuranceRefund>());
			InternalIdentity.Config(entities.Config<InternalIdentity>());
			InternalTransfer.Config(entities.Config<InternalTransfer>());
			Invoice.Config(entities.Config<Invoice>());
			Isic.Config(entities.Config<Isic>());
			IssuedConsignment.Config(entities.Config<IssuedConsignment>());
			MilesCard.Config(entities.Config<MilesCard>());
			OpeningBalance.Config(entities.Config<OpeningBalance>());
			Order.Config(entities.Config<Order>());
			OrderCheck.Config(entities.Config<OrderCheck>());
			OrderItem.Config(entities.Config<OrderItem>());
			Organization.Config(entities.Config<Organization>());
			Party.Config(entities.Config<Party>());
			Passport.Config(entities.Config<Passport>());
			Pasteboard.Config(entities.Config<Pasteboard>());
			PasteboardRefund.Config(entities.Config<PasteboardRefund>());
			Payment.Config(entities.Config<Payment>());
			PaymentSystem.Config(entities.Config<PaymentSystem>());
			Person.Config(entities.Config<Person>());
			Product.Config(entities.Config<Product>());
			ProductPassenger.Config(entities.Config<ProductPassenger>());
			RailwayDocument.Config(entities.Config<RailwayDocument>());
			Sequence.Config(entities.Config<Sequence>());
			SimCard.Config(entities.Config<SimCard>());
			SystemConfiguration.Config(entities.Config<SystemConfiguration>());
			Tour.Config(entities.Config<Tour>());
			Transfer.Config(entities.Config<Transfer>());
			User.Config(entities.Config<User>());
			WireTransfer.Config(entities.Config<WireTransfer>());
		}


		public class DomainModel
		{

			public EntityTypeConfiguration<Accommodation> Accommodation;

			public EntityTypeConfiguration<AccommodationType> AccommodationType;

			public EntityTypeConfiguration<AirlineServiceClass> AirlineServiceClass;

			public EntityTypeConfiguration<Airport> Airport;

			public EntityTypeConfiguration<AviaDocument> AviaDocument;

			public EntityTypeConfiguration<AviaMco> AviaMco;

			public EntityTypeConfiguration<AviaRefund> AviaRefund;

			public EntityTypeConfiguration<AviaTicket> AviaTicket;

			public EntityTypeConfiguration<BankAccount> BankAccount;

			public EntityTypeConfiguration<BusDocument> BusDocument;

			public EntityTypeConfiguration<BusTicket> BusTicket;

			public EntityTypeConfiguration<BusTicketRefund> BusTicketRefund;

			public EntityTypeConfiguration<CarRental> CarRental;

			public EntityTypeConfiguration<CashInOrderPayment> CashInOrderPayment;

			public EntityTypeConfiguration<CashOutOrderPayment> CashOutOrderPayment;

			public EntityTypeConfiguration<CateringType> CateringType;

			public EntityTypeConfiguration<CheckPayment> CheckPayment;

			public EntityTypeConfiguration<Consignment> Consignment;

			public EntityTypeConfiguration<Country> Country;

			public EntityTypeConfiguration<CurrencyDailyRate> CurrencyDailyRate;

			public EntityTypeConfiguration<Department> Department;

			public EntityTypeConfiguration<DocumentAccess> DocumentAccess;

			public EntityTypeConfiguration<DocumentOwner> DocumentOwner;

			public EntityTypeConfiguration<ElectronicPayment> ElectronicPayment;

			public EntityTypeConfiguration<Excursion> Excursion;

			public EntityTypeConfiguration<File> File;

			public EntityTypeConfiguration<FlightSegment> FlightSegment;

			public EntityTypeConfiguration<GdsAgent> GdsAgent;

			public EntityTypeConfiguration<GdsFile> GdsFile;

			public EntityTypeConfiguration<GenericProduct> GenericProduct;

			public EntityTypeConfiguration<GenericProductType> GenericProductType;

			public EntityTypeConfiguration<Identity> Identity;

			public EntityTypeConfiguration<Insurance> Insurance;

			public EntityTypeConfiguration<InsuranceDocument> InsuranceDocument;

			public EntityTypeConfiguration<InsuranceRefund> InsuranceRefund;

			public EntityTypeConfiguration<InternalIdentity> InternalIdentity;

			public EntityTypeConfiguration<InternalTransfer> InternalTransfer;

			public EntityTypeConfiguration<Invoice> Invoice;

			public EntityTypeConfiguration<Isic> Isic;

			public EntityTypeConfiguration<IssuedConsignment> IssuedConsignment;

			public EntityTypeConfiguration<MilesCard> MilesCard;

			public EntityTypeConfiguration<OpeningBalance> OpeningBalance;

			public EntityTypeConfiguration<Order> Order;

			public EntityTypeConfiguration<OrderCheck> OrderCheck;

			public EntityTypeConfiguration<OrderItem> OrderItem;

			public EntityTypeConfiguration<Organization> Organization;

			public EntityTypeConfiguration<Party> Party;

			public EntityTypeConfiguration<Passport> Passport;

			public EntityTypeConfiguration<Pasteboard> Pasteboard;

			public EntityTypeConfiguration<PasteboardRefund> PasteboardRefund;

			public EntityTypeConfiguration<Payment> Payment;

			public EntityTypeConfiguration<PaymentSystem> PaymentSystem;

			public EntityTypeConfiguration<Person> Person;

			public EntityTypeConfiguration<Product> Product;

			public EntityTypeConfiguration<ProductPassenger> ProductPassenger;

			public EntityTypeConfiguration<RailwayDocument> RailwayDocument;

			public EntityTypeConfiguration<Sequence> Sequence;

			public EntityTypeConfiguration<SimCard> SimCard;

			public EntityTypeConfiguration<SystemConfiguration> SystemConfiguration;

			public EntityTypeConfiguration<Tour> Tour;

			public EntityTypeConfiguration<Transfer> Transfer;

			public EntityTypeConfiguration<User> User;

			public EntityTypeConfiguration<WireTransfer> WireTransfer;


			public DomainModel(DbModelBuilder mb)
			{
				Accommodation = mb.Entity<Accommodation>();
				AccommodationType = mb.Entity<AccommodationType>();
				AirlineServiceClass = mb.Entity<AirlineServiceClass>();
				Airport = mb.Entity<Airport>();
				AviaDocument = mb.Entity<AviaDocument>();
				AviaMco = mb.Entity<AviaMco>();
				AviaRefund = mb.Entity<AviaRefund>();
				AviaTicket = mb.Entity<AviaTicket>();
				BankAccount = mb.Entity<BankAccount>();
				BusDocument = mb.Entity<BusDocument>();
				BusTicket = mb.Entity<BusTicket>();
				BusTicketRefund = mb.Entity<BusTicketRefund>();
				CarRental = mb.Entity<CarRental>();
				CashInOrderPayment = mb.Entity<CashInOrderPayment>();
				CashOutOrderPayment = mb.Entity<CashOutOrderPayment>();
				CateringType = mb.Entity<CateringType>();
				CheckPayment = mb.Entity<CheckPayment>();
				Consignment = mb.Entity<Consignment>();
				Country = mb.Entity<Country>();
				CurrencyDailyRate = mb.Entity<CurrencyDailyRate>();
				Department = mb.Entity<Department>();
				DocumentAccess = mb.Entity<DocumentAccess>();
				DocumentOwner = mb.Entity<DocumentOwner>();
				ElectronicPayment = mb.Entity<ElectronicPayment>();
				Excursion = mb.Entity<Excursion>();
				File = mb.Entity<File>();
				FlightSegment = mb.Entity<FlightSegment>();
				GdsAgent = mb.Entity<GdsAgent>();
				GdsFile = mb.Entity<GdsFile>();
				GenericProduct = mb.Entity<GenericProduct>();
				GenericProductType = mb.Entity<GenericProductType>();
				Identity = mb.Entity<Identity>();
				Insurance = mb.Entity<Insurance>();
				InsuranceDocument = mb.Entity<InsuranceDocument>();
				InsuranceRefund = mb.Entity<InsuranceRefund>();
				InternalIdentity = mb.Entity<InternalIdentity>();
				InternalTransfer = mb.Entity<InternalTransfer>();
				Invoice = mb.Entity<Invoice>();
				Isic = mb.Entity<Isic>();
				IssuedConsignment = mb.Entity<IssuedConsignment>();
				MilesCard = mb.Entity<MilesCard>();
				OpeningBalance = mb.Entity<OpeningBalance>();
				Order = mb.Entity<Order>();
				OrderCheck = mb.Entity<OrderCheck>();
				OrderItem = mb.Entity<OrderItem>();
				Organization = mb.Entity<Organization>();
				Party = mb.Entity<Party>();
				Passport = mb.Entity<Passport>();
				Pasteboard = mb.Entity<Pasteboard>();
				PasteboardRefund = mb.Entity<PasteboardRefund>();
				Payment = mb.Entity<Payment>();
				PaymentSystem = mb.Entity<PaymentSystem>();
				Person = mb.Entity<Person>();
				Product = mb.Entity<Product>();
				ProductPassenger = mb.Entity<ProductPassenger>();
				RailwayDocument = mb.Entity<RailwayDocument>();
				Sequence = mb.Entity<Sequence>();
				SimCard = mb.Entity<SimCard>();
				SystemConfiguration = mb.Entity<SystemConfiguration>();
				Tour = mb.Entity<Tour>();
				Transfer = mb.Entity<Transfer>();
				User = mb.Entity<User>();
				WireTransfer = mb.Entity<WireTransfer>();

				Payment.Ignore(a => a.SavePosted);
				Product.Ignore(a => a.GdsPassengerName);
				Product.Ignore(a => a.PassengerId);
				User.Ignore(a => a.NewPassword);
				User.Ignore(a => a.ConfirmPassword);
				User.Ignore(a => a.Roles);

				Accommodation.HasOptional(a => a.AccommodationType);
				Accommodation.HasOptional(a => a.CateringType);
				AirlineServiceClass.HasOptional(a => a.Airline, a => a.AirlineServiceClasses);
				Airport.HasOptional(a => a.Country, a => a.Airports);
				AviaMco.HasOptional(a => a.InConnectionWith, a => a.AviaMcos_InConnectionWith);
				Consignment.HasOptional(a => a.Supplier);
				Consignment.HasOptional(a => a.Acquirer);
				Department.HasOptional(a => a.Organization, a => a.Departments);
				DocumentAccess.HasOptional(a => a.Person, a => a.DocumentAccesses);
				DocumentAccess.HasOptional(a => a.Owner);
				DocumentOwner.HasOptional(a => a.Owner, a => a.DocumentOwners);
				File.HasOptional(a => a.Party, a => a.Files);
				File.HasOptional(a => a.UploadedBy);
				FlightSegment.HasOptional(a => a.Ticket, a => a.Segments);
				FlightSegment.HasOptional(a => a.FromAirport);
				FlightSegment.HasOptional(a => a.ToAirport);
				FlightSegment.HasOptional(a => a.Carrier);
				GdsAgent.HasOptional(a => a.Person, a => a.GdsAgents);
				GdsAgent.HasOptional(a => a.Office);
				GenericProduct.HasOptional(a => a.GenericType);
				InternalTransfer.HasOptional(a => a.FromOrder, a => a.OutgoingTransfers);
				InternalTransfer.HasOptional(a => a.FromParty);
				InternalTransfer.HasOptional(a => a.ToOrder, a => a.IncomingTransfers);
				InternalTransfer.HasOptional(a => a.ToParty);
				Invoice.HasOptional(a => a.Order, a => a.Invoices);
				Invoice.HasOptional(a => a.IssuedBy);
				IssuedConsignment.HasOptional(a => a.Consignment, a => a.IssuedConsignments);
				IssuedConsignment.HasOptional(a => a.IssuedBy);
				MilesCard.HasOptional(a => a.Owner, a => a.MilesCards);
				MilesCard.HasOptional(a => a.Organization, a => a.MilesCards);
				OpeningBalance.HasOptional(a => a.Party);
				Order.HasOptional(a => a.Customer);
				Order.HasOptional(a => a.BillTo);
				Order.HasOptional(a => a.ShipTo);
				Order.HasOptional(a => a.AssignedTo);
				Order.HasOptional(a => a.BankAccount);
				Order.HasOptional(a => a.Owner);
				OrderCheck.HasOptional(a => a.Order);
				OrderCheck.HasOptional(a => a.Person);
				OrderItem.HasOptional(a => a.Order, a => a.Items);
				OrderItem.HasOptional(a => a.Product);
				OrderItem.HasOptional(a => a.Consignment, a => a.OrderItems);
				Party.HasOptional(a => a.ReportsTo);
				Party.HasOptional(a => a.DefaultBankAccount);
				Passport.HasOptional(a => a.Owner, a => a.Passports);
				Passport.HasOptional(a => a.Citizenship);
				Passport.HasOptional(a => a.IssuedBy);
				Payment.HasOptional(a => a.Payer);
				Payment.HasOptional(a => a.Order, a => a.Payments);
				Payment.HasOptional(a => a.Invoice, a => a.Payments);
				Payment.HasOptional(a => a.AssignedTo);
				Payment.HasOptional(a => a.RegisteredBy);
				Payment.HasOptional(a => a.Owner);
				Payment.HasOptional(a => a.PaymentSystem);
				Person.HasOptional(a => a.Organization, a => a.Employees);
				Product.HasOptional(a => a.Producer);
				Product.HasOptional(a => a.Provider);
				Product.HasOptional(a => a.ReissueFor, a => a.Products_ReissueFor);
				Product.HasOptional(a => a.RefundedProduct, a => a.Products_RefundedProduct);
				Product.HasOptional(a => a.Customer);
				Product.HasOptional(a => a.Order, a => a.Products);
				Product.HasOptional(a => a.Intermediary);
				Product.HasOptional(a => a.Country);
				Product.HasOptional(a => a.Booker);
				Product.HasOptional(a => a.Ticketer);
				Product.HasOptional(a => a.Seller);
				Product.HasOptional(a => a.Owner);
				Product.HasOptional(a => a.OriginalDocument, a => a.Products);
				ProductPassenger.HasOptional(a => a.Product, a => a.Passengers);
				ProductPassenger.HasOptional(a => a.Passenger);
				SystemConfiguration.HasOptional(a => a.Company);
				SystemConfiguration.HasOptional(a => a.Country);
				SystemConfiguration.HasOptional(a => a.BirthdayTaskResponsible);
				Tour.HasOptional(a => a.AccommodationType);
				Tour.HasOptional(a => a.CateringType);
				User.HasOptional(a => a.Person);

				mb.Types<AviaTicket>().Configure(c =>
				{
					c.Property(a => a.FareTotal.Amount).HasColumnName("faretotal_amount");
					c.Property(a => a.FareTotal.CurrencyId).HasColumnName("faretotal_currency");
				});
				mb.Types<Consignment>().Configure(c =>
				{
					c.Property(a => a.GrandTotal.Amount).HasColumnName("grandtotal_amount");
					c.Property(a => a.GrandTotal.CurrencyId).HasColumnName("grandtotal_currency");
					c.Property(a => a.Vat.Amount).HasColumnName("vat_amount");
					c.Property(a => a.Vat.CurrencyId).HasColumnName("vat_currency");
					c.Property(a => a.Discount.Amount).HasColumnName("discount_amount");
					c.Property(a => a.Discount.CurrencyId).HasColumnName("discount_currency");
				});
				mb.Types<FlightSegment>().Configure(c =>
				{
					c.Property(a => a.Amount.Amount).HasColumnName("amount_amount");
					c.Property(a => a.Amount.CurrencyId).HasColumnName("amount_currency");
					c.Property(a => a.CouponAmount.Amount).HasColumnName("couponamount_amount");
					c.Property(a => a.CouponAmount.CurrencyId).HasColumnName("couponamount_currency");
				});
				mb.Types<Invoice>().Configure(c =>
				{
					c.Property(a => a.Total.Amount).HasColumnName("total_amount");
					c.Property(a => a.Total.CurrencyId).HasColumnName("total_currency");
					c.Property(a => a.Vat.Amount).HasColumnName("vat_amount");
					c.Property(a => a.Vat.CurrencyId).HasColumnName("vat_currency");
				});
				mb.Types<Order>().Configure(c =>
				{
					c.Property(a => a.Discount.Amount).HasColumnName("discount_amount");
					c.Property(a => a.Discount.CurrencyId).HasColumnName("discount_currency");
					c.Property(a => a.Total.Amount).HasColumnName("total_amount");
					c.Property(a => a.Total.CurrencyId).HasColumnName("total_currency");
					c.Property(a => a.Vat.Amount).HasColumnName("vat_amount");
					c.Property(a => a.Vat.CurrencyId).HasColumnName("vat_currency");
					c.Property(a => a.Paid.Amount).HasColumnName("paid_amount");
					c.Property(a => a.Paid.CurrencyId).HasColumnName("paid_currency");
					c.Property(a => a.TotalDue.Amount).HasColumnName("totaldue_amount");
					c.Property(a => a.TotalDue.CurrencyId).HasColumnName("totaldue_currency");
					c.Property(a => a.VatDue.Amount).HasColumnName("vatdue_amount");
					c.Property(a => a.VatDue.CurrencyId).HasColumnName("vatdue_currency");
				});
				mb.Types<OrderItem>().Configure(c =>
				{
					c.Property(a => a.Price.Amount).HasColumnName("price_amount");
					c.Property(a => a.Price.CurrencyId).HasColumnName("price_currency");
					c.Property(a => a.Discount.Amount).HasColumnName("discount_amount");
					c.Property(a => a.Discount.CurrencyId).HasColumnName("discount_currency");
					c.Property(a => a.GrandTotal.Amount).HasColumnName("grandtotal_amount");
					c.Property(a => a.GrandTotal.CurrencyId).HasColumnName("grandtotal_currency");
					c.Property(a => a.GivenVat.Amount).HasColumnName("givenvat_amount");
					c.Property(a => a.GivenVat.CurrencyId).HasColumnName("givenvat_currency");
					c.Property(a => a.TaxedTotal.Amount).HasColumnName("taxedtotal_amount");
					c.Property(a => a.TaxedTotal.CurrencyId).HasColumnName("taxedtotal_currency");
				});
				mb.Types<Payment>().Configure(c =>
				{
					c.Property(a => a.Amount.Amount).HasColumnName("amount_amount");
					c.Property(a => a.Amount.CurrencyId).HasColumnName("amount_currency");
					c.Property(a => a.Vat.Amount).HasColumnName("vat_amount");
					c.Property(a => a.Vat.CurrencyId).HasColumnName("vat_currency");
				});
				mb.Types<Product>().Configure(c =>
				{
					c.Property(a => a.Fare.Amount).HasColumnName("fare_amount");
					c.Property(a => a.Fare.CurrencyId).HasColumnName("fare_currency");
					c.Property(a => a.EqualFare.Amount).HasColumnName("equalfare_amount");
					c.Property(a => a.EqualFare.CurrencyId).HasColumnName("equalfare_currency");
					c.Property(a => a.FeesTotal.Amount).HasColumnName("feestotal_amount");
					c.Property(a => a.FeesTotal.CurrencyId).HasColumnName("feestotal_currency");
					c.Property(a => a.CancelFee.Amount).HasColumnName("cancelfee_amount");
					c.Property(a => a.CancelFee.CurrencyId).HasColumnName("cancelfee_currency");
					c.Property(a => a.Total.Amount).HasColumnName("total_amount");
					c.Property(a => a.Total.CurrencyId).HasColumnName("total_currency");
					c.Property(a => a.Vat.Amount).HasColumnName("vat_amount");
					c.Property(a => a.Vat.CurrencyId).HasColumnName("vat_currency");
					c.Property(a => a.ServiceFee.Amount).HasColumnName("servicefee_amount");
					c.Property(a => a.ServiceFee.CurrencyId).HasColumnName("servicefee_currency");
					c.Property(a => a.ServiceFeePenalty.Amount).HasColumnName("servicefeepenalty_amount");
					c.Property(a => a.ServiceFeePenalty.CurrencyId).HasColumnName("servicefeepenalty_currency");
					c.Property(a => a.Handling.Amount).HasColumnName("handling_amount");
					c.Property(a => a.Handling.CurrencyId).HasColumnName("handling_currency");
					c.Property(a => a.Commission.Amount).HasColumnName("commission_amount");
					c.Property(a => a.Commission.CurrencyId).HasColumnName("commission_currency");
					c.Property(a => a.CommissionDiscount.Amount).HasColumnName("commissiondiscount_amount");
					c.Property(a => a.CommissionDiscount.CurrencyId).HasColumnName("commissiondiscount_currency");
					c.Property(a => a.Discount.Amount).HasColumnName("discount_amount");
					c.Property(a => a.Discount.CurrencyId).HasColumnName("discount_currency");
					c.Property(a => a.BonusDiscount.Amount).HasColumnName("bonusdiscount_amount");
					c.Property(a => a.BonusDiscount.CurrencyId).HasColumnName("bonusdiscount_currency");
					c.Property(a => a.BonusAccumulation.Amount).HasColumnName("bonusaccumulation_amount");
					c.Property(a => a.BonusAccumulation.CurrencyId).HasColumnName("bonusaccumulation_currency");
					c.Property(a => a.RefundServiceFee.Amount).HasColumnName("refundservicefee_amount");
					c.Property(a => a.RefundServiceFee.CurrencyId).HasColumnName("refundservicefee_currency");
					c.Property(a => a.GrandTotal.Amount).HasColumnName("grandtotal_amount");
					c.Property(a => a.GrandTotal.CurrencyId).HasColumnName("grandtotal_currency");
					c.Property(a => a.CancelCommission.Amount).HasColumnName("cancelcommission_amount");
					c.Property(a => a.CancelCommission.CurrencyId).HasColumnName("cancelcommission_currency");
				});
			}
		}

	}

}