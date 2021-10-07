using System.Runtime.CompilerServices;

using MenuItemConfig = Ext.menu.ItemConfig;


namespace Luxena.Travel
{

	partial class PartySemantic
	{
		public override void Initialize()
		{
			base.Initialize();

			Name.SetEditor(-3);
			LegalName.SetEditor(-3);
			ReportsTo.SetEditor(-3);

			Phones.ToEditor = delegate
			{
				return RowPanel2c(DomainRes.Common_Phones, Phone1, null, Phone2, null);
			};
			
			Emails.ToEditor = delegate
			{
				return RowPanel2c(DomainRes.Common_Emails, Email1, null, Email2, null);
			};

			WebAddress.SetEditor(-3);

			Note.SetEditor(-3);
		}

		[PreserveCase]
		public SemanticMember Phones = Member;

		[PreserveCase]
		public SemanticMember Emails = Member;

	}
	
}
