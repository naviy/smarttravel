using System;
using System.Diagnostics;


namespace Luxena.Domain
{

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
	public abstract class SemanticTypeAttribute : Attribute
	{
		[DebuggerStepThrough]
		public abstract string ToTypeScript();
	}


	public class CustomSemanticTypeAttribute : SemanticTypeAttribute
	{
		public string TypeScriptCode { get; set; }

		public override string ToTypeScript()
		{
			return TypeScriptCode;
		}
	}


	#region Date/Time

	public class DateAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".date()";
		}
	}

	public class MonthAndYearAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".monthAndYear()";
		}
	}

	public class QuarterAndYearAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".quarterAndYear()";
		}
	}

	public class YearAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".year()";
		}
	}
	public class DateTimeAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".dateTime()";
		}
	}

	public class DateTime2Attribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".dateTime2()";
		}
	}

	public class TimeAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".time()";
		}
	}

	public class Time2Attribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".time2()";
		}
	}

	#endregion


	#region Number

	public class FloatAttribute : SemanticTypeAttribute
	{

		public int Precision { get; set; }


		public FloatAttribute() { }

		[DebuggerStepThrough]
		public FloatAttribute(int precision)
		{
			Precision = precision;
		}


		public override string ToTypeScript()
		{
			return Precision == 0 ? ".float()" : ".float(" + Precision + ")";
		}


		//		public static void ConfigureModelBuilder(DbModelBuilder modelBuilder)
		//		{
		//			modelBuilder
		//				.Properties()
		//				.Configure(c =>
		//				{
		//					var attr = c.ClrPropertyInfo.Attribute<FloatAttribute>();
		//					if (attr != null)
		//						c.HasPrecision(attr.Scale);
		//				});
		//		}

	}

	#endregion


	#region String

	public class StringAttribute : SemanticTypeAttribute
	{

		public int MaxLength { get; set; }

		public int MinLength { get; set; }

		public int Length { get; set; }


		[DebuggerStepThrough]
		public StringAttribute(int maxLength, int length, int minLength)
		{
			MaxLength = maxLength;
			Length = length;
			MinLength = minLength;
		}

		[DebuggerStepThrough]
		public StringAttribute(int maxLength)
		{
			MaxLength = maxLength;
		}

		[DebuggerStepThrough]
		public StringAttribute(int maxLength, int length)
		{
			MaxLength = maxLength;
			Length = length;
		}

		public override string ToTypeScript()
		{
			return string.Format(".string({0}, {1}, {2})",
				MaxLength,
				Length > 0 ? Length.ToString() : "undefined",
				MinLength > 0 ? MinLength.ToString() : "undefined"
			);
		}
	}

	public class PasswordAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript() => 
			".password()";
	}


	public class ConfirmPasswordAttribute : SemanticTypeAttribute
	{
		[DebuggerStepThrough]
		public ConfirmPasswordAttribute(string passwordFieldName)
		{
			PasswordFieldName = passwordFieldName;
		}

		public string PasswordFieldName { get; set; }

		public override string ToTypeScript() => 
			".confirmPassword(\"" + PasswordFieldName + "\")";
	}


	public class TextAttribute : SemanticTypeAttribute
	{
		[DebuggerStepThrough]
		public TextAttribute(int lineCount = 3)
		{
			LineCount = lineCount;
		}

		public int LineCount { get; set; }

		public override string ToTypeScript() => 
			".text(" + LineCount + ")";
	}

	public class CodeTextAttribute : TextAttribute
	{
		public CodeTextAttribute(int lineCount = 8): base(lineCount) { }

		public override string ToTypeScript() => 
			".codeText(" + LineCount + ")";
	}

	public class EmailAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript() => 
			".email()";
	}

	public class PhoneAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript() => 
			".phone()";
	}

	public class FaxAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript() => 
			".fax()";
	}
	
	public class AddressAttribute : TextAttribute
	{
		public override string ToTypeScript() =>
			".address(" + LineCount + ")";
	}

	public class Hyperlink : TextAttribute
	{
		public override string ToTypeScript() =>
			".hyperlink()";
	}

	#endregion


	public class CurrencyCodeAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".currencyCode()";
		}
	}


	public class DefaultMoneyAttribute : SemanticTypeAttribute
	{
		public override string ToTypeScript()
		{
			return ".defaultMoney()";
		}
	}

}
