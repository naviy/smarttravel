namespace LxnBase.Data
{
	public partial class Reference
	{
		public static Reference Create(string type, string name, object id)
		{
			Reference info = new Reference();

			info.Type = type;
			info.Name = name;
			info.Id = id;

			return info;
		}

		public static Reference CreateFromArray(object array)
		{
			Reference info = new Reference();

			object[] o = (object[])array;

			info.Type = (string)o[TypePos];
			info.Name = (string)o[NamePos];
			info.Id = o[IdPos];

			return info;
		}

		public static Reference Copy(Reference source)
		{
			if (source == null)
				return null;

			Reference info = new Reference();

			info.Type = source.Type;
			info.Name = source.Name;
			info.Id = source.Id;

			return info;
		}

		public static bool Equals(Reference obj1, Reference obj2)
		{
			return obj1 != null && obj2 != null && obj1.Id == obj2.Id;
		}
	}
}