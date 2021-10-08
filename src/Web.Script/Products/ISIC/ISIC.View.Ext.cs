namespace Luxena.Travel
{

	partial class IsicSemantic
	{
		public override void Initialize()
		{
			base.Initialize();

			SetOnePassengerEditorsAndColumns(PassengerName, Passenger, PassengerRow);

			Number1
				.SetColumn(false, 100, GetNumberRenderer());
			Number2
				.SetColumn(false, 20);
		}

	}

}