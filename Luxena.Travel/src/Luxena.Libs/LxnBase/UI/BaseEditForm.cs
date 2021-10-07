using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.form;
using Ext.util;
using Ext.ux;

using jQueryApi;

using Element = System.Html.Element;
using HtmlWindow = System.Html.Window;
using WindowClass = Ext.Window;


namespace LxnBase.UI
{
	public abstract class BaseEditForm : IKeyHandler
	{
		public event GenericOneArgDelegate Saved;
		public event AnonymousDelegate Canceled;
		public event AnonymousDelegate Close;

		protected BaseEditForm()
		{
			_form = new Form(CreateFormConfig().ToDictionary());

			_window = new WindowClass(CreateWindowConfig().ToDictionary());

			_actionManager = new FieldActionManager();
		}

		protected WindowClass Window { get { return _window; } }

		protected Form Form { get { return _form; } }

		protected Field[] Fields { get { return _fields; } set { _fields = value; } }

		protected Component[] ComponentSequence
		{
			get { return _componentSequence; }
			set { _componentSequence = value; }
		}

		protected bool IsCanceled { get { return _isCanceled; } }

		protected bool IsSaved { get { return _isSaved; } }

		private WindowConfig CreateWindowConfig()
		{
			_errorPanel = new Panel(new PanelConfig()
				.cls("errors")
				.hidden(true)
				.listeners(new Dictionary("render", new AnonymousDelegate(
					delegate { _errorPanel.getEl().on("click", new GenericOneArgDelegate(OnErrorPanelClick)); })))
				.ToDictionary());

			_statusBar = new StatusBar(new StatusBarConfig()
				.ctCls("status-bar")
				.ToDictionary());

			return new WindowConfig()
				.baseCls("x-panel")
				.cls("window-edit")
				.resizable(false)
				.listeners(new Dictionary(
					"afterrender", new AnonymousDelegate(OnWindowAfterRender),
					"show", new AnonymousDelegate(OnWindowShow),
					"close", new AnonymousDelegate(OnWindowClose)))
				.modal(true)
				.keys(new object[]
				{
					new Dictionary("key", 13, "fn", new Action<object, EventObject>(OnKeyEnterPress), "scope", this)
				})
				.items(new object[] { _form, _errorPanel })
				.bbar(_statusBar);
		}

		private static FormPanelConfig CreateFormConfig()
		{
			return new FormPanelConfig()
				.autoScroll(true)
				.bodyBorder(false)
				.border(false)
				.labelWidth(100)
				.labelAlign("right")
				.buttonAlign("center");
		}

		protected virtual bool OnValidate()
		{
			return true;
		}

		protected abstract void OnSave();

		protected virtual void OnSaved(object result)
		{
			_isSaved = true;

			_window.close();
		}

		protected virtual void Save()
		{
			if (Validate())
				OnSave();
		}

		protected virtual void CompleteSave(object result)
		{
			if (Saved != null)
				Saved(result);

			OnSaved(result);
		}

		protected virtual void FailSave(object result)
		{
		}

		protected void Cancel()
		{
			_isCanceled = true;

			_window.close();
		}

		protected void RegisterField(Field field)
		{
			if (_fields == null)
				_fields = new Field[0];

			_fields[_fields.Length] = field;
		}

		protected void RegisterFocusComponent(Component component)
		{
			if (_componentSequence == null)
				_componentSequence = new Component[0];

			_componentSequence[_componentSequence.Length] = component;
		}

		protected bool Validate()
		{
			if (_form.getForm().isValid() && OnValidate())
				return true;

			ShowErrors();

			return false;
		}

		protected virtual bool IsModified()
		{
			if (_fields == null) return false;

			foreach (Field field in _fields)
				if (IsFieldModified(field))
					return true;

			return false;
		}

		protected static bool IsFieldModified(Field field)
		{
			return field != null && field.isDirty();
		}

		protected void ApplyActions(Field field, FieldActionGetActionsDelegate getActions)
		{
			_actionManager.Add(field, getActions);
		}

		protected void RefreshActions()
		{
			_actionManager.Refresh();
		}

		public virtual bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			return _navigationManager.HandleKeyEvent(keyEvent) || _actionManager.HandleKeyEvent(keyEvent);
		}

		public void RestoreFocus()
		{
			_navigationManager.RestoreFocus();
		}

		private void OnWindowAfterRender()
		{
			_navigationManager = FieldNavigationManager.Create(_componentSequence);

			_actionManager.Init(_navigationManager, Form);

			if (_componentSequence == null) return;

			foreach (Component component in _componentSequence) 
			{
				if (component.hidden) continue;

				_window.defaultButton = component;

				return;
			}
		}

		private void OnWindowShow()
		{
			if (_fields != null)
			{
				foreach (Field field in _fields) 
				{
					field.on("invalid", new FieldInvalidDelegate(OnFieldValidation));
					field.on("valid", new FieldValidDelegate(OnFieldValidation));
				}
			}

			EventsManager.RegisterKeyDownHandler(this, !_window.modal);

			_statusBar.clearStatus();

			RefreshWindowShadow();
		}

