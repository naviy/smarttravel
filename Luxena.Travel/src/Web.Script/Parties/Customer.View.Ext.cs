using System.Collections;


namespace Luxena.Travel
{

	partial class CustomerSemantic
	{
		public override void Initialize()
		{
			base.Initialize();

			_prepareNewData = delegate(Dictionary data)
			{
				data["IsCustomer"] = true;
			};
		}
	}

}
