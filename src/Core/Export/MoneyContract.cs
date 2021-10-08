using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract]
	public class MoneyContract
	{
		public MoneyContract() { }

		public MoneyContract(Money money)
		{
			Currency = money.Currency.As(a => a.Code);
			Amount = money.Amount;
		}

		[DataMember]
		public string Currency { get; set; }

		[DataMember]
		public decimal Amount { get; set; }


		public static implicit operator MoneyContract(Money me)
		{
			return me == null ? null : new MoneyContract(me);
		}


		public static implicit operator Money(MoneyContract me)
		{
			return me == null ? null : new Money(me.Currency, me.Amount);
		}

	}


}