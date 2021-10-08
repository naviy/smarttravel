using System;
using System.Collections;

using Ext;
using Ext.form;
using Ext.menu;

using jQueryApi;

using Action = Ext.Action;


namespace LxnBase.UI
{
	public delegate void ActionCallbackDelegate(Action[] actions);
	public delegate void FieldActionGetActionsDelegate(Field field, ActionCallbackDelegate callback);

	public class FieldActionManager
	{
		public FieldActionManager()
		{
			_menu = new Menu(new MenuConfig()
						.cls("simple-menu testMenu")
						.listeners(new Dictionary(
							"beforeshow", new MenuBeforeshowDelegate(delegate { _menu.tryActivate(0); })
						))
						.ToDictionary());
		}

		private jQueryObject ActionButton
		{
			get { return jQuery.Select(string.Format("#{0}", _actionButtonId)); }
		}

		public void Init(FieldNavigationManager navigationManager, Form form)
		{
			_navigationManager = navigationManager;

			navigationManager.FocusChanged += delegate { Refresh(); };

			form.on("beforedestroy", new AnonymousDelegate(delegate { _menu.destroy(); }));

			_actionButtonId = string.Format("{0}-field-action-button", form.id);

			_isInitialized = true;
		}

		public void Add(Field field, FieldActionGetActionsDelegate getActions)
		{
			field.on("changeValue", new AnonymousDelegate(Refresh));

			_fieldActions.Add(new FieldActions(field, getActions));
		}

		public void Refresh()
		{
			if (!_isInitialized || _fieldActions.Count == 0)
				return;

			HideActionButton();

			foreach (object fieldAction in _fieldActions)
			{
				FieldActions fieldActions = (FieldActions)fieldAction;

				if (fieldActions.Field == _navigationManager.Current)
				{
					fieldActions.GetActions(fieldActions.Field, 
						delegate(Action[] actions)
						{
							DisplayActions(fieldActions.Field, actions);
						});

					break;
				}
			}
		}

		public bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			if (keyEvent.Which != (int)Type.GetField(typeof(EventObject), "SPACE") || !keyEvent.CtrlKey || keyEvent.AltKey || keyEvent.ShiftKey)
				return false;

			if (ActionButton.Length == 0)
				return false;

			OnActionButtonClick();

			return true;
		}

		private void DisplayActions(Field field, Action[] actions)
		{
			if (actions != null && actions.Length > 0)
			{
				SetActions(actions);

				CreateActionButton(field);
			}
		}

		private void HideActionButton()
		{
			ActionButton.Remove();
		}

		private void CreateActionButton(Field field)
		{
			ActionButton.Remove();

			jQueryObject fieldElement = jQuery.Select("#" + field.id);

			jQueryObject button = jQuery.FromHtml("<div></div>")
				.Attribute("id", _actionButtonId)
				.AddClass("field-action");

			fieldElement.Parent().Append(button);

			int delta = fieldElement.GetOuterHeight() / 2 - button.GetOuterHeight() / 2;

			int top = fieldElement.Position().Top + delta;
			int left = fieldElement.Position().Left + fieldElement.GetOuterWidth() - button.GetOuterWidth() - delta;

			button.CSS("top", top + "px");
			button.CSS("left", left + "px");

			button.Click(delegate { OnActionButtonClick(); });
		}

		private void OnActionButtonClick()
		{
			if (ActionButton.Length == 0)
				return;

			_menu.show(ActionButton[0], "tr-br?");
		}

		private void SetActions(Action[] actions)
		{
			_menu.removeAll();

			for (int i = 0; i < actions.Length; i++)
			{
				Action action = actions[i];

				_menu.addMenuItem(new ItemConfig()
					.text((string)Type.InvokeMethod(action, "getText", null))
					.handler(new AnonymousDelegate(action.execute))
					.ToDictionary());
			}
		}

		private FieldNavigationManager _navigationManager;
		private readonly ArrayList _fieldActions = new ArrayList();
		private readonly Menu _menu;
		private string _actionButtonId;

		private bool _isInitialized;
	}
}