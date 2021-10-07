using System.Runtime.CompilerServices;


namespace Luxena.Travel
{

	partial class AccommodationSemantic
	{

		public override void Initialize()
		{
			base.Initialize();

			SetManyPassengerEditorsAndColumns(PassengerName, PassengerRow);

			TourSemantic.SetHotelEditorsAndColumns(HotelName, HotelOffice, HotelCode, HotelRow);
			TourSemantic.SetHotelEditorsAndColumns(PlacementName, PlacementOffice, PlacementCode, PlacementRow);

			AccommodationType.SetEditor(-2);
			CateringType.SetEditor(-2);
		}

		[PreserveCase]
		public SemanticMember HotelRow = Member;

		[PreserveCase]
		public SemanticMember PlacementRow = Member;

	}

}