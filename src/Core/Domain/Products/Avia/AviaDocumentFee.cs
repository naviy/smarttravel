using Luxena.Base.Domain;


namespace Luxena.Travel.Domain
{

	public partial class AviaDocumentFee : Entity2
	{
		public const string VatCode = "HF";

		public const string ServiceCode = "XP";

		public const string OtherCode = "XT";


		public virtual AviaDocument Document { get; set; }

		public virtual string Code { get; set; }

		public virtual Money Amount { get; set; }


		public override object Clone()
		{
			var fee = (AviaDocumentFee) base.Clone();

			fee.Amount = Amount.Clone();

			return fee;
		}

		public static void Copy(AviaDocumentFee source, AviaDocumentFee target)
		{
			target.Code = source.Code;
			target.Amount = source.Amount.Clone();
		}

		public override string ToString()
		{
			return Code;
		}


		public class Service : Entity2Service<AviaDocumentFee>
		{

		}

	}

}