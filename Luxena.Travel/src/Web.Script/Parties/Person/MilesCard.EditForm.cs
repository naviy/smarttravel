using System;

using LxnBase.UI;


namespace Luxena.Travel
{

	public partial class MilesCardEditForm
	{

		protected override void Initialize()
		{
			Window.setWidth(500);
			Form.labelWidth = 120;
		}

		protected override string GetNameBy(object data)
		{
			MilesCardDto r = (MilesCardDto)data;
			return Script.IsValue(r.Owner) ? r.Owner.Name : r.Number;
		}

		protected override void CreateControls()
		{
			if (_args.Mode == LoadMode.Remote)
				Form.add(MainDataPanel(new object[]
				{
					se.Owner.ToField(-3),
				}));

			Form.add(MainDataPanel(new object[]
			{
				se.Owner,
				se.Number.ToField(-2),
				se.Organization,
			}));
		}

	}

}