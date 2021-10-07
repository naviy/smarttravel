using System.Collections.Generic;
using System.IO;
using System.Net.Mail;


namespace Luxena.Base.Web.Mail
{
	public class EmailArgs
	{
		public EmailArgs()
		{
			Priority = MailPriority.Normal;
			Charset = "utf-8";
		}

		public string EmailFrom { get; set; }
		public string EmailFromFriendly { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool IsBodyHtml { get; set; }
		public string Charset { get; set; }
		public MailPriority Priority { get; set; }

		public IList<string> Recepients
		{
			get { return _recepients; }
		}

		public IList<string> BccRecepients
		{
			get { return _bccRecepients; }
		}

		public IList<Attachment> Attachments
		{
			get { return _attachments; }
		}

		public void AddAttachment(Stream stream, string fileName)
		{
			var item = new Attachment(stream, fileName);

			_attachments.Add(item);
		}

		private readonly IList<string> _recepients = new List<string>();
		private readonly IList<string> _bccRecepients = new List<string>();
		private readonly IList<Attachment> _attachments = new List<Attachment>();
	}
}