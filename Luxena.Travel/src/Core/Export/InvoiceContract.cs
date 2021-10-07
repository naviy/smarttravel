using System;
using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "Invoice")]
	public class InvoiceContract
	{

		public InvoiceContract(Invoice r)
		{
			Id = r.Id.ToString();
			Number = r.Number;
			Agreement = r.Agreement;
			IssueDate = r.IssueDate;
			TimeStamp = r.TimeStamp;
			Type = r.Type;
			Order = r.Order;
			Customer = r.Customer;
			BillTo = r.BillTo ?? new PartyReference { Text = r.BillToName };
			ShipTo = r.ShipTo;
			Owner = r.Owner;
			IssuedBy = r.IssuedBy;
			Total = r.Total;
		}


		[DataMember] public string Id { get; set; }
		[DataMember] public string Number { get; set; }
		[DataMember] public string Agreement { get; set; }
		[DataMember] public DateTime IssueDate { get; set; }
		[DataMember] public DateTime TimeStamp { get; set; }
		[DataMember] public InvoiceType Type { get; set; }
		[DataMember] public EntityReference Order { get; set; }
		[DataMember] public PartyReference Customer { get; set; }
		[DataMember] public PartyReference BillTo { get; set; }
		[DataMember] public PartyReference ShipTo { get; set; }
		[DataMember] public PartyReference Owner { get; set; }
		[DataMember] public PartyReference IssuedBy { get; set; }
		[DataMember] public MoneyContract Total { get; set; }

	}

}