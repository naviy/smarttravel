using System;

using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{
	[DataContract]
	public class OrderBalance
	{
		public EntityReference Order { get; set; }

		public string Owner { get; set; }

		public DateTime? FirstDocumentDate { get; set; }

		public DateTime? LastDocumentDate { get; set; }

		public decimal Delivered { get; set; }

		public decimal Paid { get; set; }

		public decimal Balance => Paid - Delivered;
	}
}