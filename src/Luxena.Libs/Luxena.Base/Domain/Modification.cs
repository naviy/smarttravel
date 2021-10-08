using System;
using System.Collections.Generic;

using Luxena.Base.Metamodel;


namespace Luxena.Base.Domain
{
	public class Modification
	{
		public virtual int Id { get; set; }

		[DisplayFormat("g")]
		public virtual DateTime TimeStamp { get; set; }

		public virtual string Author { get; set; }

		public virtual ModificationType Type { get; set; }

		public virtual string InstanceType { get; set; }

		public virtual object InstanceId { get; set; }

		public virtual string InstanceString { get; set; }

		public virtual string Comment { get; set; }

		public virtual IDictionary<string, string> Items
		{
			get { return _items; }
		}

		private readonly IDictionary<string, string> _items = new Dictionary<string, string>();
	}
}