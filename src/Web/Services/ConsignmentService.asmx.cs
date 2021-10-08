using System.Web.Script.Services;
using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	[GenerateScriptType(typeof(EntityReference))]
	public class ConsignmentService : DomainWebService
	{

		[WebMethod]
		public byte[] GetConsignmentFile(object issuedConsignmentId)
		{
			return db.Commit(() => db.IssuedConsignment.GetFile(issuedConsignmentId));
		}

		[WebMethod]
		public byte[] GetLastIssuedConsignment(object consignmentId)
		{
			return db.Commit(() => db.Consignment.GetLastFileBy(consignmentId));
		}

		[WebMethod]
		public ConsignmentDto GetConsignment(object id)
		{
			return db.Commit(() => dc.Consignment.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateConsignment(ConsignmentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Consignment.Update(dto, @params));
		}

		[WebMethod]
		public ConsignmentItemsDto GetConsignmentItems(object[] orderIds)
		{
			return db.Commit(() => ConsignmentItemsDto.New(db, dc, orderIds));
		}

	}

}
