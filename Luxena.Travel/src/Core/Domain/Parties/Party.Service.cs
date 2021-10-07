using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Metamodel;
using Luxena.Base.Serialization;
using Luxena.Travel.Export;


namespace Luxena.Travel.Domain
{

	partial class Party
	{
		
		public class Service<TParty> : Entity3Service<TParty>
			where TParty : Party
		{

			protected Service()
			{
				Modifing += r =>
				{
					db.OnCommit(r, Export);
				};

			}

			public void Export(TParty r)
			{
				db.Resolve<IPartyExporter>().Do(a =>
					db.Try(() => a.Export(db.Unproxy(r)))
				);
			}

			public TParty ByLegalName(string legalName)
			{
				return legalName.No() ? null : By(p => p.LegalName == legalName);
			}

			public override OperationStatus CanReplace()
			{
				return db.Granted(UserRole.Administrator, UserRole.Supervisor);
			}

		}


		public partial class Service : Service<Party>
		{

			public Party CreateCustomer(string name, Type type)
			{
				var party = ByName(name);

				if (party != null)
					party.IsCustomer = true;
				else
				{
					party = type.GetClass().CreateInstance<Party>();

					party.Name = name;
					party.IsCustomer = true;

					db.Save(party);
				}

				return party;
			}

			public object CreateCustomer(string type, string name)
			{
				var customer = CreateCustomer(name, Class.Of(type).Type);

				db.Flush();

				var result = new ObjectSerializer { Properties = new List<Property>() }.Serialize(customer);

				return result;
			}


			public File AddFile(Party party, string fileName, byte[] content)
			{
				if (party == null) return null;

				var file = new File
				{
					FileName = fileName,
					Content = content,
					TimeStamp = DateTime.Now.AsUtc(),
					UploadedBy = db.Security.Person,
					Party = party
				};

				party.Files.Add(file);

				db.Save(file);

				return file;
			}


			public int Replace(object fromId, object toId)
			{
				return 0;
			}

//			public RangeResponse SuggestCustomers(RangeRequest prms)
//			{
//				var filters = prms.Filters;
//
//				PropertyFilter filter = null;
//
//				if (prms.Filters != null)
//					filter = prms.Filters.By(a => a.Property == "IsCustomer");
//
//				if (filter == null)
//				{
//					filter = new PropertyFilter { Property = "IsCustomer" };
//					prms.Filters = prms.Filters.AsConcat(filter).ToArray();
//				}
//
//				if (filter.Conditions == null || filter.Conditions.Length != 1 ||
//					filter.Conditions[0].Operator != FilterOperator.Equals || !(bool)filter.Conditions[0].Value)
//				{
//					var condition = new PropertyFilterCondition { Operator = FilterOperator.Equals, Value = true };
//					filter.Conditions = new[] { condition };
//				}
//
//				var result = Suggest(prms);
//				if (result.TotalCount != 0) 
//					return result;
//
//				prms.Filters = filters;
//				result = Suggest(prms);
//
//				return result;
//			}
			
		}

	}


	public class PartyReplaceParams
	{
		[RU("Заменяем контрагент")]
		public Party FromParty { get; set; }

		[RU("на")]
		public Party ToParty { get; set; }
	}


	//public partial class PartyManager
	//{

	//	public override Permissions GetCustomPermissions()
	//	{
	//		return new Permissions
	//		{
	//			{ "	", db.Granted(UserRole.Administrator, UserRole.Supervisor) },
	//		};
	//	}

	//}

}
