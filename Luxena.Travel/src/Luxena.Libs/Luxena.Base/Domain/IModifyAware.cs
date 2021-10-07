using System;


namespace Luxena.Base.Domain
{
	public interface ICreateAware
	{
		DateTime CreatedOn { get; set; }
		string CreatedBy { get; set; }
	}
}