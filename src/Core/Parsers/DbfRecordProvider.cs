using System;
using System.Collections.Generic;

using DotNetDBF;

namespace Luxena.Travel.Parsers
{
	public class DbfRecordProvider : IDisposable
	{
		public DbfRecordProvider(string path)
		{
			_reader = new DBFReader(path);

			for (int i = 0; i < _reader.Fields.Length; i++)
				_indices.Add(_reader.Fields[i].Name.ToLower(), i);
		}

		public DbfDataRecord Record { get; private set; }

		public bool Read()
		{
			object[] values = _reader.NextRecord();

			if (values == null)
			{
				Record = null;

				return false;
			}

			Record = new DbfDataRecord(_indices, values);

			return true;
		}

		public T Get<T>(string name)
		{
			return Record.Get<T>(name);
		}

		public void Dispose()
		{
			_reader.Dispose();
		}

		private readonly DBFReader _reader;
		private readonly Dictionary<string, int> _indices = new Dictionary<string, int>();
	}
}