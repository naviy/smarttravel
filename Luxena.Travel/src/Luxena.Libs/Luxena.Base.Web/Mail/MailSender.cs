using System.Net.Mail;
using System.Text;


namespace Luxena.Base.Web.Mail
{
	public class MailSender : MailSenderAbstract
	{
		public string Host { get; set; }

		public int Port { get; set; }

		public string User { get; set; }

		public string Password { get; set; }

		public bool UseSsl { get; set; }

		public override void Send(EmailArgs args)
		{
			MailMessage message = new MailMessage();

			foreach (string email in args.Recepients)
				message.To.Add(email);

			foreach (string email in args.BccRecepients)
				message.Bcc.Add(email);

			message.From = new MailAddress(args.EmailFrom, args.EmailFromFriendly);
			message.Subject = args.Subject;

			if (args.IsBodyHtml)
				message.IsBodyHtml = true;

			message.Body = args.Body;

			message.SubjectEncoding = Encoding.GetEncoding(args.Charset);
			message.BodyEncoding = Encoding.GetEncoding(args.Charset);

			message.Priority = args.Priority;

			foreach (Attachment file in args.Attachments)
				message.Attachments.Add(file);

			message.Headers.Add("Organization", "Luxena Software Company");

			SmtpClient smtpServer = new SmtpClient(Host, Port);
			smtpServer.Credentials = new System.Net.NetworkCredential(User, Password);
			smtpServer.EnableSsl = UseSsl;

			smtpServer.Send(message);
		}
	}
}