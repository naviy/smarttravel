using System.Linq;
using System.Web.Http;

using Luxena.Domain;
using Luxena.Domain.Contracts;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Controllers
{

	public class OrganizationsController : EntityApiController<Organization, Organization.Service>
	{

		public override object List(RangeRequest request)
		{
			//Thread.Sleep(3000);
			request.TextSearchMembers<Organization>(a => new
			{
				a.Name,
				a.LegalName,
				a.Code,
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
					r.Code,
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
				ReportsTo = r.ReportsTo.ToReference(),
				r.Code,

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
				r.Code,
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
			}, true, (db, c, r) =>
			{
				r.WebAddress = c.WebAddress.TrimStart("http://");
			});
		}

		public class PersonListArgs
		{
			public string OrganizationId { get; set; }
		}

		[HttpPost]
		public object PersonList(PersonListArgs e)
		{
			return Result(Domain.Person, q => q
				.Where(r => r.Organization.Id == e.OrganizationId)
				.AsEnumerable()
				.Select(r => new
				{
					r.Id,
					Name = r.NameForDocuments,
					Contacts = r.GetContacts()
				})
			);
		}

	}

}