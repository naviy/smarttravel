using System.Web.Services;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class UserService : DomainWebService
	{

		[WebMethod]
		public ProfileDto GetUserProfile(object id)
		{
			return db.Commit(() => dc.Profile.By(id));
		}

		[WebMethod]
		public bool ChangeUserPassword(string oldPassword, string newPassword)
		{
			return db.Commit(() => db.User.ChangePassword(oldPassword, newPassword));
		}

	}

}
