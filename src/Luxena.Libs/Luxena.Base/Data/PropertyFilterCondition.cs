using Luxena.Base.Serialization;




namespace Luxena.Base.Data
{



	[DataContract]
	public class PropertyFilterCondition
	{

		public bool Not { get; set; }

		public FilterOperator Operator { get; set; }

		public object Value { get; set; }
		
		public PropertyFilter[] Items { get; set; }

	}



}