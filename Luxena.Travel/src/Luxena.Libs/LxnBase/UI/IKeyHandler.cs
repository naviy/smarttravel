using jQueryApi;


namespace LxnBase.UI
{
	public interface IKeyHandler
	{
		bool HandleKeyEvent(jQueryEvent keyEvent);

		void RestoreFocus();
	}
}