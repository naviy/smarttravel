using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Text.RegularExpressions;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public partial class Domain : Domain<Domain>
	{

		public static string Schema;
		public readonly Domain db;

		public string AaEntryMap => EntryMap;

		public Domain()
		{
			db = this;

			if (Schema == null)
			{
				var connectionString = ConfigurationManager.ConnectionStrings["Domain"].ConnectionString;
				var match = new Regex(@"User Id=([\w_]+?);").Match(connectionString);
				Schema = match.Groups[1].Value;
			}

			Database.SetInitializer<Domain>(null);
			Configuration.UseDatabaseNullSemantics = true;

			InitQueries();
		}

		public override bool KeyIsEmpty(object key)
		{
			return ((string)key).No();
		}

		partial void InitQueries();

		protected override void OnModelCreating(DbModelBuilder mb)
		{
			mb.HasDefaultSchema(Schema);

			mb.Conventions.Remove<PluralizingTableNameConvention>();
			mb.Conventions.Add<LuxenaTravelConvention>();

			//mb.Conventions.Add(new FunctionsConvention(Schema, this.GetType()));

			//			FloatAttribute.ConfigureModelBuilder(mb);

			var m = new DomainModel(mb);

			m.Identity.MapClass("class")
				.Map<InternalIdentity>("Internal")
				.Map<User>();

			m.Party.MapClass("class")
				.Map<Department>()
				.Map<Organization>()
				.Map<Person>();

			m.Payment.MapClass("class")
				.Map<CashInOrderPayment>("CashOrder")
				.Map<CashOutOrderPayment>("CashOutOrder")
				.Map<CheckPayment>("Check")
				.Map<ElectronicPayment>("Electronic")
				.Map<WireTransfer>("WireTransfer");

			m.Product.MapClass("class")
				.Map<AviaMco>()
				.Map<AviaRefund>()
				.Map<AviaTicket>()
				.Map<Accommodation>()
				.Map<BusTicket>()
				.Map<BusTicketRefund>()
				.Map<CarRental>()
				.Map<Excursion>()
				.Map<GenericProduct>()
				.Map<Insurance>()
				.Map<InsuranceRefund>()
				.Map<Isic>()
				.Map<Pasteboard>()
				.Map<PasteboardRefund>()
				.Map<SimCard>()
				.Map<Tour>()
				.Map<Transfer>()
			;

			m.GdsFile.MapClass("class")
				.Map<AirFile>("Air")
				.Map<AmadeusXmlFile>("AmadeusXml")
				.Map<GalileoXmlFile>("GalileoXml")
				.Map<MirFile>("Mir");
		}


		static partial void ConfigEntityInfos_(EntityInfo[] entities);

		public static void ConfigEntityInfos(EntityInfo[] entities)
		{
			ConfigEntityInfos_(entities);
		}


		//public static Travel.Domain.Entity.Reference Reference(string id, string name)
		//{
		//	return new Party.Reference { Id = id, Name = name };
		//}




	}

}
