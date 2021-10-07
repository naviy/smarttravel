using System.Collections.Generic;

using Luxena.Base.Data;
using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{

	[DataContract]
	public class AppParameters
	{
		public EntityReference UserPerson { get; set; }

		public EntityReference CurrentUser { get; set; }

		public Dictionary<string, OperationPermissions> AllowedActions { get; set; }

		public object MainPageSettings { get; set; }
		
		public SystemConfigurationDto SystemConfiguration { get; set; }

		public Party.Reference[] Departments { get; set; }

		public BankAccount.Reference[] BankAccounts { get; set; }

		public string Version { get; set; }
	}

}