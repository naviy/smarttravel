using System;
using System.Collections.Generic;

namespace Luxena.Travel.Parsers
{
	public class DbfDataRecord
	{
		public DbfDataRecord(IDictionary<string, int> indices, IList<object> values)
		{
			_indices = indices;
			_values = values;
		}

		public object this[string name]
		{
			get { return _values[_indices[name.ToLower()]]; }
		}

		public bool HasValue(string name)
		{
			if (!_indices.ContainsKey(name.ToLower()))
				return false;

			var value = this[name];

			return value != null && !string.IsNullOrEmpty(value.ToString().Trim());
		}

		public T Get<T>(string name)
		{
			return HasValue(name) ? (T)Convert.ChangeType(this[name], typeof(T)) : default(T);
		}

		private readonly IDictionary<string, int> _indices;
		private readonly IList<object> _values;
	}
}