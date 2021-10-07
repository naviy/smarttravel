namespace Luxena.Base.Web.Mail
{
	public abstract class MailSenderAbstract : IMailSender
	{
		public abstract void Send(EmailArgs args);
	}
}