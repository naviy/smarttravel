using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.form;
using Ext.util;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;


namespace Luxena.Travel
{

	public delegate void InitFormMemberAction(FormMember m);
	public delegate Component ToEditorFormMemberAction();

	public partial class SemanticMember : SemanticEntity
	{
		public override void Initialize()
		{
			ToEditor = delegate
			{
				return ToField();
			};
		}

		public SemanticEntity _entity;

		//		// ReSharper disable once CSharpWarnings::CS0108
		//		public ViewMember ShortTitle(string value)
		//		{
		//			_shortTitle = value;
		//			return this;
		//		}

		//		// ReSharper disable once CSharpWarnings::CS0108
		//		public ViewMember Name(string value)
		//		{
		//			_name = value;
		//			return this;
		//		}

		// ReSharper disable once CSharpWarnings::CS0108
		public SemanticMember Title(string value)
		{
			_title = value;
			return this;
		}

		// ReSharper disable once CSharpWarnings::CS0108
		public SemanticMember Titles(string value)
		{
			_titles = value;
			return this;
		}

		public SemanticType _type;
		public SemanticMember Type(SemanticType value)
		{
			_type = value;
			return this;
		}

		//		public bool _isPersistent = true;
		//		public ViewMember IsPersistent(bool value)
		//		{
		//			_isPersistent = value;
		//			return this;
		//		}

		public string _emptyText;
		public SemanticMember EmptyText(string value)
		{
			_emptyText = value;
			return this;
		}

		public object _defaultValue;
		public SemanticMember DefaultValue(object value)
		{
			_defaultValue = value;
			return this;
		}

		public bool _required;
		public SemanticMember Required()
		{
			_required = true;
			return this;
		}

		public int _maxLength;
		public SemanticMember MaxLength(int value)
		{
			_maxLength = value;
			return this;
		}

		public int _lineCount;
		public SemanticMember LineCount(int value)
		{
			_lineCount = value;
			return this;
		}


		public SemanticMember EntityDate()
		{
			ColumnRenderer = GetDateRenderer();
			return this;
		}

		public SemanticMember EntityName()
		{
			if (_type is StringSemanticType)
				ColumnRenderer = GetNameRenderer();
			return this;
		}

		public SemanticMember Secondary()
		{
			ColumnIsHidden = true;
			return this;
		}

		public SemanticMember Utility()
		{
			ColumnIsHidden = true;
			return this;
		}

		#region Columns

		public bool ColumnIsHidden;
		public object ColumnWidth;
		public GridRenderDelegate ColumnRenderer;
		public GridColumnConfigAction ColumnInitAction;


		public override GridRenderDelegate GetDateRenderer(string className)
		{
			return delegate (object value, object metadata, Ext.data.Record record, int index, int colIndex, Ext.data.Store store)
			{
				if (!Script.IsValue(value)) return "";

				if (!Script.IsValue(className))
					className = _className;

				value = Script.IsValue(_type) ? _type.GetString(this, value) : ((Date)value).Format("d.m.Y");

				return ControlFactory
					.CreateRefrenceRenderer(className)
					.Invoke(value, metadata, record, index, colIndex, store);
			};
		}


		[AlternateSignature]
		public extern SemanticMember SetColumn(bool isHidden);

		[AlternateSignature]
		public extern SemanticMember SetColumn(bool isHidden, object elWidth);

		[AlternateSignature]
		public extern SemanticMember SetColumn(bool isHidden, object elWidth, GridRenderDelegate renderer);

		public SemanticMember SetColumn(bool isHidden, object elWidth, GridRenderDelegate renderer, GridColumnConfigAction initAction)
		{
			ColumnIsHidden = isHidden;
			ColumnWidth = elWidth;
			ColumnRenderer = renderer;
			ColumnInitAction = initAction;

			return this;
		}

		[AlternateSignature]
		public extern Dictionary ToColumn();

		[AlternateSignature]
		public extern Dictionary ToColumn(bool isHidden);

		[AlternateSignature]
		public extern Dictionary ToColumn(bool isHidden, object elWidth);

		[AlternateSignature]
		public extern Dictionary ToColumn(bool isHidden, object elWidth, GridRenderDelegate renderer);

		public Dictionary ToColumn(bool isHidden, object elWidth, GridRenderDelegate renderer, GridColumnConfigAction initAction)
		{
			if (!Script.IsValue(isHidden))
				isHidden = ColumnIsHidden;

			if (!Script.IsValue(elWidth))
				elWidth = ColumnWidth;

			if (!Script.IsValue(renderer))
				renderer = ColumnRenderer;

			if (!Script.IsValue(initAction))
				initAction = ColumnInitAction;

			Ext.grid.ColumnConfig cfg = GridForm.ColumnCfg_(_name, isHidden, elWidth, renderer, initAction);

			cfg.header(_title ?? _name);

			_type.ToColumn(this, cfg);

			if (Script.IsValue(renderer))
				cfg.renderer(renderer);


			return cfg.ToDictionary();
		}

