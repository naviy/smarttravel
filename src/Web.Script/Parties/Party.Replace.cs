using System.Collections;

using Luxena.Travel.Services;

using LxnBase;
using LxnBase.UI;


namespace Luxena.Travel
{

	public  class PartyReplaceForm : EntityEditForm
	{

		static PartyReplaceForm()
		{
			RegisterEdit("PartyReplace", typeof(PartyReplaceForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = new PartyReplaceParams();
		}

		PartyReplaceParams se;

		protected override void Initialize()
		{
			_args.Mode = LoadMode.Local;
			Window.title = "Замена контрагента";
			Window.setWidth(500);
			Form.labelWidth = 120;
		}



		protected override void CreateControls()
		{
			Form.add(MainDataPanel(new object[]
			{
				se.FromParty,
				se.ToParty,
			}));
		}

		protected override void OnSave()
		{
			Dictionary data = (Dictionary)GetData();

			PartyService.Replace((string)data["FromParty"], (string)data["ToParty"],
				delegate (object result)
				{
					int count = (int)result;

					MessageRegister.Info(Window.title, "Было заменено " + count + " записей");

					CompleteSave(result);
				}, null
			);
		}

	}


	public class PartyReplaceParams : EntitySemantic
	{
		public SemanticMember FromParty = Member
			.Title("Заменяем контрагент")
			.Reference("Party")
			.Required();

		public SemanticMember ToParty = Member
			.Title("на")
			.Reference("Party")
			.Required();
	}

}