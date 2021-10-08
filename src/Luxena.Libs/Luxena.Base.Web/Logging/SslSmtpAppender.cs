using System.Net.Mail;

using log4net.Appender;


namespace Luxena.Base.Web.Logging
{
	public class SslSmtpAppender : SmtpAppender
	{
		public bool EnableSsl { get; set; }

		protected override void SendEmail(string messageBody)
		{
			var smtpClient = new SmtpClient();

			if (!string.IsNullOrEmpty(SmtpHost))
			{
				smtpClient.Host = SmtpHost;
			}

			smtpClient.Port = Port;

			smtpClient.EnableSsl = EnableSsl;

			smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

			if (Authentication == SmtpAuthentication.Basic)
				smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
			else if (Authentication == SmtpAuthentication.Ntlm)
				smtpClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;

			var mailMessage = new MailMessage
			{
				From = new MailAddress(From),
				Subject = Subject,
				Body = messageBody
			};

			mailMessage.To.Add(To);

			smtpClient.Send(mailMessage);
		}
	}
}