using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;




namespace Luxena.Travel.Web.Services
{



	//===g






	public class AviaService : DomainWebService
	{

		//---g



		[WebMethod]
		public object GetAviaDocument(object id, string type, bool editMode)
		{
			return db.Commit(() => dc.AviaDocument.By(id));
		}



		[WebMethod]
		public AviaDocumentProcessDto[] GetAviaReservationForProcess(object documentId, string className)
		{
			return db.Commit(() => dc.AviaDocumentProcess.AviaReservationsForProcess(documentId));
		}



		[WebMethod]
		public ProcessOperationPermissionsResponse CanProcess(object documentId, string className)
		{
			return db.Commit(() => db.AviaDocument.CanProcess(documentId));
		}



		[WebMethod]
		public AviaTicketProcessDto[] ProcessAviaTickets(AviaTicketProcessDto[] dtos)
		{
			return db.Commit(() => dc.AviaTicketProcess.Process(dtos));
		}



		[WebMethod]
		public AviaRefundProcessDto[] ProcessAviaRefunds(AviaRefundProcessDto[] dtos)
		{
			return db.Commit(() => dc.AviaRefundProcess.Process(dtos));
		}



		[WebMethod]
		public AviaMcoProcessDto[] ProcessAviaMcos(AviaMcoProcessDto[] dtos)
		{
			return db.Commit(() => dc.AviaMcoProcess.Process(dtos));
		}



		[WebMethod]
		public AviaDocumentProcessDto GetAviaDocumentForHandlingByNumber(string number, string className)
		{
			return db.Commit(() => dc.AviaDocumentProcess.ForHandlingByNumber(number));
		}

		[WebMethod]
		public AviaDocumentProcessDto[] GetAviaDocumentsForProcess(object[] ids, string className)
		{
			return db.Commit(() => dc.AviaDocumentProcess.ListForProcess(ids, className));
		}



		//[WebMethod]
		//public FlightSegmentDto UpdateFlightSegment(object ticketId, Dictionary<string, object> @params)
		//{
		//	return db.Commit(() => dc.AviaTicket.UpdateFlightSegment(ticketId, @params));
		//}

		//[WebMethod]
		//public AviaTicketDto UpdateFlightSegments(object ticketId, FlightSegmentDto[] @params)
		//{
		//	return db.Commit(() => dc.AviaTicket.UpdateFlightSegments(ticketId, @params));
		//}



		[WebMethod]
		public object ChangeVoidStatus(object[] ids, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaDocument.ChangeVoidStatus(ids, @params));
		}



		[WebMethod]
		public ItemResponse UpdateAviaTicket(AviaTicketDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaTicket.Update(dto, @params));
		}



		[WebMethod]
		public ItemResponse UpdateAviaMco(AviaMcoDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaMco.Update(dto, @params));
		}



		[WebMethod]
		public ItemResponse UpdateAviaRefund(AviaRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.AviaRefund.Update(dto, @params));
		}



		[WebMethod]
		public EntityReference FindAirlineByPrefixCode(string code)
		{
			return db.Commit(() => db.Airline.ByPrefixCode(code));
		}



		[WebMethod]
		public string FindAirportNameById(object id)
		{
			return db.Commit(() => db.Airport.By(id).As(a => a.Name));
		}



		[WebMethod]
		public int GetReservationDocumentCount(object id)
		{
			return db.Commit(() => db.AviaDocument.GetReservationDocumentCount(id));
		}



		[WebMethod]
		public RangeResponse SuggestNotRefundedDocuments(RangeRequest @params)
		{
			return db.Commit(() => db.AviaDocument.SuggestNotRefunded(@params));
		}



		[WebMethod]
		public OperationStatus CanUpdate(object[] ids)
		{
			return db.Commit(() => db.AviaDocument.CanUpdate(ids));
		}



		[WebMethod]
		public int GetDocumentCountForUpdate(string className, object[] ids, object dateFrom, object dateTo)
		{
			return db.Commit(() => db.Product.GetCountForUpdate(className, ids, dateFrom, dateTo));
		}



		[WebMethod]
		public int ApplyDataToDocuments(string className, object[] ids, object dateFrom, object dateTo)
		{
			return db.Commit(() => db.Product.ApplyData(className, ids, (DateTime?)dateFrom, (DateTime?)dateTo));
		}



		[WebMethod]
		public PassportValidationResponse ValidatePassengerPassport(string ticketId, string passengerId, bool isGdsPassportNull)
		{
			return db.Commit(() => dc.Passport.ValidatePassengerPassport(ticketId, passengerId, isGdsPassportNull));
		}



		[WebMethod]
		public EntityReference[] AddDocumentsByConsoleContent(string content, string sellerId, string ownerId)
		{

			return db.Commit(() =>

				db.AviaDocument
					.AddByConsoleContent(content, sellerId, ownerId)
					.ToArray(a => new EntityReference(a.Type.ToString(), a.Id, a.Name))

			);

		}



		//---g

	}






	//===g



}