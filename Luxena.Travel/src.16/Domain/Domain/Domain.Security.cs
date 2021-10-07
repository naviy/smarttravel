using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;

using Luxena.Domain;

using NLog;


namespace Luxena.Travel.Domain
{

	partial class Domain
	{

		public User User
		{
			get
			{
				if (_exception != null) throw _exception;
				return _user;
			}
		}
		private User _user;

		public User SafeUser => _user;

		public string UserName => _user?.Name;

		public bool UserIsValid => _user != null && _user.Active;


		public static Func<string> GetIdentityName;


		public override void InitSecurity()
		{
			if (_identity != null)
				return;

			if (GetIdentityName == null)
				throw new Exception("InitSecurity: field GetIdentityName must been set.");

			var identityName = GetIdentityName();

			if (identityName.No())
			{
				_exception = new SecurityException("User is not authenticated");
				return;
			}

			if (identityName == "SYSTEM")
			{
				_identity = new InternalIdentity { Name = "SYSTEM" };
				return;
			}

			var token = AuthenticationToken.Parse(identityName);

			if (token == null)
			{
				_exception = new SecurityException("Invalid authentication token");
			}
			else
			{
				var user = Users.ByName(token.UserName);

				if (user == null)
				{
					_exception = new SecurityException($"User account '{token.UserName}' is not registered");

					Warn(_exception.Message);
				}
				//TODO: востановить при полном переходе на эту версию сайта
				/*else if (token.SessionId.Yes() && user.SessionId != token.SessionId)
				{
					_exception = new SecurityException("Session expired");

					Trace(_exception.Message);
				}*/
				else
				{
					_user = user;
				}
			}

			_identity = _user;
		}


		public string Login(string userName, string password)
		{
			var user = Users.By(a => a.Name == userName && a.Active);

			if (user == null || !user.ValidatePassword(password))
				return null;

			Commit(() =>
			{
				user.SessionId = Guid.NewGuid().ToString("N");
				user.Save(db);
			});

			return user.SessionId;
		}


		private Exception _exception;
		private object _identity;

	}

}
