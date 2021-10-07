using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class ClassDefinition
	{
		public string ClassId { get; set; }

		public string Caption { get; set; }

		public string ListCaption { get; set; }
	}
}