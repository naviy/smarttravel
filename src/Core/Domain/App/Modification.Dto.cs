using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class ModificationDto : EntityContract
	{

		public DateTime TimeStamp { get; set; }

		public string Author { get; set; }

		public int Type { get; set; }

		public string InstanceType { get; set; }

		public object InstanceId { get; set; }

		public string InstanceString { get; set; }

		public string Comment { get; set; }

		public string ItemsJson { get; set; }
	}


	public partial class ModificationContractService : EntityContractService<Modification, Modification.Service, ModificationDto>
	{

		public ModificationContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.TimeStamp = r.TimeStamp;
				c.Author = r.Author;
				c.Type = (int)r.Type;
				c.InstanceType = r.InstanceType;
				c.InstanceId = r.InstanceId;
				c.InstanceString = r.InstanceString;
				c.Comment = r.Comment;
				c.ItemsJson = r.ItemsJson;
			};
		}

	}

}