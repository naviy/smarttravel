using System;
using System.Web.Script.Services;
using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;




namespace Luxena.Travel.Web.Services
{



	//===g






	//[GenerateScriptType(typeof(OrderItemAviaLinkDto))]
	public class OrderService : DomainWebService
	{

		//---g



		[WebMethod]
		public OrderItemDto[] GetOrdersByAviaDocuments(object[] aviaDocumentIds)
		{
			return db.Commit(() => dc.OrderItem.ListByProducts(aviaDocumentIds));
		}



		[WebMethod]
		public EntityReference FindAviaDocumentByNumer(string number)
		{
			return db.Commit(() => db.AviaDocument.GetByNumber(number));
		}



		[WebMethod]
		public ItemResponse AddAviaDocuments(object orderId, object[] documentIds, bool separateServiceFee)
		{
			return db.Commit(() => dc.Order.AddProducts(orderId, documentIds, separateServiceFee));
		}



		[WebMethod]
		public GenerateOrderItemsResponse GenerateOrderItems(object[] documentIds, bool separateServiceFee, string ordreId)
		{
			return db.Commit(() => dc.OrderItem.Generate(documentIds, separateServiceFee, ordreId));
		}



		[WebMethod]
		public OrderDto GetOrder(object id)
		{
			return db.Commit(() => dc.Order.By(id));
		}



		[WebMethod]
		public ItemResponse UpdateOrder(OrderDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Order.Update(dto, @params));
		}



		[WebMethod]
		public InvoiceDto IssueInvoice(
			object id,
			string number,
			DateTime issueDate,
			object issuedById,
			object ownerId,
			object bankAccountId,
			int? formNumber,
			bool showPaid
		)
		{
			return db.Commit(() => dc.Invoice.Issue(id, number, issueDate, issuedById, ownerId, bankAccountId, formNumber, showPaid));
		}



		[WebMethod]
		public InvoiceDto IssueCompletionCertificate(
			object id,
			string number,
			DateTime issueDate,
			object issuedById,
			object ownerId,
			object bankAccountId,
			bool showPaid
		)
		{
			return db.Commit(() => dc.Invoice.IssueCompletionCertificate(id, number, issueDate, issuedById, ownerId, bankAccountId, showPaid));
		}



		[WebMethod]
		public byte[] GetInvoiceFile(object invoiceId)
		{
			return db.Commit(() => db.Invoice.GetFile(invoiceId));
		}



		[WebMethod]
		public void DeleteInvoice(object id)
		{
			db.Commit(() => db.Invoice.Delete(id));
		}



		[WebMethod]
		public OrderItemDto[] GetOrderItemsByNumber(string number)
		{
			return db.Commit(() => dc.OrderItem.ListByOrderNumber(number));
		}



		[WebMethod]
		public OrderDto[] GetOrders(object[] ids)
		{
			return db.Commit(() => dc.Order.ListByIds(ids));
		}



		[WebMethod]
		public InvoiceDto IssueReceipt(object id)
		{
			return db.Commit(() => dc.Invoice.IssueReceipt(id));
		}



		[WebMethod]
		public QuickReceiptResponse CreateQuickReceipt(QuickReceiptRequest request)
		{
			return db.Commit(() => request.GetResponse(db));
		}



		[WebMethod]
		public RangeResponse SuggestInvoices(RangeRequest @params, string paymentType)
		{
			@params.Limit = 100;
			return db.Commit(() => db.Invoice.Suggest(@params, paymentType));
		}



		[WebMethod]
		public void SetIsVoid(object orderId, bool value)
		{
			db.Commit(() => db.Order.SetIsVoid(orderId, value));
		}



		//---g

	}






	//===g



}
