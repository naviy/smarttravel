using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class User
	{
		public new class Service : Service<User>
		{

			public User By(string login)
			{
				return By(a => a.Name == login && a.Active);
			}

			public User By(string login, string password)
			{
				return By(login).If(a => a.ValidatePassword(password));
			}

			public IList<User> ListBy(Person person)
			{
				return person == null ? EmptyList : ListBy(a => a.Person == person && a.Active);
			}


			public Service()
			{
				Modifing += r =>
				{
					if (IsDirty(r, a => new { a.IsAdministrator, a.IsSupervisor, a.IsAgent, a.IsCashier, a.IsAnalyst, a.IsSubAgent, }))
						db.AppState.RegisterUserRolesChange(r);
				};
			}


			public string Login(string userName, string password)
			{
			    var user = By(userName, password);
			    if (user == null) return null;

			    user.SessionId = Guid.NewGuid().ToString("N");

			    return user.SessionId;
			}


		    public bool ChangePassword(string oldPassword, string newPassword)
			{
				var user = By(db.Security.User.Id);

				if (!user.ValidatePassword(oldPassword)) return false;

				//user.SetPassword(newPassword);
				user.Password = newPassword;
				Save(user);

				return true;
			}


			public override RangeResponse Suggest(RangeRequest prms)
			{
				return new RangeResponse(Query
					.Where(a => a.Active && a.Person.Name.Contains(prms.Query))
					.Select(a => a.Person)
					.AsEnumerable()
					.Select(Reference.ToArray)
					.ToArray()
				);
			}

		}
	}

}