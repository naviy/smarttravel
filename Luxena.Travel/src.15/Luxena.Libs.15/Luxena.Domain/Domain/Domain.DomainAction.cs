using System.ComponentModel.DataAnnotations;


namespace Luxena.Domain
{
	
	partial class Domain<TDomain>
	{

		public abstract class DomainAction
		{
			public TDomain db;

			[Key]
			public string Id { get; set; }

			public int? Version { get; set; }


			public virtual void CalculateDefaults() { }

			public abstract void Calculate();

			public abstract void Execute();

			//public bool IsNew()
			//{
			//	return Id.No();
			//}
		}

	}

}
