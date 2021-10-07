using System;
using System.Diagnostics;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{

	[DataContract]
	[DebuggerDisplay("{Name} (Id: {Id}, Type: {Type})")]
	public class EntityReference
	{
		public EntityReference() { }

		public EntityReference(string type, object id, string name)
		{
			Type = type;
			Id = id;
			Name = name;
		}

		//public Reference(object entity, object id, string text)
		//{
		//	var entityType = NHibernateUtil.GetClass(entity);
		//	Type = entityType.Name.As(a => a.Substring(0, 1).ToLower() + a.Substring(1));
		//	Id = id;
		//	Text = text;
		//}

		public EntityReference(IEntity entity)
		{
			var cls = entity.GetClass();

			if (cls.EntityNameProperty == null)
				throw new ArgumentException($"Reference property must be set in class {cls}");

			Id = entity.Id;
			Type = cls.Id;

			var name = cls.EntityNameProperty.GetValue(entity);
			if (name != null)
				Name = name.ToString();
			else
			{
				var entity3 = entity as INamedEntity;
				if (entity3 != null)
					Name = entity3.Name;
			}
		}

		public EntityReference(IEntity entity, string name)
		{
			var cls = entity.GetClass();

			Id = entity.Id;
			Name = name;
			Type = cls.Id;
		}

		public const int IdPos = 0;
		public const int NamePos = 1;
		public const int TypePos = 2;

		public object Id { get; set; }

		public string Name
		{
			get { return _name; }
			set { _name = string.IsNullOrWhiteSpace(value) ? null : value.Trim(); }
		}
		private string _name;

		public string Type { get; set; }


		public static object[] ToArray(IEntity entity)
		{
			var info = new EntityReference(entity);

			var obj = new object[3];

			obj[IdPos] = info.Id;
			obj[NamePos] = info.Name;
			obj[TypePos] = info.Type;

			return obj;
		}

		public static object[] FromArray(object id, object name, string type)
		{
			var obj = new object[3];

			obj[IdPos] = id;
			obj[NamePos] = name;
			obj[TypePos] = type;

			return obj;
		}

		public override string ToString()
		{
			return Name ?? (Id?.ToString() ?? base.ToString());
		}

		public static implicit operator bool(EntityReference me) => me != null;
	}


	public static  class ReferenceExtentions
	{
		//public static string AsName(this EntityReference me)
		//{
		//	return me?.Name;
		//}
	}

}