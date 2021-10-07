namespace Luxena.Base.Web.Mail
{
	public interface IMailSender
	{
		void Send(EmailArgs args);
	}
}