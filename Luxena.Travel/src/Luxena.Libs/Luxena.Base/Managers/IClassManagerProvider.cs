using Luxena.Base.Metamodel;

namespace Luxena.Base.Managers
{
	public interface IClassManagerProvider
	{
		GenericManager GetClassManager(Class cls);
	}
}