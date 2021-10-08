using System.Runtime.CompilerServices;


namespace LxnBase
{
	public delegate void MessageRegisterEventHandler(object sender, MessageRegisterEventArgs e);

	public static class MessageRegister
	{
		public static event MessageRegisterEventHandler NewMessage;

		[AlternateSignature]
		public static extern void Error(string message);

		[AlternateSignature]
		public static extern void Error(string message, string details);

		public static void Error(string message, string details, bool handled)
		{
			if (NewMessage != null)
				NewMessage(null, new MessageRegisterEventArgs(MessageType.Error, message, details, null, handled));
		}

		[AlternateSignature]
		public static extern void Warn(string message);

		public static void Warn(string message, string details)
		{
			if (NewMessage != null)
				NewMessage(null, new MessageRegisterEventArgs(MessageType.Warn, message, details));
		}

		[AlternateSignature]
		public static extern void Info(string messageCaption);

		[AlternateSignature]
		public static extern void Info(string messageCaption, string message);

		public static void Info(string messageCaption, string message, string details)
		{
			if (NewMessage != null)
				NewMessage(null, new MessageRegisterEventArgs(MessageType.Info, message, details, messageCaption));
		}
	}
}