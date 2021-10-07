using Luxena.Base.Data;


namespace Luxena.Domain.Entities
{

	public interface IEntityPermissions
	{
		OperationStatus CanCreate();

		OperationStatus CanCopy();

		OperationStatus CanDelete();

		OperationStatus CanList();

		OperationStatus CanReplace();

		OperationStatus CanUpdate();

		OperationStatus CanView();
	}

}