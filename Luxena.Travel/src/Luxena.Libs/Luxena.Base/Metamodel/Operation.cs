using System;


namespace Luxena.Base.Metamodel
{
	public sealed class Operation
	{
		public Operation(Class owner, string name)
		{
			if (owner == null)
				throw new ArgumentNullException("owner");

			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			Owner = owner;
			Name = name;
		}

		public Class Owner { get; private set; }

		public string Name { get; private set; }

		public string Caption
		{
			get { return Owner.Type.GetCaption(Name); }
		}
	}
}