using System;

using Luxena.Base.Metamodel;


namespace Luxena.Travel.Domain
{

	[SupervisorPrivileges(
		Create = new object[0]
	)]
	public partial class IssuedConsignment : Entity
	{
		public virtual string Number { get; set; }

		[EntityName]
		[ReadOnly]
		[DisplayFormat("d.m.Y H:i")]
		public virtual DateTime TimeStamp { get; set; }

		[ReadOnly]
		public virtual byte[] Content { get; set; }

		public virtual Consignment Consignment { get; set; }

		public virtual Person IssuedBy { get; set; }

		public override string ToString()
		{
			return Number;
		}

	}

}