using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Travel.Export;
using Luxena.Travel.Services;


namespace Luxena.Travel.Domain
{

	partial class Payment
	{

		public class Service<TPayment> : Entity2Service<TPayment>
			where TPayment : Payment
		{


			#region Permissions

			public AviaPaymentResponse CanCreateByProducts(object[] productIds)
			{

				var docs = db.Product.ListByIds(productIds);


				var unorderedDocs = docs.Where(doc => doc.Order == null).ToList();


				Party customer = null;
				Money total = null;
				Money vat = null;


				if (unorderedDocs.Count > 0)
				{
					var orderItems = db.OrderItem.New(unorderedDocs, ServiceFeeMode.AlwaysJoin, false);

					customer = unorderedDocs.All(doc => doc.Customer == unorderedDocs[0].Customer) ? unorderedDocs[0].Customer : null;

					db.Order.CalcFinanceData(orderItems, db.Configuration.DefaultCurrency, out total, out _, out vat);
				}


				var response = new AviaPaymentResponse
				{
					Total = total,
					Vat = vat,
					Payer = customer,
					DocumentIds = unorderedDocs.Select(a => a.Id).ToArray(),
					OrderItems = docs.Where(a => a.Order != null).Select(a => new OrderItemDto(a.Order, a)).ToArray(),
				};


				return response;

			}

			public override OperationStatus CanUpdate(TPayment r)
			{
				var status = base.CanUpdate(r);
				if (!status)
					return status;

				if (db.ClosedPeriod.IsOpened(r.Date)) 
					return db.IsGranted(UserRole.Administrator, UserRole.Supervisor) || Equals(r.RegisteredBy, db.Security.Person);

				status.DisableInfo = Exceptions.Document_Closed;
				return status;
			}

			public void CheckCanUpdate(TPayment payment)
			{
				var status = payment.Id == null ? CanCreate(payment) : CanUpdate(payment);

				if (status.IsHidden || status.DisableInfo.Yes())
					throw new DomainException(string.Format(Exceptions.CannotUpdateDocument, payment,
						status.DisableInfo.No() ? Exceptions.AccessDenied : status.DisableInfo));
			}

			public override OperationStatus CanDelete(TPayment r)
			{
				var status = base.CanDelete(r);
				if (!status)
					return status;

				if (!db.ClosedPeriod.IsOpened(r.Date))
					status.DisableInfo = Exceptions.Document_Closed;

				return status;
			}

			#endregion


			#region Modify

			public Service()
			{
				Calculating += r =>
				{
					if (r.Number.No())
						r.Number = db.Payment.NewSequence();

					if (r.RegisteredBy == null)
						r.RegisteredBy = db.Security.Person;

					if (r.IsCashOrder && r.DocumentNumber.No())
					{
						r.DocumentNumber = db.Sequence.Next(
							r.IsCashOutOrder ? "CashOutOrderPayment" : "CashOrderPayment",
							r.Owner.As(a => a.Id).AsString(),
							num => db.Payment.Exists(a => a.DocumentNumber == num && !a.IsVoid)
						);
					}
				};

				Modified += r => r.As<CashInOrderPayment>()
					.Do(a => db.Issue(a))
					.Else(() => Export(r));

				Deleting += r => r.Order.Do(a => a.RemovePayment(r));

			}

			#endregion
			

			#region Operations

			public void Import(TPayment r)
			{
				if (!db.IsGranted(UserRole.Supervisor)) return;

				r.Post();

				r.Number = db.Payment.NewSequence();

				r.SetOrder(db.Order.By(r.Order.Id));

				r.Amount += db;
				r.Vat += db;

				r.PaymentForm = PaymentForm.WireTransfer;

				r.Owner = r.Order.Owner;

				db.Save(r);

				//db.OnCommit(r.Order, db.Order.Export);
			}

			public void Export(TPayment payment)
			{
				if (payment != null && payment.IsPosted)
					db.Resolve<IExporter>().Do(a =>
					{
						a.Export(payment);
						a.Export(payment.Order);
					});
			}

			#endregion


			public IList<TPayment> ChangeVoidStatus(object[] ids)
			{
				var payments = ListByIds(ids);

				if (payments.Count != ids.Length)
					throw new ObjectsNotFoundException(ids.Length == 1 ? Exceptions.NoRowById_Translation : Exceptions.ObjectsNotFound_Error);

				AssertUpdate(payments);

				foreach (var payment in payments) 
				{
					payment.SetIsVoid(!payment.IsVoid);
				}

				return payments;
			}

		}


		public partial class Service : Service<Payment>
		{

		}

	}

}