using System;

using Ext.form;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public class BoolSemanticType : SemanticType
	{
		public string FormatString;

		public override int DefaultWidth { get { return 24; } }


		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			cfg.width(80);
			cfg.renderer(new RenderDelegate(delegate(object value)
			{
				return (bool)value ? @"<div class='checkBoxDisabled checked'></div>" : "";
			}));
		}


		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			CheckboxConfig config = new CheckboxConfig();

			if (!Script.IsValue(width))
				width = -1;

			InitFieldConfig(form, sm, width, config);

			if (width < 0 && Script.IsValue(sm._title))
			{
				config.fieldLabel("");
				config.labelSeparator("");
				config.boxLabel(sm._title);
			}

			FormMember member = form.Members.Add2(form, sm, config, initMember);

			return member.SetField(new Checkbox(config.ToDictionary()));
		}

	}


	public static partial class ViewTypes
	{
		public static SemanticType Bool = new BoolSemanticType();
	}

	public partial class SemanticMember
	{
		public bool IsBool;

		public SemanticMember Bool()
		{
			_type = ViewTypes.Bool;
			_required = true;
			IsBool = true;
			return this;
		}
	}

}