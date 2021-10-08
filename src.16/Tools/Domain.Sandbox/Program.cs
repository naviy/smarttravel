using System;
using System.Linq;

using Luxena;


namespace Domain.Sandbox
{


	using Luxena.Travel.Domain;


	class Program
	{

		static void Main()
		{
			Console.WindowWidth = 120;

			var lng = new DefaultLocalizationTypesSource();

			var entities = Domain.CreateEntityInfos(lng, typeof(Entity2), typeof(Entity3), typeof(Entity3D));
			Domain.ConfigEntityInfos(entities);

			var df = entities.By(a => a.EntityName == "Order_TotalByIssueDate");


			df.DbProperties.ForEach(a=> Console.WriteLine(a.Name));

			//entities.Where(a => a.IsDomainFunction).ForEach(a => Console.WriteLine($"{a} {a.AllDbProperties.Length}"));


			//var tags = SemanticCodeGenerator.GetCodeForTypeScript(typeof(Entity3).GetProperty("Name"), lng);
			//Console.WriteLine(tags.Join("\r\n"));

			////var loc = typeof(OrderItemLinkType).GetMember("ProductData").One().Localization(lng);
			////Console.WriteLine(loc != null);

			//var entities = Domain.CreateEntityInfos(lng, typeof(Entity2), typeof(Entity3), typeof(Entity3D));
			//Domain.ConfigEntityInfos(entities);

			//var actions = entities.By(typeof(Payment)).EntityActions;

			//var tags = SemanticCodeGenerator.GetCodeForTypeScript(actions.One().Member, lng);

			//Console.WriteLine(tags.Join("\r\n"));

			//var props = entities.By(typeof(Accommodation)).NavigationProperties;
			//Console.WriteLine(props.Select(a => a.Name).Join(", "));

			//			var setup = SemanticSetup.Invoke<AviaDocument>();
			//			Console.WriteLine(setup.PropertyByName("Producer").Localization(lng).Default);
			//			Console.WriteLine(setup.PropertyByName("ReissueFor").Localization(lng).Default);


			//
			//			var l = typeof(EverydayProfitReport).GetProperty("Product").Localization(lng);
			//			Console.WriteLine(l.Default);

			//var enumInfo = typeof(ProductType).GetMember("AviaTicket").One();
			//var loc = enumInfo.Localization(lng);
			//var icon = enumInfo.Semantic<IconAttribute>() ?? loc.SourceMember.As(a => a.Semantic<IconAttribute>());
			//Console.WriteLine(icon.IconName);


			//SemanticSetup.Invoke<ProductSummary>();

			//			var setup = SemanticSetup.Invoke<ProductTotalByYear>();
			//			if (setup != null)
			//			{
			//				foreach (var setupProp in setup.Properties)
			//				{
			//					foreach (var prop in setupProp.Properties.Where(a => a.Name == "Total"))
			//					{
			//						var tags = SemanticCodeGenerator.GetCodeForTypeScript(setupProp, prop, lng);
			//					}
			//				}
			//			}



			//			var bus = entities.By(typeof(BusTicket));
			//
			//			var props = bus.EntityType
			//				.GetProperties(BindingFlags.Instance | BindingFlags.Public);

			//using (var db = new Domain())
			//{
			//	db.Database.Log = Console.WriteLine;

			//	var org = (
			//		from a in db.Organizations
			//		where a.Id == "06381928063242a493a839f67756efe6"
			//		select new { a.Id, a.Name }
			//	).Take(20).ToList();

			//	//var r = new Isic
			//	//{
			//	//	Name = "Product1",
			//	//	EqualFare = new Money("UAH", 8),
			//	//	ServiceFee = new Money("UAH", 2),
			//	//	Number1 = "111",
			//	//	Number2 = "1",
			//	//};

			//	//try
			//	//{
			//	//	db.Commit(() => r.Save(db));
			//	//}
			//	//catch (Exception ex)
			//	//{
			//	//	Console.WriteLine(ex.FullMessage());
			//	//}

			//	//Console.WriteLine(r.Id + " - " + r.Total.Amount + r.Total.CurrencyId);
			//}

			//Console.WriteLine("Press any key");
			//Console.ReadKey();
		}

	}

}
