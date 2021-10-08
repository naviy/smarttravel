using System;
using System.Diagnostics;
using System.Reflection;


namespace Luxena.Domain
{

	[AttributeUsage(AttributeTargets.Class)]
	public class SemanticEntityAttribute : Attribute { }

	
	[AttributeUsage(AttributeTargets.Method)]
	public class EntityActionAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EntityDateAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EntityNameAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EntityTypeAttribute : Attribute { }


	/// <summary>
	/// Указывает свойство имя-сущности в случае, если EntityName уже указано для предка
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EntityName2Attribute : Attribute { }


//	[AttributeUsage(AttributeTargets.Class)]
//	public class ExtendsAttribute : Attribute
//	{
//		public Type BaseType { get; set; }
//
//		[DebuggerStepThrough]
//		public ExtendsAttribute(Type baseType)
//		{
//			BaseType = baseType;
//		}
//	}


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EntityPositionAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum)]
	public class FlagsAttribute : System.FlagsAttribute { }



	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field)]
	public class IconAttribute : Attribute
	{
		[DebuggerStepThrough]
		public IconAttribute(string iconName)
		{
			IconName = iconName;
		}

		[DebuggerStepThrough]
		public IconAttribute(Type iconSource)
		{
			IconSource = iconSource;
		}

		public MemberInfo IconSource { get; }

		public string IconName
		{
			get
			{
				if (_iconName == null && IconSource != null)
					_iconName = IconSource.Semantic<IconAttribute>()?.IconName;

				return _iconName;
			}
			set { _iconName = value; }
		}
		private string _iconName;

	}


	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Enum)]
	public class LengthAttribute : Attribute
	{
		[DebuggerStepThrough]
		public LengthAttribute(int length, int min = 0, int max = 0)
		{
			Length = length;
			MinLength = min;
			MaxLength = max;
		}

		public int Length { get; set; }
		public int MinLength { get; set; }
		public int MaxLength { get; set; }
	}


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class LineCountAttribute : Attribute
	{
		[DebuggerStepThrough]
		public LineCountAttribute(int count)
		{
			Count = count;
		}

		public int Count { get; set; }
	}


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class NotDbMappedAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
	public class NotUiMappedAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class ReadOnlyAttribute : Attribute { }


	//[AttributeUsage(AttributeTargets.Property)]
	public class MemberPatternAttribute : Attribute
	{
		public MemberInfo Pattern;

		public MemberPatternAttribute(MemberInfo pattern)
		{

			var prop = pattern as PropertyInfo;
			if (prop != null && prop.PropertyType.IsEntity())
				pattern = prop.DeclaringType?.GetField("_" + prop.Name, BindingFlags.Instance | BindingFlags.NonPublic) ?? pattern;

			Pattern = pattern;
		}

		public MemberPatternAttribute(Type type, string memberName)
			: this(type.PropertyOrField(memberName))
		{
		}
	}


	/// <summary>
	/// Признак вспомогательного свойства
	/// Колонка скрыта
	/// </summary>
	public class SecondaryAttribute : Attribute { }


	//[AttributeUsage(AttributeTargets.Class)]
	//public class SingletonAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Class)]
	public class BigAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class)]
	public class SmallAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class)]
	public class WideAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SubjectAttribute : Attribute { }


	/// <summary>
	/// Указывает источник для SemanticMember.Reference()
	/// </summary>
	public class LookupAttribute : Attribute
	{
		public Type ReferenceType { get; set; }
		
		[DebuggerStepThrough]
		public LookupAttribute(Type referenceType)
		{
			ReferenceType = referenceType;
		}
	}
	//public class LookupAttribute : MemberPatternAttribute
	//{
	//	public Type ReferenceType { get; set; }

	//	[DebuggerStepThrough]
	//	public LookupAttribute(Type referenceType)
	//		: base(referenceType)
	//	{
	//		ReferenceType = referenceType;
	//	}
	//}




	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DbRequiredAttribute : Attribute { }

	/// <summary>
	/// Необходимо ввести в UI, не касается DB и OData
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class UiRequiredAttribute : Attribute { }


	/// <summary>
	/// Свойство является уникальным
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class UniqueAttribute : Attribute { }


	/// <summary>
	/// Признак утилитного свойства
	/// Колонка скрыта
	/// </summary>
	public class UtilityAttribute : Attribute { }


}
