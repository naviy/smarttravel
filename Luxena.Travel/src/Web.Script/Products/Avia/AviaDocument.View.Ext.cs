using System;

using Ext;

using Luxena.Travel.Services;

using Field = Ext.form.Field;
using MenuItemConfig = Ext.menu.ItemConfig;


namespace Luxena.Travel
{

	partial class AviaDocumentSemantic
	{
		public override void Initialize()
		{
			base.Initialize();

			FullNumber.ToEditor = new AviaDocument_FullNumberEditor(this).ToEditor;

			RefundedProduct
				.SetEditor(0, delegate(FormMember m)
				{
					m.DataProxy(AviaService.SuggestNotRefundedDocumentsProxy());
				});

			SetOnePassengerEditorsAndColumns(PassengerName, Passenger, PassengerRow);

			GdsPassportStatus
				.SetEditor(-2);

			Country._required = false;
		}
	}

	
	public class AviaDocument_FullNumberEditor
	{
		public AviaDocument_FullNumberEditor(AviaDocumentSemantic v)
		{
			this.v = v;
		}

		protected AviaDocumentSemantic v;

		public Component ToEditor()
		{
			return v.RowPanel(new Component[]
			{
				_airlinePrefixCodeField = v.AirlinePrefixCode.ToField(35, delegate(FormMember m)
				{
					m.Label(string.Format("{0} / {1}", DomainRes.Common_Number, DomainRes.Airline));
					m.BoldLabel();
					m.OnBlur(FindAirline);
					m.OnKeyPress(AirlineCodeKeyPress);
				}),

				v.TextComponent("-"),

				v.Number.ToField(90, delegate(FormMember m)
				{
					m.HideLabel();
				}),

				v.TextComponent("/"),

				_producerField = v.Producer.ToField(v.Producer.EditForm.FieldMaxWidth + 19, delegate(FormMember m)
				{
					m.HideLabel();
				})
			});
		}

		void FindAirline()
		{
			string value = (string)_airlinePrefixCodeField.getValue();
			if (value == null || value == string.Empty) return;

			string oldValue = (string)Type.GetField(_airlinePrefixCodeField, "startValue");

			if (value != oldValue || _airlineCodeChanded)
				AviaService.FindAirlineByPrefixCode(value,
					delegate(object result)
					{
						if (result != null)
							_producerField.setValue(result);
					}, null);

			_airlineCodeChanded = false;
		}

		void AirlineCodeKeyPress(object arg1, object arg2)
		{
			EventObject eventObject = (EventObject)arg2;

			if (eventObject.getKey() >= (double)Type.GetField(eventObject, "ZERO") && eventObject.getKey() <= (double)Type.GetField(eventObject, "NINE"))
				_airlineCodeChanded = true;
		}


		private Field _airlinePrefixCodeField;
		private Field _producerField;
		private bool _airlineCodeChanded;

	}
	
}
