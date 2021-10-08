namespace Luxena.Travel
{

	partial class InsuranceSemantic
	{
		public override void Initialize()
		{
			base.Initialize();

			Number.SetColumn(false, 100, GetNumberRenderer());

			SetManyPassengerEditorsAndColumns(PassengerName, PassengerRow);
		}

	}

}