using System;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;
using Luxena.Travel.Export;




namespace Luxena.Travel.Domain
{



	//===g






	partial class Product
	{

		//---g



		public override Entity Resolve(Domain db)
		{

			var r = this;


			GdsAgent booker = null;


			var ticketer = db.GdsAgent.By(r.Origin, r.TicketerOffice, r.TicketerCode);

			if (r.Ticketer == null && ticketer != null)
				r.Ticketer = ticketer.Person;


			if (r.BookerOffice != null && r.BookerCode != null)
			{

				if (r.BookerOffice == r.TicketerOffice && r.BookerCode == r.TicketerCode)
				{
					booker = ticketer;

					if (r.Booker == null)
						r.Booker = r.Ticketer;
				}
				else
				{
					booker = db.GdsAgent.By(r.Origin, r.BookerOffice, r.BookerCode);

					if (r.Booker == null && booker != null)
						r.Booker = booker.Person;
				}

			}


			var seller = r.Seller ?? r.IsTicketerRobot ? booker : ticketer;

			if (seller != null)
			{
				r.Seller = seller.Person;
				r.Owner = seller.Office;
				r.Do((AviaDocument a) => a.Provider = seller.Provider);
				r.LegalEntity = seller.LegalEntity;
			}


			if (r.Customer != null)
				r.SetCustomer(db, db.Party.ByLegalName(r.Customer.LegalName));


			r.Fare += db;
			r.EqualFare += db;
			r.Commission += db;
			r.BookingFee += db;
			r.FeesTotal += db;
			r.ConsolidatorCommission += db;

			r.Vat += db;
			r.Total += db;
			r.ServiceFee += db;
			r.Handling += db;
			r.HandlingN += db;
			r.Discount += db;
			r.CommissionDiscount += db;
			r.ServiceFeePenalty += db;
			r.RefundServiceFee += db;
			r.BonusAccumulation += db;
			r.BonusDiscount += db;
			r.CancelFee += db;
			r.CancelCommission += db;
			r.GrandTotal += db;


			return r;

		}



		//---g




		public abstract class Service<TProduct> : Entity3Service<TProduct>
			where TProduct : Product
		{

			//---g



			#region Permissions

			public override OperationStatus CanDelete(TProduct r)
			{
				var status = base.CanDelete(r);
				return !status ? status : CanModify(r);
			}

			public override OperationStatus CanUpdate(TProduct r)
			{
				var status = base.CanUpdate(r);
				return !status ? status : CanModify(r);
			}

			public bool CanUpdateAll()
			{
				return db.IsGranted(UserRole.Administrator, UserRole.Supervisor);
			}



			private OperationStatus CanModify(TProduct r)
			{

				if (!db.ClosedPeriod.IsOpened(r.IssueDate))
				{
					return new OperationStatus(Exceptions.Document_Closed);
				}


				if (CanUpdateAll())
				{
					return true;
				}


				if (!db.Configuration.AllowOtherAgentsToModifyProduct)
				{
					var user = db.Security.User;

					return db.Granted(
						db.DocumentAccess.HasAccess(r.Owner, out var fullDocumentControl) &&
						(fullDocumentControl || Equals(r.Seller, user.Person))
					);
				}


				return true;

			}


			#endregion



			//---g



			protected Service()
			{

				Modifing += r =>
				{

					if (r.Customer != null)
						r.Customer.IsCustomer = true;

				};


				Calculating += r =>
				{

					r.IsProcessed = !r.MustBeUnprocessed && GetIsProcessed(r);

					if (r.EqualFare == null && r.Fare != null && r.Fare.Currency.Code == db.Configuration.DefaultCurrency.Code)
						r.EqualFare = r.Fare.Clone();

					r.Seller |= db.Security.Person;

					if (r.IsProcessed)
					{
						if (!OldValue(r, a => a.IsProcessed)
							|| IsDirty(r, a => new { a.GrandTotal, a.Total, a.ServiceFee, a.Commission, a.CancelCommission })
						)
						{
							db.OnCommit(r, Export);
						}
					}

				};

			}



			protected virtual bool GetIsProcessed(TProduct r)
			{
				return
					r.Customer != null &&
					(!db.Configuration.IsOrderRequiredForProcessedDocument || r.Order != null) &&
					(!db.Configuration.UseDefaultCurrencyForInput || r.EqualFare != null) &&
					r.ServiceFee != null &&
					r.GrandTotal != null &&
					(!r.IsRefund || (r.ServiceFeePenalty != null && r.RefundServiceFee != null))
				;
			}



			public virtual void Export(TProduct r)
			{
				db.Resolve<IExporter>().Do(a => a.Export(db.Unproxy(r)));
			}



			public int GetCountForUpdate(string className, object[] ids, object dateFrom, object dateTo)
			{

				var count = 0;

				var cls = Class.Of(className);


				if (cls.Is<Organization>())
				{
					count = ids.Sum(id => db.Airline.GetNoAirlineDocumentCount(db.Airline.By(id), (DateTime?)dateFrom, (DateTime?)dateTo));
				}

				else if (cls.Is<AirlineServiceClass>())
				{
					count = ids.Sum(id => db.Airline.GetNoServiceClassTicketCount(db.AirlineServiceClass.By(id), (DateTime?)dateFrom, (DateTime?)dateTo));
				}

				else if (cls.Is<GdsAgent>())
				{
					count = ids.Sum(id => db.GdsAgent.GetNoGdsAgentProductCount(db.GdsAgent.By(id), (DateTime?)dateFrom, (DateTime?)dateTo));
				}


				return count;

			}



			public int ApplyData(string className, object[] ids, DateTime? dateFrom, DateTime? dateTo)
			{

				var count = 0;

				var cls = Class.Of(className);


				if (cls.Is<Organization>())
				{
					db.Airline.CanApply().Assert("Operation is not permitted");

					count += ids.Sum(id => db.Airline.Apply(db.Airline.By(id), dateFrom, dateTo));
				}

				else if (cls.Is<AirlineServiceClass>())
				{
					db.AirlineServiceClass.CanApply().Assert("Operation is not permitted");

					count += ids.Sum(id => db.AirlineServiceClass.Apply(db.AirlineServiceClass.By(id), dateFrom, dateTo));
				}

				else if (cls.Is<GdsAgent>())
				{
					db.GdsAgent.CanApply().Assert("Operation is not permitted");

					count += ids.Sum(id => db.GdsAgent.Apply(db.GdsAgent.By(id), dateFrom, dateTo));
				}


				return count;

			}



			//---g

		}




		//---g



		public partial class Service : Service<Product>
		{

			public override void Export(Product r)
			{

				if (r is AviaDocument doc)
					db.AviaDocument.Export(doc);
				else
					base.Export(r);

			}

		}




		//---g

	}






	//===g



}