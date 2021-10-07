using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace Luxena
{

	public class ObjectValues
	{

		public readonly Dictionary<string, object> Values = new Dictionary<string, object>();


		public ObjectValues<TEntity> For<TEntity>()
		{
			return new ObjectValues<TEntity>(Values);
		}


		public object this[string propertyName]
		{
			get { return Values[propertyName]; }
			set { Values[propertyName] = value; }
		}

	}


	public class ObjectValues<TEntity>
	{

		public readonly Dictionary<string, object> Values;


		public ObjectValues()
		{
			Values = new Dictionary<string, object>();
		}
		public ObjectValues(Dictionary<string, object> values)
		{
			Values = values;
		}

		public object this[Expression<Func<TEntity, object>> properties]
		{
			set { Set(properties, value); }
		}


		public void Set<TValue>(Expression<Func<TEntity, TValue>> properties, TValue value)
		{
			foreach (var prop in properties.GetProperties())
			{
				Values[prop.Name] = value;
			}
		}
	}

}
