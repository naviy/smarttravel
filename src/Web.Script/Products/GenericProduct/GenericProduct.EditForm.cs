using System;


namespace Luxena.Travel
{

	public partial class GenericProductEditForm
	{

		protected override string GetNameBy(object data)
		{
			GenericProductDto dto = (GenericProductDto)data;
			return 
				(Script.IsValue(dto.GenericType) ? dto.GenericType.Name : DomainRes.GenericProduct) +
				(Script.IsValue(dto.Number) ? " #" + dto.Number : "");
		}

		protected override void CreateControls()
		{
			Form.add(ColumnPanel(new object[]
			{
				MainDataPanel(new object[]
				{

					se.RowPanel2v(se.IssueDate, null, se.ReissueFor, null),
					se.Number,
					se.PnrAndTourCodes,

					se.PassengerRow,

					se.Provider,

					se.GenericType,

			
					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,
					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}