using System;
using System.Data.Entity;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public enum CheckType
	{
		[Patterns.Unknown]
		Unknown = 0,

		[RU("Чек продажи")]
		Sale = 1,

		[RU("Чек возврата")]
		Return = 2,
	}

	public enum CheckPaymentType
	{
		[RU("Наличные")]
		Cash,

		[RU("Кредит")]
		Credit,

		[RU("Чек")]
		Check,

		[RU("Карточка")]
		Card,
	}

	[RU("Чек", "Чеки")]
	public partial class OrderCheck : Entity2
	{

		[EntityDate, Patterns.Date, DateTime2]
		public DateTimeOffset Date { get; set; }

		//		[EntityDate, Patterns.Date, Date]
		//		public DateTimeOffset DateOnly { get { return Date.Date; } }


		protected Order _Order;

		[RU("Печатал чек")]
		protected Person _Person;

		[RU("Тип чека")]
		public CheckType CheckType { get; set; }

		[RU("Номер чека"), EntityName, Length(10)]
		public string CheckNumber { get; set; }

		[Patterns.Currency]
		public string Currency { get; set; }

		[RU("Сумма чека"), Float(2)]
		public decimal? CheckAmount { get; set; }

		[Patterns.Vat, Float(2)]
		public decimal? CheckVat { get; set; }

		[RU("Сумма оплаты", ruDesc: "Деньги, которые клиент передал кассиру (из которых последний возвращает сдачу)"), Float(2)]
		public decimal? PayAmount { get; set; }

		[RU("Тип оплаты")]
		public CheckPaymentType? PaymentType { get; set; }

		[Patterns.Description]
		public string Description { get; set; }


		static partial void Config_(Domain.EntityConfiguration<OrderCheck> entity)
		{
			entity.Association(a => a.Order);//, a => a.CheckPrintings);
			entity.Association(a => a.Person);//, a => a.CheckPrintings);
		}


		protected override void Bind()
		{
			base.Bind();

			if (!IsNew()) return;

			
			if (Order.Payments?.All(a => a.DocumentNumber != CheckNumber) ?? false)
			{
				var sign = CheckType == CheckType.Return ? -1 : 1;

				new CheckPayment
				{
					Date = Date,
					DocumentNumber = CheckNumber,
					OrderId = OrderId,
					RegisteredBy = Person,
					AssignedTo = Person,
					Owner = Person?.GetDefaultOwner(db),
					Amount = new Money(Currency, sign * CheckAmount),
					Vat = new Money(Currency, sign * CheckVat),
					PostedOn = Date,
				}.Save(db);
			}
		}

	}


	partial class Domain
	{
		public DbSet<OrderCheck> OrderChecks { get; set; }
	}

}
