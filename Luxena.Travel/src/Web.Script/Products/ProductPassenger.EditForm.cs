using System;

using LxnBase.UI;


namespace Luxena.Travel
{

	public partial class ProductPassengerEditForm
	{

		protected override string GetNameBy(object data)
		{
			ProductPassengerDto r = (ProductPassengerDto)data;
			return Script.IsValue(r.Passenger) ? r.Passenger.Name : r.PassengerName;
		}

		protected override void CreateControls()
		{
			if (_args.Mode == LoadMode.Remote)
				Form.add(MainDataPanel(new object[]
				{
					se.Product.ToField(-3),
				}));

			Form.add(MainDataPanel(new object[]
			{
				se.PassengerRow,
			}));
		}

	}

}