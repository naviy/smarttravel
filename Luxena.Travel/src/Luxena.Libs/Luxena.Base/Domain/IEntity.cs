
using System;

namespace Luxena.Base.Domain
{

	public interface IEntity
	{
		object Id { get; set; }
		int Version { get; set; }
	}

	public interface IEntity2 : IEntity, ICreateAware, IModifyAware, ICloneable
	{
	}

	public interface IEntity3 : IEntity2, INamedEntity
	{
	}

	public interface IEntity3D : IEntity3, IDescriptedEntity
	{
	}



	public interface INamedEntity
	{
		string Name { get; set; }
	}

	public interface IDescriptedEntity
	{
		string Description { get; set; }
	}

}
