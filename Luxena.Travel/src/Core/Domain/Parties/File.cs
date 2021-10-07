using System;


namespace Luxena.Travel.Domain
{

	public partial class File : Entity
	{
		public virtual string FileName { get; set; }

		[ReadOnly]
		public virtual DateTime TimeStamp { get; set; }

		public virtual byte[] Content { get; set; }

		public virtual Person UploadedBy { get; set; }

		public virtual Party Party { get; set; }
	}

}
