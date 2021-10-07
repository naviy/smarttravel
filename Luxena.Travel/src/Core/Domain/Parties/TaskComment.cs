namespace Luxena.Travel.Domain
{

	public partial class TaskComment : Entity2
	{
		public virtual Task Task { get; set; }

		public virtual string Text { get; set; }
	}

}