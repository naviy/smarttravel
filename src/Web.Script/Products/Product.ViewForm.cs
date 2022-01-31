using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;

using LxnBase;
using LxnBase.Data;
using LxnBase.Net;
using LxnBase.UI;

using Luxena.Travel.Services;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public abstract class ProductViewForm : BasicViewForm
	{

		protected ProductViewForm(string tabId, object id, string type)
			: base(tabId, id, type, "aviaDocumentView")
		{
		}

		public static void ViewProduct(Type viewFormType, string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string)id, delegate(string tabId)
			{
				return (ProductViewForm)Type.CreateInstance(viewFormType, tabId, id, type);
			});
		}


		public abstract ProductDto Product { get; set; }
		public virtual bool IsRefund { get { return false; } }


		protected virtual void OnLoadFail(WebServiceFailureArgs e)
		{
			Tabs.Close(this);
		}


		protected override object[] CreateToolbarItems()
		{
			return (object[])CreateStdToolbarItems();
		}


		protected virtual ArrayList CreateStdToolbarItems()
		{
			ArrayList list = new ArrayList();


			list.AddRange(new object[]
			{
				_voidButton = Action(Res.Void_Action.ToLowerCase(), ToggleVoid),

				_addRefundButton = Action(Res.AviaDocument_AddRefund_Action.ToLowerCase(), AddRefund),

				new ToolbarSeparator(),

				_createOrderButton = MenuAction(Res.AviaDocument_AddToOrder.ToLowerCase(),
					new object[]
					{
						MenuItem(Res.AviaDocument_AddToOrderNew.ToLowerCase(), delegate { CreateNewOrder(); }),
						MenuItem(Res.AviaDocument_AddToOrderExist.ToLowerCase(), delegate { TryAddToExistingOrder(); })
					}
				),

				new ToolbarSeparator(),

				_copyButton = Action(BaseRes.Copy.ToLowerCase(), CopyEntity),

				_editButton = Action(BaseRes.Edit.ToLowerCase(), EditEntity),

				_deleteButton = Action(BaseRes.Remove.ToLowerCase(), RemoveEntity),
			});

			return list;
		}


		protected override void LoadInstance()
		{
			ProductService.GetObject1(GetClassName(), _id, Load, OnLoadFail);
		}


		protected override void Load(object response)
		{
			StdLoad(response);
		}

		protected void StdLoad(object response)
		{
			Product = (ProductDto)response;

			PreRender();
		}

		protected virtual void PrepareButtons()
		{
			if (!ProductDto.IsCanDelete(Product))
				_deleteButton.hide();

			if (!ProductDto.IsCanUpdate(Product))
			{
				_voidButton.setDisabled(true);
				_editButton.setDisabled(true);
				_copyButton.setDisabled(true);

				_createOrderButton.setDisabled(true);
			}
			else
				_createOrderButton.setDisabled(Product.IsVoid);

			if (Product.IsRefund)
				_addRefundButton.hide();
			else if (Script.IsValue(Product.Refund))
				_addRefundButton.setDisabled(true);
		}


		protected virtual void PreRender()
		{
			ProductDto r = Product;
			if (Script.IsNullOrUndefined(r)) return;


			PrepareButtons();


			setTitle(r.Name ?? GetClassTitle());

			_titleLabel.setText(GetTitle());

			_titleLabel.removeClass("processed");
			_titleLabel.removeClass("unprocessed");
			_titleLabel.removeClass("voided");

			_voidButton.setText(Res.Void_Action.ToLowerCase());

			if (r.RequiresProcessing)
				_titleLabel.addClass("unprocessed");
			else if (r.IsVoid)
			{
				_titleLabel.addClass("voided");

				_voidButton.setText(Res.Restore_Action.ToLowerCase());
			}
			else
				_titleLabel.addClass("processed");

			_contentPanel.body.dom.InnerHTML = GetFullHtml();
		}


		protected abstract string GetClassName();
		protected abstract string GetClassTitle();

		protected override string GetTitle()
		{
			return GetProductTitle(GetClassTitle());
		}

		protected string GetProductTitle(string classTitle)
		{
			ProductDto r = Product;

			string caption = classTitle;

			string status = r.RequiresProcessing ? Res.AviaDocument_Unprocessed : (r.IsVoid ? Res.AviaDocument_Voided : Res.AviaDocument_Processed);

			caption = string.Format("{0} ({1})", caption, status.ToLowerCase());

			if (Script.IsNullOrUndefined(r.Name))
				caption = string.Format("{0} - {1}", caption, Res.AviaDocument_NotIssued);

			return caption;
		}


		private void CreateNewOrder()
		{
			Dictionary values = new Dictionary("AviaDocuments", new object[] { Product.Id });
			values["SeparateServiceFee"] = true;

			FormsRegistry.EditObject(ClassNames.Order, null, values, delegate (object arg)
			{
				LoadInstance();
				ItemResponse response = (ItemResponse)arg;
				FormsRegistry.ViewObject(ClassNames.Order, ((OrderDto)response.Item).Id);
			}, null);
		}

		private void TryAddToExistingOrder()
		{
			Dictionary documents = new Dictionary();
			documents[Product.Id] = Product.Name;


			ProductDto.TryAddToExistingOrder(
				documents,
				true,
				Product.Name,
				LoadInstance
			);
		}



		#region Html

		protected virtual string GetFullHtml()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(GetCommonDataHtml());

			sb.Append(GetFinanceDataHtml());

			sb.Append(HasValue(Product.Note, delegate
			{
				return string.Format(
					@"<div class='note'><h2>{0}</h2><pre>{1}</pre></div>",
					DomainRes.Common_Note, Product.Note
				);
			}));

			return sb.ToString();
		}

		protected abstract string GetCommonDataHtml();

		[AlternateSignature]
		protected extern string GetCompositeFieldHtml(object r, SemanticMember view1, object value1, SemanticMember view2, object value2);

		protected string GetCompositeFieldHtml(object r, SemanticMember view1, object value1, SemanticMember view2, object value2, SemanticMember view3, object value3)
		{

			if (Script.IsNullOrUndefined(r))
				return "";


			string labels = view1._title + " / " + view2._title;
			string values = view1.GetValueHtml(r, value1) + " / " + view2.GetValueHtml(r, value2);


			if (!Script.IsNullOrUndefined(view3))
			{
				labels += " / " + view3._title;
				values += " / " + view3.GetValueHtml(r, value3);
			}


			return
				@"<tr><td class='fieldLabel'>" + labels + @":</td>" +
				@"<td class='fieldValue'>" + values + @"</td></tr>"
			;

		}



		protected string GetPnrCodeAndTourCodeHtml()
		{

			ProductDto r = Product;

			return HasValue(new object[] { r.PnrCode, r.TourCode }, delegate
			{

				string codes = NotEmpty(r.PnrCode);

				if (Script.IsValue(r.TourCode))
					codes += " / " + (r.TourCode ?? "");
				
				return
					@"<tr><td class='fieldLabel'>" + DomainRes.AviaDocument_PnrCode + " / " + DomainRes.AviaDocument_TourCode + @":</td>" +
					@"<td class='fieldValue'>" + NotEmpty(codes) + "</td></tr>"
				;

			});
		}



		protected string GetPassengerHtml(Reference passenger, string passengerName)
		{
			string text = ProductSemantic.GetPassengerValueHtml(passenger, passengerName);

			return "<tr><td class='fieldLabel'>" + DomainRes.AviaDocument_Passenger + ":</td><td class='fieldValue'>" + text + @"</td></tr>";
		}

		protected string GetPassengersHtml(ProductPassengerDto[] passengers)
		{
			string text = "";

			foreach (ProductPassengerDto passenger in passengers)
			{
				text += ProductSemantic.GetPassengerValueHtml(passenger.Passenger, passenger.PassengerName) + "<br/>";
			}

			return "<tr><td class='fieldLabel'>" + DomainRes.AviaDocument_Passenger + ":</td><td class='fieldValue'>" + text + @"</td></tr>";
		}



		protected string GetCustomerAndIntermediaryHtml(Reference customer, Reference intermediary)
		{

			ProductDto r = Product;

			return
				"<tr name='Customer'><td class='fieldLabel " + (r.Customer == null ? "error" : string.Empty) + @"'>" + DomainRes.Common_Customer + " / " + DomainRes.Common_Intermediary + @":</td>" +
				HasValue(new object[] { r.Customer, r.Intermediary }, delegate
				{
					return @"<td class='fieldValue'>" + Link(r.Customer) + " / " + Link(r.Intermediary) + @"</td></tr>";
				}, "<td/></tr>")
			;

		}



		protected string GetHotelHtml(SemanticMember nameMember, string name, string office, string code)
		{

			return HasValue(new object[] { name, office, code }, delegate
			{

				if (Script.IsValue(office) || Script.IsValue(code))
					name += " / " + (office ?? "") + " - " + (code ?? "");

				return
					@"<tr><td class='fieldLabel'>" + nameMember._title + @":</td>" +
					@"<td class='fieldValue'>" + NotEmpty(name) + "</td></tr>"
				;

			});

		}



		protected virtual string GetFinanceDataHtml()
		{
			ProductDto r = Product;
			ProductSemantic v = new SemanticDomain(this).Product;

			return
				@"<div class='financeData'><table>" +
					v.Fare.ToHtmlTr4(r, true) +
					v.EqualFare.ToHtmlTr4(r, false, true) +
					v.FeesTotal.ToHtmlTr4(r) +
					v.ConsolidatorCommission.ToHtmlTr4(r) +

					v.Total.ToHtmlTr4(r, true, false, true) +

					v.Vat.ToHtmlTr4(r, false) +

					v.Commission.ToHtmlTr4(r, false, false, false,
						HasValue(r.CommissionPercent, delegate { return "<td class='fieldValue gray'>(" + r.CommissionPercent.Format("N2") + "%)</td>"; })
					) +

					v.ServiceFee.ToHtmlTr4(r, true, false, true) +
					v.CommissionDiscount.ToHtmlTr4(r) +
					v.Handling.ToHtmlTr4(r) +
					v.HandlingN.ToHtmlTr4(r) +
					v.Discount.ToHtmlTr4(r) +
					v.BonusDiscount.ToHtmlTr4(r) +
					v.BonusAccumulation.ToHtmlTr4(r) +

					(!IsRefund ? "" :
						v.RefundServiceFee.ToHtmlTr4(r, true) +
						v.ServiceFeePenalty.ToHtmlTr4(r, true)
					) +

					v.GrandTotal.ToHtmlTr4(r, true) +

					v.PaymentType.ToHtmlTr4(r) +

					@"</table></div>";
		}

		#endregion


		protected virtual void ToggleVoid()
		{
			ProductDto r = Product;

			MessageBoxWrap.Confirm(BaseRes.Confirmation, r.IsVoid ? Res.Document_Restore_Confirmation : Res.Document_Void_Confirmation,
				delegate(string button, string text)
				{
					if (button != "yes")
						return;

					ProductService.ChangeVoidStatus(new object[] { r.Id }, null,
						delegate(object result)
						{
							ProductDto dto = (ProductDto)((object[])result)[0];

							r.IsVoid = dto.IsVoid;
							r.RequiresProcessing = dto.RequiresProcessing;
							r.Order = dto.Order;

							PreRender();
						},
						null);
				});
		}


		protected virtual void AddRefund()
		{
			throw new Exception("NotImplement");
		}

		protected void StdAddRefund(ProductDto r, string className, string[] addProperties)
		{
			Dictionary dictionary = new Dictionary();

			dictionary["IssueDate"] = Date.Now;

			foreach (string addProperty in addProperties)
			{
				AddValue(dictionary, r, addProperty);
			}

			dictionary["RefundedProduct"] = Reference.Create(_type, r.Name, r.Id);

			AddValue(dictionary, r, "Producer");
			AddValue(dictionary, r, "Provider");

			AddValue(dictionary, r, "Customer");
			AddValue(dictionary, r, "Intermediary");

			AddValue(dictionary, r, "Originator");

			AddValue(dictionary, r, "Order");

			dictionary["Seller"] = AppManager.CurrentPerson;

			AddValue(dictionary, r, "Fare");
			AddValue(dictionary, r, "EqualFare");
			AddValue(dictionary, r, "FeesTotal");
			AddValue(dictionary, r, "Total");
			AddValue(dictionary, r, "Vat");
			AddValue(dictionary, r, "Commission");
			AddValue(dictionary, r, "ServiceFee");
			AddValue(dictionary, r, "Discount");
			AddValue(dictionary, r, "GrandTotal");

			AddValue(dictionary, r, "TaxRateOfProduct");
			AddValue(dictionary, r, "TaxRateOfServiceFee");



			FormsRegistry.EditObject(className, null, dictionary,
				delegate(object arg)
				{
					if (arg == null)
						return;

					ProductDto refund = (ProductDto)((ItemResponse)arg).Item;

					r.Refund = Reference.Create(className, refund.Name, refund.Id);

					Load(r);

					FormsRegistry.ViewObject(className, refund.Id);
				}, null);
		}

		public static string GetOriginHtml(ProductDto r)
		{
			return HasValue(r.OriginString, delegate { return @"
				<tr><td class='fieldLabel'>" + DomainRes.AviaDocument_Origin + @":</td>
				<td class='fieldValue'>" + Link(r.OriginalDocument, NotEmpty(r.OriginString)) + @"</td></tr>";
			});
		}

		public static string GetBookerAndTicketerHtml(ProductSemantic v, ProductDto r)
		{
			string bookerStr = GetAgentStr(r.Booker, r.BookerOffice, r.BookerCode);

			string ticketerStr = GetAgentStr(r.Ticketer, r.TicketerOffice, r.TicketerCode);

			return
				HasValue(bookerStr, delegate { return @"
					<tr><td class='fieldLabel'>" + v.Booker._title + @":</td>
					<td class='fieldValue'>" + bookerStr + @"</td></tr>";
				}) +
				HasValue(ticketerStr, delegate { return @"
					<tr><td class='fieldLabel'>" + v.Ticketer._title + @":</td>
					<td class='fieldValue'>" + ticketerStr + @"</td></tr>";
				});
		}

		public static string GetAgentStr(Reference person, string office, string code)
		{
			string str = null;

			if (!string.IsNullOrEmpty(office) && !string.IsNullOrEmpty(code))
				str = string.Format("{0}-{1}", office, code);
			else if (!string.IsNullOrEmpty(office))
				str = office;
			else if (!string.IsNullOrEmpty(code))
				str = code;

			if (Script.IsValue(person) && Script.IsValue(str))
				str = string.Format("{0} ({1})", Link(person), str);
			else if (Script.IsValue(person))
				str = Link(person);

			return str;
		}


		protected Action _printButton;
		protected Action _blankButton;

		protected Action _createOrderButton;
		protected Action _voidButton;
		protected Action _addRefundButton;
		protected Action _editButton;
		protected Action _copyButton;
		protected Action _deleteButton;
	}


}