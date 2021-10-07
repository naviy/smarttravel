using System;


namespace Luxena.Travel.Domain
{

	public enum UserRole
	{
		[RU("Все")]
		Everyone,

		[RU("Администратор")]
		Administrator,

		[RU("Супервизор")]
		Supervisor,

		[RU("Агент")]
		Agent,

		[RU("Кассир")]
		Cashier,

		[RU("Аналитик")]
		Analyst,

		[RU("Субагент")]
		SubAgent,
	}


	public class GenericPrivilegesExtAttribute : GenericPrivilegesAttribute
	{

		public UserRole List2
		{
			get { throw new NotImplementedException(); }
			set { List = GetUserRolesByMin(value); }
		}

		public UserRole Create2
		{
			get { throw new NotImplementedException(); }
			set { Create = GetUserRolesByMin(value); }
		}

		public UserRole Copy2
		{
			get { throw new NotImplementedException(); }
			set { Copy = GetUserRolesByMin(value); }
		}

		public UserRole Update2
		{
			get { throw new NotImplementedException(); }
			set { Update = GetUserRolesByMin(value); }
		}

		public UserRole Delete2
		{
			get { throw new NotImplementedException(); }
			set { Delete = GetUserRolesByMin(value); }
		}

		public UserRole Replace2
		{
			get { throw new NotImplementedException(); }
			set { Replace = GetUserRolesByMin(value); }
		}


		protected object[] GetUserRolesByMin(UserRole role)
		{
			if (role == UserRole.Administrator)
				return new object[] { UserRole.Administrator, };

			if (role == UserRole.Supervisor)
				return new object[] { UserRole.Administrator, UserRole.Supervisor, };

			return new object[] { UserRole.Administrator, UserRole.Supervisor, UserRole.Agent, };
		}

	}



	public class AdminOnlyPrivilegesAttribute : GenericPrivilegesExtAttribute
	{
		public AdminOnlyPrivilegesAttribute()
		{
			List2 = UserRole.Administrator;
			Create2 = UserRole.Administrator;
			Update2 = UserRole.Administrator;
			Delete2 = UserRole.Administrator;
			Replace2 = UserRole.Administrator;
		}
	}

	public class AdminPrivilegesAttribute : GenericPrivilegesExtAttribute
	{
		public AdminPrivilegesAttribute()
		{
			List2 = UserRole.Supervisor;
			Create2 = UserRole.Administrator;
			Update2 = UserRole.Administrator;
			Delete2 = UserRole.Administrator;
			Replace2 = UserRole.Administrator;
		}
	}

	public class AgentPrivilegesAttribute : GenericPrivilegesExtAttribute
	{
		public AgentPrivilegesAttribute()
		{
			List2 = UserRole.Agent;
			Create2 = UserRole.Agent;
			Update2 = UserRole.Agent;
			Delete2 = UserRole.Agent;
			Replace2 = UserRole.Supervisor;
		}
	}

	public class SupervisorPrivilegesAttribute : GenericPrivilegesExtAttribute
	{
		public SupervisorPrivilegesAttribute()
		{
			List2 = UserRole.Supervisor;
			Create2 = UserRole.Supervisor;
			Update2 = UserRole.Supervisor;
			Delete2 = UserRole.Supervisor;
			Replace2 = UserRole.Supervisor;
		}
	}

}