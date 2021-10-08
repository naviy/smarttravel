using System;

using LxnBase.UI;


namespace Luxena.Travel
{

	public partial class PassportEditForm
	{

		protected override void Initialize()
		{
			Window.setWidth(500);
			Form.labelWidth = 120;
		}

		protected override string GetNameBy(object data)
		{
			PassportDto r = (PassportDto)data;
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
				se.Number.ToField(-2),
				se.FirstName,
				se.MiddleName,
				se.LastName,
				se.Citizenship,
				se.Birthday,
				se.Gender.ToField(-2),
				se.IssuedBy,
				se.ExpiredOn,
				se.Note,
			}));
		}

	}

}