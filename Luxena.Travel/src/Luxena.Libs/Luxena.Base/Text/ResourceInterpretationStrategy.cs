using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;


namespace Luxena.Base.Text
{
	public class ResourceInterpretationStrategy : IInterpretationStrategy
	{
		public ResourceInterpretationStrategy(IDictionary managers)
		{
			_managers = new Dictionary<string, ResourceManager>();

			foreach (DictionaryEntry item in managers)
				_managers.Add(item.Key.ToString(), new ResourceManager(Type.GetType((string) item.Value)));
		}

		public string Interpret(string value)
		{
			string resource = value.Substring(0, value.LastIndexOf('.'));
			string key = value.Substring(value.LastIndexOf('.') + 1);

			return _managers[resource].GetString(key);
		}

		private readonly Dictionary<string, ResourceManager> _managers;
	}
}