using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Luxena.Travel.Domain
{

	[DebuggerDisplay("{MoneyString}")]
	public class Money
	{

		public Money()
		{
		}

		public Money(Currency currency)
		{
			Currency = currency;
		}

		public Money(Currency currency, decimal amount)
		{
			Currency = currency;
			Amount = amount;
		}

		public Money(string currency, decimal amount)
		{
			Currency = new Currency(currency);
			Amount = amount;
		}

		public Money(int currency, decimal amount)
		{
			Currency = new Currency { NumericCode = currency };
			Amount = amount;
		}

		public Money(Money source)
		{
			Currency = source.Currency;
			Amount = source.Amount;
		}

		public Currency Currency { get; set; }

		public decimal Amount { [DebuggerStepThrough] get { return _amount; } set { _amount = Math.Round(value + 0.0001m, 2); } }
		private decimal _amount;

		public string MoneyString => ToMoneyString(Amount, Currency);

		public static string ToMoneyString(decimal amount, Currency currency)
		{
			return $"{amount.ToMoneyString()} {currency}";
		}


		[DebuggerStepThrough]
		public static Money operator +(Money m, decimal d)
		{
			return m == null ? null : new Money(m.Currency, m.Amount + d);
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m, decimal d)
		{
			return m == null ? null : new Money(m.Currency, m.Amount - d);
		}

		[DebuggerStepThrough]
		public static Money operator +(Money m, Money n)
		{
			if (m == null || n == null)
				return m ?? n;

			if (Equals(m.Currency, n.Currency))
				return new Money(m.Currency, m.Amount + n.Amount);

			if (n.Amount == 0)
				return new Money(m);

			return m.Amount == 0 ? new Money(n) : null;
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m, Money n)
		{
			if (m == null || n == null)
				return m ?? -n;

			if (Equals(m.Currency, n.Currency))
				return new Money(m.Currency, m.Amount - n.Amount);

			if (n.Amount == 0)
				return new Money(m);

			return m.Amount == 0 ? new Money(n.Currency, -n.Amount) : null;
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m)
		{
			return m == null ? null : new Money(m.Currency, -m.Amount);
		}

		[DebuggerStepThrough]
		public static Money operator *(int multiplier, Money m)
		{
			return m == null ? null : new Money(m.Currency, m.Amount * multiplier);
		}

		[DebuggerStepThrough]
		public static Money operator *(Money m, decimal multiplier)
		{
			return m == null ? null : new Money(m.Currency, m.Amount * multiplier);
		}

		[DebuggerStepThrough]
		public static Money operator /(Money m, decimal multiplier)
		{
			return m == null ? null : new Money(m.Currency, m.Amount / multiplier);
		}

		[DebuggerStepThrough]
		public static Money operator |(Money m, Money empty)
		{
			return m ?? empty;
		}

		[DebuggerStepThrough]
		public static Money operator +(Money r, Domain db)
		{
			if (r == null) return null;

			r.Currency += db;

			return r;
		}


		public override bool Equals(object obj)
		{
			var money = obj as Money;

			return money != null && money.Amount == Amount && Equals(money.Currency, Currency);
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			return HashCodeUtility.GetHashCode(Amount, Currency);
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}

		public override string ToString()
		{
			return Amount.ToMoneyString();
		}

		public string ToString(string format)
		{
			return Amount.ToString(format);
		}
	}


	public static class MoneyExtentions
	{

		[DebuggerStepThrough]
		public static decimal AsAmount(this Money me)
		{
			return me?.Amount ?? 0;
		}

		[DebuggerStepThrough]
		public static decimal? AsAmountn(this Money me)
		{
			return me == null ? null : me.Amount == 0 ? (decimal?)null : me.Amount;
		}

		[DebuggerStepThrough]
		public static Currency AsCurrency(this Money me)
		{
			return me?.Currency;
		}

		[DebuggerStepThrough]
		public static Money Clone(this Money me)
		{
			return me == null ? null : new Money(me);
		}

		[DebuggerStepThrough]
		public static bool Yes(this Money me)
		{
			return me?.Currency != null && me.Amount != 0;
		}
		[DebuggerStepThrough]
		public static bool No(this Money me)
		{
			return me?.Currency == null || me.Amount == 0;
		}


		[DebuggerStepThrough]
		public static Money Else(this Money me, Money defaults)
		{
			return me.Yes() ? me : defaults;
		}

		[DebuggerStepThrough]
		public static Money Else(this Money me, Func<Money> defaults)
		{
			return me.Yes() ? me : defaults();
		}

		[DebuggerStepThrough]
		public static Money Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
		{
			if (source == null || selector == null) return null;

			Money sum = null;

			foreach (var item in source)
			{
				sum += selector(item);
			}

			return sum;
		}

	}

}