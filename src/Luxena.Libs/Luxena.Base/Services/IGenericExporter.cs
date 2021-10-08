using Luxena.Base.Data;
using Luxena.Base.Metamodel;

namespace Luxena.Base.Services
{
	public interface IGenericExporter
	{
		byte[] Export(Class clazz, string[] visibleProperties, RecordConfig config, object[] list);
	}
}