		#endregion


		#region Editor and Field

		public ToEditorFormMemberAction ToEditor;
		public ToEditorFormMemberAction ToEditor1;
		public ToEditorFormMemberAction ToEditor2;


		[AlternateSignature]
		public extern SemanticMember SetEditor(int width);

		public SemanticMember SetEditor(int width, InitFormMemberAction initMember)
		{
			ToEditor = delegate
			{
				return ToField(width, initMember);
			};

			return this;
		}


		[AlternateSignature]
		public extern Field ToField();

		[AlternateSignature]
		public extern Field ToField(int width);

		public Field ToField(int width, InitFormMemberAction initMember)
		{
			if (!Script.IsValue(initMember))
				initMember = null;

			if (_field != null)
				return _field;

			if (!Script.IsValue(_type))
				Log.loga("ERROR: ViewMember.ToField: _type is null", this);

			return _field = _type.ToField(EditForm, this, width, initMember);
		}

		public string ToHtmlTd2(object r, bool required)
		{
			if (Script.IsNullOrUndefined(r)) return "";

			object value = ((Dictionary)r)[_name];

			if (Script.IsNullOrUndefined(value))
				if (required)
					return @"<td class='fieldLabel'>" + _title + @":</td><td/>";
				else
					return "";

			string label = @"<td class='fieldLabel'>" + _title + @":</td>";


			return label + @"<td class='fieldValue'>" + GetValueHtml(r, value) + @"</td>";

		}


		public string GetValueHtml(object r, object value)
		{
			if (Script.IsNullOrUndefined(value))
				value = ((Dictionary)r)[_name];

			value = _type.GetString(this, value);

			return Script.IsNullOrUndefined(value) ? "&nbsp;" : value.ToString();
		}

		[AlternateSignature]
		public extern string ToHtmlTr2(object r);

		public string ToHtmlTr2(object r, bool required)
		{
			if (Script.IsNullOrUndefined(r)) return "";

			string html = ToHtmlTd2(r, required);

			return string.IsNullOrEmpty(html) ? html : "<tr>" + html + "</tr>";
		}


		public string ToHtmlTd4(object r, bool required, bool hideLabel, bool bold, string onclick)
		{
			if (!Script.IsValue(r)) return "";

			object value = ((Dictionary)r)[_name];

			if (!Script.IsValue(value) || IsMoney && ((MoneyDto)value).Amount == 0)
				if (required)
					return @"<td class='fieldLabel error'>" + (hideLabel ? "" : _title) + @":</td><td/><td/><td/>";
				else
					return "";

			string label = hideLabel ? "<td/>" : @"<td class='fieldLabel'>" + _title + @":</td>";

			string btn =
				string.IsNullOrEmpty(onclick) ? "<td/>" :
				onclick.IndexOf("</") > 0 ? onclick :
				@"<td><div id='showFeesButton' class='showFeesBtn feesClosed' href='javascript:void(0)' onclick='" +
				onclick + @"' ></div></td>";

			string cls = _name == "GrandTotal" ? "total" : bold ? "" : "gray";


			if (IsMoney)
			{
				MoneyDto money = (MoneyDto)value;
				return label +
					@"<td class='fieldValue " + cls + @" rightAlign amount'>" + money.Amount.Format("N2") + @"</td>" +
					@"<td class='fieldValue " + cls + @"'>" + (money.Currency != null ? money.Currency.Name : "&nbsp;") + @"</td>" +
					btn;
			}

			if (IsEnum)
			{
				return label +
					@"<td class='fieldValue " + cls + @" rightAlign'>" + GetEnumItemName(value) + @"</td>" +
					@"<td/>" +
					btn;
			}

			return label +
				@"<td class='fieldValue " + cls + @"'>" + value + @"</td><td/>" + btn;

		}

		[AlternateSignature]
		public extern string ToHtmlTr4(object r);

		[AlternateSignature]
		public extern string ToHtmlTr4(object r, bool required);

		[AlternateSignature]
		public extern string ToHtmlTr4(object r, bool required, bool hideLabel);

		[AlternateSignature]
		public extern string ToHtmlTr4(object r, bool required, bool hideLabel, bool bold);

		public string ToHtmlTr4(object r, bool required, bool hideLabel, bool bold, string onclick)
		{
			if (Script.IsNullOrUndefined(r)) return "";

			string html = ToHtmlTd4(r, required, hideLabel, bold, onclick);

			return string.IsNullOrEmpty(html) ? html : "<tr>" + html + "</tr>";
		}


		private Field _field;

		#endregion


		public string GetString(object data)
		{
			return _type.GetString(this, ((Dictionary)data)[_name]);
		}
	}

}