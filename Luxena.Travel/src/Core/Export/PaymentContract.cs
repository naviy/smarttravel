using System;
using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "Payment")]
	public class PaymentContract : EntityContract
	{

		public PaymentContract(Payment r) : base(r)
		{
			Number = r.Number;
			Date = r.Date;
			PaymentForm = r.PaymentForm;
			DocumentNumber = r.DocumentNumber;
			Payer = r.Payer;
			Order = r.Order;
			if (r.Invoice != null)
				Invoice = new EntityReference { Id = r.Invoice.Id.ToString(), Text = r.Invoice.Number, Type = nameof(Invoice) };
			Amount = r.Sign * r.Amount;
			Vat = r.Sign * r.Vat;
			ReceivedFrom = r.ReceivedFrom;
			PostedOn = r.PostedOn;
			Note = r.Note;
			IsVoid = r.IsVoid;
			AssignedTo = r.AssignedTo;
			RegisteredBy = r.RegisteredBy;
			Owner = r.Owner;
		}


		[DataMember] public string Number { get; set; }
		[DataMember] public DateTime Date { get; set; }
		[DataMember] public PaymentForm PaymentForm { get; set; }
		[DataMember] public string DocumentNumber { get; set; }
		[DataMember] public PartyReference Payer { get; set; }
		[DataMember] public EntityReference Order { get; set; }
		[DataMember] public EntityReference Invoice { get; set; }
		[DataMember] public MoneyContract Amount { get; set; }
		[DataMember] public MoneyContract Vat { get; set; }
		[DataMember] public string ReceivedFrom { get; set; }
		[DataMember] public DateTime? PostedOn { get; set; }
		[DataMember] public string Note { get; set; }
		[DataMember] public bool IsVoid { get; set; }
		[DataMember] public PartyReference AssignedTo { get; set; }
		[DataMember] public PartyReference RegisteredBy { get; set; }
		[DataMember] public PartyReference Owner { get; set; }

	}

}