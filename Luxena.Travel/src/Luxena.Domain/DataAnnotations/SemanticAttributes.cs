using System;


namespace Luxena.Domain
{
	
	/// <summary>
	/// Указывает свойство имя-сущности в случае, если EntityName уже указано для предка
	/// </summary>
	public class EntityName2Attribute : Attribute { }


	public class ExtendsAttribute : Attribute
	{
		public Type BaseType { get; set; }

		public ExtendsAttribute(Type baseType)
		{
			BaseType = baseType;
		}
	}

	public class LineCountAttribute : Attribute
	{
		public LineCountAttribute(int count)
		{
			Count = count;
		}

		public int Count { get; set; }
	}


	/// <summary>
	/// Признак вспомогательного свойства
	/// Колонка скрыта
	/// </summary>
	public class SecondaryAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Class)]
	public class SingletonAttribute : Attribute { }


	[AttributeUsage(AttributeTargets.Class)]
	public class SmallAttribute : Attribute { }


	/// <summary>
	/// Указывает источник для SemanticMember.Reference()
	/// </summary>
	public class SuggestAttribute : Attribute
	{
		public Type ReferenceType { get; set; }


		public SuggestAttribute(Type referenceType)
		{
			ReferenceType = referenceType;
		}
	}


	/// <summary>
	/// Признак утилитного свойства
	/// Колонка скрыта
	/// </summary>
	public class UtilityAttribute : Attribute { }


}
