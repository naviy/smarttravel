using Luxena.Base.Domain;


namespace Luxena.Travel.Domain
{
	public abstract partial class Preferences : Entity2
	{
		public virtual Identity Identity { get; set; }

		public override string ToString()
		{
			return GetType().GetCaption();
		}
	}
}