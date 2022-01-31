namespace Luxena.Travel.Domain
{

	public partial class PrintFile : GdsFile
	{
		public override GdsFileType FileType => GdsFileType.PrintFile;

		public virtual string FilePath { get; set; }

		public virtual string UserName { get; set; }

		public virtual string Office { get; set; }
	}

}