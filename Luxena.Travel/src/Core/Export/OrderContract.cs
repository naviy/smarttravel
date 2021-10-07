using System;
using System.Linq;
using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "Order")]
	public class OrderContract : EntityContract
	{

		public OrderContract(Order r) : base(r)
		{
			Number = r.Number;
			IssueDate = r.IssueDate;
			IsVoid = r.IsVoid;
			Customer = r.Customer;
			BillTo = r.BillTo ?? new PartyReference { Text = r.BillToName };
			ShipTo = r.ShipTo;
			Intermediary = r.Intermediary;
			Items = r.Items.Select(a => new OrderItemContract(a)).ToArray();
			Discount = r.Discount;
			Total = r.Total;
			Paid = r.Paid;
			TotalDue = r.TotalDue;
			Vat = r.Vat;
			AssignedTo = r.AssignedTo;
			Owner = r.Owner;
			BonusDate = r.BonusDate;
			BonusSpentAmount = r.BonusSpentAmount;
			BonusRecipient = r.BonusRecipient;
			Note = r.Note;
		}


		[DataMember] public string Number { get; set; }
		[DataMember] public DateTime IssueDate { get; set; }
		[DataMember] public bool IsVoid { get; set; }
		[DataMember] public PartyReference Customer { get; set; }
		[DataMember] public PartyReference BillTo { get; set; }
		[DataMember] public PartyReference ShipTo { get; set; }
		[DataMember] public PartyReference Intermediary { get; set; }
		[DataMember] public OrderItemContract[] Items { get; set; }

		[DataMember] public MoneyContract Discount { get; set; }
		[DataMember] public MoneyContract Total { get; set; }
		[DataMember] public MoneyContract Vat { get; set; }
		[DataMember] public MoneyContract Paid { get; set; }
		[DataMember] public MoneyContract TotalDue { get; set; }

		[DataMember] public PartyReference AssignedTo { get; set; }
		[DataMember] public PartyReference Owner { get; set; }

		[DataMember] public DateTime? BonusDate { get; set; }
		[DataMember] public decimal? BonusSpentAmount { get; set; }
		[DataMember] public PartyReference BonusRecipient { get; set; }

		[DataMember] public string Note { get; set; }

	}

}
