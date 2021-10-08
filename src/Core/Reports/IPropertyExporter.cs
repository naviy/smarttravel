

namespace Luxena.Travel.Reports
{
	public interface IPropertyExporter
	{
		string ToCsv(object value, string separator);
	}
}