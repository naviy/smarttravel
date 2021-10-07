using System;
using System.Collections;
using System.Html;

using Ext;
using Ext.form;
using Ext.ux.grid;

using jQueryApi;

using Element = Ext.Element;
using Field = Ext.form.Field;


namespace LxnBase.UI
{
	public delegate void FormFieldFocusChangedDelegate(Component current);

	public class FieldNavigationManager
	{
		public static FieldNavigationManager Create(Component[] components)
		{
			return new FieldNavigationManager(components);
		}

		public event FormFieldFocusChangedDelegate FocusChanged;

		public FieldNavigationManager(Component[] components)
		{
			_controlSequence.AddRange(components);

			foreach (object control in _controlSequence)
				RegisterFocus((Component) control);
		}

		public Component Current { get { return _currentFocused; } }

		public bool HandleKeyEvent(jQueryEvent e)
		{
			if (e.Which != EventObject.ENTER || e.CtrlKey || e.AltKey)
				return false;

			if (e.Target.TagName.ToLowerCase() == "button" && !e.ShiftKey)
				return true;

			Component component = _currentFocused;

			if (component is TextArea && !e.ShiftKey)
				return true;

			if (component is Button)
			{
				if (e.ShiftKey)
				{
					e.StopPropagation();
					e.PreventDefault();

					Focus(Previous(component));
				}
				else
				{
					Element element = (Element) component.getEl().child(((Button) component).buttonSelector);

					if (Document.ActiveElement != element.dom)
						Focus(Previous(component));
				}
			}
			else
				ChangeFocus(e, component);

			return true;
		}

		public void RestoreFocus()
		{
			Focus(_currentFocused ?? (Component)_controlSequence[0]);
		}

		private void RegisterFocus(Component component)
		{
			AnonymousDelegate handler = 
				delegate
				{
					_currentFocused = component;

					OnFocusChanged();
				};

			if (component is Field)
				component.on("focus", handler);
			else if (component is Button)
			{
				Element btn = (Element) component.getEl().child(((Button) component).buttonSelector);
				btn.on("focus", handler);
			}
			else
				component.getEl().on("focus", handler);
		}

		private void ChangeFocus(jQueryEvent keyEvent, Component current)
		{
			bool noAdditionalKey = !keyEvent.ShiftKey && !keyEvent.CtrlKey && !keyEvent.AltKey;
			bool shiftKey = keyEvent.ShiftKey && !keyEvent.CtrlKey && !keyEvent.AltKey;

			if (!noAdditionalKey && !shiftKey) return;

			keyEvent.PreventDefault();
			keyEvent.StopPropagation();

			if (current is TriggerField)
				Type.InvokeMethod(current, "triggerBlur");

			if (shiftKey)
				Focus(Previous(current));
			else
				Focus(Next(current));
		}

		private Component Next(Component component)
		{
			int index = _controlSequence.IndexOf(component);

			for (int i = index + 1; i < _controlSequence.Count; i++)
			{
				Component next = (Component) _controlSequence[i];

				if (!next.hidden && !next.disabled && !(next is DisplayField))
					return next;
			}

			return null;
		}

		private Component Previous(Component component)
		{
			int index = _controlSequence.IndexOf(component);

			for (int i = index - 1; i >= 0; i--)
			{
				Component previous = (Component) _controlSequence[i];

				if (!previous.hidden && !previous.disabled && !(previous is DisplayField))
					return previous;
			}

			return null;
		}

		private static void Focus(Component component)
		{
			if (component == null)
				return;

			component.focus();
		}

		private void OnFocusChanged()
		{
			if (FocusChanged != null) 
				FocusChanged(_currentFocused);
		}

		private readonly ArrayList _controlSequence = new ArrayList();

		private Component _currentFocused;
	}
}