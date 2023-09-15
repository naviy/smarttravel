using System;
using System.Collections;




namespace Luxena.Travel
{



	public partial class AmadeusAviaSftpRsaKeyListTab
	{


		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.CreatedOn.ToColumn(false),
				se.PPK,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}


	}




	public partial class AmadeusAviaSftpRsaKeyEditForm
	{


		protected override void CreateControls()
		{

			Window.width = -3;
			Form.labelWidth = 180;
			FieldMaxWidth = 360;


			Form.add(MainDataPanel(new object[]
			{
				se.PPK.ToField(-1),
			}));

		}


	}



}