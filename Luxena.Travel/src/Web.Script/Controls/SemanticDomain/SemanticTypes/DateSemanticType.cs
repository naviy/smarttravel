using System;

using Ext.form;
using Ext.util;

using LxnBase.UI.AutoControls;


namespace Luxena.Travel
{

	public class DateSemanticType : SemanticType
	{
		public string FormatString;

		public DateSemanticType(string formatString)
		{
			FormatString = formatString;
		}


		public override string GetString(SemanticMember sm, object value)
		{
			return !Script.IsValue(value) ? "" : Format.date((Date)value, FormatString);
		}


		public override void ToColumn(SemanticMember sm, Ext.grid.ColumnConfig cfg)
		{
			cfg.renderer(new RenderDelegate(delegate(object value) { return GetString(sm, value); }));
		}

		public override Field ToField(IEditForm form, SemanticMember sm, int width, InitFormMemberAction initMember)
		{
			DateFieldConfig config = new DateFieldConfig();

			InitTextFieldConfig(form, sm, width, config);

			config.format(FormatString);
			
			FormMember member = form.Members.Add2(form, sm, config, initMember);

			return member.SetField(new DateField(config.ToDictionary()));
		}

		public override int ConvertWidth(IEditForm form, int width)
		{
			if (Script.IsNullOrUndefined(width) || width == -2)
				return 103;

			return base.ConvertWidth(form, width);
		}

	}


	public static partial class ViewTypes
	{
		public static SemanticType Date = new DateSemanticType("d.m.Y");
		public static SemanticType DateTime = new DateSemanticType("d.m.Y G:i");
		public static SemanticType DateTime2 = new DateSemanticType("d.m.Y G:i:s");
		public static SemanticType Time = new DateSemanticType("G:i");
		public static SemanticType Time2 = new DateSemanticType("G:i:s");
	}


	public partial class SemanticMember
	{
		public bool IsDate;

		public SemanticMember Date()
		{
			_type = ViewTypes.Date;
			IsDate = true;
			ColumnWidth = 90;
			return this;
		}

		public SemanticMember DateTime()
		{
			_type = ViewTypes.DateTime;
			IsDate = true;
			ColumnWidth = 140;
			return this;
		}

		public SemanticMember DateTime2()
		{
			_type = ViewTypes.DateTime2;
			IsDate = true;
			ColumnWidth = 160;
			return this;
		}

		public SemanticMember Time()
		{
			_type = ViewTypes.Time;
			IsDate = true;
			return this;
		}

		public SemanticMember Time2()
		{
			_type = ViewTypes.Time2;
			IsDate = true;
			return this;
		}

	}

}