using System.Runtime.Serialization;

using Luxena.Base.Metamodel;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract]
	public class EntityReference
	{
		public EntityReference()
		{
		}

		public EntityReference(Entity2 entity)
		{
			Id = entity.Id.ToString();
			var cls = entity.GetClass();
			Text = cls.EntityNameProperty == null ? entity.ToString() : cls.EntityNameProperty.GetString(entity);
			Type = cls.Type.Name;
		}

		[DataMember] public string Id { get; set; }
		[DataMember] public string Text { get; set; }
		[DataMember] public string Type { get; set; }


		public static implicit operator EntityReference (Entity2 me)
		{
			return me == null ? null : new EntityReference(me);
		}
	}




	[DataContract]
	public class PartyReference : EntityReference
	{

		[DataMember]
		public string LegalName { get; set; }

		[DataMember]
		public string Code { get; set; }

		[DataMember]
		public string BonusCardNumber { get; set; }


		public PartyReference() { }

		public PartyReference(Party r)
		{
			Id = r.Id.ToString();
			Text = r.Name;
			Type = r.Type.ToString();
			LegalName = r.LegalName;
			Code = r.Code;
			BonusCardNumber = (r as Person)?.BonusCardNumber;
		}


		public static implicit operator PartyReference(Party me)
		{
			return me == null ? null : new PartyReference(me);
		}

	}




	[DataContract]
	public class ProductReference : EntityReference
	{
		public ProductReference() { }

		public ProductReference(Product r)
		{
			Id = r.Id.ToString();
			Text = r.Name;
			Type = r.Type.ToString();
		}

		public static implicit operator ProductReference(Product me)
		{
			return me == null ? null : new ProductReference(me);
		}
	}


}