using System;
using System.Runtime.CompilerServices;


namespace LxnBase
{
	public class MessageRegisterEventArgs : EventArgs
	{
		[AlternateSignature]
		public extern MessageRegisterEventArgs(MessageType type, string message, string details);

		[AlternateSignature]
		public extern MessageRegisterEventArgs(MessageType type, string message, string details, string messageCaption);

		public MessageRegisterEventArgs(MessageType type, string message, string details, string messageCaption, bool handled)
		{
			_messageCaption = messageCaption;
			_message = message;
			_details = details;
			_type = type;
			_handled = handled;
		}

		public MessageType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public string MessageCaption
		{
			get { return _messageCaption; }
			set { _messageCaption = value; }
		}

		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		public string Details
		{
			get { return _details; }
			set { _details = value; }
		}

		public bool Handled
		{
			get { return _handled; }
			set { _handled = value; }
		}

		private string _messageCaption;
		private string _message;
		private string _details;
		private MessageType _type;
		private bool _handled;
	}
}