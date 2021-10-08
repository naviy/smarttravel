using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;

using Common.Logging;

using Luxena.Base.Data;
using Luxena.Base.Managers;
using Luxena.Travel.Security;


namespace Luxena.Travel.Domain
{

	public class SecurityService : DomainService, ISecurityContext
	{
		public User User
		{
			get
			{
				if (_exception != null)
					throw _exception;
				return _user;
			}
		}
		private User _user;
		//public User SafeUser => _user;

		public string UserName => _user?.Name;


		public Person Person { get { return User.As(a => a.Person); } }

		public bool IsValid => _user != null && _user.Active;

		public bool IsSystem { get; private set; }


		#region Initialize

		public override void Init()
		{
			base.Init();

			if (db.GetIdentityName == null)
				throw new Exception("SecurityService: field GetIdentityName must been set.");

			Initialize(db.GetIdentityName());
		}

		protected void Initialize(string identityName)
		{
			if (identityName.No())
			{
				_exception = new SecurityException("User is not authenticated");
				return;
			}

			IsSystem = identityName == "SYSTEM";
			if (IsSystem)
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
				db.Commit(() =>
				{
					var user = db.User.By(token.UserName);

					if (user == null)
					{
						_exception = new SecurityException($"User account '{token.UserName}' is not registered");

						_log.Warn(_exception.Message);
					}
					//else if (user.SessionId != token.SessionId)
					// !!! TODO: remove this shit - force clients to use Galileo server for MIR files export
					else if (token.SessionId.Yes() && user.SessionId != token.SessionId)
					{
						_exception = new SecurityException("Session expired");

						_log.Trace(_exception.Message);
					}
					else
					{
						_user = user;
					}
				});
			}

			_identity = _user;
		}

		#endregion


		#region IsGranted

		public bool IsGranted(IEnumerable<UserRole> priveleges)
		{
			return IsSystem || priveleges == null || priveleges.Any(IsGranted);
		}

		public bool IsGranted(params UserRole[] priveleges)
		{
			return IsGranted((IEnumerable<UserRole>)priveleges);
		}

		public bool IsGranted(UserRole priveledge)
		{
			if (IsSystem)
				return true;

			if (!IsValid)
				return false;

			if (User.IsAdministrator)
				return true;

			switch (priveledge)
			{
				case UserRole.Everyone:
					return true;

				case UserRole.Supervisor:
					return User.IsSupervisor;

				case UserRole.Agent:
					return User.IsAgent;

				case UserRole.Cashier:
					return User.IsCashier;

				case UserRole.Analyst:
					return User.IsAnalyst;

				case UserRole.SubAgent:
					return User.IsSubAgent;
			}

			return false;
		}

		#endregion


		#region ISecurityContext

		public object Identity
		{
			get
			{
				if (_exception != null)
					throw _exception;
				return _identity;
			}
		}

		public bool IsGranted(params object[] priveleges)
		{
			return priveleges == null || IsGranted(priveleges.Cast<UserRole>());
		}

		public bool IsGranted(IEnumerable priveleges)
		{
			return priveleges == null || IsGranted(priveleges.Cast<UserRole>());
		}

		#endregion

		private static readonly ILog _log = LogManager.GetLogger(typeof(SecurityContext));
		private Exception _exception;
		private object _identity;
	}


	partial class Domain
	{

		#region IsGranted

		public bool IsGranted(params UserRole[] priveleges)
		{
			return Security.IsGranted(priveleges);
		}

		public bool IsGranted(IEnumerable<UserRole> priveleges)
		{
			return Security.IsGranted(priveleges);
		}

		public OperationStatus Granted(bool allow)
		{
			return allow ? OperationStatus.Enabled() : OperationStatus.Hidden();
		}

		public OperationStatus Granted(params UserRole[] priveleges)
		{
			return Granted(Security.IsGranted(priveleges));
		}

		#endregion

	}

}