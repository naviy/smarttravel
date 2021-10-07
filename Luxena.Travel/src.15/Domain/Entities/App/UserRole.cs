using System;
using System.Diagnostics;


namespace Luxena.Travel.Domain
{

	[Flags]
	public enum UserRole
	{
		None = 0,

		[RU("Все")]
		Everyone = 1,

		[RU("Администратор")]
		Administrator = 2,

		[RU("Супервизор")]
		Supervisor = 4,

		[RU("Агент")]
		Agent = 8,

		[RU("Кассир")]
		Cashier = 16,

		[RU("Аналитик")]
		Analyst = 32,

		[RU("Субагент")]
		SubAgent = 64,
	}


	[AttributeUsage(AttributeTargets.Class)]
	public class GenericPrivilegesAttribute : Attribute
	{
		public object[] List { get; set; }
		public object[] Create { get; set; }
		public object[] Copy { get; set; }
		public object[] View { get; set; }
		public object[] Update { get; set; }
		public object[] Delete { get; set; }
		public object[] Replace { get; set; }

		public UserRole List2
		{
			[DebuggerStepThrough]
			get { throw new NotImplementedException(); }
			[DebuggerStepThrough]
			set { List = GetUserRolesByMin(value); }
		}

		public UserRole Create2
		{
			[DebuggerStepThrough]
			get { throw new NotImplementedException(); }
			[DebuggerStepThrough]
			set { Create = GetUserRolesByMin(value); }
		}

		public UserRole Copy2
		{
			[DebuggerStepThrough]
			get { throw new NotImplementedException(); }
			[DebuggerStepThrough]
			set { Copy = GetUserRolesByMin(value); }
		}

		public UserRole Update2
		{
			[DebuggerStepThrough]
			get { throw new NotImplementedException(); }
			[DebuggerStepThrough]
			set { Update = GetUserRolesByMin(value); }
		}

		public UserRole Delete2
		{
			[DebuggerStepThrough]
			get { throw new NotImplementedException(); }
			[DebuggerStepThrough]
			set { Delete = GetUserRolesByMin(value); }
		}

		public UserRole Replace2
		{
			[DebuggerStepThrough]
			get { throw new NotImplementedException(); }
			[DebuggerStepThrough]
			set { Replace = GetUserRolesByMin(value); }
		}


		[DebuggerStepThrough]
		protected object[] GetUserRolesByMin(UserRole role)
		{
			if (role == UserRole.None)
				return new object[0];

			if (role == UserRole.Administrator)
				return new object[] { UserRole.Administrator, };

			if (role == UserRole.Supervisor)
				return new object[] { UserRole.Administrator, UserRole.Supervisor, };

			return new object[] { UserRole.Administrator, UserRole.Supervisor, UserRole.Agent, };
		}

	}



	public class AdminPrivilegesAttribute : GenericPrivilegesAttribute
	{
		[DebuggerStepThrough]
		public AdminPrivilegesAttribute()
		{
			List2 = UserRole.Supervisor;
			Create2 = UserRole.Administrator;
			Update2 = UserRole.Administrator;
			Delete2 = UserRole.Administrator;
			Replace2 = UserRole.Administrator;
		}
	}

	public class AgentPrivilegesAttribute : GenericPrivilegesAttribute
	{
		[DebuggerStepThrough]
		public AgentPrivilegesAttribute()
		{
			List2 = UserRole.Agent;
			Create2 = UserRole.Agent;
			Update2 = UserRole.Agent;
			Delete2 = UserRole.Agent;
			Replace2 = UserRole.Supervisor;
		}
	}

	public class SupervisorPrivilegesAttribute : GenericPrivilegesAttribute
	{
		[DebuggerStepThrough]
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