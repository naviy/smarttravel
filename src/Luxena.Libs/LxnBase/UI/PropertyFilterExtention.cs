using LxnBase.Data;


namespace LxnBase.UI
{
	public static class PropertyFilterExtention
	{
		public static PropertyFilter CreateFilter(string propertyName, FilterOperator op, object value, bool not)
		{
			PropertyFilterCondition condition = new PropertyFilterCondition();
			condition.Not = not;
			condition.Operator = op;
			condition.Value = value;

			PropertyFilter filter = new PropertyFilter();
			filter.Property = propertyName;
			filter.Conditions = new PropertyFilterCondition[] { condition };

			return filter;
		}
	}
}