using Luxena.Base.Metamodel;


namespace Luxena.Base.Domain
{

	public abstract class Entity3 : Entity2, IEntity3
	{

		[Reference]
		public virtual string Name { get; set; }


		public override string ToString()
		{
			return Name;
		}

	}

	public abstract class Entity3D : Entity3, IEntity3D
	{

		public virtual string Description { get; set; }

	}

}