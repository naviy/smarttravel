using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;


namespace Luxena.Travel.Domain
{

	[DebuggerDisplay("{MoneyString}")]
	[ComplexType]
	public class Money
	{
		public string CurrencyId { get; set; }

		public decimal? Amount
		{
			get { return _amount; }
			set { _amount = value.HasValue ? Math.Round(value.Value, 2) : (decimal?)null; }
		}
		private decimal? _amount;

		//[NotMapped]
		//public Currency Currency
		//{
		//	get
		//	{
		//		return _currency ?? (_currency = CurrencyId);
		//	}
		//	set
		//	{
		//		_currency = value;

		//		if (value != null)
		//			CurrencyId = value.Id;
		//	}
		//}
		//private Currency _currency;

		#region Constructors

		public Money() { }

		//public Money(Currency currency)
		//{
		//	Currency = currency;
		//}

		//public Money(Currency currency, decimal? amount)
		//{
		//	CurrencyId = currency;
		//	Amount = amount;
		//}

		public Money(string currencyId, decimal? amount = null)
		{
			CurrencyId = currencyId;
			Amount = amount;
		}

		public Money(Money source)
		{
			CurrencyId = source.CurrencyId;
			Amount = source.Amount;
		}

		#endregion


		[NotMapped]
		public string MoneyString => ToMoneyString(CurrencyId, Amount);

		public static string ToMoneyString(string currencyId, decimal? amount)
		{
			return $"{amount:N2} {currencyId}";
		}

		public override string ToString() => MoneyString;

		public string ToString(string format) => Amount?.ToString(format);


		public override bool Equals(object obj)
		{
			var money = obj as Money;

			return money != null && money.Amount == Amount && Equals(money.CurrencyId, CurrencyId);
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.GetHashCode(Amount, CurrencyId);
		}


		#region Operators

		[DebuggerStepThrough]
		public static bool operator ==(Money a, Money b)
		{
			return Equals(a, b);
		}

		[DebuggerStepThrough]
		public static bool operator !=(Money a, Money b)
		{
			return !Equals(a, b);
		}


		[DebuggerStepThrough]
		public static Money operator +(Money m, decimal d)
		{
			return m == null ? null : new Money(m.CurrencyId, (m.Amount ?? 0) + d);
		}

		[DebuggerStepThrough]
		public static Money operator +(Money m, decimal? d)
		{
			return m == null ? null : new Money(m.CurrencyId, (m.Amount ?? 0) + (d ?? 0));
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m, decimal d)
		{
			return m == null ? null : new Money(m.CurrencyId, (m.Amount ?? 0) - d);
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m, decimal? d)
		{
			return m == null ? null : new Money(m.CurrencyId, (m.Amount ?? 0) - (d ?? 0));
		}

		[DebuggerStepThrough]
		public static Money operator +(Money m, Money n)
		{
			if (m.IsNull()) return n.Clone() ?? m;
			if (n.IsNull()) return m.Clone() ?? n;

			if (m.CurrencyId == n.CurrencyId)
				return new Money(m.CurrencyId, (m.Amount ?? 0) + (n.Amount ?? 0));

			return n.Amount == 0 ? new Money(m) : new Money(n);
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m, Money n)
		{
			if (m.IsNull()) return -n.Clone() ?? m;
			if (n.IsNull()) return m.Clone() ?? n;

			if (m.CurrencyId == n.CurrencyId)
				return new Money(m.CurrencyId, (m.Amount ?? 0) - (n.Amount ?? 0));

			return n.Amount == 0 ? new Money(m) : new Money(n.CurrencyId, -n.Amount);
		}

		[DebuggerStepThrough]
		public static Money operator -(Money m)
		{
			return m == null ? null : new Money(m.CurrencyId, -m.Amount);
		}

		[DebuggerStepThrough]
		public static Money operator *(int multiplier, Money m)
		{
			return m == null ? null : new Money(m.CurrencyId, m.Amount * multiplier);
		}

