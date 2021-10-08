using System;
using System.Collections;


namespace Luxena.Travel
{

	public partial class CurrencyDailyRateEditForm
	{

		protected override Dictionary GetInitData()
		{
			return new Dictionary(
				"Date", Date.Today
			);
		}

		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.Date,

				se.UAH_EUR.ToField(-2),
				se.UAH_RUB.ToField(-2),
				se.UAH_USD.ToField(-2),
				se.RUB_EUR.ToField(-2),
				se.RUB_USD.ToField(-2),
				se.EUR_USD.ToField(-2),
			}));
		}

	}

}