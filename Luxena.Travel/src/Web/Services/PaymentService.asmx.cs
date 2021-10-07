using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class PaymentService : DomainWebService
	{

//		[WebMethod]
//		public PaymentDto GetPayment(object id)
//		{
//			return db.Commit(() => dc.Payment.By(id));
//		}
//
//		[WebMethod]
//		public ItemResponse UpdatePayment(PaymentDto dto, string className, RangeRequest @params)
//		{
//			return db.Commit(() => dc.Payment.Update(dto, className, @params));
//		}

		[WebMethod]
		public OperationStatus CanUpdate(object[] ids)
		{
			return db.Commit(() => db.Payment.CanUpdate(ids));
		}

		[WebMethod]
		public ItemListResponse ChangeVoidStatus(object[] ids, RangeRequest @params)
		{
			return db.Commit(() => dc.Payment.ChangeVoidStatus(ids, @params));
		}

		[WebMethod]
		public AviaPaymentResponse CanCreatePayment(object[] aviaDocumentIds)
		{
			return db.Commit(() => db.Payment.CanCreateByProducts(aviaDocumentIds));
		}

		[WebMethod]
		public CashPaymentResponse CreateCashPayment(CashInOrderPaymentRequest request)
		{
			return db.Commit(() => request.GetResponse(db, dc));
		}

		[WebMethod]
		public ItemListResponse PostPayments(object[] ids, RangeRequest @params)
		{
			return db.Commit(() => dc.Payment.Post(ids, @params));
		}

		[WebMethod]
		public byte[] GetCashOrderFile(object paymentId)
		{
			return db.Commit(() => db.CashInOrderPayment.GetFile(paymentId));
		}

		[WebMethod]
		public EntityReference[] GetPaymentSystems()
		{
			return db.Commit(() => db.PaymentSystem.ReferenceList());
		}

	}

}
