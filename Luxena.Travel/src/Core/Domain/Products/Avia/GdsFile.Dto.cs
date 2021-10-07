using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class GdsFileDto : Entity3Contract
	{

		public int FileType { get; set; }

		public DateTime TimeStamp { get; set; }

		public string Content { get; set; }

		public int ImportResult { get; set; }

		public string ImportOutput { get; set; }

	}


	public partial class GdsFileContractService : Entity3ContractService<GdsFile, GdsFile.Service, GdsFileDto>
	{
		public GdsFileContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.FileType = (int)r.FileType;
				c.TimeStamp = r.TimeStamp;
				c.Content = r.Content;
				c.ImportResult = (int)r.ImportResult;
				c.ImportOutput = r.ImportOutput;
			};

			EntityFromContract += (r, c) =>
			{
				throw new NotImplementedException();
			};
		}
	}

}