using System.Linq;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class ProfileDto : EntityContract
	{

		public Person.Reference Person { get; set; }

		public string Login { get; set; }

		public string Roles { get; set; }

		public GdsAgentDto[] GdsAgents { get; set; }

	}


	public partial class ProfileContractService : EntityContractService<User, User.Service, ProfileDto>
	{
		public ProfileContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Person = r.Person;
				c.Login = r.Name;
				c.Roles = r.Roles;
				c.GdsAgents = db.GdsAgent.ListByPersonId(r.Person.Id).Select(a => dc.GdsAgent.New(a)).ToArray();
			};

			EntityFromContract += (r, c) =>
			{
				throw new System.NotImplementedException();
			};
		}
	}

}