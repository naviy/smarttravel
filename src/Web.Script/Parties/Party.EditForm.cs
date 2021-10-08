using Ext;


namespace Luxena.Travel
{

	public abstract class PartyEditForm : EntityEditForm
	{

		protected override void PreInitialize()
		{
			base.PreInitialize();

			Window.cls += " aviaDocument-edit tabbed";
			Form.cls = "tabbed";
			Form.labelWidth = 140;
			Window.width = -3;
		}

		protected Panel AddressPanel(PartySemantic se, int height)
		{
			int itemHeight = height / 3 - 40;

			return TabMainPane(DomainRes.Common_Addresses, new object[]
			{
				se.ActualAddress.SetEditor(-3, delegate(FormMember m) { m.Height(itemHeight); }),
				se.LegalAddress.SetEditor(-3, delegate(FormMember m) { m.Height(itemHeight); }),
				se.Details.SetEditor(-3, delegate(FormMember m) { m.Height(itemHeight); }),
				se.InvoiceSuffix,
			});
		}

		protected Panel BonusPanel(PartySemantic se)
		{
			return TabMainPane("Бонусы", new object[]
			{
				EmptyRow(),
				se.BonusCardNumber.ToField(-1),
				se.BonusAmount,
			});
		}

	}

}
