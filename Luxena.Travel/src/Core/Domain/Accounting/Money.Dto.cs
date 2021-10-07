using System.Diagnostics;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	[DebuggerDisplay("{MoneyString}")]
	public class MoneyDto
	{

		public decimal Amount { get; set; }

		public Currency.Reference Currency { get; set; }


		public string MoneyString => $"{Amount.ToMoneyString()} {Currency}";


		public MoneyDto() {}

		public MoneyDto(decimal amount, Currency.Reference currency)
		{
			Amount = amount;
			Currency = currency;
		}

		[DebuggerStepThrough]
		public static implicit operator MoneyDto(Money money)
		{
			if (money == null)
				return null;

			return new MoneyDto
			{
				Amount = money.Amount,
				Currency = money.Currency
			};
		}


		[DebuggerStepThrough]
		public static Money operator +(MoneyDto dto, Domain db)
		{
			return dto == null ? null : new Money(db.Currency.By(dto.Currency), dto.Amount);
		}

		[DebuggerStepThrough]
		public static MoneyDto operator *(MoneyDto dto, int y)
		{
			return dto == null ? null : new MoneyDto(dto.Amount * y, dto.Currency);
		}

	}

}