using System.Collections;


namespace Luxena.Base.Managers
{
	public interface ISecurityContext
	{
		object Identity { get; }

		bool IsValid { get; }

		bool IsGranted(params object[] priveleges);

		bool IsGranted(IEnumerable priveleges);
	}
}