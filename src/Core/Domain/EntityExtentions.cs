using System;
using System.Collections.Generic;
using System.Diagnostics;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Domain.Entities;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Domain
{

	partial class Domain
	{
			
		public AccommodationProvider.Service AccommodationProvider { [DebuggerStepThrough] get { return ResolveService(ref _accommodationProvider); } }
		private AccommodationProvider.Service _accommodationProvider;
			
		public Airline.Service Airline { [DebuggerStepThrough] get { return ResolveService(ref _airline); } }
		private Airline.Service _airline;
			
		public BusTicketProvider.Service BusTicketProvider { [DebuggerStepThrough] get { return ResolveService(ref _busTicketProvider); } }
		private BusTicketProvider.Service _busTicketProvider;
			
		public CarRentalProvider.Service CarRentalProvider { [DebuggerStepThrough] get { return ResolveService(ref _carRentalProvider); } }
		private CarRentalProvider.Service _carRentalProvider;
			
		public Customer.Service Customer { [DebuggerStepThrough] get { return ResolveService(ref _customer); } }
		private Customer.Service _customer;
			
		public GenericProductProvider.Service GenericProductProvider { [DebuggerStepThrough] get { return ResolveService(ref _genericProductProvider); } }
		private GenericProductProvider.Service _genericProductProvider;
			
		public InsuranceCompany.Service InsuranceCompany { [DebuggerStepThrough] get { return ResolveService(ref _insuranceCompany); } }
		private InsuranceCompany.Service _insuranceCompany;
			
		public PasteboardProvider.Service PasteboardProvider { [DebuggerStepThrough] get { return ResolveService(ref _pasteboardProvider); } }
		private PasteboardProvider.Service _pasteboardProvider;
			
		public Receipt.Service Receipt { [DebuggerStepThrough] get { return ResolveService(ref _receipt); } }
		private Receipt.Service _receipt;
			
		public RoamingOperator.Service RoamingOperator { [DebuggerStepThrough] get { return ResolveService(ref _roamingOperator); } }
		private RoamingOperator.Service _roamingOperator;
			
		public TourProvider.Service TourProvider { [DebuggerStepThrough] get { return ResolveService(ref _tourProvider); } }
		private TourProvider.Service _tourProvider;
			
		public TransferProvider.Service TransferProvider { [DebuggerStepThrough] get { return ResolveService(ref _transferProvider); } }
		private TransferProvider.Service _transferProvider;

	}



	public partial class AccommodationProviderManager : EntityManager<Organization, AccommodationProvider.Service> { }

	public partial class AirlineManager : EntityManager<Organization, Airline.Service> { }

	public partial class BusTicketProviderManager : EntityManager<Organization, BusTicketProvider.Service> { }

	public partial class CarRentalProviderManager : EntityManager<Organization, CarRentalProvider.Service> { }

	public partial class CustomerManager : EntityManager<Party, Customer.Service> { }

	public partial class GenericProductProviderManager : EntityManager<Organization, GenericProductProvider.Service> { }

	public partial class InsuranceCompanyManager : EntityManager<Organization, InsuranceCompany.Service> { }

	public partial class PasteboardProviderManager : EntityManager<Organization, PasteboardProvider.Service> { }

	public partial class ReceiptManager : EntityManager<Invoice, Receipt.Service> { }

	public partial class RoamingOperatorManager : EntityManager<Organization, RoamingOperator.Service> { }

	public partial class TourProviderManager : EntityManager<Organization, TourProvider.Service> { }

	public partial class TransferProviderManager : EntityManager<Organization, TransferProvider.Service> { }

	
	partial class Domain
	{
			
		public AppService App { [DebuggerStepThrough] get { return ResolveService(ref _app); } }
		private AppService _app;
			
		public AppStateService AppState { [DebuggerStepThrough] get { return ResolveService(ref _appState); } }
		private AppStateService _appState;
			
		public ReportService Report { [DebuggerStepThrough] get { return ResolveService(ref _report); } }
		private ReportService _report;
			
		public SecurityService Security { [DebuggerStepThrough] get { return ResolveService(ref _security); } }
		private SecurityService _security;
			
		public SequenceService Sequence { [DebuggerStepThrough] get { return ResolveService(ref _sequence); } }
		private SequenceService _sequence;

	}

						
	#region Accommodation

	partial class Domain : IEntityServiceContainer<Domain, Accommodation>
	{

		public Accommodation.Service Accommodation { [DebuggerStepThrough] get { return ResolveService(ref _accommodation); } }
		private Accommodation.Service _accommodation;

		EntityService<Domain, Accommodation> IEntityServiceContainer<Domain, Accommodation>.Service => Accommodation;
		
		[DebuggerStepThrough]
		public static Accommodation operator +(Accommodation r, Domain db)
		{
			return (Accommodation)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Accommodation Export(Accommodation r)
		{
			Accommodation.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Accommodation> entities)
		{
			foreach (var r in entities)
				Accommodation.Export(r);
		}

	}

	partial class Accommodation
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Accommodation operator |(Accommodation r1, Accommodation r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Accommodation entity) : base(entity) { }

			public Reference(Accommodation entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Accommodation entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Accommodation operator +(Reference reference, Domain db)
			{
				return db.Accommodation.Load(reference);
			}

		}

	}

	public partial class AccommodationManager : ProductManager<Accommodation, Accommodation.Service> { }

	#endregion

						
	#region AccommodationType

	partial class Domain : IEntityServiceContainer<Domain, AccommodationType>
	{

		public AccommodationType.Service AccommodationType { [DebuggerStepThrough] get { return ResolveService(ref _accommodationType); } }
		private AccommodationType.Service _accommodationType;

		EntityService<Domain, AccommodationType> IEntityServiceContainer<Domain, AccommodationType>.Service => AccommodationType;
		
		[DebuggerStepThrough]
		public static AccommodationType operator +(AccommodationType r, Domain db)
		{
			return (AccommodationType)r?.Resolve(db);
		}

	}

	partial class AccommodationType
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AccommodationType operator |(AccommodationType r1, AccommodationType r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AccommodationType entity) : base(entity) { }

			public Reference(AccommodationType entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AccommodationType entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AccommodationType operator +(Reference reference, Domain db)
			{
				return db.AccommodationType.Load(reference);
			}

		}

	}

	public partial class AccommodationTypeManager : EntityManager<AccommodationType, AccommodationType.Service> { }

	#endregion

						
	#region AirFile

	partial class Domain : IEntityServiceContainer<Domain, AirFile>
	{

		public AirFile.Service AirFile { [DebuggerStepThrough] get { return ResolveService(ref _airFile); } }
		private AirFile.Service _airFile;

		EntityService<Domain, AirFile> IEntityServiceContainer<Domain, AirFile>.Service => AirFile;
		
		[DebuggerStepThrough]
		public static AirFile operator +(AirFile r, Domain db)
		{
			return (AirFile)r?.Resolve(db);
		}

	}

	partial class AirFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AirFile operator |(AirFile r1, AirFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AirFile entity) : base(entity) { }

			public Reference(AirFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AirFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AirFile operator +(Reference reference, Domain db)
			{
				return db.AirFile.Load(reference);
			}

		}

	}

	public partial class AirFileManager : EntityManager<AirFile, AirFile.Service> { }

	#endregion

						
	#region AirlineCommissionPercents

	partial class Domain : IEntityServiceContainer<Domain, AirlineCommissionPercents>
	{

		public AirlineCommissionPercents.Service AirlineCommissionPercents { [DebuggerStepThrough] get { return ResolveService(ref _airlineCommissionPercents); } }
		private AirlineCommissionPercents.Service _airlineCommissionPercents;

		EntityService<Domain, AirlineCommissionPercents> IEntityServiceContainer<Domain, AirlineCommissionPercents>.Service => AirlineCommissionPercents;
		
		[DebuggerStepThrough]
		public static AirlineCommissionPercents operator +(AirlineCommissionPercents r, Domain db)
		{
			return (AirlineCommissionPercents)r?.Resolve(db);
		}

	}

	partial class AirlineCommissionPercents
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AirlineCommissionPercents operator |(AirlineCommissionPercents r1, AirlineCommissionPercents r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AirlineCommissionPercents entity) : base(entity) { }

			public Reference(AirlineCommissionPercents entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AirlineCommissionPercents entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AirlineCommissionPercents operator +(Reference reference, Domain db)
			{
				return db.AirlineCommissionPercents.Load(reference);
			}

		}

	}

	public partial class AirlineCommissionPercentsManager : EntityManager<AirlineCommissionPercents, AirlineCommissionPercents.Service> { }

	#endregion

						
	#region AirlineMonthCommission

	partial class Domain : IEntityServiceContainer<Domain, AirlineMonthCommission>
	{

		public AirlineMonthCommission.Service AirlineMonthCommission { [DebuggerStepThrough] get { return ResolveService(ref _airlineMonthCommission); } }
		private AirlineMonthCommission.Service _airlineMonthCommission;

		EntityService<Domain, AirlineMonthCommission> IEntityServiceContainer<Domain, AirlineMonthCommission>.Service => AirlineMonthCommission;
		
		[DebuggerStepThrough]
		public static AirlineMonthCommission operator +(AirlineMonthCommission r, Domain db)
		{
			return (AirlineMonthCommission)r?.Resolve(db);
		}

	}

	partial class AirlineMonthCommission
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AirlineMonthCommission operator |(AirlineMonthCommission r1, AirlineMonthCommission r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AirlineMonthCommission entity) : base(entity) { }

			public Reference(AirlineMonthCommission entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AirlineMonthCommission entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AirlineMonthCommission operator +(Reference reference, Domain db)
			{
				return db.AirlineMonthCommission.Load(reference);
			}

		}

	}

	public partial class AirlineMonthCommissionManager : EntityManager<AirlineMonthCommission, AirlineMonthCommission.Service> { }

	#endregion

						
	#region AirlineServiceClass

	partial class Domain : IEntityServiceContainer<Domain, AirlineServiceClass>
	{

		public AirlineServiceClass.Service AirlineServiceClass { [DebuggerStepThrough] get { return ResolveService(ref _airlineServiceClass); } }
		private AirlineServiceClass.Service _airlineServiceClass;

		EntityService<Domain, AirlineServiceClass> IEntityServiceContainer<Domain, AirlineServiceClass>.Service => AirlineServiceClass;
		
		[DebuggerStepThrough]
		public static AirlineServiceClass operator +(AirlineServiceClass r, Domain db)
		{
			return (AirlineServiceClass)r?.Resolve(db);
		}

	}

	partial class AirlineServiceClass
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AirlineServiceClass operator |(AirlineServiceClass r1, AirlineServiceClass r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AirlineServiceClass entity) : base(entity) { }

			public Reference(AirlineServiceClass entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AirlineServiceClass entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AirlineServiceClass operator +(Reference reference, Domain db)
			{
				return db.AirlineServiceClass.Load(reference);
			}

		}

	}

	public partial class AirlineServiceClassManager : EntityManager<AirlineServiceClass, AirlineServiceClass.Service> { }

	#endregion

						
	#region AirplaneModel

	partial class Domain : IEntityServiceContainer<Domain, AirplaneModel>
	{

		public AirplaneModel.Service AirplaneModel { [DebuggerStepThrough] get { return ResolveService(ref _airplaneModel); } }
		private AirplaneModel.Service _airplaneModel;

		EntityService<Domain, AirplaneModel> IEntityServiceContainer<Domain, AirplaneModel>.Service => AirplaneModel;
		
		[DebuggerStepThrough]
		public static AirplaneModel operator +(AirplaneModel r, Domain db)
		{
			return (AirplaneModel)r?.Resolve(db);
		}

	}

	partial class AirplaneModel
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AirplaneModel operator |(AirplaneModel r1, AirplaneModel r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AirplaneModel entity) : base(entity) { }

			public Reference(AirplaneModel entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AirplaneModel entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AirplaneModel operator +(Reference reference, Domain db)
			{
				return db.AirplaneModel.Load(reference);
			}

		}

	}

	public partial class AirplaneModelManager : EntityManager<AirplaneModel, AirplaneModel.Service> { }

	#endregion

						
	#region Airport

	partial class Domain : IEntityServiceContainer<Domain, Airport>
	{

		public Airport.Service Airport { [DebuggerStepThrough] get { return ResolveService(ref _airport); } }
		private Airport.Service _airport;

		EntityService<Domain, Airport> IEntityServiceContainer<Domain, Airport>.Service => Airport;
		
		[DebuggerStepThrough]
		public static Airport operator +(Airport r, Domain db)
		{
			return (Airport)r?.Resolve(db);
		}

	}

	partial class Airport
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Airport operator |(Airport r1, Airport r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Airport entity) : base(entity) { }

			public Reference(Airport entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Airport entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Airport operator +(Reference reference, Domain db)
			{
				return db.Airport.Load(reference);
			}

		}

	}

	public partial class AirportManager : EntityManager<Airport, Airport.Service> { }

	#endregion

						
	#region AmadeusXmlFile

	partial class Domain : IEntityServiceContainer<Domain, AmadeusXmlFile>
	{

		public AmadeusXmlFile.Service AmadeusXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _amadeusXmlFile); } }
		private AmadeusXmlFile.Service _amadeusXmlFile;

		EntityService<Domain, AmadeusXmlFile> IEntityServiceContainer<Domain, AmadeusXmlFile>.Service => AmadeusXmlFile;
		
		[DebuggerStepThrough]
		public static AmadeusXmlFile operator +(AmadeusXmlFile r, Domain db)
		{
			return (AmadeusXmlFile)r?.Resolve(db);
		}

	}

	partial class AmadeusXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AmadeusXmlFile operator |(AmadeusXmlFile r1, AmadeusXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AmadeusXmlFile entity) : base(entity) { }

			public Reference(AmadeusXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AmadeusXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AmadeusXmlFile operator +(Reference reference, Domain db)
			{
				return db.AmadeusXmlFile.Load(reference);
			}

		}

	}

	public partial class AmadeusXmlFileManager : EntityManager<AmadeusXmlFile, AmadeusXmlFile.Service> { }

	#endregion

						
	#region AviaDocument

	partial class Domain : IEntityServiceContainer<Domain, AviaDocument>
	{

		public AviaDocument.Service AviaDocument { [DebuggerStepThrough] get { return ResolveService(ref _aviaDocument); } }
		private AviaDocument.Service _aviaDocument;

		EntityService<Domain, AviaDocument> IEntityServiceContainer<Domain, AviaDocument>.Service => AviaDocument;
		
		[DebuggerStepThrough]
		public static AviaDocument operator +(AviaDocument r, Domain db)
		{
			return (AviaDocument)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public AviaDocument Export(AviaDocument r)
		{
			AviaDocument.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<AviaDocument> entities)
		{
			foreach (var r in entities)
				AviaDocument.Export(r);
		}

	}

	partial class AviaDocument
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(AviaDocument r, Action<Action<AviaDocument>> onCommit)
			{
				if (r == null) return false;

				r = db.Unproxy(r);
				var aviaMco = r as AviaMco;
				if (aviaMco != null)
					return db.AviaMco.Delete(aviaMco);

				var aviaRefund = r as AviaRefund;
				if (aviaRefund != null)
					return db.AviaRefund.Delete(aviaRefund);

				var aviaTicket = r as AviaTicket;
				if (aviaTicket != null)
					return db.AviaTicket.Delete(aviaTicket);

				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override AviaDocument Save(AviaDocument r, Action<Action<AviaDocument>, Action<AviaDocument>> onCommit)
			{
				if (r == null) return null;

				r = db.Unproxy(r);

				var aviaMco = r as AviaMco;
				if (aviaMco != null)
					return db.AviaMco.Save(aviaMco);

				var aviaRefund = r as AviaRefund;
				if (aviaRefund != null)
					return db.AviaRefund.Save(aviaRefund);

				var aviaTicket = r as AviaTicket;
				if (aviaTicket != null)
					return db.AviaTicket.Save(aviaTicket);

				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static AviaDocument operator |(AviaDocument r1, AviaDocument r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AviaDocument entity) : base(entity) { }

			public Reference(AviaDocument entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AviaDocument entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AviaDocument operator +(Reference reference, Domain db)
			{
				return db.AviaDocument.Load(reference);
			}

		}

	}

	public partial class AviaDocumentManager : ProductManager<AviaDocument, AviaDocument.Service> { }

	#endregion

						
	#region AviaDocumentFee

	partial class Domain : IEntityServiceContainer<Domain, AviaDocumentFee>
	{

		public AviaDocumentFee.Service AviaDocumentFee { [DebuggerStepThrough] get { return ResolveService(ref _aviaDocumentFee); } }
		private AviaDocumentFee.Service _aviaDocumentFee;

		EntityService<Domain, AviaDocumentFee> IEntityServiceContainer<Domain, AviaDocumentFee>.Service => AviaDocumentFee;
		
		[DebuggerStepThrough]
		public static AviaDocumentFee operator +(AviaDocumentFee r, Domain db)
		{
			return (AviaDocumentFee)r?.Resolve(db);
		}

	}

	partial class AviaDocumentFee
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AviaDocumentFee operator |(AviaDocumentFee r1, AviaDocumentFee r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class AviaDocumentFeeManager : EntityManager<AviaDocumentFee, AviaDocumentFee.Service> { }

	#endregion

						
	#region AviaDocumentVoiding

	partial class Domain : IEntityServiceContainer<Domain, AviaDocumentVoiding>
	{

		public AviaDocumentVoiding.Service AviaDocumentVoiding { [DebuggerStepThrough] get { return ResolveService(ref _aviaDocumentVoiding); } }
		private AviaDocumentVoiding.Service _aviaDocumentVoiding;

		EntityService<Domain, AviaDocumentVoiding> IEntityServiceContainer<Domain, AviaDocumentVoiding>.Service => AviaDocumentVoiding;
		
		[DebuggerStepThrough]
		public static AviaDocumentVoiding operator +(AviaDocumentVoiding r, Domain db)
		{
			return (AviaDocumentVoiding)r?.Resolve(db);
		}

	}

	partial class AviaDocumentVoiding
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AviaDocumentVoiding operator |(AviaDocumentVoiding r1, AviaDocumentVoiding r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AviaDocumentVoiding entity) : base(entity) { }

			public Reference(AviaDocumentVoiding entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AviaDocumentVoiding entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AviaDocumentVoiding operator +(Reference reference, Domain db)
			{
				return db.AviaDocumentVoiding.Load(reference);
			}

		}

	}

	public partial class AviaDocumentVoidingManager : EntityManager<AviaDocumentVoiding, AviaDocumentVoiding.Service> { }

	#endregion

						
	#region AviaMco

	partial class Domain : IEntityServiceContainer<Domain, AviaMco>
	{

		public AviaMco.Service AviaMco { [DebuggerStepThrough] get { return ResolveService(ref _aviaMco); } }
		private AviaMco.Service _aviaMco;

		EntityService<Domain, AviaMco> IEntityServiceContainer<Domain, AviaMco>.Service => AviaMco;
		
		[DebuggerStepThrough]
		public static AviaMco operator +(AviaMco r, Domain db)
		{
			return (AviaMco)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public AviaMco Export(AviaMco r)
		{
			AviaMco.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<AviaMco> entities)
		{
			foreach (var r in entities)
				AviaMco.Export(r);
		}

	}

	partial class AviaMco
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AviaMco operator |(AviaMco r1, AviaMco r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : AviaDocument.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AviaMco entity) : base(entity) { }

			public Reference(AviaMco entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AviaMco entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AviaMco operator +(Reference reference, Domain db)
			{
				return db.AviaMco.Load(reference);
			}

		}

	}

	public partial class AviaMcoManager : ProductManager<AviaMco, AviaMco.Service> { }

	#endregion

						
	#region AviaRefund

	partial class Domain : IEntityServiceContainer<Domain, AviaRefund>
	{

		public AviaRefund.Service AviaRefund { [DebuggerStepThrough] get { return ResolveService(ref _aviaRefund); } }
		private AviaRefund.Service _aviaRefund;

		EntityService<Domain, AviaRefund> IEntityServiceContainer<Domain, AviaRefund>.Service => AviaRefund;
		
		[DebuggerStepThrough]
		public static AviaRefund operator +(AviaRefund r, Domain db)
		{
			return (AviaRefund)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public AviaRefund Export(AviaRefund r)
		{
			AviaRefund.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<AviaRefund> entities)
		{
			foreach (var r in entities)
				AviaRefund.Export(r);
		}

	}

	partial class AviaRefund
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AviaRefund operator |(AviaRefund r1, AviaRefund r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : AviaDocument.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AviaRefund entity) : base(entity) { }

			public Reference(AviaRefund entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AviaRefund entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AviaRefund operator +(Reference reference, Domain db)
			{
				return db.AviaRefund.Load(reference);
			}

		}

	}

	public partial class AviaRefundManager : ProductManager<AviaRefund, AviaRefund.Service> { }

	#endregion

						
	#region AviaTicket

	partial class Domain : IEntityServiceContainer<Domain, AviaTicket>
	{

		public AviaTicket.Service AviaTicket { [DebuggerStepThrough] get { return ResolveService(ref _aviaTicket); } }
		private AviaTicket.Service _aviaTicket;

		EntityService<Domain, AviaTicket> IEntityServiceContainer<Domain, AviaTicket>.Service => AviaTicket;
		
		[DebuggerStepThrough]
		public static AviaTicket operator +(AviaTicket r, Domain db)
		{
			return (AviaTicket)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public AviaTicket Export(AviaTicket r)
		{
			AviaTicket.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<AviaTicket> entities)
		{
			foreach (var r in entities)
				AviaTicket.Export(r);
		}

	}

	partial class AviaTicket
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static AviaTicket operator |(AviaTicket r1, AviaTicket r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : AviaDocument.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(AviaTicket entity) : base(entity) { }

			public Reference(AviaTicket entity, string name) : base(entity, name) { }


			public static implicit operator Reference(AviaTicket entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static AviaTicket operator +(Reference reference, Domain db)
			{
				return db.AviaTicket.Load(reference);
			}

		}

	}

	public partial class AviaTicketManager : ProductManager<AviaTicket, AviaTicket.Service> { }

	#endregion

						
	#region BankAccount

	partial class Domain : IEntityServiceContainer<Domain, BankAccount>
	{

		public BankAccount.Service BankAccount { [DebuggerStepThrough] get { return ResolveService(ref _bankAccount); } }
		private BankAccount.Service _bankAccount;

		EntityService<Domain, BankAccount> IEntityServiceContainer<Domain, BankAccount>.Service => BankAccount;
		
		[DebuggerStepThrough]
		public static BankAccount operator +(BankAccount r, Domain db)
		{
			return (BankAccount)r?.Resolve(db);
		}

	}

	partial class BankAccount
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static BankAccount operator |(BankAccount r1, BankAccount r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(BankAccount entity) : base(entity) { }

			public Reference(BankAccount entity, string name) : base(entity, name) { }


			public static implicit operator Reference(BankAccount entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static BankAccount operator +(Reference reference, Domain db)
			{
				return db.BankAccount.Load(reference);
			}

		}

	}

	public partial class BankAccountManager : EntityManager<BankAccount, BankAccount.Service> { }

	#endregion

						
	#region BusTicket

	partial class Domain : IEntityServiceContainer<Domain, BusTicket>
	{

		public BusTicket.Service BusTicket { [DebuggerStepThrough] get { return ResolveService(ref _busTicket); } }
		private BusTicket.Service _busTicket;

		EntityService<Domain, BusTicket> IEntityServiceContainer<Domain, BusTicket>.Service => BusTicket;
		
		[DebuggerStepThrough]
		public static BusTicket operator +(BusTicket r, Domain db)
		{
			return (BusTicket)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public BusTicket Export(BusTicket r)
		{
			BusTicket.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<BusTicket> entities)
		{
			foreach (var r in entities)
				BusTicket.Export(r);
		}

	}

	partial class BusTicket
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static BusTicket operator |(BusTicket r1, BusTicket r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(BusTicket entity) : base(entity) { }

			public Reference(BusTicket entity, string name) : base(entity, name) { }


			public static implicit operator Reference(BusTicket entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static BusTicket operator +(Reference reference, Domain db)
			{
				return db.BusTicket.Load(reference);
			}

		}

	}

	public partial class BusTicketManager : ProductManager<BusTicket, BusTicket.Service> { }

	#endregion

						
	#region BusTicketRefund

	partial class Domain : IEntityServiceContainer<Domain, BusTicketRefund>
	{

		public BusTicketRefund.Service BusTicketRefund { [DebuggerStepThrough] get { return ResolveService(ref _busTicketRefund); } }
		private BusTicketRefund.Service _busTicketRefund;

		EntityService<Domain, BusTicketRefund> IEntityServiceContainer<Domain, BusTicketRefund>.Service => BusTicketRefund;
		
		[DebuggerStepThrough]
		public static BusTicketRefund operator +(BusTicketRefund r, Domain db)
		{
			return (BusTicketRefund)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public BusTicketRefund Export(BusTicketRefund r)
		{
			BusTicketRefund.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<BusTicketRefund> entities)
		{
			foreach (var r in entities)
				BusTicketRefund.Export(r);
		}

	}

	partial class BusTicketRefund
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static BusTicketRefund operator |(BusTicketRefund r1, BusTicketRefund r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : BusTicket.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(BusTicketRefund entity) : base(entity) { }

			public Reference(BusTicketRefund entity, string name) : base(entity, name) { }


			public static implicit operator Reference(BusTicketRefund entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static BusTicketRefund operator +(Reference reference, Domain db)
			{
				return db.BusTicketRefund.Load(reference);
			}

		}

	}

	public partial class BusTicketRefundManager : ProductManager<BusTicketRefund, BusTicketRefund.Service> { }

	#endregion

						
	#region CarRental

	partial class Domain : IEntityServiceContainer<Domain, CarRental>
	{

		public CarRental.Service CarRental { [DebuggerStepThrough] get { return ResolveService(ref _carRental); } }
		private CarRental.Service _carRental;

		EntityService<Domain, CarRental> IEntityServiceContainer<Domain, CarRental>.Service => CarRental;
		
		[DebuggerStepThrough]
		public static CarRental operator +(CarRental r, Domain db)
		{
			return (CarRental)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public CarRental Export(CarRental r)
		{
			CarRental.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<CarRental> entities)
		{
			foreach (var r in entities)
				CarRental.Export(r);
		}

	}

	partial class CarRental
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CarRental operator |(CarRental r1, CarRental r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CarRental entity) : base(entity) { }

			public Reference(CarRental entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CarRental entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CarRental operator +(Reference reference, Domain db)
			{
				return db.CarRental.Load(reference);
			}

		}

	}

	public partial class CarRentalManager : ProductManager<CarRental, CarRental.Service> { }

	#endregion

						
	#region CashInOrderPayment

	partial class Domain : IEntityServiceContainer<Domain, CashInOrderPayment>
	{

		public CashInOrderPayment.Service CashInOrderPayment { [DebuggerStepThrough] get { return ResolveService(ref _cashInOrderPayment); } }
		private CashInOrderPayment.Service _cashInOrderPayment;

		EntityService<Domain, CashInOrderPayment> IEntityServiceContainer<Domain, CashInOrderPayment>.Service => CashInOrderPayment;
		
		[DebuggerStepThrough]
		public static CashInOrderPayment operator +(CashInOrderPayment r, Domain db)
		{
			return (CashInOrderPayment)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public CashInOrderPayment Export(CashInOrderPayment r)
		{
			CashInOrderPayment.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<CashInOrderPayment> entities)
		{
			foreach (var r in entities)
				CashInOrderPayment.Export(r);
		}

		[DebuggerStepThrough]
		public CashInOrderPayment Import(CashInOrderPayment r)
		{
			CashInOrderPayment.Import(r);
			return r;
		}
		[DebuggerStepThrough]
		public void Import(IEnumerable<CashInOrderPayment> entities)
		{
			foreach (var r in entities)
				CashInOrderPayment.Import(r);
		}

		[DebuggerStepThrough]
		public CashInOrderPayment Issue(CashInOrderPayment r)
		{
			CashInOrderPayment.Issue(r);
			return r;
		}

	}

	partial class CashInOrderPayment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CashInOrderPayment operator |(CashInOrderPayment r1, CashInOrderPayment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Payment.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CashInOrderPayment entity) : base(entity) { }

			public Reference(CashInOrderPayment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CashInOrderPayment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CashInOrderPayment operator +(Reference reference, Domain db)
			{
				return db.CashInOrderPayment.Load(reference);
			}

		}

	}

	public partial class CashInOrderPaymentManager : EntityManager<CashInOrderPayment, CashInOrderPayment.Service> { }

	#endregion

						
	#region CashOutOrderPayment

	partial class Domain : IEntityServiceContainer<Domain, CashOutOrderPayment>
	{

		public CashOutOrderPayment.Service CashOutOrderPayment { [DebuggerStepThrough] get { return ResolveService(ref _cashOutOrderPayment); } }
		private CashOutOrderPayment.Service _cashOutOrderPayment;

		EntityService<Domain, CashOutOrderPayment> IEntityServiceContainer<Domain, CashOutOrderPayment>.Service => CashOutOrderPayment;
		
		[DebuggerStepThrough]
		public static CashOutOrderPayment operator +(CashOutOrderPayment r, Domain db)
		{
			return (CashOutOrderPayment)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public CashOutOrderPayment Export(CashOutOrderPayment r)
		{
			CashOutOrderPayment.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<CashOutOrderPayment> entities)
		{
			foreach (var r in entities)
				CashOutOrderPayment.Export(r);
		}

		[DebuggerStepThrough]
		public CashOutOrderPayment Import(CashOutOrderPayment r)
		{
			CashOutOrderPayment.Import(r);
			return r;
		}
		[DebuggerStepThrough]
		public void Import(IEnumerable<CashOutOrderPayment> entities)
		{
			foreach (var r in entities)
				CashOutOrderPayment.Import(r);
		}

	}

	partial class CashOutOrderPayment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CashOutOrderPayment operator |(CashOutOrderPayment r1, CashOutOrderPayment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Payment.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CashOutOrderPayment entity) : base(entity) { }

			public Reference(CashOutOrderPayment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CashOutOrderPayment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CashOutOrderPayment operator +(Reference reference, Domain db)
			{
				return db.CashOutOrderPayment.Load(reference);
			}

		}

	}

	public partial class CashOutOrderPaymentManager : EntityManager<CashOutOrderPayment, CashOutOrderPayment.Service> { }

	#endregion

						
	#region CateringType

	partial class Domain : IEntityServiceContainer<Domain, CateringType>
	{

		public CateringType.Service CateringType { [DebuggerStepThrough] get { return ResolveService(ref _cateringType); } }
		private CateringType.Service _cateringType;

		EntityService<Domain, CateringType> IEntityServiceContainer<Domain, CateringType>.Service => CateringType;
		
		[DebuggerStepThrough]
		public static CateringType operator +(CateringType r, Domain db)
		{
			return (CateringType)r?.Resolve(db);
		}

	}

	partial class CateringType
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CateringType operator |(CateringType r1, CateringType r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CateringType entity) : base(entity) { }

			public Reference(CateringType entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CateringType entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CateringType operator +(Reference reference, Domain db)
			{
				return db.CateringType.Load(reference);
			}

		}

	}

	public partial class CateringTypeManager : EntityManager<CateringType, CateringType.Service> { }

	#endregion

						
	#region CheckPayment

	partial class Domain : IEntityServiceContainer<Domain, CheckPayment>
	{

		public CheckPayment.Service CheckPayment { [DebuggerStepThrough] get { return ResolveService(ref _checkPayment); } }
		private CheckPayment.Service _checkPayment;

		EntityService<Domain, CheckPayment> IEntityServiceContainer<Domain, CheckPayment>.Service => CheckPayment;
		
		[DebuggerStepThrough]
		public static CheckPayment operator +(CheckPayment r, Domain db)
		{
			return (CheckPayment)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public CheckPayment Export(CheckPayment r)
		{
			CheckPayment.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<CheckPayment> entities)
		{
			foreach (var r in entities)
				CheckPayment.Export(r);
		}

		[DebuggerStepThrough]
		public CheckPayment Import(CheckPayment r)
		{
			CheckPayment.Import(r);
			return r;
		}
		[DebuggerStepThrough]
		public void Import(IEnumerable<CheckPayment> entities)
		{
			foreach (var r in entities)
				CheckPayment.Import(r);
		}

	}

	partial class CheckPayment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CheckPayment operator |(CheckPayment r1, CheckPayment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Payment.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CheckPayment entity) : base(entity) { }

			public Reference(CheckPayment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CheckPayment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CheckPayment operator +(Reference reference, Domain db)
			{
				return db.CheckPayment.Load(reference);
			}

		}

	}

	public partial class CheckPaymentManager : EntityManager<CheckPayment, CheckPayment.Service> { }

	#endregion

						
	#region ClosedPeriod

	partial class Domain : IEntityServiceContainer<Domain, ClosedPeriod>
	{

		public ClosedPeriod.Service ClosedPeriod { [DebuggerStepThrough] get { return ResolveService(ref _closedPeriod); } }
		private ClosedPeriod.Service _closedPeriod;

		EntityService<Domain, ClosedPeriod> IEntityServiceContainer<Domain, ClosedPeriod>.Service => ClosedPeriod;
		
		[DebuggerStepThrough]
		public static ClosedPeriod operator +(ClosedPeriod r, Domain db)
		{
			return (ClosedPeriod)r?.Resolve(db);
		}

	}

	partial class ClosedPeriod
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static ClosedPeriod operator |(ClosedPeriod r1, ClosedPeriod r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class ClosedPeriodManager : EntityManager<ClosedPeriod, ClosedPeriod.Service> { }

	#endregion

						
	#region Consignment

	partial class Domain : IEntityServiceContainer<Domain, Consignment>
	{

		public Consignment.Service Consignment { [DebuggerStepThrough] get { return ResolveService(ref _consignment); } }
		private Consignment.Service _consignment;

		EntityService<Domain, Consignment> IEntityServiceContainer<Domain, Consignment>.Service => Consignment;
		
		[DebuggerStepThrough]
		public static Consignment operator +(Consignment r, Domain db)
		{
			return (Consignment)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Consignment Issue(Consignment r)
		{
			Consignment.Issue(r);
			return r;
		}

	}

	partial class Consignment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Consignment operator |(Consignment r1, Consignment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Consignment entity) : base(entity) { }

			public Reference(Consignment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Consignment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Consignment operator +(Reference reference, Domain db)
			{
				return db.Consignment.Load(reference);
			}

		}

	}

	public partial class ConsignmentManager : EntityManager<Consignment, Consignment.Service> { }

	#endregion

						
	#region Contract

	partial class Domain : IEntityServiceContainer<Domain, Contract>
	{

		public Contract.Service Contract { [DebuggerStepThrough] get { return ResolveService(ref _contract); } }
		private Contract.Service _contract;

		EntityService<Domain, Contract> IEntityServiceContainer<Domain, Contract>.Service => Contract;
		
		[DebuggerStepThrough]
		public static Contract operator +(Contract r, Domain db)
		{
			return (Contract)r?.Resolve(db);
		}

	}

	partial class Contract
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Contract operator |(Contract r1, Contract r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Contract entity) : base(entity) { }

			public Reference(Contract entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Contract entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Contract operator +(Reference reference, Domain db)
			{
				return db.Contract.Load(reference);
			}

		}

	}

	public partial class ContractManager : EntityManager<Contract, Contract.Service> { }

	#endregion

						
	#region Country

	partial class Domain : IEntityServiceContainer<Domain, Country>
	{

		public Country.Service Country { [DebuggerStepThrough] get { return ResolveService(ref _country); } }
		private Country.Service _country;

		EntityService<Domain, Country> IEntityServiceContainer<Domain, Country>.Service => Country;
		
		[DebuggerStepThrough]
		public static Country operator +(Country r, Domain db)
		{
			return (Country)r?.Resolve(db);
		}

	}

	partial class Country
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Country operator |(Country r1, Country r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Country entity) : base(entity) { }

			public Reference(Country entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Country entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Country operator +(Reference reference, Domain db)
			{
				return db.Country.Load(reference);
			}

		}

	}

	public partial class CountryManager : EntityManager<Country, Country.Service> { }

	#endregion

						
	#region CrazyllamaPnrFile

	partial class Domain : IEntityServiceContainer<Domain, CrazyllamaPnrFile>
	{

		public CrazyllamaPnrFile.Service CrazyllamaPnrFile { [DebuggerStepThrough] get { return ResolveService(ref _crazyllamaPnrFile); } }
		private CrazyllamaPnrFile.Service _crazyllamaPnrFile;

		EntityService<Domain, CrazyllamaPnrFile> IEntityServiceContainer<Domain, CrazyllamaPnrFile>.Service => CrazyllamaPnrFile;
		
		[DebuggerStepThrough]
		public static CrazyllamaPnrFile operator +(CrazyllamaPnrFile r, Domain db)
		{
			return (CrazyllamaPnrFile)r?.Resolve(db);
		}

	}

	partial class CrazyllamaPnrFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CrazyllamaPnrFile operator |(CrazyllamaPnrFile r1, CrazyllamaPnrFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CrazyllamaPnrFile entity) : base(entity) { }

			public Reference(CrazyllamaPnrFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CrazyllamaPnrFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CrazyllamaPnrFile operator +(Reference reference, Domain db)
			{
				return db.CrazyllamaPnrFile.Load(reference);
			}

		}

	}

	public partial class CrazyllamaPnrFileManager : EntityManager<CrazyllamaPnrFile, CrazyllamaPnrFile.Service> { }

	#endregion

						
	#region Currency

	partial class Domain : IEntityServiceContainer<Domain, Currency>
	{

		public Currency.Service Currency { [DebuggerStepThrough] get { return ResolveService(ref _currency); } }
		private Currency.Service _currency;

		EntityService<Domain, Currency> IEntityServiceContainer<Domain, Currency>.Service => Currency;
		
		[DebuggerStepThrough]
		public static Currency operator +(Currency r, Domain db)
		{
			return (Currency)r?.Resolve(db);
		}

	}

	partial class Currency
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Currency operator |(Currency r1, Currency r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Currency entity) : base(entity) { }

			public Reference(Currency entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Currency entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Currency operator +(Reference reference, Domain db)
			{
				return db.Currency.Load(reference);
			}

		}

	}

	public partial class CurrencyManager : EntityManager<Currency, Currency.Service> { }

	#endregion

						
	#region CurrencyDailyRate

	partial class Domain : IEntityServiceContainer<Domain, CurrencyDailyRate>
	{

		public CurrencyDailyRate.Service CurrencyDailyRate { [DebuggerStepThrough] get { return ResolveService(ref _currencyDailyRate); } }
		private CurrencyDailyRate.Service _currencyDailyRate;

		EntityService<Domain, CurrencyDailyRate> IEntityServiceContainer<Domain, CurrencyDailyRate>.Service => CurrencyDailyRate;
		
		[DebuggerStepThrough]
		public static CurrencyDailyRate operator +(CurrencyDailyRate r, Domain db)
		{
			return (CurrencyDailyRate)r?.Resolve(db);
		}

	}

	partial class CurrencyDailyRate
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static CurrencyDailyRate operator |(CurrencyDailyRate r1, CurrencyDailyRate r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(CurrencyDailyRate entity) : base(entity) { }

			public Reference(CurrencyDailyRate entity, string name) : base(entity, name) { }


			public static implicit operator Reference(CurrencyDailyRate entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static CurrencyDailyRate operator +(Reference reference, Domain db)
			{
				return db.CurrencyDailyRate.Load(reference);
			}

		}

	}

	public partial class CurrencyDailyRateManager : EntityManager<CurrencyDailyRate, CurrencyDailyRate.Service> { }

	#endregion

						
	#region Department

	partial class Domain : IEntityServiceContainer<Domain, Department>
	{

		public Department.Service Department { [DebuggerStepThrough] get { return ResolveService(ref _department); } }
		private Department.Service _department;

		EntityService<Domain, Department> IEntityServiceContainer<Domain, Department>.Service => Department;
		
		[DebuggerStepThrough]
		public static Department operator +(Department r, Domain db)
		{
			return (Department)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Department Export(Department r)
		{
			Department.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Department> entities)
		{
			foreach (var r in entities)
				Department.Export(r);
		}

	}

	partial class Department
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Department operator |(Department r1, Department r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Party.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Department entity) : base(entity) { }

			public Reference(Department entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Department entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Department operator +(Reference reference, Domain db)
			{
				return db.Department.Load(reference);
			}

		}

	}

	public partial class DepartmentManager : EntityManager<Department, Department.Service> { }

	#endregion

						
	#region DocumentAccess

	partial class Domain : IEntityServiceContainer<Domain, DocumentAccess>
	{

		public DocumentAccess.Service DocumentAccess { [DebuggerStepThrough] get { return ResolveService(ref _documentAccess); } }
		private DocumentAccess.Service _documentAccess;

		EntityService<Domain, DocumentAccess> IEntityServiceContainer<Domain, DocumentAccess>.Service => DocumentAccess;
		
		[DebuggerStepThrough]
		public static DocumentAccess operator +(DocumentAccess r, Domain db)
		{
			return (DocumentAccess)r?.Resolve(db);
		}

	}

	partial class DocumentAccess
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static DocumentAccess operator |(DocumentAccess r1, DocumentAccess r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(DocumentAccess entity) : base(entity) { }

			public Reference(DocumentAccess entity, string name) : base(entity, name) { }


			public static implicit operator Reference(DocumentAccess entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static DocumentAccess operator +(Reference reference, Domain db)
			{
				return db.DocumentAccess.Load(reference);
			}

		}

	}

	public partial class DocumentAccessManager : EntityManager<DocumentAccess, DocumentAccess.Service> { }

	#endregion

						
	#region DocumentOwner

	partial class Domain : IEntityServiceContainer<Domain, DocumentOwner>
	{

		public DocumentOwner.Service DocumentOwner { [DebuggerStepThrough] get { return ResolveService(ref _documentOwner); } }
		private DocumentOwner.Service _documentOwner;

		EntityService<Domain, DocumentOwner> IEntityServiceContainer<Domain, DocumentOwner>.Service => DocumentOwner;
		
		[DebuggerStepThrough]
		public static DocumentOwner operator +(DocumentOwner r, Domain db)
		{
			return (DocumentOwner)r?.Resolve(db);
		}

	}

	partial class DocumentOwner
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static DocumentOwner operator |(DocumentOwner r1, DocumentOwner r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(DocumentOwner entity) : base(entity) { }

			public Reference(DocumentOwner entity, string name) : base(entity, name) { }


			public static implicit operator Reference(DocumentOwner entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static DocumentOwner operator +(Reference reference, Domain db)
			{
				return db.DocumentOwner.Load(reference);
			}

		}

	}

	public partial class DocumentOwnerManager : EntityManager<DocumentOwner, DocumentOwner.Service> { }

	#endregion

						
	#region DrctXmlFile

	partial class Domain : IEntityServiceContainer<Domain, DrctXmlFile>
	{

		public DrctXmlFile.Service DrctXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _drctXmlFile); } }
		private DrctXmlFile.Service _drctXmlFile;

		EntityService<Domain, DrctXmlFile> IEntityServiceContainer<Domain, DrctXmlFile>.Service => DrctXmlFile;
		
		[DebuggerStepThrough]
		public static DrctXmlFile operator +(DrctXmlFile r, Domain db)
		{
			return (DrctXmlFile)r?.Resolve(db);
		}

	}

	partial class DrctXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static DrctXmlFile operator |(DrctXmlFile r1, DrctXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(DrctXmlFile entity) : base(entity) { }

			public Reference(DrctXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(DrctXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static DrctXmlFile operator +(Reference reference, Domain db)
			{
				return db.DrctXmlFile.Load(reference);
			}

		}

	}

	public partial class DrctXmlFileManager : EntityManager<DrctXmlFile, DrctXmlFile.Service> { }

	#endregion

						
	#region ElectronicPayment

	partial class Domain : IEntityServiceContainer<Domain, ElectronicPayment>
	{

		public ElectronicPayment.Service ElectronicPayment { [DebuggerStepThrough] get { return ResolveService(ref _electronicPayment); } }
		private ElectronicPayment.Service _electronicPayment;

		EntityService<Domain, ElectronicPayment> IEntityServiceContainer<Domain, ElectronicPayment>.Service => ElectronicPayment;
		
		[DebuggerStepThrough]
		public static ElectronicPayment operator +(ElectronicPayment r, Domain db)
		{
			return (ElectronicPayment)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public ElectronicPayment Export(ElectronicPayment r)
		{
			ElectronicPayment.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<ElectronicPayment> entities)
		{
			foreach (var r in entities)
				ElectronicPayment.Export(r);
		}

		[DebuggerStepThrough]
		public ElectronicPayment Import(ElectronicPayment r)
		{
			ElectronicPayment.Import(r);
			return r;
		}
		[DebuggerStepThrough]
		public void Import(IEnumerable<ElectronicPayment> entities)
		{
			foreach (var r in entities)
				ElectronicPayment.Import(r);
		}

	}

	partial class ElectronicPayment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static ElectronicPayment operator |(ElectronicPayment r1, ElectronicPayment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Payment.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(ElectronicPayment entity) : base(entity) { }

			public Reference(ElectronicPayment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(ElectronicPayment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static ElectronicPayment operator +(Reference reference, Domain db)
			{
				return db.ElectronicPayment.Load(reference);
			}

		}

	}

	public partial class ElectronicPaymentManager : EntityManager<ElectronicPayment, ElectronicPayment.Service> { }

	#endregion

						
	#region Excursion

	partial class Domain : IEntityServiceContainer<Domain, Excursion>
	{

		public Excursion.Service Excursion { [DebuggerStepThrough] get { return ResolveService(ref _excursion); } }
		private Excursion.Service _excursion;

		EntityService<Domain, Excursion> IEntityServiceContainer<Domain, Excursion>.Service => Excursion;
		
		[DebuggerStepThrough]
		public static Excursion operator +(Excursion r, Domain db)
		{
			return (Excursion)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Excursion Export(Excursion r)
		{
			Excursion.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Excursion> entities)
		{
			foreach (var r in entities)
				Excursion.Export(r);
		}

	}

	partial class Excursion
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Excursion operator |(Excursion r1, Excursion r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Excursion entity) : base(entity) { }

			public Reference(Excursion entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Excursion entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Excursion operator +(Reference reference, Domain db)
			{
				return db.Excursion.Load(reference);
			}

		}

	}

	public partial class ExcursionManager : ProductManager<Excursion, Excursion.Service> { }

	#endregion

						
	#region File

	partial class Domain : IEntityServiceContainer<Domain, File>
	{

		public File.Service File { [DebuggerStepThrough] get { return ResolveService(ref _file); } }
		private File.Service _file;

		EntityService<Domain, File> IEntityServiceContainer<Domain, File>.Service => File;
		
		[DebuggerStepThrough]
		public static File operator +(File r, Domain db)
		{
			return (File)r?.Resolve(db);
		}

	}

	partial class File
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static File operator |(File r1, File r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class FileManager : EntityManager<File, File.Service> { }

	#endregion

						
	#region FlightSegment

	partial class Domain : IEntityServiceContainer<Domain, FlightSegment>
	{

		public FlightSegment.Service FlightSegment { [DebuggerStepThrough] get { return ResolveService(ref _flightSegment); } }
		private FlightSegment.Service _flightSegment;

		EntityService<Domain, FlightSegment> IEntityServiceContainer<Domain, FlightSegment>.Service => FlightSegment;
		
		[DebuggerStepThrough]
		public static FlightSegment operator +(FlightSegment r, Domain db)
		{
			return (FlightSegment)r?.Resolve(db);
		}

	}

	partial class FlightSegment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static FlightSegment operator |(FlightSegment r1, FlightSegment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(FlightSegment entity) : base(entity) { }

			public Reference(FlightSegment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(FlightSegment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static FlightSegment operator +(Reference reference, Domain db)
			{
				return db.FlightSegment.Load(reference);
			}

		}

	}

	public partial class FlightSegmentManager : EntityManager<FlightSegment, FlightSegment.Service> { }

	#endregion

						
	#region GalileoBusXmlFile

	partial class Domain : IEntityServiceContainer<Domain, GalileoBusXmlFile>
	{

		public GalileoBusXmlFile.Service GalileoBusXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _galileoBusXmlFile); } }
		private GalileoBusXmlFile.Service _galileoBusXmlFile;

		EntityService<Domain, GalileoBusXmlFile> IEntityServiceContainer<Domain, GalileoBusXmlFile>.Service => GalileoBusXmlFile;
		
		[DebuggerStepThrough]
		public static GalileoBusXmlFile operator +(GalileoBusXmlFile r, Domain db)
		{
			return (GalileoBusXmlFile)r?.Resolve(db);
		}

	}

	partial class GalileoBusXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GalileoBusXmlFile operator |(GalileoBusXmlFile r1, GalileoBusXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GalileoBusXmlFile entity) : base(entity) { }

			public Reference(GalileoBusXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GalileoBusXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GalileoBusXmlFile operator +(Reference reference, Domain db)
			{
				return db.GalileoBusXmlFile.Load(reference);
			}

		}

	}

	public partial class GalileoBusXmlFileManager : EntityManager<GalileoBusXmlFile, GalileoBusXmlFile.Service> { }

	#endregion

						
	#region GalileoRailXmlFile

	partial class Domain : IEntityServiceContainer<Domain, GalileoRailXmlFile>
	{

		public GalileoRailXmlFile.Service GalileoRailXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _galileoRailXmlFile); } }
		private GalileoRailXmlFile.Service _galileoRailXmlFile;

		EntityService<Domain, GalileoRailXmlFile> IEntityServiceContainer<Domain, GalileoRailXmlFile>.Service => GalileoRailXmlFile;
		
		[DebuggerStepThrough]
		public static GalileoRailXmlFile operator +(GalileoRailXmlFile r, Domain db)
		{
			return (GalileoRailXmlFile)r?.Resolve(db);
		}

	}

	partial class GalileoRailXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GalileoRailXmlFile operator |(GalileoRailXmlFile r1, GalileoRailXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GalileoRailXmlFile entity) : base(entity) { }

			public Reference(GalileoRailXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GalileoRailXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GalileoRailXmlFile operator +(Reference reference, Domain db)
			{
				return db.GalileoRailXmlFile.Load(reference);
			}

		}

	}

	public partial class GalileoRailXmlFileManager : EntityManager<GalileoRailXmlFile, GalileoRailXmlFile.Service> { }

	#endregion

						
	#region GalileoXmlFile

	partial class Domain : IEntityServiceContainer<Domain, GalileoXmlFile>
	{

		public GalileoXmlFile.Service GalileoXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _galileoXmlFile); } }
		private GalileoXmlFile.Service _galileoXmlFile;

		EntityService<Domain, GalileoXmlFile> IEntityServiceContainer<Domain, GalileoXmlFile>.Service => GalileoXmlFile;
		
		[DebuggerStepThrough]
		public static GalileoXmlFile operator +(GalileoXmlFile r, Domain db)
		{
			return (GalileoXmlFile)r?.Resolve(db);
		}

	}

	partial class GalileoXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GalileoXmlFile operator |(GalileoXmlFile r1, GalileoXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GalileoXmlFile entity) : base(entity) { }

			public Reference(GalileoXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GalileoXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GalileoXmlFile operator +(Reference reference, Domain db)
			{
				return db.GalileoXmlFile.Load(reference);
			}

		}

	}

	public partial class GalileoXmlFileManager : EntityManager<GalileoXmlFile, GalileoXmlFile.Service> { }

	#endregion

						
	#region GdsAgent

	partial class Domain : IEntityServiceContainer<Domain, GdsAgent>
	{

		public GdsAgent.Service GdsAgent { [DebuggerStepThrough] get { return ResolveService(ref _gdsAgent); } }
		private GdsAgent.Service _gdsAgent;

		EntityService<Domain, GdsAgent> IEntityServiceContainer<Domain, GdsAgent>.Service => GdsAgent;
		
		[DebuggerStepThrough]
		public static GdsAgent operator +(GdsAgent r, Domain db)
		{
			return (GdsAgent)r?.Resolve(db);
		}

	}

	partial class GdsAgent
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GdsAgent operator |(GdsAgent r1, GdsAgent r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GdsAgent entity) : base(entity) { }

			public Reference(GdsAgent entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GdsAgent entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GdsAgent operator +(Reference reference, Domain db)
			{
				return db.GdsAgent.Load(reference);
			}

		}

	}

	public partial class GdsAgentManager : EntityManager<GdsAgent, GdsAgent.Service> { }

	#endregion

						
	#region GdsFile

	partial class Domain : IEntityServiceContainer<Domain, GdsFile>
	{

		public GdsFile.Service GdsFile { [DebuggerStepThrough] get { return ResolveService(ref _gdsFile); } }
		private GdsFile.Service _gdsFile;

		EntityService<Domain, GdsFile> IEntityServiceContainer<Domain, GdsFile>.Service => GdsFile;
		
		[DebuggerStepThrough]
		public static GdsFile operator +(GdsFile r, Domain db)
		{
			return (GdsFile)r?.Resolve(db);
		}

	}

	partial class GdsFile
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(GdsFile r, Action<Action<GdsFile>> onCommit)
			{
				if (r == null) return false;

				r = db.Unproxy(r);
				var drctXmlFile = r as DrctXmlFile;
				if (drctXmlFile != null)
					return db.DrctXmlFile.Delete(drctXmlFile);

				var galileoBusXmlFile = r as GalileoBusXmlFile;
				if (galileoBusXmlFile != null)
					return db.GalileoBusXmlFile.Delete(galileoBusXmlFile);

				var crazyllamaPnrFile = r as CrazyllamaPnrFile;
				if (crazyllamaPnrFile != null)
					return db.CrazyllamaPnrFile.Delete(crazyllamaPnrFile);

				var travelPointXmlFile = r as TravelPointXmlFile;
				if (travelPointXmlFile != null)
					return db.TravelPointXmlFile.Delete(travelPointXmlFile);

				var luxenaXmlFile = r as LuxenaXmlFile;
				if (luxenaXmlFile != null)
					return db.LuxenaXmlFile.Delete(luxenaXmlFile);

				var galileoRailXmlFile = r as GalileoRailXmlFile;
				if (galileoRailXmlFile != null)
					return db.GalileoRailXmlFile.Delete(galileoRailXmlFile);

				var sabreFilFile = r as SabreFilFile;
				if (sabreFilFile != null)
					return db.SabreFilFile.Delete(sabreFilFile);

				var airFile = r as AirFile;
				if (airFile != null)
					return db.AirFile.Delete(airFile);

				var amadeusXmlFile = r as AmadeusXmlFile;
				if (amadeusXmlFile != null)
					return db.AmadeusXmlFile.Delete(amadeusXmlFile);

				var galileoXmlFile = r as GalileoXmlFile;
				if (galileoXmlFile != null)
					return db.GalileoXmlFile.Delete(galileoXmlFile);

				var mirFile = r as MirFile;
				if (mirFile != null)
					return db.MirFile.Delete(mirFile);

				var printFile = r as PrintFile;
				if (printFile != null)
					return db.PrintFile.Delete(printFile);

				var sirenaFile = r as SirenaFile;
				if (sirenaFile != null)
					return db.SirenaFile.Delete(sirenaFile);

				var tktFile = r as TktFile;
				if (tktFile != null)
					return db.TktFile.Delete(tktFile);

				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override GdsFile Save(GdsFile r, Action<Action<GdsFile>, Action<GdsFile>> onCommit)
			{
				if (r == null) return null;

				r = db.Unproxy(r);

				var drctXmlFile = r as DrctXmlFile;
				if (drctXmlFile != null)
					return db.DrctXmlFile.Save(drctXmlFile);

				var galileoBusXmlFile = r as GalileoBusXmlFile;
				if (galileoBusXmlFile != null)
					return db.GalileoBusXmlFile.Save(galileoBusXmlFile);

				var crazyllamaPnrFile = r as CrazyllamaPnrFile;
				if (crazyllamaPnrFile != null)
					return db.CrazyllamaPnrFile.Save(crazyllamaPnrFile);

				var travelPointXmlFile = r as TravelPointXmlFile;
				if (travelPointXmlFile != null)
					return db.TravelPointXmlFile.Save(travelPointXmlFile);

				var luxenaXmlFile = r as LuxenaXmlFile;
				if (luxenaXmlFile != null)
					return db.LuxenaXmlFile.Save(luxenaXmlFile);

				var galileoRailXmlFile = r as GalileoRailXmlFile;
				if (galileoRailXmlFile != null)
					return db.GalileoRailXmlFile.Save(galileoRailXmlFile);

				var sabreFilFile = r as SabreFilFile;
				if (sabreFilFile != null)
					return db.SabreFilFile.Save(sabreFilFile);

				var airFile = r as AirFile;
				if (airFile != null)
					return db.AirFile.Save(airFile);

				var amadeusXmlFile = r as AmadeusXmlFile;
				if (amadeusXmlFile != null)
					return db.AmadeusXmlFile.Save(amadeusXmlFile);

				var galileoXmlFile = r as GalileoXmlFile;
				if (galileoXmlFile != null)
					return db.GalileoXmlFile.Save(galileoXmlFile);

				var mirFile = r as MirFile;
				if (mirFile != null)
					return db.MirFile.Save(mirFile);

				var printFile = r as PrintFile;
				if (printFile != null)
					return db.PrintFile.Save(printFile);

				var sirenaFile = r as SirenaFile;
				if (sirenaFile != null)
					return db.SirenaFile.Save(sirenaFile);

				var tktFile = r as TktFile;
				if (tktFile != null)
					return db.TktFile.Save(tktFile);

				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static GdsFile operator |(GdsFile r1, GdsFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GdsFile entity) : base(entity) { }

			public Reference(GdsFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GdsFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GdsFile operator +(Reference reference, Domain db)
			{
				return db.GdsFile.Load(reference);
			}

		}

	}

	public partial class GdsFileManager : EntityManager<GdsFile, GdsFile.Service> { }

	#endregion

						
	#region GenericProduct

	partial class Domain : IEntityServiceContainer<Domain, GenericProduct>
	{

		public GenericProduct.Service GenericProduct { [DebuggerStepThrough] get { return ResolveService(ref _genericProduct); } }
		private GenericProduct.Service _genericProduct;

		EntityService<Domain, GenericProduct> IEntityServiceContainer<Domain, GenericProduct>.Service => GenericProduct;
		
		[DebuggerStepThrough]
		public static GenericProduct operator +(GenericProduct r, Domain db)
		{
			return (GenericProduct)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public GenericProduct Export(GenericProduct r)
		{
			GenericProduct.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<GenericProduct> entities)
		{
			foreach (var r in entities)
				GenericProduct.Export(r);
		}

	}

	partial class GenericProduct
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GenericProduct operator |(GenericProduct r1, GenericProduct r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GenericProduct entity) : base(entity) { }

			public Reference(GenericProduct entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GenericProduct entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GenericProduct operator +(Reference reference, Domain db)
			{
				return db.GenericProduct.Load(reference);
			}

		}

	}

	public partial class GenericProductManager : ProductManager<GenericProduct, GenericProduct.Service> { }

	#endregion

						
	#region GenericProductType

	partial class Domain : IEntityServiceContainer<Domain, GenericProductType>
	{

		public GenericProductType.Service GenericProductType { [DebuggerStepThrough] get { return ResolveService(ref _genericProductType); } }
		private GenericProductType.Service _genericProductType;

		EntityService<Domain, GenericProductType> IEntityServiceContainer<Domain, GenericProductType>.Service => GenericProductType;
		
		[DebuggerStepThrough]
		public static GenericProductType operator +(GenericProductType r, Domain db)
		{
			return (GenericProductType)r?.Resolve(db);
		}

	}

	partial class GenericProductType
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GenericProductType operator |(GenericProductType r1, GenericProductType r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(GenericProductType entity) : base(entity) { }

			public Reference(GenericProductType entity, string name) : base(entity, name) { }


			public static implicit operator Reference(GenericProductType entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static GenericProductType operator +(Reference reference, Domain db)
			{
				return db.GenericProductType.Load(reference);
			}

		}

	}

	public partial class GenericProductTypeManager : EntityManager<GenericProductType, GenericProductType.Service> { }

	#endregion

						
	#region GlobalSearchEntity

	partial class Domain : IEntityServiceContainer<Domain, GlobalSearchEntity>
	{

		public GlobalSearchEntity.Service GlobalSearchEntity { [DebuggerStepThrough] get { return ResolveService(ref _globalSearchEntity); } }
		private GlobalSearchEntity.Service _globalSearchEntity;

		EntityService<Domain, GlobalSearchEntity> IEntityServiceContainer<Domain, GlobalSearchEntity>.Service => GlobalSearchEntity;
		
		[DebuggerStepThrough]
		public static GlobalSearchEntity operator +(GlobalSearchEntity r, Domain db)
		{
			return (GlobalSearchEntity)r?.Resolve(db);
		}

	}

	partial class GlobalSearchEntity
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static GlobalSearchEntity operator |(GlobalSearchEntity r1, GlobalSearchEntity r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class GlobalSearchEntityManager : EntityManager<GlobalSearchEntity, GlobalSearchEntity.Service> { }

	#endregion

						
	#region Identity

	partial class Domain : IEntityServiceContainer<Domain, Identity>
	{

		public Identity.Service Identity { [DebuggerStepThrough] get { return ResolveService(ref _identity); } }
		private Identity.Service _identity;

		EntityService<Domain, Identity> IEntityServiceContainer<Domain, Identity>.Service => Identity;
		
		[DebuggerStepThrough]
		public static Identity operator +(Identity r, Domain db)
		{
			return (Identity)r?.Resolve(db);
		}

	}

	partial class Identity
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(Identity r, Action<Action<Identity>> onCommit)
			{
				if (r == null) return false;

				r = db.Unproxy(r);
				var internalIdentity = r as InternalIdentity;
				if (internalIdentity != null)
					return db.InternalIdentity.Delete(internalIdentity);

				var user = r as User;
				if (user != null)
					return db.User.Delete(user);

				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override Identity Save(Identity r, Action<Action<Identity>, Action<Identity>> onCommit)
			{
				if (r == null) return null;

				r = db.Unproxy(r);

				var internalIdentity = r as InternalIdentity;
				if (internalIdentity != null)
					return db.InternalIdentity.Save(internalIdentity);

				var user = r as User;
				if (user != null)
					return db.User.Save(user);

				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static Identity operator |(Identity r1, Identity r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Identity entity) : base(entity) { }

			public Reference(Identity entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Identity entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Identity operator +(Reference reference, Domain db)
			{
				return db.Identity.Load(reference);
			}

		}

	}

	public partial class IdentityManager : EntityManager<Identity, Identity.Service> { }

	#endregion

						
	#region Insurance

	partial class Domain : IEntityServiceContainer<Domain, Insurance>
	{

		public Insurance.Service Insurance { [DebuggerStepThrough] get { return ResolveService(ref _insurance); } }
		private Insurance.Service _insurance;

		EntityService<Domain, Insurance> IEntityServiceContainer<Domain, Insurance>.Service => Insurance;
		
		[DebuggerStepThrough]
		public static Insurance operator +(Insurance r, Domain db)
		{
			return (Insurance)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Insurance Export(Insurance r)
		{
			Insurance.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Insurance> entities)
		{
			foreach (var r in entities)
				Insurance.Export(r);
		}

	}

	partial class Insurance
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Insurance operator |(Insurance r1, Insurance r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Insurance entity) : base(entity) { }

			public Reference(Insurance entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Insurance entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Insurance operator +(Reference reference, Domain db)
			{
				return db.Insurance.Load(reference);
			}

		}

	}

	public partial class InsuranceManager : ProductManager<Insurance, Insurance.Service> { }

	#endregion

						
	#region InsuranceRefund

	partial class Domain : IEntityServiceContainer<Domain, InsuranceRefund>
	{

		public InsuranceRefund.Service InsuranceRefund { [DebuggerStepThrough] get { return ResolveService(ref _insuranceRefund); } }
		private InsuranceRefund.Service _insuranceRefund;

		EntityService<Domain, InsuranceRefund> IEntityServiceContainer<Domain, InsuranceRefund>.Service => InsuranceRefund;
		
		[DebuggerStepThrough]
		public static InsuranceRefund operator +(InsuranceRefund r, Domain db)
		{
			return (InsuranceRefund)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public InsuranceRefund Export(InsuranceRefund r)
		{
			InsuranceRefund.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<InsuranceRefund> entities)
		{
			foreach (var r in entities)
				InsuranceRefund.Export(r);
		}

	}

	partial class InsuranceRefund
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static InsuranceRefund operator |(InsuranceRefund r1, InsuranceRefund r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Insurance.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(InsuranceRefund entity) : base(entity) { }

			public Reference(InsuranceRefund entity, string name) : base(entity, name) { }


			public static implicit operator Reference(InsuranceRefund entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static InsuranceRefund operator +(Reference reference, Domain db)
			{
				return db.InsuranceRefund.Load(reference);
			}

		}

	}

	public partial class InsuranceRefundManager : ProductManager<InsuranceRefund, InsuranceRefund.Service> { }

	#endregion

						
	#region InternalIdentity

	partial class Domain : IEntityServiceContainer<Domain, InternalIdentity>
	{

		public InternalIdentity.Service InternalIdentity { [DebuggerStepThrough] get { return ResolveService(ref _internalIdentity); } }
		private InternalIdentity.Service _internalIdentity;

		EntityService<Domain, InternalIdentity> IEntityServiceContainer<Domain, InternalIdentity>.Service => InternalIdentity;
		
		[DebuggerStepThrough]
		public static InternalIdentity operator +(InternalIdentity r, Domain db)
		{
			return (InternalIdentity)r?.Resolve(db);
		}

	}

	partial class InternalIdentity
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static InternalIdentity operator |(InternalIdentity r1, InternalIdentity r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Identity.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(InternalIdentity entity) : base(entity) { }

			public Reference(InternalIdentity entity, string name) : base(entity, name) { }


			public static implicit operator Reference(InternalIdentity entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static InternalIdentity operator +(Reference reference, Domain db)
			{
				return db.InternalIdentity.Load(reference);
			}

		}

	}

	public partial class InternalIdentityManager : EntityManager<InternalIdentity, InternalIdentity.Service> { }

	#endregion

						
	#region InternalTransfer

	partial class Domain : IEntityServiceContainer<Domain, InternalTransfer>
	{

		public InternalTransfer.Service InternalTransfer { [DebuggerStepThrough] get { return ResolveService(ref _internalTransfer); } }
		private InternalTransfer.Service _internalTransfer;

		EntityService<Domain, InternalTransfer> IEntityServiceContainer<Domain, InternalTransfer>.Service => InternalTransfer;
		
		[DebuggerStepThrough]
		public static InternalTransfer operator +(InternalTransfer r, Domain db)
		{
			return (InternalTransfer)r?.Resolve(db);
		}

	}

	partial class InternalTransfer
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static InternalTransfer operator |(InternalTransfer r1, InternalTransfer r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(InternalTransfer entity) : base(entity) { }

			public Reference(InternalTransfer entity, string name) : base(entity, name) { }


			public static implicit operator Reference(InternalTransfer entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static InternalTransfer operator +(Reference reference, Domain db)
			{
				return db.InternalTransfer.Load(reference);
			}

		}

	}

	public partial class InternalTransferManager : EntityManager<InternalTransfer, InternalTransfer.Service> { }

	#endregion

						
	#region Invoice

	partial class Domain : IEntityServiceContainer<Domain, Invoice>
	{

		public Invoice.Service Invoice { [DebuggerStepThrough] get { return ResolveService(ref _invoice); } }
		private Invoice.Service _invoice;

		EntityService<Domain, Invoice> IEntityServiceContainer<Domain, Invoice>.Service => Invoice;
		
		[DebuggerStepThrough]
		public static Invoice operator +(Invoice r, Domain db)
		{
			return (Invoice)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Invoice Export(Invoice r)
		{
			Invoice.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Invoice> entities)
		{
			foreach (var r in entities)
				Invoice.Export(r);
		}

	}

	partial class Invoice
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Invoice operator |(Invoice r1, Invoice r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Invoice entity) : base(entity) { }

			public Reference(Invoice entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Invoice entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Invoice operator +(Reference reference, Domain db)
			{
				return db.Invoice.Load(reference);
			}

		}

	}

	public partial class InvoiceManager : EntityManager<Invoice, Invoice.Service> { }

	#endregion

						
	#region Isic

	partial class Domain : IEntityServiceContainer<Domain, Isic>
	{

		public Isic.Service Isic { [DebuggerStepThrough] get { return ResolveService(ref _isic); } }
		private Isic.Service _isic;

		EntityService<Domain, Isic> IEntityServiceContainer<Domain, Isic>.Service => Isic;
		
		[DebuggerStepThrough]
		public static Isic operator +(Isic r, Domain db)
		{
			return (Isic)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Isic Export(Isic r)
		{
			Isic.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Isic> entities)
		{
			foreach (var r in entities)
				Isic.Export(r);
		}

	}

	partial class Isic
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Isic operator |(Isic r1, Isic r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Isic entity) : base(entity) { }

			public Reference(Isic entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Isic entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Isic operator +(Reference reference, Domain db)
			{
				return db.Isic.Load(reference);
			}

		}

	}

	public partial class IsicManager : ProductManager<Isic, Isic.Service> { }

	#endregion

						
	#region IssuedConsignment

	partial class Domain : IEntityServiceContainer<Domain, IssuedConsignment>
	{

		public IssuedConsignment.Service IssuedConsignment { [DebuggerStepThrough] get { return ResolveService(ref _issuedConsignment); } }
		private IssuedConsignment.Service _issuedConsignment;

		EntityService<Domain, IssuedConsignment> IEntityServiceContainer<Domain, IssuedConsignment>.Service => IssuedConsignment;
		
		[DebuggerStepThrough]
		public static IssuedConsignment operator +(IssuedConsignment r, Domain db)
		{
			return (IssuedConsignment)r?.Resolve(db);
		}

	}

	partial class IssuedConsignment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static IssuedConsignment operator |(IssuedConsignment r1, IssuedConsignment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(IssuedConsignment entity) : base(entity) { }

			public Reference(IssuedConsignment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(IssuedConsignment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static IssuedConsignment operator +(Reference reference, Domain db)
			{
				return db.IssuedConsignment.Load(reference);
			}

		}

	}

	public partial class IssuedConsignmentManager : EntityManager<IssuedConsignment, IssuedConsignment.Service> { }

	#endregion

						
	#region LuxenaXmlFile

	partial class Domain : IEntityServiceContainer<Domain, LuxenaXmlFile>
	{

		public LuxenaXmlFile.Service LuxenaXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _luxenaXmlFile); } }
		private LuxenaXmlFile.Service _luxenaXmlFile;

		EntityService<Domain, LuxenaXmlFile> IEntityServiceContainer<Domain, LuxenaXmlFile>.Service => LuxenaXmlFile;
		
		[DebuggerStepThrough]
		public static LuxenaXmlFile operator +(LuxenaXmlFile r, Domain db)
		{
			return (LuxenaXmlFile)r?.Resolve(db);
		}

	}

	partial class LuxenaXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static LuxenaXmlFile operator |(LuxenaXmlFile r1, LuxenaXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(LuxenaXmlFile entity) : base(entity) { }

			public Reference(LuxenaXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(LuxenaXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static LuxenaXmlFile operator +(Reference reference, Domain db)
			{
				return db.LuxenaXmlFile.Load(reference);
			}

		}

	}

	public partial class LuxenaXmlFileManager : EntityManager<LuxenaXmlFile, LuxenaXmlFile.Service> { }

	#endregion

						
	#region MilesCard

	partial class Domain : IEntityServiceContainer<Domain, MilesCard>
	{

		public MilesCard.Service MilesCard { [DebuggerStepThrough] get { return ResolveService(ref _milesCard); } }
		private MilesCard.Service _milesCard;

		EntityService<Domain, MilesCard> IEntityServiceContainer<Domain, MilesCard>.Service => MilesCard;
		
		[DebuggerStepThrough]
		public static MilesCard operator +(MilesCard r, Domain db)
		{
			return (MilesCard)r?.Resolve(db);
		}

	}

	partial class MilesCard
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static MilesCard operator |(MilesCard r1, MilesCard r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(MilesCard entity) : base(entity) { }

			public Reference(MilesCard entity, string name) : base(entity, name) { }


			public static implicit operator Reference(MilesCard entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static MilesCard operator +(Reference reference, Domain db)
			{
				return db.MilesCard.Load(reference);
			}

		}

	}

	public partial class MilesCardManager : EntityManager<MilesCard, MilesCard.Service> { }

	#endregion

						
	#region MirFile

	partial class Domain : IEntityServiceContainer<Domain, MirFile>
	{

		public MirFile.Service MirFile { [DebuggerStepThrough] get { return ResolveService(ref _mirFile); } }
		private MirFile.Service _mirFile;

		EntityService<Domain, MirFile> IEntityServiceContainer<Domain, MirFile>.Service => MirFile;
		
		[DebuggerStepThrough]
		public static MirFile operator +(MirFile r, Domain db)
		{
			return (MirFile)r?.Resolve(db);
		}

	}

	partial class MirFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static MirFile operator |(MirFile r1, MirFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(MirFile entity) : base(entity) { }

			public Reference(MirFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(MirFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static MirFile operator +(Reference reference, Domain db)
			{
				return db.MirFile.Load(reference);
			}

		}

	}

	public partial class MirFileManager : EntityManager<MirFile, MirFile.Service> { }

	#endregion

						
	#region Modification

	partial class Domain : IEntityServiceContainer<Domain, Modification>
	{

		public Modification.Service Modification { [DebuggerStepThrough] get { return ResolveService(ref _modification); } }
		private Modification.Service _modification;

		EntityService<Domain, Modification> IEntityServiceContainer<Domain, Modification>.Service => Modification;
		
		[DebuggerStepThrough]
		public static Modification operator +(Modification r, Domain db)
		{
			return (Modification)r?.Resolve(db);
		}

	}

	partial class Modification
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Modification operator |(Modification r1, Modification r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class ModificationManager : EntityManager<Modification, Modification.Service> { }

	#endregion

						
	#region OpeningBalance

	partial class Domain : IEntityServiceContainer<Domain, OpeningBalance>
	{

		public OpeningBalance.Service OpeningBalance { [DebuggerStepThrough] get { return ResolveService(ref _openingBalance); } }
		private OpeningBalance.Service _openingBalance;

		EntityService<Domain, OpeningBalance> IEntityServiceContainer<Domain, OpeningBalance>.Service => OpeningBalance;
		
		[DebuggerStepThrough]
		public static OpeningBalance operator +(OpeningBalance r, Domain db)
		{
			return (OpeningBalance)r?.Resolve(db);
		}

	}

	partial class OpeningBalance
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static OpeningBalance operator |(OpeningBalance r1, OpeningBalance r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(OpeningBalance entity) : base(entity) { }

			public Reference(OpeningBalance entity, string name) : base(entity, name) { }


			public static implicit operator Reference(OpeningBalance entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static OpeningBalance operator +(Reference reference, Domain db)
			{
				return db.OpeningBalance.Load(reference);
			}

		}

	}

	public partial class OpeningBalanceManager : EntityManager<OpeningBalance, OpeningBalance.Service> { }

	#endregion

						
	#region Order

	partial class Domain : IEntityServiceContainer<Domain, Order>
	{

		public Order.Service Order { [DebuggerStepThrough] get { return ResolveService(ref _order); } }
		private Order.Service _order;

		EntityService<Domain, Order> IEntityServiceContainer<Domain, Order>.Service => Order;
		
		[DebuggerStepThrough]
		public static Order operator +(Order r, Domain db)
		{
			return (Order)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Order Export(Order r)
		{
			Order.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Order> entities)
		{
			foreach (var r in entities)
				Order.Export(r);
		}

	}

	partial class Order
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Order operator |(Order r1, Order r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Order entity) : base(entity) { }

			public Reference(Order entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Order entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Order operator +(Reference reference, Domain db)
			{
				return db.Order.Load(reference);
			}

		}

	}

	public partial class OrderManager : EntityManager<Order, Order.Service> { }

	#endregion

						
	#region OrderItem

	partial class Domain : IEntityServiceContainer<Domain, OrderItem>
	{

		public OrderItem.Service OrderItem { [DebuggerStepThrough] get { return ResolveService(ref _orderItem); } }
		private OrderItem.Service _orderItem;

		EntityService<Domain, OrderItem> IEntityServiceContainer<Domain, OrderItem>.Service => OrderItem;
		
		[DebuggerStepThrough]
		public static OrderItem operator +(OrderItem r, Domain db)
		{
			return (OrderItem)r?.Resolve(db);
		}

	}

	partial class OrderItem
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static OrderItem operator |(OrderItem r1, OrderItem r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(OrderItem entity) : base(entity) { }

			public Reference(OrderItem entity, string name) : base(entity, name) { }


			public static implicit operator Reference(OrderItem entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static OrderItem operator +(Reference reference, Domain db)
			{
				return db.OrderItem.Load(reference);
			}

		}

	}

	public partial class OrderItemManager : EntityManager<OrderItem, OrderItem.Service> { }

	#endregion

						
	#region Organization

	partial class Domain : IEntityServiceContainer<Domain, Organization>
	{

		public Organization.Service Organization { [DebuggerStepThrough] get { return ResolveService(ref _organization); } }
		private Organization.Service _organization;

		EntityService<Domain, Organization> IEntityServiceContainer<Domain, Organization>.Service => Organization;
		
		[DebuggerStepThrough]
		public static Organization operator +(Organization r, Domain db)
		{
			return (Organization)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Organization Export(Organization r)
		{
			Organization.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Organization> entities)
		{
			foreach (var r in entities)
				Organization.Export(r);
		}

	}

	partial class Organization
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Organization operator |(Organization r1, Organization r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Party.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Organization entity) : base(entity) { }

			public Reference(Organization entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Organization entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Organization operator +(Reference reference, Domain db)
			{
				return db.Organization.Load(reference);
			}

		}

	}

	public partial class OrganizationManager : EntityManager<Organization, Organization.Service> { }

	#endregion

						
	#region Party

	partial class Domain : IEntityServiceContainer<Domain, Party>
	{

		public Party.Service Party { [DebuggerStepThrough] get { return ResolveService(ref _party); } }
		private Party.Service _party;

		EntityService<Domain, Party> IEntityServiceContainer<Domain, Party>.Service => Party;
		
		[DebuggerStepThrough]
		public static Party operator +(Party r, Domain db)
		{
			return (Party)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Party Export(Party r)
		{
			Party.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Party> entities)
		{
			foreach (var r in entities)
				Party.Export(r);
		}

	}

	partial class Party
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(Party r, Action<Action<Party>> onCommit)
			{
				if (r == null) return false;

				r = db.Unproxy(r);
				var department = r as Department;
				if (department != null)
					return db.Department.Delete(department);

				var organization = r as Organization;
				if (organization != null)
					return db.Organization.Delete(organization);

				var person = r as Person;
				if (person != null)
					return db.Person.Delete(person);

				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override Party Save(Party r, Action<Action<Party>, Action<Party>> onCommit)
			{
				if (r == null) return null;

				r = db.Unproxy(r);

				var department = r as Department;
				if (department != null)
					return db.Department.Save(department);

				var organization = r as Organization;
				if (organization != null)
					return db.Organization.Save(organization);

				var person = r as Person;
				if (person != null)
					return db.Person.Save(person);

				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static Party operator |(Party r1, Party r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Party entity) : base(entity) { }

			public Reference(Party entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Party entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Party operator +(Reference reference, Domain db)
			{
				return db.Party.Load(reference);
			}

		}

	}

	public partial class PartyManager : EntityManager<Party, Party.Service> { }

	#endregion

						
	#region Passport

	partial class Domain : IEntityServiceContainer<Domain, Passport>
	{

		public Passport.Service Passport { [DebuggerStepThrough] get { return ResolveService(ref _passport); } }
		private Passport.Service _passport;

		EntityService<Domain, Passport> IEntityServiceContainer<Domain, Passport>.Service => Passport;
		
		[DebuggerStepThrough]
		public static Passport operator +(Passport r, Domain db)
		{
			return (Passport)r?.Resolve(db);
		}

	}

	partial class Passport
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Passport operator |(Passport r1, Passport r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Passport entity) : base(entity) { }

			public Reference(Passport entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Passport entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Passport operator +(Reference reference, Domain db)
			{
				return db.Passport.Load(reference);
			}

		}

	}

	public partial class PassportManager : EntityManager<Passport, Passport.Service> { }

	#endregion

						
	#region Pasteboard

	partial class Domain : IEntityServiceContainer<Domain, Pasteboard>
	{

		public Pasteboard.Service Pasteboard { [DebuggerStepThrough] get { return ResolveService(ref _pasteboard); } }
		private Pasteboard.Service _pasteboard;

		EntityService<Domain, Pasteboard> IEntityServiceContainer<Domain, Pasteboard>.Service => Pasteboard;
		
		[DebuggerStepThrough]
		public static Pasteboard operator +(Pasteboard r, Domain db)
		{
			return (Pasteboard)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Pasteboard Export(Pasteboard r)
		{
			Pasteboard.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Pasteboard> entities)
		{
			foreach (var r in entities)
				Pasteboard.Export(r);
		}

	}

	partial class Pasteboard
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Pasteboard operator |(Pasteboard r1, Pasteboard r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Pasteboard entity) : base(entity) { }

			public Reference(Pasteboard entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Pasteboard entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Pasteboard operator +(Reference reference, Domain db)
			{
				return db.Pasteboard.Load(reference);
			}

		}

	}

	public partial class PasteboardManager : ProductManager<Pasteboard, Pasteboard.Service> { }

	#endregion

						
	#region PasteboardRefund

	partial class Domain : IEntityServiceContainer<Domain, PasteboardRefund>
	{

		public PasteboardRefund.Service PasteboardRefund { [DebuggerStepThrough] get { return ResolveService(ref _pasteboardRefund); } }
		private PasteboardRefund.Service _pasteboardRefund;

		EntityService<Domain, PasteboardRefund> IEntityServiceContainer<Domain, PasteboardRefund>.Service => PasteboardRefund;
		
		[DebuggerStepThrough]
		public static PasteboardRefund operator +(PasteboardRefund r, Domain db)
		{
			return (PasteboardRefund)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public PasteboardRefund Export(PasteboardRefund r)
		{
			PasteboardRefund.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<PasteboardRefund> entities)
		{
			foreach (var r in entities)
				PasteboardRefund.Export(r);
		}

	}

	partial class PasteboardRefund
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static PasteboardRefund operator |(PasteboardRefund r1, PasteboardRefund r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Pasteboard.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(PasteboardRefund entity) : base(entity) { }

			public Reference(PasteboardRefund entity, string name) : base(entity, name) { }


			public static implicit operator Reference(PasteboardRefund entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static PasteboardRefund operator +(Reference reference, Domain db)
			{
				return db.PasteboardRefund.Load(reference);
			}

		}

	}

	public partial class PasteboardRefundManager : ProductManager<PasteboardRefund, PasteboardRefund.Service> { }

	#endregion

						
	#region Payment

	partial class Domain : IEntityServiceContainer<Domain, Payment>
	{

		public Payment.Service Payment { [DebuggerStepThrough] get { return ResolveService(ref _payment); } }
		private Payment.Service _payment;

		EntityService<Domain, Payment> IEntityServiceContainer<Domain, Payment>.Service => Payment;
		
		[DebuggerStepThrough]
		public static Payment operator +(Payment r, Domain db)
		{
			return (Payment)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Payment Export(Payment r)
		{
			Payment.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Payment> entities)
		{
			foreach (var r in entities)
				Payment.Export(r);
		}

		[DebuggerStepThrough]
		public Payment Import(Payment r)
		{
			Payment.Import(r);
			return r;
		}
		[DebuggerStepThrough]
		public void Import(IEnumerable<Payment> entities)
		{
			foreach (var r in entities)
				Payment.Import(r);
		}

	}

	partial class Payment
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(Payment r, Action<Action<Payment>> onCommit)
			{
				if (r == null) return false;

				r = db.Unproxy(r);
				var cashOutOrderPayment = r as CashOutOrderPayment;
				if (cashOutOrderPayment != null)
					return db.CashOutOrderPayment.Delete(cashOutOrderPayment);

				var cashInOrderPayment = r as CashInOrderPayment;
				if (cashInOrderPayment != null)
					return db.CashInOrderPayment.Delete(cashInOrderPayment);

				var checkPayment = r as CheckPayment;
				if (checkPayment != null)
					return db.CheckPayment.Delete(checkPayment);

				var electronicPayment = r as ElectronicPayment;
				if (electronicPayment != null)
					return db.ElectronicPayment.Delete(electronicPayment);

				var wireTransfer = r as WireTransfer;
				if (wireTransfer != null)
					return db.WireTransfer.Delete(wireTransfer);

				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override Payment Save(Payment r, Action<Action<Payment>, Action<Payment>> onCommit)
			{
				if (r == null) return null;

				r = db.Unproxy(r);

				var cashOutOrderPayment = r as CashOutOrderPayment;
				if (cashOutOrderPayment != null)
					return db.CashOutOrderPayment.Save(cashOutOrderPayment);

				var cashInOrderPayment = r as CashInOrderPayment;
				if (cashInOrderPayment != null)
					return db.CashInOrderPayment.Save(cashInOrderPayment);

				var checkPayment = r as CheckPayment;
				if (checkPayment != null)
					return db.CheckPayment.Save(checkPayment);

				var electronicPayment = r as ElectronicPayment;
				if (electronicPayment != null)
					return db.ElectronicPayment.Save(electronicPayment);

				var wireTransfer = r as WireTransfer;
				if (wireTransfer != null)
					return db.WireTransfer.Save(wireTransfer);

				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static Payment operator |(Payment r1, Payment r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Payment entity) : base(entity) { }

			public Reference(Payment entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Payment entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Payment operator +(Reference reference, Domain db)
			{
				return db.Payment.Load(reference);
			}

		}

	}

	public partial class PaymentManager : EntityManager<Payment, Payment.Service> { }

	#endregion

						
	#region PaymentSystem

	partial class Domain : IEntityServiceContainer<Domain, PaymentSystem>
	{

		public PaymentSystem.Service PaymentSystem { [DebuggerStepThrough] get { return ResolveService(ref _paymentSystem); } }
		private PaymentSystem.Service _paymentSystem;

		EntityService<Domain, PaymentSystem> IEntityServiceContainer<Domain, PaymentSystem>.Service => PaymentSystem;
		
		[DebuggerStepThrough]
		public static PaymentSystem operator +(PaymentSystem r, Domain db)
		{
			return (PaymentSystem)r?.Resolve(db);
		}

	}

	partial class PaymentSystem
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static PaymentSystem operator |(PaymentSystem r1, PaymentSystem r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(PaymentSystem entity) : base(entity) { }

			public Reference(PaymentSystem entity, string name) : base(entity, name) { }


			public static implicit operator Reference(PaymentSystem entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static PaymentSystem operator +(Reference reference, Domain db)
			{
				return db.PaymentSystem.Load(reference);
			}

		}

	}

	public partial class PaymentSystemManager : EntityManager<PaymentSystem, PaymentSystem.Service> { }

	#endregion

						
	#region PenalizeOperation

	partial class Domain : IEntityServiceContainer<Domain, PenalizeOperation>
	{

		public PenalizeOperation.Service PenalizeOperation { [DebuggerStepThrough] get { return ResolveService(ref _penalizeOperation); } }
		private PenalizeOperation.Service _penalizeOperation;

		EntityService<Domain, PenalizeOperation> IEntityServiceContainer<Domain, PenalizeOperation>.Service => PenalizeOperation;
		
		[DebuggerStepThrough]
		public static PenalizeOperation operator +(PenalizeOperation r, Domain db)
		{
			return (PenalizeOperation)r?.Resolve(db);
		}

	}

	partial class PenalizeOperation
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static PenalizeOperation operator |(PenalizeOperation r1, PenalizeOperation r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class PenalizeOperationManager : EntityManager<PenalizeOperation, PenalizeOperation.Service> { }

	#endregion

						
	#region Person

	partial class Domain : IEntityServiceContainer<Domain, Person>
	{

		public Person.Service Person { [DebuggerStepThrough] get { return ResolveService(ref _person); } }
		private Person.Service _person;

		EntityService<Domain, Person> IEntityServiceContainer<Domain, Person>.Service => Person;
		
		[DebuggerStepThrough]
		public static Person operator +(Person r, Domain db)
		{
			return (Person)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Person Export(Person r)
		{
			Person.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Person> entities)
		{
			foreach (var r in entities)
				Person.Export(r);
		}

	}

	partial class Person
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Person operator |(Person r1, Person r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Party.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Person entity) : base(entity) { }

			public Reference(Person entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Person entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Person operator +(Reference reference, Domain db)
			{
				return db.Person.Load(reference);
			}

		}

	}

	public partial class PersonManager : EntityManager<Person, Person.Service> { }

	#endregion

						
	#region Preferences

	partial class Domain : IEntityServiceContainer<Domain, Preferences>
	{

		public Preferences.Service Preferences { [DebuggerStepThrough] get { return ResolveService(ref _preferences); } }
		private Preferences.Service _preferences;

		EntityService<Domain, Preferences> IEntityServiceContainer<Domain, Preferences>.Service => Preferences;
		
		[DebuggerStepThrough]
		public static Preferences operator +(Preferences r, Domain db)
		{
			return (Preferences)r?.Resolve(db);
		}

	}

	partial class Preferences
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(Preferences r, Action<Action<Preferences>> onCommit)
			{
				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override Preferences Save(Preferences r, Action<Action<Preferences>, Action<Preferences>> onCommit)
			{
				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static Preferences operator |(Preferences r1, Preferences r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class PreferencesManager : EntityManager<Preferences, Preferences.Service> { }

	#endregion

						
	#region PrintFile

	partial class Domain : IEntityServiceContainer<Domain, PrintFile>
	{

		public PrintFile.Service PrintFile { [DebuggerStepThrough] get { return ResolveService(ref _printFile); } }
		private PrintFile.Service _printFile;

		EntityService<Domain, PrintFile> IEntityServiceContainer<Domain, PrintFile>.Service => PrintFile;
		
		[DebuggerStepThrough]
		public static PrintFile operator +(PrintFile r, Domain db)
		{
			return (PrintFile)r?.Resolve(db);
		}

	}

	partial class PrintFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static PrintFile operator |(PrintFile r1, PrintFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(PrintFile entity) : base(entity) { }

			public Reference(PrintFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(PrintFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static PrintFile operator +(Reference reference, Domain db)
			{
				return db.PrintFile.Load(reference);
			}

		}

	}

	public partial class PrintFileManager : EntityManager<PrintFile, PrintFile.Service> { }

	#endregion

						
	#region Product

	partial class Domain : IEntityServiceContainer<Domain, Product>
	{

		public Product.Service Product { [DebuggerStepThrough] get { return ResolveService(ref _product); } }
		private Product.Service _product;

		EntityService<Domain, Product> IEntityServiceContainer<Domain, Product>.Service => Product;
		
		[DebuggerStepThrough]
		public static Product operator +(Product r, Domain db)
		{
			return (Product)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Product Export(Product r)
		{
			Product.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Product> entities)
		{
			foreach (var r in entities)
				Product.Export(r);
		}

	}

	partial class Product
	{ 


		partial class Service
		{

			//[DebuggerStepThrough]
			protected override bool Delete(Product r, Action<Action<Product>> onCommit)
			{
				if (r == null) return false;

				r = db.Unproxy(r);
				var accommodation = r as Accommodation;
				if (accommodation != null)
					return db.Accommodation.Delete(accommodation);

				var aviaMco = r as AviaMco;
				if (aviaMco != null)
					return db.AviaMco.Delete(aviaMco);

				var aviaRefund = r as AviaRefund;
				if (aviaRefund != null)
					return db.AviaRefund.Delete(aviaRefund);

				var aviaTicket = r as AviaTicket;
				if (aviaTicket != null)
					return db.AviaTicket.Delete(aviaTicket);

				var busTicket = r as BusTicket;
				if (busTicket != null)
					return db.BusTicket.Delete(busTicket);

				var busTicketRefund = r as BusTicketRefund;
				if (busTicketRefund != null)
					return db.BusTicketRefund.Delete(busTicketRefund);

				var carRental = r as CarRental;
				if (carRental != null)
					return db.CarRental.Delete(carRental);

				var excursion = r as Excursion;
				if (excursion != null)
					return db.Excursion.Delete(excursion);

				var genericProduct = r as GenericProduct;
				if (genericProduct != null)
					return db.GenericProduct.Delete(genericProduct);

				var insurance = r as Insurance;
				if (insurance != null)
					return db.Insurance.Delete(insurance);

				var insuranceRefund = r as InsuranceRefund;
				if (insuranceRefund != null)
					return db.InsuranceRefund.Delete(insuranceRefund);

				var isic = r as Isic;
				if (isic != null)
					return db.Isic.Delete(isic);

				var pasteboard = r as Pasteboard;
				if (pasteboard != null)
					return db.Pasteboard.Delete(pasteboard);

				var pasteboardRefund = r as PasteboardRefund;
				if (pasteboardRefund != null)
					return db.PasteboardRefund.Delete(pasteboardRefund);

				var simCard = r as SimCard;
				if (simCard != null)
					return db.SimCard.Delete(simCard);

				var tour = r as Tour;
				if (tour != null)
					return db.Tour.Delete(tour);

				var transfer = r as Transfer;
				if (transfer != null)
					return db.Transfer.Delete(transfer);

				throw new NotImplementedException();
			}

			//[DebuggerStepThrough]
			protected override Product Save(Product r, Action<Action<Product>, Action<Product>> onCommit)
			{
				if (r == null) return null;

				r = db.Unproxy(r);

				var accommodation = r as Accommodation;
				if (accommodation != null)
					return db.Accommodation.Save(accommodation);

				var aviaMco = r as AviaMco;
				if (aviaMco != null)
					return db.AviaMco.Save(aviaMco);

				var aviaRefund = r as AviaRefund;
				if (aviaRefund != null)
					return db.AviaRefund.Save(aviaRefund);

				var aviaTicket = r as AviaTicket;
				if (aviaTicket != null)
					return db.AviaTicket.Save(aviaTicket);

				var busTicket = r as BusTicket;
				if (busTicket != null)
					return db.BusTicket.Save(busTicket);

				var busTicketRefund = r as BusTicketRefund;
				if (busTicketRefund != null)
					return db.BusTicketRefund.Save(busTicketRefund);

				var carRental = r as CarRental;
				if (carRental != null)
					return db.CarRental.Save(carRental);

				var excursion = r as Excursion;
				if (excursion != null)
					return db.Excursion.Save(excursion);

				var genericProduct = r as GenericProduct;
				if (genericProduct != null)
					return db.GenericProduct.Save(genericProduct);

				var insurance = r as Insurance;
				if (insurance != null)
					return db.Insurance.Save(insurance);

				var insuranceRefund = r as InsuranceRefund;
				if (insuranceRefund != null)
					return db.InsuranceRefund.Save(insuranceRefund);

				var isic = r as Isic;
				if (isic != null)
					return db.Isic.Save(isic);

				var pasteboard = r as Pasteboard;
				if (pasteboard != null)
					return db.Pasteboard.Save(pasteboard);

				var pasteboardRefund = r as PasteboardRefund;
				if (pasteboardRefund != null)
					return db.PasteboardRefund.Save(pasteboardRefund);

				var simCard = r as SimCard;
				if (simCard != null)
					return db.SimCard.Save(simCard);

				var tour = r as Tour;
				if (tour != null)
					return db.Tour.Save(tour);

				var transfer = r as Transfer;
				if (transfer != null)
					return db.Transfer.Save(transfer);

				throw new NotImplementedException();
			}

		}


		#region Operators

		[DebuggerStepThrough]
		public static Product operator |(Product r1, Product r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Product entity) : base(entity) { }

			public Reference(Product entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Product entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Product operator +(Reference reference, Domain db)
			{
				return db.Product.Load(reference);
			}

		}

	}

	public partial class ProductManager : ProductManager<Product, Product.Service> { }

	#endregion

						
	#region ProductPassenger

	partial class Domain : IEntityServiceContainer<Domain, ProductPassenger>
	{

		public ProductPassenger.Service ProductPassenger { [DebuggerStepThrough] get { return ResolveService(ref _productPassenger); } }
		private ProductPassenger.Service _productPassenger;

		EntityService<Domain, ProductPassenger> IEntityServiceContainer<Domain, ProductPassenger>.Service => ProductPassenger;
		
		[DebuggerStepThrough]
		public static ProductPassenger operator +(ProductPassenger r, Domain db)
		{
			return (ProductPassenger)r?.Resolve(db);
		}

	}

	partial class ProductPassenger
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static ProductPassenger operator |(ProductPassenger r1, ProductPassenger r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class ProductPassengerManager : EntityManager<ProductPassenger, ProductPassenger.Service> { }

	#endregion

						
	#region SabreFilFile

	partial class Domain : IEntityServiceContainer<Domain, SabreFilFile>
	{

		public SabreFilFile.Service SabreFilFile { [DebuggerStepThrough] get { return ResolveService(ref _sabreFilFile); } }
		private SabreFilFile.Service _sabreFilFile;

		EntityService<Domain, SabreFilFile> IEntityServiceContainer<Domain, SabreFilFile>.Service => SabreFilFile;
		
		[DebuggerStepThrough]
		public static SabreFilFile operator +(SabreFilFile r, Domain db)
		{
			return (SabreFilFile)r?.Resolve(db);
		}

	}

	partial class SabreFilFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static SabreFilFile operator |(SabreFilFile r1, SabreFilFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(SabreFilFile entity) : base(entity) { }

			public Reference(SabreFilFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(SabreFilFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static SabreFilFile operator +(Reference reference, Domain db)
			{
				return db.SabreFilFile.Load(reference);
			}

		}

	}

	public partial class SabreFilFileManager : EntityManager<SabreFilFile, SabreFilFile.Service> { }

	#endregion

						
	#region SimCard

	partial class Domain : IEntityServiceContainer<Domain, SimCard>
	{

		public SimCard.Service SimCard { [DebuggerStepThrough] get { return ResolveService(ref _simCard); } }
		private SimCard.Service _simCard;

		EntityService<Domain, SimCard> IEntityServiceContainer<Domain, SimCard>.Service => SimCard;
		
		[DebuggerStepThrough]
		public static SimCard operator +(SimCard r, Domain db)
		{
			return (SimCard)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public SimCard Export(SimCard r)
		{
			SimCard.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<SimCard> entities)
		{
			foreach (var r in entities)
				SimCard.Export(r);
		}

	}

	partial class SimCard
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static SimCard operator |(SimCard r1, SimCard r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(SimCard entity) : base(entity) { }

			public Reference(SimCard entity, string name) : base(entity, name) { }


			public static implicit operator Reference(SimCard entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static SimCard operator +(Reference reference, Domain db)
			{
				return db.SimCard.Load(reference);
			}

		}

	}

	public partial class SimCardManager : ProductManager<SimCard, SimCard.Service> { }

	#endregion

						
	#region SirenaFile

	partial class Domain : IEntityServiceContainer<Domain, SirenaFile>
	{

		public SirenaFile.Service SirenaFile { [DebuggerStepThrough] get { return ResolveService(ref _sirenaFile); } }
		private SirenaFile.Service _sirenaFile;

		EntityService<Domain, SirenaFile> IEntityServiceContainer<Domain, SirenaFile>.Service => SirenaFile;
		
		[DebuggerStepThrough]
		public static SirenaFile operator +(SirenaFile r, Domain db)
		{
			return (SirenaFile)r?.Resolve(db);
		}

	}

	partial class SirenaFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static SirenaFile operator |(SirenaFile r1, SirenaFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(SirenaFile entity) : base(entity) { }

			public Reference(SirenaFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(SirenaFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static SirenaFile operator +(Reference reference, Domain db)
			{
				return db.SirenaFile.Load(reference);
			}

		}

	}

	public partial class SirenaFileManager : EntityManager<SirenaFile, SirenaFile.Service> { }

	#endregion

						
	#region SystemConfiguration

	partial class Domain : IEntityServiceContainer<Domain, SystemConfiguration>
	{

		public SystemConfiguration.Service SystemConfiguration { [DebuggerStepThrough] get { return ResolveService(ref _systemConfiguration); } }
		private SystemConfiguration.Service _systemConfiguration;

		EntityService<Domain, SystemConfiguration> IEntityServiceContainer<Domain, SystemConfiguration>.Service => SystemConfiguration;
		
		[DebuggerStepThrough]
		public static SystemConfiguration operator +(SystemConfiguration r, Domain db)
		{
			return (SystemConfiguration)r?.Resolve(db);
		}

	}

	partial class SystemConfiguration
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static SystemConfiguration operator |(SystemConfiguration r1, SystemConfiguration r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class SystemConfigurationManager : EntityManager<SystemConfiguration, SystemConfiguration.Service> { }

	#endregion

						
	#region Task

	partial class Domain : IEntityServiceContainer<Domain, Task>
	{

		public Task.Service Task { [DebuggerStepThrough] get { return ResolveService(ref _task); } }
		private Task.Service _task;

		EntityService<Domain, Task> IEntityServiceContainer<Domain, Task>.Service => Task;
		
		[DebuggerStepThrough]
		public static Task operator +(Task r, Domain db)
		{
			return (Task)r?.Resolve(db);
		}

	}

	partial class Task
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Task operator |(Task r1, Task r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public class Reference : EntityReference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Task entity) : base(entity) { }

			public Reference(Task entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Task entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Task operator +(Reference reference, Domain db)
			{
				return db.Task.Load(reference);
			}

		}

	}

	public partial class TaskManager : EntityManager<Task, Task.Service> { }

	#endregion

						
	#region TaskComment

	partial class Domain : IEntityServiceContainer<Domain, TaskComment>
	{

		public TaskComment.Service TaskComment { [DebuggerStepThrough] get { return ResolveService(ref _taskComment); } }
		private TaskComment.Service _taskComment;

		EntityService<Domain, TaskComment> IEntityServiceContainer<Domain, TaskComment>.Service => TaskComment;
		
		[DebuggerStepThrough]
		public static TaskComment operator +(TaskComment r, Domain db)
		{
			return (TaskComment)r?.Resolve(db);
		}

	}

	partial class TaskComment
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static TaskComment operator |(TaskComment r1, TaskComment r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class TaskCommentManager : EntityManager<TaskComment, TaskComment.Service> { }

	#endregion

						
	#region TktFile

	partial class Domain : IEntityServiceContainer<Domain, TktFile>
	{

		public TktFile.Service TktFile { [DebuggerStepThrough] get { return ResolveService(ref _tktFile); } }
		private TktFile.Service _tktFile;

		EntityService<Domain, TktFile> IEntityServiceContainer<Domain, TktFile>.Service => TktFile;
		
		[DebuggerStepThrough]
		public static TktFile operator +(TktFile r, Domain db)
		{
			return (TktFile)r?.Resolve(db);
		}

	}

	partial class TktFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static TktFile operator |(TktFile r1, TktFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(TktFile entity) : base(entity) { }

			public Reference(TktFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(TktFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static TktFile operator +(Reference reference, Domain db)
			{
				return db.TktFile.Load(reference);
			}

		}

	}

	public partial class TktFileManager : EntityManager<TktFile, TktFile.Service> { }

	#endregion

						
	#region Tour

	partial class Domain : IEntityServiceContainer<Domain, Tour>
	{

		public Tour.Service Tour { [DebuggerStepThrough] get { return ResolveService(ref _tour); } }
		private Tour.Service _tour;

		EntityService<Domain, Tour> IEntityServiceContainer<Domain, Tour>.Service => Tour;
		
		[DebuggerStepThrough]
		public static Tour operator +(Tour r, Domain db)
		{
			return (Tour)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Tour Export(Tour r)
		{
			Tour.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Tour> entities)
		{
			foreach (var r in entities)
				Tour.Export(r);
		}

	}

	partial class Tour
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Tour operator |(Tour r1, Tour r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Tour entity) : base(entity) { }

			public Reference(Tour entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Tour entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Tour operator +(Reference reference, Domain db)
			{
				return db.Tour.Load(reference);
			}

		}

	}

	public partial class TourManager : ProductManager<Tour, Tour.Service> { }

	#endregion

						
	#region Transfer

	partial class Domain : IEntityServiceContainer<Domain, Transfer>
	{

		public Transfer.Service Transfer { [DebuggerStepThrough] get { return ResolveService(ref _transfer); } }
		private Transfer.Service _transfer;

		EntityService<Domain, Transfer> IEntityServiceContainer<Domain, Transfer>.Service => Transfer;
		
		[DebuggerStepThrough]
		public static Transfer operator +(Transfer r, Domain db)
		{
			return (Transfer)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public Transfer Export(Transfer r)
		{
			Transfer.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<Transfer> entities)
		{
			foreach (var r in entities)
				Transfer.Export(r);
		}

	}

	partial class Transfer
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static Transfer operator |(Transfer r1, Transfer r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Product.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(Transfer entity) : base(entity) { }

			public Reference(Transfer entity, string name) : base(entity, name) { }


			public static implicit operator Reference(Transfer entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static Transfer operator +(Reference reference, Domain db)
			{
				return db.Transfer.Load(reference);
			}

		}

	}

	public partial class TransferManager : ProductManager<Transfer, Transfer.Service> { }

	#endregion

						
	#region TravelPointXmlFile

	partial class Domain : IEntityServiceContainer<Domain, TravelPointXmlFile>
	{

		public TravelPointXmlFile.Service TravelPointXmlFile { [DebuggerStepThrough] get { return ResolveService(ref _travelPointXmlFile); } }
		private TravelPointXmlFile.Service _travelPointXmlFile;

		EntityService<Domain, TravelPointXmlFile> IEntityServiceContainer<Domain, TravelPointXmlFile>.Service => TravelPointXmlFile;
		
		[DebuggerStepThrough]
		public static TravelPointXmlFile operator +(TravelPointXmlFile r, Domain db)
		{
			return (TravelPointXmlFile)r?.Resolve(db);
		}

	}

	partial class TravelPointXmlFile
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static TravelPointXmlFile operator |(TravelPointXmlFile r1, TravelPointXmlFile r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : GdsFile.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(TravelPointXmlFile entity) : base(entity) { }

			public Reference(TravelPointXmlFile entity, string name) : base(entity, name) { }


			public static implicit operator Reference(TravelPointXmlFile entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static TravelPointXmlFile operator +(Reference reference, Domain db)
			{
				return db.TravelPointXmlFile.Load(reference);
			}

		}

	}

	public partial class TravelPointXmlFileManager : EntityManager<TravelPointXmlFile, TravelPointXmlFile.Service> { }

	#endregion

						
	#region User

	partial class Domain : IEntityServiceContainer<Domain, User>
	{

		public User.Service User { [DebuggerStepThrough] get { return ResolveService(ref _user); } }
		private User.Service _user;

		EntityService<Domain, User> IEntityServiceContainer<Domain, User>.Service => User;
		
		[DebuggerStepThrough]
		public static User operator +(User r, Domain db)
		{
			return (User)r?.Resolve(db);
		}

	}

	partial class User
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static User operator |(User r1, User r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Identity.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(User entity) : base(entity) { }

			public Reference(User entity, string name) : base(entity, name) { }


			public static implicit operator Reference(User entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static User operator +(Reference reference, Domain db)
			{
				return db.User.Load(reference);
			}

		}

	}

	public partial class UserManager : EntityManager<User, User.Service> { }

	#endregion

						
	#region UserVisit

	partial class Domain : IEntityServiceContainer<Domain, UserVisit>
	{

		public UserVisit.Service UserVisit { [DebuggerStepThrough] get { return ResolveService(ref _userVisit); } }
		private UserVisit.Service _userVisit;

		EntityService<Domain, UserVisit> IEntityServiceContainer<Domain, UserVisit>.Service => UserVisit;
		
		[DebuggerStepThrough]
		public static UserVisit operator +(UserVisit r, Domain db)
		{
			return (UserVisit)r?.Resolve(db);
		}

	}

	partial class UserVisit
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static UserVisit operator |(UserVisit r1, UserVisit r2)
		{
			return r1 ?? r2;
		}

		#endregion

	}

	public partial class UserVisitManager : EntityManager<UserVisit, UserVisit.Service> { }

	#endregion

						
	#region WireTransfer

	partial class Domain : IEntityServiceContainer<Domain, WireTransfer>
	{

		public WireTransfer.Service WireTransfer { [DebuggerStepThrough] get { return ResolveService(ref _wireTransfer); } }
		private WireTransfer.Service _wireTransfer;

		EntityService<Domain, WireTransfer> IEntityServiceContainer<Domain, WireTransfer>.Service => WireTransfer;
		
		[DebuggerStepThrough]
		public static WireTransfer operator +(WireTransfer r, Domain db)
		{
			return (WireTransfer)r?.Resolve(db);
		}

		[DebuggerStepThrough]
		public WireTransfer Export(WireTransfer r)
		{
			WireTransfer.Export(r);
			return r;
		}

		[DebuggerStepThrough]
		public void Export(IEnumerable<WireTransfer> entities)
		{
			foreach (var r in entities)
				WireTransfer.Export(r);
		}

		[DebuggerStepThrough]
		public WireTransfer Import(WireTransfer r)
		{
			WireTransfer.Import(r);
			return r;
		}
		[DebuggerStepThrough]
		public void Import(IEnumerable<WireTransfer> entities)
		{
			foreach (var r in entities)
				WireTransfer.Import(r);
		}

	}

	partial class WireTransfer
	{ 


		#region Operators

		[DebuggerStepThrough]
		public static WireTransfer operator |(WireTransfer r1, WireTransfer r2)
		{
			return r1 ?? r2;
		}

		#endregion


		public new class Reference : Payment.Reference
		{

			public Reference() { }

			public Reference(string type, object id, string name) : base(type, id, name) { }

			public Reference(WireTransfer entity) : base(entity) { }

			public Reference(WireTransfer entity, string name) : base(entity, name) { }


			public static implicit operator Reference(WireTransfer entity)
			{
				return entity == null ? null : new Reference(entity);
			}

			[DebuggerStepThrough]
			public static WireTransfer operator +(Reference reference, Domain db)
			{
				return db.WireTransfer.Load(reference);
			}

		}

	}

	public partial class WireTransferManager : EntityManager<WireTransfer, WireTransfer.Service> { }

	#endregion


	#region Enums

	partial class Domain
	{

		public static AirlinePassportRequirement operator +(AirlinePassportRequirement x, Domain y)
		{
			return x;
		}
		public static AirlinePassportRequirement? operator +(AirlinePassportRequirement? x, Domain y)
		{
			return x;
		}

		public static AmadeusRizUsingMode operator +(AmadeusRizUsingMode x, Domain y)
		{
			return x;
		}
		public static AmadeusRizUsingMode? operator +(AmadeusRizUsingMode? x, Domain y)
		{
			return x;
		}

		public static AviaDocumentState operator +(AviaDocumentState x, Domain y)
		{
			return x;
		}
		public static AviaDocumentState? operator +(AviaDocumentState? x, Domain y)
		{
			return x;
		}

		public static AviaDocumentVatOptions operator +(AviaDocumentVatOptions x, Domain y)
		{
			return x;
		}
		public static AviaDocumentVatOptions? operator +(AviaDocumentVatOptions? x, Domain y)
		{
			return x;
		}

		public static AviaOrderItemGenerationOption operator +(AviaOrderItemGenerationOption x, Domain y)
		{
			return x;
		}
		public static AviaOrderItemGenerationOption? operator +(AviaOrderItemGenerationOption? x, Domain y)
		{
			return x;
		}

		public static DocumentAccessRestriction operator +(DocumentAccessRestriction x, Domain y)
		{
			return x;
		}
		public static DocumentAccessRestriction? operator +(DocumentAccessRestriction? x, Domain y)
		{
			return x;
		}

		public static FlightSegmentType operator +(FlightSegmentType x, Domain y)
		{
			return x;
		}
		public static FlightSegmentType? operator +(FlightSegmentType? x, Domain y)
		{
			return x;
		}

		public static GdsFileType operator +(GdsFileType x, Domain y)
		{
			return x;
		}
		public static GdsFileType? operator +(GdsFileType? x, Domain y)
		{
			return x;
		}

		public static GdsOriginator operator +(GdsOriginator x, Domain y)
		{
			return x;
		}
		public static GdsOriginator? operator +(GdsOriginator? x, Domain y)
		{
			return x;
		}

		public static GdsPassportStatus operator +(GdsPassportStatus x, Domain y)
		{
			return x;
		}
		public static GdsPassportStatus? operator +(GdsPassportStatus? x, Domain y)
		{
			return x;
		}

		public static Gender operator +(Gender x, Domain y)
		{
			return x;
		}
		public static Gender? operator +(Gender? x, Domain y)
		{
			return x;
		}

		public static ImportResult operator +(ImportResult x, Domain y)
		{
			return x;
		}
		public static ImportResult? operator +(ImportResult? x, Domain y)
		{
			return x;
		}

		public static InvoiceNumberMode operator +(InvoiceNumberMode x, Domain y)
		{
			return x;
		}
		public static InvoiceNumberMode? operator +(InvoiceNumberMode? x, Domain y)
		{
			return x;
		}

		public static InvoiceType operator +(InvoiceType x, Domain y)
		{
			return x;
		}
		public static InvoiceType? operator +(InvoiceType? x, Domain y)
		{
			return x;
		}

		public static IsicCardType operator +(IsicCardType x, Domain y)
		{
			return x;
		}
		public static IsicCardType? operator +(IsicCardType? x, Domain y)
		{
			return x;
		}

		public static MealType operator +(MealType x, Domain y)
		{
			return x;
		}
		public static MealType? operator +(MealType? x, Domain y)
		{
			return x;
		}

		public static ModificationType operator +(ModificationType x, Domain y)
		{
			return x;
		}
		public static ModificationType? operator +(ModificationType? x, Domain y)
		{
			return x;
		}

		public static OrderItemLinkType operator +(OrderItemLinkType x, Domain y)
		{
			return x;
		}
		public static OrderItemLinkType? operator +(OrderItemLinkType? x, Domain y)
		{
			return x;
		}

		public static PartyType operator +(PartyType x, Domain y)
		{
			return x;
		}
		public static PartyType? operator +(PartyType? x, Domain y)
		{
			return x;
		}

		public static PassportValidationResult operator +(PassportValidationResult x, Domain y)
		{
			return x;
		}
		public static PassportValidationResult? operator +(PassportValidationResult? x, Domain y)
		{
			return x;
		}

		public static PasteboardServiceClass operator +(PasteboardServiceClass x, Domain y)
		{
			return x;
		}
		public static PasteboardServiceClass? operator +(PasteboardServiceClass? x, Domain y)
		{
			return x;
		}

		public static PaymentForm operator +(PaymentForm x, Domain y)
		{
			return x;
		}
		public static PaymentForm? operator +(PaymentForm? x, Domain y)
		{
			return x;
		}

		public static PaymentType operator +(PaymentType x, Domain y)
		{
			return x;
		}
		public static PaymentType? operator +(PaymentType? x, Domain y)
		{
			return x;
		}

		public static PenalizeOperationStatus operator +(PenalizeOperationStatus x, Domain y)
		{
			return x;
		}
		public static PenalizeOperationStatus? operator +(PenalizeOperationStatus? x, Domain y)
		{
			return x;
		}

		public static PenalizeOperationType operator +(PenalizeOperationType x, Domain y)
		{
			return x;
		}
		public static PenalizeOperationType? operator +(PenalizeOperationType? x, Domain y)
		{
			return x;
		}

		public static PeriodState operator +(PeriodState x, Domain y)
		{
			return x;
		}
		public static PeriodState? operator +(PeriodState? x, Domain y)
		{
			return x;
		}

		public static ProductOrigin operator +(ProductOrigin x, Domain y)
		{
			return x;
		}
		public static ProductOrigin? operator +(ProductOrigin? x, Domain y)
		{
			return x;
		}

		public static ProductType operator +(ProductType x, Domain y)
		{
			return x;
		}
		public static ProductType? operator +(ProductType? x, Domain y)
		{
			return x;
		}

		public static ProductTypes operator +(ProductTypes x, Domain y)
		{
			return x;
		}
		public static ProductTypes? operator +(ProductTypes? x, Domain y)
		{
			return x;
		}

		public static ServiceClass operator +(ServiceClass x, Domain y)
		{
			return x;
		}
		public static ServiceClass? operator +(ServiceClass? x, Domain y)
		{
			return x;
		}

		public static ServiceFeeMode operator +(ServiceFeeMode x, Domain y)
		{
			return x;
		}
		public static ServiceFeeMode? operator +(ServiceFeeMode? x, Domain y)
		{
			return x;
		}

		public static TaskStatus operator +(TaskStatus x, Domain y)
		{
			return x;
		}
		public static TaskStatus? operator +(TaskStatus? x, Domain y)
		{
			return x;
		}

		public static TaxRate operator +(TaxRate x, Domain y)
		{
			return x;
		}
		public static TaxRate? operator +(TaxRate? x, Domain y)
		{
			return x;
		}

		public static UserRole operator +(UserRole x, Domain y)
		{
			return x;
		}
		public static UserRole? operator +(UserRole? x, Domain y)
		{
			return x;
		}

	}

	#endregion

}
