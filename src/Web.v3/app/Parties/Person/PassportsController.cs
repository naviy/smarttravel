using Luxena.Domain.Contracts;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Controllers
{

	public class PassportsController : EntityApiController<Passport, Passport.Service>
	{

		protected override EntityContractService NewEditContract()
		{
			return NewContract((db, r) => new
			{
				r.Id,
				r.CreatedOn,
				r.ModifiedOn,

				Owner = r.Owner.ToReference(),

				r.Number,

				r.FirstName,
				r.MiddleName,
				r.LastName,

				r.Note,
			},
			true,
			(db, m, r) =>
			{
			},
			(db, prms) => new
			{
				Owner = prms.By("personId").As(a => db.Person[a].ToReference()),
			});
		}

	}

}