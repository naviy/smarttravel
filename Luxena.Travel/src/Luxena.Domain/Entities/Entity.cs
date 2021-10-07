using System;


namespace Luxena.Domain.Entities
{

	public class UniqueAttribute : Attribute
	{
		public UniqueAttribute() { }

		public UniqueAttribute(string key) { Key = key; }

		public string Key { get; set; }
	}

}