		private void OnWindowClose()
		{
			if (!_isSaved && Canceled != null)
				Canceled();

			EventsManager.UnregisterKeyDownHandler(this);

			if (Close != null)
				Close();
		}

		[AlternateSignature]
		private extern void OnFieldValidation(Field field);

		private void OnFieldValidation(Field field, string msg)
		{
			if (!_isSubscribeOnStatusClick)
			{
				((Component) Type.GetField(_statusBar, "statusEl")).getEl().on("click", new AnonymousDelegate(
					delegate
					{
						if (!_errorPanel.hidden)
							HideErrors();
						else if (_errors.getCount() > 0)
							ShowErrors();
					}));

				_isSubscribeOnStatusClick = true;
			}

			if (!Script.IsNullOrUndefined(msg))
				_errors.add(field.id, new Dictionary("field", field, "message", msg));
			else
				_errors.removeKey(field.id);

			UpdateErrorList();
		}

		private static void OnErrorPanelClick(object e)
		{
			EventObject arg = (EventObject) e;

			Element element = arg.getTarget("li");

			if (!Script.IsNullOrUndefined(element))
				ComponentMgr.get(element.ID.Split("errorFieldId-")[1]).focus();

			arg.stopEvent();
		}

		private void ShowErrors()
		{
			if (!_errorPanel.hidden)
				return;

			_errorPanel.setWidth((double) Type.GetField(_form.getBox(), "width"));
			_errorPanel.show();

			_statusBar.setText(BaseRes.ValiadationStatusHideError_Msg);

			RefreshWindowShadow();
		}

		private void HideErrors()
		{
			if (_errorPanel.hidden) return;
			
			_errorPanel.hide();

			_statusBar.setText(BaseRes.ValiadationStatusSowError_Msg);

			RefreshWindowShadow();
		}

		private void UpdateErrorList()
		{
			if (_errors.getCount() > 0)
			{
				string html = "<ul>";

				_errors.each(new GenericOneArgDelegate(
					delegate(object obj)
					{
						Field field = (Field) Type.GetField(obj, "field");
						string message = (string) Type.GetField(obj, "message");

						html += string.Format("<li id='errorFieldId-{2}'><a href='javascript:void(0)'>{0}: {1}</a></li>", field.fieldLabel, message, field.id);
					}));

				new Template(html).overwrite(_errorPanel.body);

				if (_errorPanel.hidden && _statusBar.getText() != BaseRes.ValiadationStatusSowError_Msg)
				{
					_statusBar.setStatus(new StatusBarConfig()
						.text(BaseRes.ValiadationStatusSowError_Msg)
						.iconCls("error-icon")
						.ToDictionary());
				}
				else if (!_errorPanel.hidden && _statusBar.getText() != BaseRes.ValiadationStatusHideError_Msg)
				{
					_statusBar.setStatus(new StatusBarConfig()
						.text(BaseRes.ValiadationStatusHideError_Msg)
						.iconCls("error-icon")
						.ToDictionary());
				}
			}
			else
			{
				new Template("<div></div>").overwrite(_errorPanel.body);

				_errorPanel.hide();

				_statusBar.clearStatus();

				RefreshWindowShadow();
			}

			if (!_errorPanel.hidden)
				RefreshWindowShadow();
		}

		private void OnKeyEnterPress(object key, EventObject e)
		{
			if (!e.shiftKey && e.ctrlKey && !e.altKey)
			{
				//e.stopEvent();

				//Save();
			}
		}

		protected void RefreshWindowShadow()
		{
			if (_form.ownerCt != null && _form.ownerCt is WindowClass)
				Type.InvokeMethod(_form.ownerCt.getEl(), "enableShadow", true);
		}


		#region Components

		[AlternateSignature]
		public extern Panel EmptyRow();

		public Panel EmptyRow(int rowCount)
		{
			if (!Script.IsValue(rowCount))
				rowCount = 1;

			return new Panel(new PanelConfig()
				.height(29 * rowCount)
				.ToDictionary()
			);
		}

		public Panel TabPanel(int height, Component[] items)
		{
			return new TabPanel(new TabPanelConfig()
				.items(items)
				.activeTab(0)
				.height(height)
				.ToDictionary()
			);
		}

		public static BoxComponent TextComponent(string html)
		{
			return new BoxComponent(new BoxComponentConfig()
				.autoEl(new Dictionary("tag", "div", "html", html))
				.cls("x-form-item float-left box-label")
				.ToDictionary());
		}

		public static Panel RowPanel(Component[] items)
		{
			return new Panel(new PanelConfig()
				.layout("form")
				.itemCls("float-left")
				.items(items)
				.ToDictionary());
		}

		#endregion





		private readonly WindowClass _window;
		private readonly Form _form;
		private Panel _errorPanel;
		private StatusBar _statusBar;

		private Field[] _fields;
		private Component[] _componentSequence;

		private readonly MixedCollection _errors = new MixedCollection();

		private bool _isSaved;
		private bool _isCanceled;

		private bool _isSubscribeOnStatusClick;
		private FieldNavigationManager _navigationManager;
		private readonly FieldActionManager _actionManager;
	}
}