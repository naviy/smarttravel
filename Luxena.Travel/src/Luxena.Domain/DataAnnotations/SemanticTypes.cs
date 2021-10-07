using System;


namespace Luxena.Domain
{

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
	public abstract class SemanticTypeAttribute : Attribute
	{
		public abstract string ToJS();
		public abstract string ToScriptSharp();
	}


	#region Date/Time

	public class DateAttribute : SemanticTypeAttribute
	{
		public override string ToJS()
		{
			return ".date()";
		}
		public override string ToScriptSharp()
		{
			return ".Date()";
		}
	}

	public class DateTimeAttribute : SemanticTypeAttribute
	{
		public override string ToJS()
		{
			return ".dateTime()";
		}
		public override string ToScriptSharp()
		{
			return ".DateTime()";
		}
	}

	public class DateTime2Attribute : SemanticTypeAttribute
	{
		public override string ToJS()
		{
			return ".dateTime2()";
		}
		public override string ToScriptSharp()
		{
			return ".DateTime2()";
		}
	}

	public class TimeAttribute : SemanticTypeAttribute
	{
		public override string ToJS()
		{
			return ".time()";
		}
		public override string ToScriptSharp()
		{
			return ".Time()";
		}
	}

	public class Time2Attribute : SemanticTypeAttribute
	{
		public override string ToJS()
		{
			return ".time2()";
		}
		public override string ToScriptSharp()
		{
			return ".Time2()";
		}
	}

	#endregion


	#region Number

	public class FloatAttribute : SemanticTypeAttribute
	{

		public byte Scale { get; set; }


		public FloatAttribute() { }

		public FloatAttribute(byte scale)
		{
			Scale = scale;
		}


		public override string ToJS()
		{
			return Scale == 0 ? ".float()" : ".float(" + Scale + ")";
		}

		public override string ToScriptSharp()
		{
			return Scale == 0 ? ".Float2()" : ".Float" + Scale + "()";
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

	public class PasswordAttribute : SemanticTypeAttribute
	{
		
		public override string ToJS()
		{
			return ".password()";
		}

		public override string ToScriptSharp()
		{
			return ".Password()";
		}
	}


	public class ConfirmPasswordAttribute : SemanticTypeAttribute
	{
		public ConfirmPasswordAttribute(string passwordFieldName)
		{
			PasswordFieldName = passwordFieldName;
		}

		public string PasswordFieldName { get; set; }

		public override string ToJS()
		{
			return ".confirmPassword(\"" + PasswordFieldName + "\")";
		}

		public override string ToScriptSharp()
		{
			return ".ConfirmPassword(\"" + PasswordFieldName + "\")";
		}
	}


	public class TextAttribute : SemanticTypeAttribute
	{
		public TextAttribute(int lineCount = 3)
		{
			LineCount = lineCount;
		}

		public int LineCount { get; set; }
		
		public override string ToJS()
		{
			return ".text(" + LineCount + ")";
		}

		public override string ToScriptSharp()
		{
			return ".Text(" + LineCount + ")";
		}
	}


	#endregion


	public class DefaultMoneyAttribute : SemanticTypeAttribute
	{
		public override string ToJS()
		{
			return ".defaultMoney()";
		}

		public override string ToScriptSharp()
		{
			return ".DefaultMoney()";
		}
	}
}
