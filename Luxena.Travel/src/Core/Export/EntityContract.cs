using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract]
	public class EntityContract
	{

		public EntityContract() { }

		public EntityContract(Entity2 r)
		{
			Id = r.Id?.ToString();
		}

		[DataMember]
		public string Id { get; set; }
	}

	
	[DataContract]
	public class Entity3Contract : EntityContract
	{
		public Entity3Contract() { }

		public Entity3Contract(Entity3 r): base (r)
		{
			Name = r.Name;
		}

		[DataMember]
		public string Name { get; set; }
	}

}