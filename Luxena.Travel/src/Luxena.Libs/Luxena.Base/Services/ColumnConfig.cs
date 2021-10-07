using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class ColumnConfig
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public TypeEnum Type { get; set; }

		public bool IsRequired { get; set; }

		public bool IsReadOnly { get; set; }

		public bool IsPersistent { get; set; }

		public bool IsReference { get; set; }

		public decimal ListWidth { get; set; }

		public bool Hidden { get; set; }

		public object DefaultValue { get; set; }
	}
}