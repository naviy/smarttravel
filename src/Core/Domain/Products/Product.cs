using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Услуга", "Все услуги")]
	[AgentPrivileges]
	[DebuggerDisplay("{Type} {Name}")]
	public abstract partial class Product : Entity2, IEntity3
	{

		public abstract ProductType Type { get; }

		[Patterns.Name, EntityName]
		public virtual string Name { get; set; }

		[Patterns.IssueDate]
		public virtual DateTime IssueDate { get; set; }

		public virtual string PureNumber => Name;

		[RU("Продюсер")]
		public virtual Organization Producer { get; set; }

		[RU("Провайдер")]
		public virtual Organization Provider { get; set; }

		[RU("Перевыпуск для")]
		public virtual Product ReissueFor { get; set; }

		[RU("Исходный документ")]
		public virtual Product RefundedProduct { get; set; }


		[Patterns.Passenger]
		public virtual string PassengerName
		{
			get { return null; }
			set { throw new NotImplementedException(); }
		}

		public virtual IList<ProductPassenger> Passengers { get { return _passengers; } set { _passengers = value; } }

		public virtual ProductPassengerDto[] PassengerDtos
		{
			get
			{
				var dc = new Contracts();
				return dc.ProductPassenger.New(Passengers);
			}
		}


		#region Status

		public virtual bool IsRefund => false;

		public virtual bool IsReservation => false;

		[RU("Обработан")]
		public virtual bool IsProcessed { get; set; }

		public virtual bool MustBeUnprocessed { get; set; }

		[Patterns.IsVoid]
		public virtual bool IsVoid { get; set; }

		[RU("К обработке")]
		public virtual bool RequiresProcessing => !IsProcessed && !IsVoid;

		public virtual bool IsDelivered => !IsReservation && !IsVoid;


		[Patterns.IsPaid, DataPath("Order.IsPaid")]
		public virtual bool IsPaid =>
			Order != null && Order.IsPaid;

		#endregion


		[Patterns.Customer]
		public virtual Party Customer { get; protected set; }

		public virtual Order Order { get; protected set; }

		[RU("Посредник")]
		public virtual Party Intermediary { get; set; }


		[Required]
		public virtual Country Country { get; set; }

		[RU("Бронировка")]
		public virtual string PnrCode { get; set; }

		[RU("Туркод")]
		public virtual string TourCode { get; set; }


		[RU("Бронировщик")]
		public virtual Person Booker { get; set; }

		[RU("Офис бронировщика", ruShort: "офис"), MaxLength(20)]
		public virtual string BookerOffice { get; set; }

		[RU("Код бронировщика", ruShort: "код"), MaxLength(20)]
		public virtual string BookerCode { get; set; }


		[RU("Тикетер")]
		public virtual Person Ticketer { get; set; }

		[RU("Офис тикетера", ruShort: "офис"), MaxLength(20)]
		public virtual string TicketerOffice { get; set; }

		[RU("Код тикетера", ruShort: "код"), MaxLength(20)]
		public virtual string TicketerCode { get; set; }

		[RU("IATA офис"), MaxLength(10)]
		public virtual string TicketingIataOffice { get; set; }

		public virtual bool IsTicketerRobot { get; set; }

		[Patterns.Seller]
		public virtual Person Seller { get; set; }

		[Patterns.Owner, Suggest(typeof(DocumentOwner))]
		public virtual Party Owner { get; set; }

		[Patterns.LegalEntity]
		public virtual Organization LegalEntity { get; set; }


		#region Finance

		protected CurrencyDailyRate Rate;

		public virtual void SetRate(CurrencyDailyRate value)
		{
			Rate = value;
		}
		//public virtual void LoadRate(Domain db)
		//{
		//	Rate = db.CurrencyDailyRate.By(a => a.Date == IssueDate);
		//}

		[RU("Тариф")]
		public virtual Money Fare { get; set; }
		public virtual Money Fare_EUR => Fare.ToEUR(Rate);
		public virtual Money Fare_USD => Fare.ToUSD(Rate);

		[RU("Экв. тариф"), DefaultMoney]
		public virtual Money EqualFare { get; set; }
		public virtual Money EqualFare_EUR => EqualFare.ToEUR(Rate);
		public virtual Money EqualFare_USD => EqualFare.ToUSD(Rate);

		[RU("Таксы"), DefaultMoney]
		public virtual Money FeesTotal { get; set; }
		public virtual Money FeesTotal_EUR => FeesTotal.ToEUR(Rate);
		public virtual Money FeesTotal_USD => FeesTotal.ToUSD(Rate);

		[RU("Комиссия консолидатора")]
		public virtual Money ConsolidatorCommission { get; set; }

		[Patterns.Total, DefaultMoney]
		public virtual Money Total { get; set; }
		public virtual Money Total_EUR => Total.ToEUR(Rate);
		public virtual Money Total_USD => Total.ToUSD(Rate);

		[RU("Сбор системы бронирования")]
		public virtual Money BookingFee { get; set; }

		[RU("Штраф за отмену"), DefaultMoney]
		public virtual Money CancelFee { get; set; }
		public virtual Money CancelFee_EUR => CancelFee.ToEUR(Rate);
		public virtual Money CancelFee_USD => CancelFee.ToUSD(Rate);

		[Patterns.Vat, DefaultMoney]
		public virtual Money Vat { get; set; }
		public virtual Money Vat_EUR => Vat.ToEUR(Rate);
		public virtual Money Vat_USD => Vat.ToUSD(Rate);

		[RU("Комиссия"), DefaultMoney]
		public virtual Money Commission { get; set; }
		public virtual Money Commission_EUR => Commission.ToEUR(Rate);
		public virtual Money Commission_USD => Commission.ToUSD(Rate);

		[RU("Скидка от комиссии"), DefaultMoney]
		public virtual Money CommissionDiscount { get; set; }
		public virtual Money CommissionDiscount_EUR => CommissionDiscount.ToEUR(Rate);
		public virtual Money CommissionDiscount_USD => CommissionDiscount.ToUSD(Rate);

		[RU("Сервисный сбор"), DefaultMoney]
		public virtual Money ServiceFee { get; set; }
		public virtual Money ServiceFee_EUR => ServiceFee.ToEUR(Rate);
		public virtual Money ServiceFee_USD => ServiceFee.ToUSD(Rate);

		[RU("Доп. доход"), DefaultMoney]
		public virtual Money Handling { get; set; }
		public virtual Money Handling_EUR => Handling.ToEUR(Rate);
		public virtual Money Handling_USD => Handling.ToUSD(Rate);

		[RU("Доп. расход"), DefaultMoney]
		public virtual Money HandlingN { get; set; }
		public virtual Money HandlingN_EUR => HandlingN.ToEUR(Rate);
		public virtual Money HandlingN_USD => HandlingN.ToUSD(Rate);

		[RU("Скидка"), DefaultMoney]
		public virtual Money Discount { get; set; }
		public virtual Money Discount_EUR => Discount.ToEUR(Rate);
		public virtual Money Discount_USD => Discount.ToUSD(Rate);

		[RU("Бонусная скидка"), DefaultMoney]
		public virtual Money BonusDiscount { get; set; }
		public virtual Money BonusDiscount_EUR => BonusDiscount.ToEUR(Rate);
		public virtual Money BonusDiscount_USD => BonusDiscount.ToUSD(Rate);

		[RU("Бонусное накопление"), DefaultMoney]
		public virtual Money BonusAccumulation { get; set; }
		public virtual Money BonusAccumulation_EUR => BonusAccumulation.ToEUR(Rate);
		public virtual Money BonusAccumulation_USD => BonusAccumulation.ToUSD(Rate);

		[RU("Cбор за возврат"), DefaultMoney]
		public virtual Money RefundServiceFee { get; set; }
		public virtual Money RefundServiceFee_EUR => RefundServiceFee.ToEUR(Rate);
		public virtual Money RefundServiceFee_USD => RefundServiceFee.ToUSD(Rate);

		[RU("Штраф сервисного сбора"), DefaultMoney]
		public virtual Money ServiceFeePenalty { get; set; }
		public virtual Money ServiceFeePenalty_EUR => ServiceFeePenalty.ToEUR(Rate);
		public virtual Money ServiceFeePenalty_USD => ServiceFeePenalty.ToUSD(Rate);

		public virtual Money ServiceTotal =>
			ServiceFee - RefundServiceFee - ServiceFeePenalty;

		[RU("К оплате"), DefaultMoney]
		public virtual Money GrandTotal { get; set; }
		public virtual Money GrandTotal_EUR => GrandTotal.ToEUR(Rate);
		public virtual Money GrandTotal_USD => GrandTotal.ToUSD(Rate);

		public virtual decimal? CancelCommissionPercent { get; set; }

		[RU("Комисия за возврат")]
		public virtual Money CancelCommission { get; set; }
		public virtual Money CancelCommission_EUR => CancelCommission.ToEUR(Rate);
		public virtual Money CancelCommission_USD => CancelCommission.ToUSD(Rate);

		[RU("% комиссии")]
		public virtual decimal? CommissionPercent { get; set; }


		//		public virtual Money FeesTotalWithoutVat
		//		{
		//			get { return FeesTotal - Vat; }
		//		}

		public virtual Money TotalToTransfer =>
			Total - Commission + CancelCommission;

		public virtual Money Profit =>
			GrandTotal - TotalToTransfer;

		public virtual Money ExtraCharge =>
			GrandTotal - Total;


		//public virtual void Recalculate(Domain db)
		//{
		//	Total = GetTotal();
		//	GrandTotal = GetGrandTotal();
		//	RefreshOrder(db);
		//}


		public virtual Money GetTotal()
		{
			return EqualFare + FeesTotal + ConsolidatorCommission - CancelFee;
		}

		public virtual Money GetGrandTotal()
		{
			return
				Total + ServiceFee + Handling - HandlingN
				- CommissionDiscount - Discount - BonusDiscount
				- RefundServiceFee - ServiceFeePenalty;

		}

		#endregion


		public virtual PaymentType PaymentType { get; set; }

		[RU("Ставка НДС для услуги")]
		public virtual TaxRate TaxRateOfProduct { get; set; }

		[RU("Ставка НДС для сбора")]
		public virtual TaxRate TaxRateOfServiceFee { get; set; }

		[Patterns.Note]
		public virtual string Note { get; set; }


		[RU("Оригинатор")]
		public virtual GdsOriginator Originator { get; set; }

		[RU("Источник")]
		public virtual ProductOrigin Origin { get; set; }

		[RU("Оригинальный документ")]
		public virtual GdsFile OriginalDocument { get; set; }

		public virtual string ProducerOrProviderAirlineIataCode
		{
			get { return Producer.As(a => a.AirlineIataCode) ?? Provider.As(a => a.AirlineIataCode); }
		}



		public static implicit operator string (Product me)
		{
			return me?.Name;
		}

		public override string ToString()
		{
			return Name;
		}

		public override object Clone()
		{
			var clone = (Product)base.Clone();

			clone.Fare = Fare.Clone();
			clone.EqualFare = EqualFare.Clone();
			clone.BookingFee = BookingFee.Clone();
			clone.Commission = Commission.Clone();
			clone.FeesTotal = FeesTotal.Clone();
			clone.Vat = Vat.Clone();
			clone.Total = Total.Clone();
			clone.ServiceFee = ServiceFee.Clone();
			clone.Handling = Handling.Clone();
			clone.HandlingN = HandlingN.Clone();
			clone.CommissionDiscount = CommissionDiscount.Clone();
			clone.Discount = Discount.Clone();

			clone.CancelFee = CancelFee.Clone();
			clone.CancelCommission = CancelCommission.Clone();
			clone.ServiceFeePenalty = ServiceFeePenalty.Clone();
			clone.RefundServiceFee = RefundServiceFee.Clone();

			clone.GrandTotal = GrandTotal.Clone();

			clone.Note = Note;

			clone._passengers = _passengers
				.Select(a => a.Clone<ProductPassenger>())
				.ToList()
				.Do(list => list.ForEach(a => a.Product = clone));


			return clone;
		}

		public virtual ProductPassenger AddPassenger(string passengerName, Person passenger)
		{
			var r = new ProductPassenger
			{
				Product = this,
				PassengerName = passengerName,
				Passenger = passenger,
			};

			Passengers.Add(r);

			return r;
		}

		public virtual void AddPassenger(ProductPassenger item)
		{
			item.Product = this;
			_passengers.Add(item);
		}

		public virtual void RemovePassenger(ProductPassenger item)
		{
			_passengers.Remove(item);
		}


		public virtual string GetPassengerNames()
		{
			return Passengers.No() ? null : string.Join(", ", Passengers.Select(a => a.PassengerName).OrderBy(a => a));
		}

		public virtual bool SetCustomer(Domain db, Party value)
		{
			if (Equals(value, Customer))
				return false;

			CheckCanModify(db);

			Customer = value;

			return true;
		}

		public virtual void SetCustomer2(Party value)
		{
			Customer = value;
		}

		public virtual bool SetOrder(Domain db, Order value)
		{
			if (Equals(value, Order)) return false;

			CheckCanModify(db);

			Order = value;

			if (value != null)
				Customer = value.Customer;

			return true;
		}

		public virtual bool SetOrder2(Domain db, Order value)
		{
			if (Equals(value, Order))
			{
				db.OnCommit(this, RefreshOrderKey, r => RefreshOrder(db));
				//db.OnCommit(this, db.Product.RefreshOrder);
				return false;
			}

			if (Order != null)
			{
				Order.Remove(db, this);
				Order = null;
			}

			value?.Add(db, this, saveDocuments: false);

			return true;
		}


		private void RefreshOrderKey(Product r) { }
		public virtual void RefreshOrder(Domain db)
		{
			var order = Order;
			if (order != null)
			{
				if (Customer != null && !Equals(order.Customer, Customer))
					throw new DomainException(Exceptions.DifferentCustomer_Error, order.Number);

				if (db.Configuration.AviaOrderItemGenerationOption != AviaOrderItemGenerationOption.AlwaysOneOrderItem)
				{
					var serviceFeeItem = order.ItemsBy(this, a => a.IsServiceFee).FirstOrDefault();

					if (ServiceFee.No() && serviceFeeItem != null || ServiceFee.Yes() && serviceFeeItem == null)
					{
						order
							.ItemsBy(this)
							.ForEach(a => order.RemoveOrderItem(db, a));
						order
							.Add(db, this);
					}
				}

				order.Recalculate(db);
			}

			if (db.IsDirty(this, a => a.Order))
			{
				var oldOrder = db.OldValue(this, a => a.Order);
				oldOrder?.Recalculate(db);
			}
		}

		public virtual void SetVoidStatus(Domain db, bool value)
		{
			IsVoid = value;

			if (value)
				Order?.Remove(db, this);
		}

		private void CheckCanModify(Domain db)
		{
			if (_canModifyChecked || db == null)
				return;

			db.Product.AssertModify(this);

			_canModifyChecked = true;
		}


		protected virtual string GetPassengerName()
		{
			return Passengers.One(a => a.PassengerName);
		}

		protected virtual void SetPassengerName(string value)
		{
			if (value == null) return;

			var psg = Passengers.One();

			if (psg != null)
				psg.PassengerName = value;
			else
				AddPassenger(value, null);
		}

		protected virtual Person GetPassenger()
		{
			return Passengers.One(a => a.Passenger);
		}

		protected virtual void SetPassenger(Person value)
		{
			if (value == null) return;

			Passengers.One()
				.Do(a => a.Passenger = value)
				.Else(() => AddPassenger(null, value));
		}


		#region OrderItem

		public virtual string TextForOrderItem => _getTextByProductType[(int)Type](this);

		// ReSharper disable once RedundantExplicitArraySize
		private static readonly Func<Product, string>[] _getTextByProductType = new Func<Product, string>[(int)ProductTypes.MaxValue + 1]
		{
			r => DomainRes.AviaTicket + GetText3((AviaDocument)r),
			//r => CommonRes.OrderItem_AviaTicketSource + GetText2(r),
			r => DomainRes.AviaRefund + GetText3((AviaDocument)r),
			//r => CommonRes.OrderItem_AviaRefundSource + GetText2(r),

			r =>
			{
				var mco = (AviaMco)r;

				if (mco.Description.Yes())
					return mco.Description + GetText2(r);

				return DomainRes.AviaMco + GetText2(r);
			},

			rr => rr.As<Pasteboard>().As(r =>
				DomainRes.Pasteboard +
				(r.DeparturePlace + r.ArrivalPlace.As(a => " - " + a)).As(a => " " + a) +
				r.DepartureDate.As(a => ", " + CommonRes.OrderItem_PasteboardSource_StartDate + " " + a.AsDateString()) +
				r.ServiceClass.Translate().As(a => ", " + a) +
				r.TrainNumber.As(a => ", " + CommonRes.OrderItem_PasteboardSource_TrainNumber + " " + a) +
				r.CarNumber.As(a => ", " + CommonRes.OrderItem_PasteboardSource_CarNumber + " " + a) +
				r.GetPassengerNames().As(a => ", " + a)
			),

			r => DomainRes.SimCard + GetText2(r),
			r => DomainRes.Isic + GetText2(r),
			r => DomainRes.Excursion + GetText2(r),
			r => DomainRes.Tour + GetText2(r),

			rr => rr.As<Accommodation>().As(r =>
				DomainRes.Accommodation +
				r.HotelName.As(a => ": " + DomainRes.Common_Hotel.ToLower() + " " + a) +
				r.PlacementName.As(a => ", " + a) +
				r.Country.As(a => ", " + a) +
				r.AccommodationType.As(a => ", " + a) +
				", " + r.StartDate.ToDateString() + r.FinishDate.As(a => " - " + a.AsDateString()) +
				r.CateringType.As(a => ", " + a) +
				r.GetPassengerNames().As(a => ", " + a)
			),

			r => DomainRes.Transfer + GetText2(r),
			r => DomainRes.Insurance + GetText2(r),
			r => DomainRes.CarRental + GetText2(r),

			rr => rr.As<GenericProduct>().As(r =>
				r.GenericType.As(a => a.Name + " ") +
				r.Number.As(a => CommonRes.Number_Short + " " + a + " ") +
				CommonRes.OrderItem_From + " " + r.IssueDate.ToDateString() +
				rr.GetPassengerNames().As(a => ", " + a)
			),

			rr => rr.As<BusTicket>().As(r =>
				DomainRes.BusTicket +
				(r.DeparturePlace + r.ArrivalPlace.As(a => " - " + a)).As(a => " " + a) +
				r.DepartureDate.As(a => ", " + CommonRes.OrderItem_PasteboardSource_StartDate + " " + a.AsDateString()) +
				r.SeatNumber.As(a => ", " + CommonRes.OrderItem_BusTicketSource_SeatNumber + " " + a) +
				r.GetPassengerNames().As(a => ", " + a)
			),

			rr => rr.As<PasteboardRefund>().As(r =>
				DomainRes.PasteboardRefund +
				(r.DeparturePlace + r.ArrivalPlace.As(a => " - " + a)).As(a => " " + a) +
				r.DepartureDate.As(a => ", " + CommonRes.OrderItem_PasteboardSource_StartDate + " " + a.AsDateString()) +
				r.ServiceClass.Translate().As(a => ", " + a) +
				r.TrainNumber.As(a => ", " + CommonRes.OrderItem_PasteboardSource_TrainNumber + " " + a) +
				r.CarNumber.As(a => ", " + CommonRes.OrderItem_PasteboardSource_CarNumber + " " + a) +
				r.GetPassengerNames().As(a => ", " + a)
			),

			r => DomainRes.InsuranceRefund + GetText2(r),

			rr => rr.As<BusTicketRefund>().As(r =>
				DomainRes.BusTicketRefund +
				(r.DeparturePlace + r.ArrivalPlace.As(a => " - " + a)).As(a => " " + a) +
				r.DepartureDate.As(a => ", " + CommonRes.OrderItem_PasteboardSource_StartDate + " " + a.AsDateString()) +
				r.SeatNumber.As(a => ", " + CommonRes.OrderItem_BusTicketSource_SeatNumber + " " + a) +
				r.GetPassengerNames().As(a => ", " + a)
			),

		};



		public static string GetAirportString(Airport airport)
		{
			if (airport == null) return null;

			var settlement = airport.LocalizedSettlement ?? airport.Settlement;

			if (settlement.No())
				return airport.Code;

			return $"{settlement} ({airport.Code})";
		}

		private static string GetText2(Product r)
		{
			var sb = new StringBuilder();

			if (!r.IsReservation)
			{
				sb
					.Append(' ')
					.Append(CommonRes.Number_Short)
					.Append(' ')
					.Append(r.ToString())
					.Append(' ')
					.Append(CommonRes.OrderItem_From)
					.Append(' ')
					.Append(r.IssueDate.ToDateString())
					.Append(',');

				if (r.Country != null)
					sb
						.Append(' ')
						.Append(r.Country.ToString())
						.Append(", ");
			}

			r.GetPassengerNames().Do(a =>
				sb
					.Append(' ')
					.Append(a)
					.Append(',')
			);

			r.As<IItineraryContainer>().As(a => a.GetItinerary(GetAirportString, true, true)).Do(a =>
				sb
					.Append(' ')
					.Append(DomainRes.OrderItem_Text_Itinerary)
					.Append(' ')
					.Append(a)
			);

			return sb.ToString().TrimEnd(',');
		}

		private static string GetText3(AviaDocument r)
		{
			var sb = new StringBuilder();

			if (!r.IsReservation)
			{
				sb.Append(
					", " + r.AirlineIataCode + ' ' + r + ' ' +
					CommonRes.OrderItem_From + ' ' +
					r.IssueDate.ToDateString()
				);

				r.Country.Do(a => sb.Append(", " + r.Country));
			}

			r.GetPassengerNames().Do(a => sb.Append(", " + a));

			r.As<IItineraryContainer>().As(a => a.GetItinerary(GetAirportString, true, true)).Do(a =>
				sb.Append(", " + DomainRes.OrderItem_Text_Itinerary + ' ' + a)
			);

			// для BSV
			r.ReissueFor.Do(a => sb.Append(" (" + DomainRes.Common_Reissue.ToLower() + " " + a.Name + ")"));

			return sb.ToString().TrimStart(',');
		}

		#endregion





		private bool _canModifyChecked;
		private IList<ProductPassenger> _passengers = new List<ProductPassenger>();


	}

}