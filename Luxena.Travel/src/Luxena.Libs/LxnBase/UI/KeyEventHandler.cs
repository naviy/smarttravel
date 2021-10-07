using System.Html;


namespace LxnBase.UI
{
	public class KeyEventHandler
	{
		public KeyEventHandler(IKeyHandler handler, bool allowBubbling)
		{
			_handler = handler;
			_allowBubbling = allowBubbling;
		}

		public IKeyHandler Handler
		{
			get { return _handler; }
			set { _handler = value; }
		}

		public bool AllowBubbling
		{
			get { return _allowBubbling; }
			set { _allowBubbling = value; }
		}

		public Element FocusedElement
		{
			get { return _focusedElement; }
			set { _focusedElement = value; }
		}

		private IKeyHandler _handler;
		private bool _allowBubbling;
		private Element _focusedElement;
	}
}