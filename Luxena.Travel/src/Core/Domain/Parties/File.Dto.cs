using System;

using Luxena.Base.Data;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class FileDto : EntityContract
	{

		public string FileName { get; set; }

		public DateTime TimeStamp { get; set; }

		public EntityReference UploadedBy { get; set; }

		public EntityReference Party { get; set; }

	}


	public partial class FileContractService : EntityContractService<File, File.Service, FileDto>
	{
		public FileContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.FileName = r.FileName;
				c.TimeStamp = r.TimeStamp;
				c.UploadedBy = r.UploadedBy;
				c.Party = r.Party;
			};

			EntityFromContract += (r, c) =>
			{
				throw new NotImplementedException();
			};
		}

		public UploadFileResponse Upload(string partyId, string fileName, byte[] content)
		{
			UploadFileResponse result;

			try
			{
				if (content != null && fileName != null)
				{
					result = new UploadFileResponse
					{
						success = true,
						File = New(db.Party.AddFile(db.Party.By(partyId), fileName, content))
					};
				}
				else
				{
					result = new UploadFileResponse
					{
						success = false,
						ErrorMessage = Exceptions.PartyFileUpload_FileNameOrContentMissing
					};
				}
			}
			catch (Exception e)
			{
				db.Error("Saving uploaded file", e);

				string message;

				if (e.Message.Contains("could not insert"))
					message = Exceptions.FileMaximumLengthExceeded;
				else if (e.Message.Contains("Maximum request length exceeded"))
					message = Exceptions.FileMaximumLengthExceeded;
				else
					message = db.Translate(e).Message;

				result = new UploadFileResponse { success = false, ErrorMessage = message };
			}

			return result;
		}

	}

}