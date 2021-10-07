using System;
using System.Collections;
using System.Runtime.CompilerServices;

using LxnBase.Data;


namespace LxnBase.UI
{
	public delegate void ListObjectsCallback(ListArgs args, bool newTab);

	public delegate void ViewObjectCallback(string type, object id, bool newTab);

	public delegate void EditObjectCallback(EditFormArgs args);

	public delegate void SelectObjectCallback(SelectArgs args);

	public static class FormsRegistry
	{
		public static void RegisterList(string type, ListObjectsCallback callback)
		{
			if (_lists.ContainsKey(type))
				throw new Exception(string.Format(RegisterExceptionFormat, "List", type));

			_lists[type] = callback;
		}

		public static void RegisterDefaultList(ListObjectsCallback callback)
		{
			_defaultList = callback;
		}

		[AlternateSignature]
		public static extern void RegisterView(string type, ViewObjectCallback callback);

		public static void RegisterView(string type, ViewObjectCallback callback, bool hasForm)
		{
			if (_views.ContainsKey(type))
				throw new Exception(string.Format(RegisterExceptionFormat, "View", type));

			ViewAction viewAction = new ViewAction();
			viewAction.Callback = callback;

			if (!Script.IsUndefined(hasForm))
				viewAction.HasForm = hasForm;

			_views[type] = viewAction;
		}

		public static void RegisterDefaultView(ViewObjectCallback callback)
		{
			_defaultView = callback;
		}

		public static void RegisterEdit(string type, EditObjectCallback callback)
		{
			if (_edits.ContainsKey(type))
				throw new Exception(string.Format(RegisterExceptionFormat, "Edit", type));

			_edits[type] = callback;
		}

		public static void RegisterDefaultEdit(EditObjectCallback callback)
		{
			_defaultEdit = callback;
		}

		public static void RegisterSelect(string type, SelectObjectCallback callback)
		{
			if (_selects.ContainsKey(type))
				throw new Exception(string.Format(RegisterExceptionFormat, "Select", type));

			_selects[type] = callback;
		}

		public static void RegisterDefaultSelect(SelectObjectCallback callback)
		{
			_defaultSelect = callback;
		}

		[AlternateSignature]
		public static extern void ListObjects(string type);

		public static void ListObjects(string type, RangeRequest baseRequest, bool newTab)
		{
			ListObjectsCallback list = (ListObjectsCallback)_lists[type] ?? _defaultList;

			if (Script.IsNullOrUndefined(list))
				throw new Exception(string.Format(NoFormExceptionFormat, "list", type));

			list(new ListArgs(type, baseRequest), newTab);
		}

		[AlternateSignature]
		public static extern void ViewObject(string type, object id);


		public static void ViewObject(string type, object id, bool newTab)
		{
			ViewObjectCallback view = Script.IsNullOrUndefined(_views[type]) ? _defaultView : ((ViewAction) _views[type]).Callback;

			if (Script.IsNullOrUndefined(view))
				throw new Exception(string.Format(NoFormExceptionFormat, "view", type));

			view(type, id, newTab);
		}


		public static bool HasViewForm(string type)
		{
			return Script.IsNullOrUndefined(_views[type]) || ((ViewAction)_views[type]).HasForm;
		}

		[AlternateSignature]
		public static extern void EditObject(string type, object id, Dictionary values, GenericOneArgDelegate onSave, AnonymousDelegate onCancel);

		[AlternateSignature]
		public static extern void EditObject(string type, object id, Dictionary values, GenericOneArgDelegate onSave, AnonymousDelegate onCancel, RangeRequest @params);

		[AlternateSignature]
		public static extern void EditObject(string type, object id, Dictionary values, GenericOneArgDelegate onSave,
											 AnonymousDelegate onCancel, RangeRequest @params, LoadMode mode);

		public static void EditObject(string type, object id, Dictionary values, GenericOneArgDelegate onSave, AnonymousDelegate onCancel, RangeRequest @params, LoadMode mode, bool isCopy)
		{
			EditObjectCallback edit = (EditObjectCallback) _edits[type] ?? _defaultEdit;

			if (Script.IsNullOrUndefined(edit))
				throw new Exception(string.Format(NoFormExceptionFormat, "edit", type));

			edit(new EditFormArgs(id, type, values, Script.IsValue(@params) ? @params : null, onSave, onCancel,
				Script.IsValue(mode) ? mode : LoadMode.Remote, isCopy));
		}

		public static void SelectObjects(string type, RangeRequest baseRequest, bool singleSelect, GenericOneArgDelegate onSelect, AnonymousDelegate onCancel)
		{
			SelectObjectCallback select = (SelectObjectCallback)_selects[type] ?? _defaultSelect;

			if (Script.IsNullOrUndefined(select))
				throw new Exception(string.Format(NoFormExceptionFormat, "select", type));

			select(new SelectArgs(type, baseRequest, singleSelect, onSelect, onCancel));
		}

		private const string NoFormExceptionFormat = "There is no registered {0} form for type '{1}'";
		private const string RegisterExceptionFormat = "{0} form is alreay registered for type '{1}'";

		private static readonly Dictionary _lists = new Dictionary();
		private static ListObjectsCallback _defaultList;

		private static readonly Dictionary _views = new Dictionary();
		private static ViewObjectCallback _defaultView;

		private static readonly Dictionary _edits = new Dictionary();
		private static EditObjectCallback _defaultEdit;

		private static readonly Dictionary _selects = new Dictionary();
		private static SelectObjectCallback _defaultSelect;
	}

	public sealed class ViewAction
	{
		public ViewAction()
		{
			HasForm = true;
		}

		public ViewObjectCallback Callback;
		public bool HasForm;
	}
}