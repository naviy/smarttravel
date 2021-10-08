using System;


namespace Luxena.Base.Domain
{
	public interface IModifyAware
	{
		DateTime? ModifiedOn { get; set; }
		string ModifiedBy { get; set; }
	}
}