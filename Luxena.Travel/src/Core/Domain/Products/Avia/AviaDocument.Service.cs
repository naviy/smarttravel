using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Travel.Export;
using Luxena.Travel.Parsers;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Domain
{

	partial class AviaDocument
	{
		public override Entity Resolve(Domain db)
		{
			base.Resolve(db);

			var r = this;

			if (r.Producer != null)
			{
				r.AirlineIataCode = r.Producer.AirlineIataCode ?? r.AirlineIataCode;
				r.AirlinePrefixCode = r.Producer.AirlinePrefixCode ?? r.AirlinePrefixCode;

				r.Producer += db.Airline;

				r.AirlineIataCode = r.Producer?.AirlineIataCode ?? r.AirlineIataCode;
				r.AirlinePrefixCode = r.Producer?.AirlinePrefixCode ?? r.AirlinePrefixCode;
			}
			else if (r.AirlineIataCode.Yes())
			{
				r.Producer = db.Airline.ByIataCode(r.AirlineIataCode);

				if (r.Producer != null && r.Producer.AirlinePrefixCode.Yes())
					r.AirlinePrefixCode = r.Producer.AirlinePrefixCode;
			}
			else if (r.AirlinePrefixCode.Yes())
			{
				r.Producer = db.Airline.ByPrefixCode(r.AirlinePrefixCode);
			}


			//!!! to be replaced by an unique constraint
			if (db.AviaDocument.IsExists(r))
				throw new DomainException(string.Format(Exceptions.ImportGdsFile_DocumentAlreadyExists, r));


			var vatFee = r.Fees.By(fee => fee.Code == AviaDocumentFee.VatCode);
			if (vatFee != null)
			{
				if (db.Configuration.AviaDocumentVatOptions == AviaDocumentVatOptions.UseHFTax)
				{
					r.Vat = vatFee.Amount;
				}
				else
				{
					r.Vat = r.Total * db.Configuration.VatRate / (100 + db.Configuration.VatRate);
				}
			}

			r.Fees.ForEach(a => a.Amount += db);

			if (r.Customer != null)
				r.SetCustomer(db, db.Party.ByLegalName(r.Customer.LegalName));

			if (r.GdsPassport.No())
			{
				r.GdsPassportStatus = GdsPassportStatus.Unknown;
			}
			else
			{
				r.GdsPassportStatus = GdsPassportStatus.Exist;

				var passportNumber = r.ParseGdsPassport().Number;
				if (passportNumber.Yes())
				{
					var passport = db.Passport.ByNumber(passportNumber);

					if (passport != null && db.AviaDocument.ValidatePassengerPassport(r, passport, false) == PassportValidationResult.Valid)
						r.Passenger = passport.Owner;
				}
			}

			return r;
		}




		public new class Service<TAviaDocument> : Product.Service<TAviaDocument>
			where TAviaDocument : AviaDocument
		{

			#region Read

			public TAviaDocument ByNumber(string airlinePrefixCode, string number)
			{
				return airlinePrefixCode.Yes()
					? By(a => a.Number == number && a.AirlinePrefixCode == airlinePrefixCode)
					: By(a => a.Number == number);
			}

			public TAviaDocument ByNumber(TAviaDocument document)
			{
				if (document == null || !document.Number.Yes())
					return null;

				var prefixCode = document.AirlinePrefixCode;
				var iataCode = document.AirlineIataCode;
				if (prefixCode.No() && iataCode.No())
					return null;

				return prefixCode.Yes() 
					? By(a => a.Number == document.Number && a.AirlinePrefixCode == prefixCode) 
					: By(a => a.Number == document.Number && a.AirlineIataCode == iataCode);
			}

			public TAviaDocument ByNumber(string number)
			{
				return number == null ? null : By(a => a.Number == number);
			}

			public TAviaDocument ByFullNumber(string fullNumber)
			{
				if (fullNumber.No()) return null;

				var split = fullNumber.Split('-');

				return split.Length == 1
					? ByNumber(fullNumber)
					: ByNumber(split[0].Clip(), split[1].Clip());
			}

			public bool IsExists(TAviaDocument r)
			{
				if (r == null || r.Number.No() || r.AirlinePrefixCode.No())
					return false;

				var id = Query
					.Where(a =>
						a.Number == r.Number &&
						a.AirlinePrefixCode == r.AirlinePrefixCode &&
						a.Type == r.Type &&
						(r.Id == null || a.Id != r.Id)
					)
					.Select(a => a.Id)
					.FirstOrDefault();

				return id != null;
			}

			public AviaDocument FindToVoid(AviaDocumentVoiding voiding)
			{
				if (voiding.Document == null || voiding.Document.Number.No())
					throw new DomainException(Exceptions.ImportGdsFile_DocumentToVoidNotSpecified);

				var number = voiding.Document.Number;
				var prefixCode = voiding.Document.AirlinePrefixCode;

				AviaDocument doc;

				if (voiding.Document is AviaRefund)
					doc = db.AviaRefund.ByNumber(prefixCode, number);

				else if (voiding.Document is AviaMco)
					doc = db.AviaMco.ByNumber(prefixCode, number);

				else
				{
					AviaDocument ticket = db.AviaTicket.ByNumber(prefixCode, number);
					AviaDocument mco = db.AviaMco.ByNumber(prefixCode, number);

					if (ticket != null && mco != null)
						throw new DomainException(string.Format(Exceptions.ImportGdsFile_TwoDocumentsToVoid, voiding.Document));

					doc = ticket ?? mco;
				}

				if (doc == null)
					throw new DomainException(string.Format(Exceptions.ImportGdsFile_NotFoundDocumentToVoid, voiding.Document));

				return doc;
			}



			#endregion


			#region Permissions

			public override OperationStatus CanDelete(TAviaDocument r)
			{
				var status = base.CanDelete(r);
				if (!status) return status;

				return r.Origin == ProductOrigin.Manual || CanUpdateAll();
			}


			public OperationStatus CanProcess(AviaDocument document)
			{
				var status = OperationStatus.Enabled();

				if (/*document.Total == null || document.Commission == null || */ 
					document.PassengerName.No() || document.Seller == null
				)
					status.DisableInfo = Exceptions.AviaDocument_Handle_NotFull;

				return status;
			}

			public ProcessOperationPermissionsResponse CanProcess(object documentId)
			{
				var document = db.AviaDocument.Load(documentId);

				var status = new ProcessOperationPermissionsResponse
				{
					CanUpdate = db.AviaDocument.CanUpdate(document),
					CustomActionPermissions = new Dictionary<string, OperationStatus>
					{
						{ "CanProcess", db.AviaDocument.CanProcess(document) }
					},
					Args = new AviaDocumentProcessArgs
					{
						HasAccessToDocumentList = db.AviaDocument.CanUpdateAll(),
						AllowEditParty = db.Party.CanUpdate()
					}
				};

				return status;
			}

			#endregion


			#region Modify

			public Service()
			{
				Validating += r =>
				{
					if (!db.IsNew(r) && IsExists(r))
						throw new Exception(
							r.IsAviaTicket ? Exceptions.AviaTicket_lt_avia_document_number__type_airlineprefixcode_key :
							r.IsAviaRefund ? Exceptions.AviaRefund_lt_avia_document_number__type_airlineprefixcode_key :
							Exceptions.AviaMco_lt_avia_document_number__type_airlineprefixcode_key
						);
				};
				//   Luxena.Travel.Export.GdsFileExporter.AddToOrder(GdsFileExportDestination dest, IEnumerable`1 docs) in D:\data\git\Luxena.Travel\src\Core\Export\GdsFileExporter.cs:line 135   at Luxena.Travel.Export.GdsFileExporter.Export(GdsFile file, IEnumerable`1 documents) in D:\data\git\Luxena.Travel\src\Core\Export\GdsFileExporter.cs:line 79   at Luxena.Travel.Export.GdsFileExporter.Export(AviaDocument document) in D:\data\git\Luxena.Travel\src\Core\Export\GdsFileExporter.cs:line 99   at Luxena.Travel.Domain.AviaDocument.Service`1.<.ctor>b__9_2(TAviaDocument r) in D:\data\git\Luxena.Travel\src\Core\Domain\Products\Avia\AviaDocument.Service.cs:line 256   at Luxena.Domain.Entities.EntityService`2.<>c__DisplayClass103_0.<Save>b__1(TEntity rr) in D:\data\git\Luxena.Travel\src\Luxena.Domain\Entities\EntityService.cs:line 554   at Luxena.Domain.Entities.EntityEvent.<>c__DisplayClass4_0`1.<Add>b__1(Object r) in D:\data\git\Luxena.Travel\src\Luxena.Domain\Entities\EntityEvents.cs:line 32   at Luxena.Domain.Entities.EntityEvent.Exec() in D:\data\git\Luxena.Travel\src\Luxena.Domain\Entities\EntityEvents.cs:line 38   at Luxena.Domain.Entities.EntityEvents.Exec() in D:\data\git\Luxena.Travel\src\Luxena.Domain\Entities\EntityEvents.cs:line 113   at Luxena.Domain.Entities.DomainBase.Committing() in D:\data\git\Luxena.Travel\src\Luxena.Domain\Entities\Domain.cs:line 168   at Luxena.Domain.Contracts.EntityContractService`5.ItemUpdate(TContract c, RangeRequest prms, Func`1 newEntity, Func`2 newResponseContract) in D:\data\git\Luxena.Travel\src\Luxena.Domain\Contracts\EntityContractService.cs:line 199   at Luxena.Domain.Contracts.EntityContractService`5.Update(TContract c, RangeRequest prms) in D:\data\git\Luxena.Travel\src\Luxena.Domain\Contracts\EntityContractService.cs:line 230   at Luxena.Travel.Web.Services.AviaService.<>c__DisplayClass9_0.<UpdateAviaTicket>b__0() in D:\data\git\Luxena.Travel\src\Web\Services\AviaService.asmx.cs:line 85   at Luxena.Domain.Entities.DomainBase.Commit[T](Func`1 func) in D:\data\git\Luxena.Travel\src\Luxena.Domain\Entities\Domain.cs:line 213   at Luxena.Travel.Web.Services.AviaService.UpdateAviaTicket(AviaTicketDto dto, RangeRequest params) in D:\data\git\Luxena.Travel\src\Web\Services\AviaService.asmx.cs:line 85
				Inserted += r =>
				{
					db.Resolve<GdsFileExporter>()?.Export(r);
				};

				Updated += r =>
				{
					if (!r.GdsFileIsExported && IsDirty(r, a => a.Owner))
						db.Resolve<GdsFileExporter>()?.Export(r);

					if (r.IsProcessed)
						db.Resolve<ExportFtp2011>()?.Export(r);
				};
			}

			#endregion


			#region Passport

			public Passport GetPassport(AviaDocument document)
			{
				var passport = document.ParseGdsPassport();
				if (passport == null) return null;

				passport.Citizenship = db.Country.ByCode(passport.Citizenship.TwoCharCode);
				passport.IssuedBy = db.Country.ByCode(passport.IssuedBy.TwoCharCode);

				return passport;
			}


			public PassportValidationResult ValidatePassengerPassport(AviaDocument document, Passport passport, bool isGdsPassportNull)
			{
				if (document.GdsPassport.No() || isGdsPassportNull)
				{
					if (document is AviaTicket && passport.ExpiredOn.HasValue && ((AviaTicket)document).Departure > passport.ExpiredOn.Value)
						return PassportValidationResult.ExpirationDateNotValid;

					return PassportValidationResult.Valid;
				}

				var gdsPassport = GetPassport(document);

				if (gdsPassport == null || !string.Equals(passport.Number, gdsPassport.Number, StringComparison.InvariantCultureIgnoreCase) || passport.IssuedBy != gdsPassport.IssuedBy)
					return PassportValidationResult.NoPassport;

				var isValid = passport.Citizenship == gdsPassport.Citizenship
					&& passport.IssuedBy == gdsPassport.IssuedBy
					&& string.Equals(passport.LastName, gdsPassport.LastName, StringComparison.InvariantCultureIgnoreCase)
					&& string.Equals(passport.FirstName, gdsPassport.FirstName, StringComparison.InvariantCultureIgnoreCase)
					&& passport.Birthday == gdsPassport.Birthday
					&& passport.ExpiredOn == gdsPassport.ExpiredOn
					&& passport.Gender == gdsPassport.Gender;

				return isValid ? PassportValidationResult.Valid : PassportValidationResult.NotValid;
			}

			public PassportValidationResult ValidatePassengerPassports(AviaDocument ticket, Person passenger, bool isGdsPassportNull, out Passport passport)
			{
				passport = null;

				if (passenger == null)
					return PassportValidationResult.NoPassport;

				foreach (var passport_ in passenger.Passports)
				{
					var result = ValidatePassengerPassport(ticket, passport_, isGdsPassportNull);

					if (result == PassportValidationResult.NoPassport || result == PassportValidationResult.ExpirationDateNotValid)
						continue;

					passport = passport_;
					return result;
				}

				return PassportValidationResult.NoPassport;
			}


			public bool IsPassportRequired(AviaDocument ticket)
			{
				if (ticket.Producer == null)
					return false;

				return ticket.Producer.AirlinePassportRequirement == AirlinePassportRequirement.Required ||
					(ticket.Producer.AirlinePassportRequirement == AirlinePassportRequirement.SystemDefault &&
						db.Configuration.IsPassengerPassportRequired);
			}

			#endregion


			#region Resolve

			public void ResolvePrintDocumentCommission(AviaDocument r)
			{
				if (r.Commission != null || r.EqualFare == null)
					return;

				var commissionPercents = db.AirlineCommissionPercents.By(r.Producer);

				if (commissionPercents == null || r.IsAviaMco)
				{
					r.Commission = new Money(r.EqualFare.Currency);

					return;
				}

				var ticket = (AviaTicket)r;

				if (!ticket.Interline)
				{
					ticket.CommissionPercent = !ticket.Domestic
						? commissionPercents.Domestic
						: commissionPercents.International;
				}
				else
				{
					ticket.CommissionPercent = !ticket.Domestic
						? commissionPercents.InterlineDomestic
						: commissionPercents.InterlineInternational;
				}

				ticket.Commission = ticket.EqualFare * ticket.CommissionPercent.Value / 100;
			}




			#endregion


			public override void Export(TAviaDocument document)
			{
				db.Resolve<IAviaDocumentExporter>().Do(a =>
					db.Try(() => a.Export(db.Unproxy(document)))
				);
			}

		}


		public new partial class Service : Service<AviaDocument>
		{

			public AviaDocument GetByNumber(string number)
			{
				var r = ByFullNumber(number);

				if (r != null && !db.DocumentAccess.HasAccess(r.Owner))
					throw new OperationDeniedException(string.Format(Exceptions.AviaDocumentAccessDenied_Msg, r.FullNumber));

				return r;
			}


			public IList<AviaDocument> GetReservationList(AviaDocument document)
			{
				var type = document.GetClass().Type;

				if (type != typeof(AviaRefund))
				{
					if (document.PnrCode.No())
						return new[] { document };

					var reservation = Session
						.CreateQuery(@"
					     from " + type.Name + @" as d
						where d.Id <> :id
						  and d.PnrCode = :pnrCode
						  and d.IsVoid = false"
						)
						.SetParameter("id", document.Id)
						.SetString("pnrCode", document.PnrCode)
						.SetReadOnly(true)
						.List<AviaDocument>();
					reservation.Insert(0, document);

					return reservation;
				}

				if (document.Number == null)
					return new[] { document };

				var ticket = db.AviaTicket.ByFullNumber(document.Number);

				if (ticket == null || ticket.PnrCode.No())
					return new[] { document };

				var reservation2 = Session
					.CreateQuery(@"
						 from AviaRefund as r
						where r.Id <> :id
							and r.Number in (
							select t.Number
								from AviaTicket as t
								where t.PnrCode = :pnrCode
							)
							and r.IsVoid = false"
					)
					.SetParameter("id", document.Id)
					.SetString("pnrCode", ticket.PnrCode)
					.SetReadOnly(true)
					.List<AviaDocument>();

				reservation2.Insert(0, document);
				return reservation2;
			}

			public int GetReservationDocumentCount(object id)
			{
				return GetReservationList(Load(id)).Count;
			}


			public AviaDocument ForHandlingByNumber(string number)
			{
				var document = db.AviaDocument.ByFullNumber(number);

				if (document != null && !db.AviaDocument.CanUpdate(document))
					throw new OperationDeniedException(string.Format(Exceptions.AviaDocumentAccessDenied_Msg, document.FullNumber));

				return document;
			}

			public IList<AviaDocument> AddByConsoleContent(string content, string sellerId, string ownerId)
			{
				var docs = AirConsoleParser.Parse(content, db.Configuration.DefaultCurrency);

				var seller = db.Person.By(sellerId) ?? db.Security.Person;
				var owner = db.Party.By(ownerId);

				foreach (var doc in docs)
				{
					doc.Owner = owner;
					doc.Seller = seller;

					if (doc.TicketerCode.No() || doc.TicketerOffice.No())
						doc.Ticketer = seller;

					doc.Resolve(db);
					//db.Commit(() => Save(doc));
					Save(doc);
				}

				return docs;
			}



			public RangeResponse SuggestNotRefunded(RangeRequest prms)
			{
				return new RangeResponse(Query
					.Where(a => (a.Type == ProductType.AviaTicket || a.Type == ProductType.AviaMco) && a.Name.Contains(prms.Query))
					.Select(a => Reference.FromArray(a.Id, a.Name, a.GetType().Name))
					.Take(50)
					.ToArray()
				);
			}


			public void Import(IEnumerable<Entity2> documents, Dictionary<Entity2, ImportStatus> importedDocuments)
			{
				foreach (var r in documents)
				{
					ImportStatus status = null;
					try
					{
						r.Do((AviaDocument a) => a.MustBeUnprocessed = true);

						db.Save(r + db);

						db.AppState.RegisterEntity(r);
						
						status = new ImportStatus(ImportResult.Success, "Ok");
					}
					catch (Exception ex)
					{
						if (ex is DomainException || ex is GdsImportException)
						{
							status = new ImportStatus(ImportResult.Warn, ex.Message);
						}
						else
							status = new ImportStatus(ImportResult.Error, ex.Message);

						db.Warn(ex);
					}
					finally
					{
						importedDocuments[r] = status;
					}
				}
			}

		}

	}

}