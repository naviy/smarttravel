namespace Luxena.Travel
{


	public class GdsFileEditForm : Entity3EditForm
	{
		static GdsFileEditForm()
		{
			RegisterEdit("GdsFile", typeof(GdsFileEditForm));
		}

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GdsFile;
		}

		protected override void CreateControls()
		{
			Window.width = -3;
			Form.labelWidth = 150;

			Form.add(MainDataPanel(new object[]
			{
				se.Name.ToField(-3),
				se.FileType,

				EmptyRow(),

				se.Content.ToField(-3, delegate(FormMember m) {  }),
				se.TimeStamp,

				EmptyRow(),

				se.ImportResult.ToField(-3),
				se.ImportOutput.ToField(-3),
			}));
		}


		private GdsFileSemantic se;

	}



}
