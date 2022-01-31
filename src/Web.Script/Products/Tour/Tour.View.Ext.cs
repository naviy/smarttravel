using System;
using System.Runtime.CompilerServices;

using Ext;
using Ext.data;

using Record = Ext.data.Record;


namespace Luxena.Travel
{

	partial class TourSemantic
	{


		public override void Initialize()
		{

			base.Initialize();
			

			SetManyPassengerEditorsAndColumns(PassengerName, PassengerRow);

			SetHotelEditorsAndColumns(HotelName, HotelOffice, HotelCode, HotelRow);
			SetHotelEditorsAndColumns(PlacementName, PlacementOffice, PlacementCode, PlacementRow);

			AccommodationType.SetEditor(-2);
			CateringType.SetEditor(-2);

			AviaDescription.SetEditor(-3);
			TransferDescription.SetEditor(-3);

		}



		public static void SetHotelEditorsAndColumns(SemanticMember name, SemanticMember office, SemanticMember code, SemanticMember row)
		{

			SemanticEntity v = name;
			row._name = name._name;


			row.SetColumn(true, 100, delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
			{
				object nameValue = record.get(name._name);
				object officeValue = record.get(office._name);
				object codeValue = record.get(code._name);

				if (Script.IsValue(officeValue) || Script.IsValue(codeValue))
					nameValue += " / " + (officeValue ?? "") + " - " + (codeValue ?? "");

				return nameValue;
			});


			row.ToEditor = delegate
			{
				return name.RowPanel(new Component[]
				{
					name.ToField(),
					v.TextComponent("/"),
					office.ToField(80, delegate(FormMember m) { m.HideLabel(); }),
					v.TextComponent("-"),
					code.ToField(64, delegate(FormMember m) { m.HideLabel(); }),
				});
			};

		}


		[PreserveCase]
		public SemanticMember HotelRow = Member;

		[PreserveCase]
		public SemanticMember PlacementRow = Member;

	}

}