using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{
	[DataContract]
	public class ObjectStateInfo : EntityReference
	{
		public ObjectStateInfo()
		{
		}

		protected ObjectStateInfo(Entity2 entity, object state) : base(entity)
		{
			State = state;
		}

		public static ObjectStateInfo Create(Entity2 entity, object state)
		{
			if (entity == null)
				return null;

			return new ObjectStateInfo(entity, state);
		}

		public object State { get; set; }
	}
}