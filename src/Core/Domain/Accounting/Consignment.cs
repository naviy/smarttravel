using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Travel.Reports;


namespace Luxena.Travel.Domain
{

	[SupervisorPrivileges]
	public partial class Consignment : Entity2
	{
		[EntityName]
		public virtual string Number { get; set; }

		public virtual DateTime IssueDate { get; set; }

		public virtual Party Supplier { get; set; }

		public virtual Party Acquirer { get; set; }

		[ReadOnly]
		public virtual string AcquirerCode => (Acquirer as Organization)?.Code;

		[ReadOnly]
		public virtual Order Order
		{
			get { return _orderItems?.Select(a => a.Order).One() ?? _order; }
			set { _order = value; }
		}

		private Order _order;

		[ReadOnly]
		public virtual Money GrandTotal { get; set; }

		[ReadOnly]
		public virtual Money Vat { get; set; }

		[ReadOnly]
		public virtual Money Total
		{
			get
			{
				var total = GrandTotal.Clone();
				total.Amount -= Vat.Amount;
				return total;
			}
		}

		public virtual Money Discount { get; set; }

		public virtual string TotalSupplied { get; set; }

		public virtual IList<OrderItem> OrderItems => _orderItems;

		public virtual IList<IssuedConsignment> IssuedConsignments => _issuedConsignments;

		public virtual void AddOrderItem(OrderItem item)
		{
			item.Consignment = this;

			_orderItems.Add(item);
		}

		public virtual void RemoveOrderItem(OrderItem item)
		{
			item.Consignment = null;

			_orderItems.Remove(item);
		}

		public virtual void AddIssuedConsignment(IssuedConsignment issuedConsignment)
		{
			issuedConsignment.Consignment = this;

			_issuedConsignments.Add(issuedConsignment);
		}

		public override string ToString()
		{
			return Number;
		}


		public virtual IssuedConsignment LastIssuedConsignment()
		{
			IssuedConsignment result = null;

			var maxTimestamp = DateTime.MinValue;

			foreach (var issuedConsignment in IssuedConsignments)
			{
				if (issuedConsignment.TimeStamp <= maxTimestamp) continue;
				result = issuedConsignment;
				maxTimestamp = issuedConsignment.TimeStamp;
			}

			return result;
		}


		private readonly IList<OrderItem> _orderItems = new List<OrderItem>();
		private readonly IList<IssuedConsignment> _issuedConsignments = new List<IssuedConsignment>();


		// ReSharper disable once RedundantExplicitArraySize
		private static readonly Func<OrderItem[], string>[] _totalSuppliedByProductType = new Func<OrderItem[], string>[(int)ProductTypes.MaxValue + 1]
		{
			items => CountToWords(
				items.Count(a => a.Product.IsAviaTicket),
				0,
				ReportRes.ConsignmentPrinter_AviaTicketOne,
				ReportRes.ConsignmentPrinter_AviaTicketTwo,
				ReportRes.ConsignmentPrinter_AviaTicketFive
			),
			null,
			null,
			items => CountToWords(
				items.Count(a => a.Product.IsPasteboard),
				0,
				ReportRes.ConsignmentPrinter_PasteboardOne,
				ReportRes.ConsignmentPrinter_PasteboardTwo,
				ReportRes.ConsignmentPrinter_PasteboardFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsSimCard),
				1,
				ReportRes.ConsignmentPrinter_SimCardOne,
				ReportRes.ConsignmentPrinter_SimCardTwo,
				ReportRes.ConsignmentPrinter_SimCardFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsIsic),
				1,
				ReportRes.ConsignmentPrinter_IsicOne,
				ReportRes.ConsignmentPrinter_IsicTwo,
				ReportRes.ConsignmentPrinter_IsicFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsExcursion),
				1,
				ReportRes.ConsignmentPrinter_ExcursionOne,
				ReportRes.ConsignmentPrinter_ExcursionTwo,
				ReportRes.ConsignmentPrinter_ExcursionFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsTour),
				0,
				ReportRes.ConsignmentPrinter_TourOne,
				ReportRes.ConsignmentPrinter_TourTwo,
				ReportRes.ConsignmentPrinter_TourFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsAccommodation),
				2,
				ReportRes.ConsignmentPrinter_AccommodationOne,
				ReportRes.ConsignmentPrinter_AccommodationTwo,
				ReportRes.ConsignmentPrinter_AccommodationFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsTransfer),
				0,
				ReportRes.ConsignmentPrinter_TransferOne,
				ReportRes.ConsignmentPrinter_TransferTwo,
				ReportRes.ConsignmentPrinter_TransferFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsInsurance),
				1,
				ReportRes.ConsignmentPrinter_InsuranceOne,
				ReportRes.ConsignmentPrinter_InsuranceTwo,
				ReportRes.ConsignmentPrinter_InsuranceFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsCarRental),
				1,
				ReportRes.ConsignmentPrinter_CarRentalOne,
				ReportRes.ConsignmentPrinter_CarRentalTwo,
				ReportRes.ConsignmentPrinter_CarRentalFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsGenericProduct),
				1,
				ReportRes.ConsignmentPrinter_GenericProductOne,
				ReportRes.ConsignmentPrinter_GenericProductTwo,
				ReportRes.ConsignmentPrinter_GenericProductFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsBusTicket),
				1,
				ReportRes.ConsignmentPrinter_BusTicketOne,
				ReportRes.ConsignmentPrinter_BusTicketTwo,
				ReportRes.ConsignmentPrinter_BusTicketFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsPasteboardRefund),
				2,
				ReportRes.ConsignmentPrinter_PasteboardRefundOne,
				ReportRes.ConsignmentPrinter_PasteboardRefundTwo,
				ReportRes.ConsignmentPrinter_PasteboardRefundFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsInsuranceRefund),
				2,
				ReportRes.ConsignmentPrinter_InsuranceRefundOne,
				ReportRes.ConsignmentPrinter_InsuranceRefundTwo,
				ReportRes.ConsignmentPrinter_InsuranceRefundFive
			),
			items => CountToWords(
				items.Count(a => a.Product.IsBusTicketRefund),
				2,
				ReportRes.ConsignmentPrinter_BusTicketRefundOne,
				ReportRes.ConsignmentPrinter_BusTicketRefundTwo,
				ReportRes.ConsignmentPrinter_BusTicketRefundFive
			),
		};

		public virtual string GetTotalSupplied(Domain db)
		{
			var items = OrderItems.Where(item => item.Product != null && (item.IsProductData || item.IsFullDocument)).ToArray();

			var sb = new StringWrapper();

			foreach (var a in _totalSuppliedByProductType)
			{
				if (a == null) continue;

				var s = a(items);
				if (s.No()) continue;

				if (sb > 0)
					sb += ", ";

				sb += s;
			}

			return sb.ToString().ToUpperFirstLetter();
		}

		/// <summary>
		/// </summary>
		/// <param name="gender">Род: 0 - мужской, 1 - женский, 2 - средний</param>
		/// <returns></returns>
		private static string CountToWords(int value, int gender, string one, string two, string five)
		{
			if (value == 0) return null;

			var countPart = value % 100 < 20
				? MoneyExtensions.GetUnities(gender)[value % 100]
				: string.Concat(MoneyExtensions.GetTens()[value % 100 / 10], " ", MoneyExtensions.GetUnities(gender)[value % 10]);

			var aviaTicketPart = MoneyExtensions.GetNumberText(value, one, two, five).ToLower();

			return countPart + " " + aviaTicketPart;
		}

	}

}