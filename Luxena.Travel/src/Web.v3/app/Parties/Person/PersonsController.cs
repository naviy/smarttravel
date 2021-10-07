using System.Linq;
using System.Web.Http;

using Luxena.Domain;
using Luxena.Domain.Contracts;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Controllers
{

	public class PersonsController : EntityApiController<Person, Person.Service>
	{

		public override object List(RangeRequest request)
		{
			//Thread.Sleep(3000);
			request.TextSearchMembers<Person>(a => new
			{
				a.Name,
				a.LegalName,
				a.Phone1,
				a.Phone2,
				a.Fax,
				a.Email1,
				a.Email2,
				a.WebAddress,
				a.Note,
			});

			return Range(request, q => q.AsEnumerable()
				.Select(r => new
				{
					r.Id,
					Name = r.NameForDocuments,
					Organization = r.Organization.ToReference(),
					ReportsTo = r.ReportsTo.ToReference(),
					Note = r.Note.Ellipsis(135),
					Contacts = r.GetContacts(),
				})
				.ToList()
			);
		}

		[HttpGet]
		public object Suggest(string name)
		{
			return Suggest(q =>
				from r in q
				where r.NameForDocuments.Contains(name)
				orderby r.NameForDocuments
				select r.ToReference()
			);
		}


		protected override EntityContractService NewViewContract()
		{
			return NewContract((db, r) => new
			{
				r.Id,
				Name = r.NameForDocuments,
				Organization = r.Organization.ToReference(),
				ReportsTo = r.ReportsTo.ToReference(),

				r.IsCustomer,
				r.IsSupplier,

				Contacts = r.GetContacts(),

				r.LegalAddress,
				r.ActualAddress,

				r.Note,
			});
		}

		protected override EntityContractService NewEditContract()
		{
			return NewContract((db, r) => new
			{
				r.Id,
				r.CreatedOn,
				r.ModifiedOn,
				r.Name,
				r.LegalName,
				Organization = r.Organization.ToReference(),
				ReportsTo = r.ReportsTo.ToReference(),
				r.IsCustomer,
				r.IsSupplier,
				r.Phone1,
				r.Phone2,
				r.Email1,
				r.Email2,
				r.Fax,
				WebAddress = r.WebAddress.Clip().As(a => a.StartsWith("http://") ? a : "http://" + a),
				r.LegalAddress,
				r.ActualAddress,
				r.Note,
			},
			true,
			(db, m, r) =>
			{
				r.WebAddress = m.WebAddress.TrimStart("http://");
			},
			(db, prms) => new
			{
				Organization = prms.By("organizationId").As(a => db.Organization[a].ToReference()),
			});
		}

		[HttpPost]
		public void RemoveFromOrganization(string id)
		{
			Domain.Commit(() =>
			{
				var r = Service.By(id);
				r.Organization = null;
				Service.Save(r);
			});
		}


		public class PassportListArgs
		{
			public string PersonId { get; set; }
		}

		[HttpPost]
		public object PassportList(PassportListArgs e)
		{
			return Result(Domain.Passport, q =>
				from r in q
				where r.Owner.Id == e.PersonId
				select new
				{
					r.Id,
					r.Number,
					r.Name,
					//Citizenship = r.Citizenship.ToReference(),
					r.Note
				}
			);
		}


	}

}