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
					se.IssueDate,
					se.ReissueFor,
					se.PassengerRow,
					se.Provider,

					se.GenericType,
					se.Number,

					se.RowPanel2v(se.StartDate, null, se.FinishDate, null),
			
					se.CustomerAndOrder,
					se.Intermediary,

					se.Country,
					se.PnrAndTourCodes,

					se.SellerAndOwnerRow,
					se.LegalEntity,
				}), 

				se.Finance,
			}));

			Form.add(MainDataPanel(new object[] { se.Note }));
		}

	}

}