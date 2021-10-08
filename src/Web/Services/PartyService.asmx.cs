using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class PartyService : DomainWebService
	{

		[WebMethod]
		public DepartmentDto GetDepartment(object id)
		{
			return db.Commit(() => dc.Department.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateDepartment(DepartmentDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Department.Update(dto, @params));
		}


		[WebMethod]
		public OrganizationDto GetOrganization(object id)
		{
			return db.Commit(() => dc.Organization.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateOrganization(OrganizationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Organization.Update(dto, @params));
		}


		[WebMethod]
		public PersonDto GetPerson(object id)
		{
			return db.Commit(() => dc.Person.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePerson(PersonDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Person.Update(dto, @params));
		}



		[WebMethod]
		public RangeResponse SuggestCustomers(RangeRequest @params)
		{
			return db.Commit(() => db.Customer.Suggest(@params));
		}

		[WebMethod]
		public RangeResponse SuggestUsers(RangeRequest @params)
		{
			return db.Commit(() => db.User.Suggest(@params));
		}

		[WebMethod]
		public object CreateCustomer(string type, string name)
		{
			return db.Commit(() => db.Party.CreateCustomer(type, name));
		}

		[WebMethod]
		public int Replace(string fromId, string toId)
		{
			return db.Commit(() => db.Party.Replace(fromId, toId));
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
		public void UploadFile()
		{
			db.Commit(() => 
				SetJsonResponse(dc.File.Upload(GetPartyId(), GetFileName(), GetFileBytes()))
			);
		}

		[WebMethod]
		public void DeleteFile(object id)
		{
			db.Commit(() => db.File.Delete(id));
		}

		[WebMethod]
		public byte[] GetFile(object id)
		{
			return db.Commit(() => db.File.ContentBy(id));
		}


		private static byte[] GetFileBytes()
		{
			var file = HttpContext.Current.Request.Files["uploadedFile"];

			if (file == null)
				return null;

			var stream = file.InputStream;

			var originalPosition = stream.Position;

			stream.Position = 0;

			try
			{
				var readBuffer = new byte[4096];

				var totalBytesRead = 0;

				int bytesRead;

				while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
				{
					totalBytesRead += bytesRead;
					if (totalBytesRead != readBuffer.Length) continue;

					var nextByte = stream.ReadByte();
					if (nextByte == -1) continue;

					var temp = new byte[readBuffer.Length * 2];
					Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
					Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
					readBuffer = temp;
					totalBytesRead++;
				}

				var buffer = readBuffer;

				if (readBuffer.Length != totalBytesRead)
				{
					buffer = new byte[totalBytesRead];
					Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
				}
				return buffer;
			}
			finally
			{
				stream.Position = originalPosition;
			}
		}

		private static string GetFileName()
		{
			if (HttpContext.Current.Request.Files["uploadedFile"] != null)
			{
				var filePath = HttpContext.Current.Request.Files["uploadedFile"].FileName.Split('\\');

				return filePath[filePath.Length - 1];
			}

			return null;
		}

		private static string GetPartyId()
		{
			return HttpContext.Current.Request.Params["partyId"];
		}

		private static void SetJsonResponse(object response)
		{
			// text/html is required by browser to handle upload response correctly
			// DO NOT!!! change to application/json
			HttpContext.Current.Response.ContentType = "text/html; charset=utf-8";
			HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
			HttpContext.Current.Response.Flush();

			var javaScriptSerializer = new JavaScriptSerializer();

			HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(response));
		}

	}

}
