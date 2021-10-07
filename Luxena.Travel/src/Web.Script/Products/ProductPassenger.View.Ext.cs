using System.Runtime.CompilerServices;


namespace Luxena.Travel
{

	partial class ProductPassengerSemantic
	{
		public override void Initialize()
		{
			
			ProductSemantic.SetOnePassengerEditorsAndColumns(PassengerName, Passenger, PassengerRow);

		}


		[PreserveCase]
		public SemanticMember PassengerRow = Member;

	}

}