		[DebuggerStepThrough]
		public static Money operator *(Money m, decimal multiplier)
		{
			return m == null ? null : new Money(m.CurrencyId, m.Amount * multiplier);
		}

		[DebuggerStepThrough]
		public static Money operator /(Money m, decimal multiplier)
		{
			return m == null ? null : new Money(m.CurrencyId, m.Amount / multiplier);
		}

		//[DebuggerStepThrough]
		//public static Money operator |(Money m, Money zero)
		//{
		//	if (zero == null)
		//		return new Money(m);

		//	if (m == null)
		//		return zero.Clone();

		//	if (m.CurrencyId == null)
		//		m.CurrencyId = zero.CurrencyId;

		//	return m;
		//}


		[DebuggerStepThrough]
		public static implicit operator Money(string defaultCurrency)
		{
			return new Money(defaultCurrency);
		}

		[DebuggerStepThrough]
		public static Money operator +(Money m, string defaultCurrency)
		{
			if (defaultCurrency == null)
				return new Money(m);

			if (m == null)
				return new Money(defaultCurrency);

			if (m.CurrencyId == null)
				return new Money(defaultCurrency, m.Amount);

			return new Money(m);
		}



		[DebuggerStepThrough]
		public static implicit operator decimal? (Money m) => m?.Amount;

		[DebuggerStepThrough]
		public static implicit operator bool (Money m) => m?.Amount != null && m.CurrencyId != null && m.Amount != 0;

		#endregion

	}


	public static class MoneyExtentions
	{

		//		[DebuggerStepThrough]
		//		public static decimal? AsAmount(this Money me)
		//		{
		//			return me == null ? 0 : me.Amount;
		//		}
		//
		//		[DebuggerStepThrough]
		//		public static Currency AsCurrency(this Money me)
		//		{
		//			return me == null ? null : me.CurrencyId;
		//		}

		[DebuggerStepThrough]
		public static Money Clone(this Money me)
		{
			return me == null ? null : new Money(me);
		}

		[DebuggerStepThrough]
		public static Money Clone(this Money me, string defaultCurrency)
		{
			return me == null ? new Money(defaultCurrency) : new Money(me);
		}

		[DebuggerStepThrough]
		public static bool IsNull(this Money me)
		{
			return me?.Amount == null || me.CurrencyId == null;
		}

		//[DebuggerStepThrough]
		//public static bool Yes(this Money me)
		//{
		//	return me?.Amount != null && me.CurrencyId != null;
		//}
		//[DebuggerStepThrough]
		//public static bool No(this Money me)
		//{
		//	return me?.Amount == null || me.CurrencyId == null;
		//}
		//[DebuggerStepThrough]
		//public static bool Yes(this Money me, Func<Money, bool> match)
		//{
		//	return me?.Amount != null && me.CurrencyId != null && match(me);
		//}
		//[DebuggerStepThrough]
		//public static bool No(this Money me, Func<Money, bool> match)
		//{
		//	return me?.Amount == null || me.CurrencyId == null || !match(me);
		//}


		//[DebuggerStepThrough]
		//public static bool Has(this Money me)
		//{
		//	return me?.Amount != null && me.CurrencyId != null && me.Amount != 0;
		//}
		//[DebuggerStepThrough]
		//public static bool NotHas(this Money me)
		//{
		//	return me?.Amount == null || me.CurrencyId == null && me.Amount == 0;
		//}


		[DebuggerStepThrough]
		public static Money Else(this Money me, Money defaults)
		{
			return !me.IsNull() ? me : defaults;
		}

		[DebuggerStepThrough]
		public static Money Else(this Money me, Func<Money> defaults)
		{
			return !me.IsNull() ? me : defaults();
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
		[DebuggerStepThrough]
		public static Money AsSum<TSource>(this IEnumerable<TSource> source, Func<TSource, Money> selector)
		{
			return Sum(source, selector);
		}

	}



}