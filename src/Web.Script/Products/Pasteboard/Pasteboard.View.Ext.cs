namespace Luxena.Travel
{

	partial class PasteboardSemantic
	{
		public override void Initialize()
		{
			base.Initialize();

			SetOnePassengerEditorsAndColumns(PassengerName, Passenger, PassengerRow);

			Number
				.SetColumn(false, 80, GetNumberRenderer())
				.SetEditor(-1);
		}

	}

}