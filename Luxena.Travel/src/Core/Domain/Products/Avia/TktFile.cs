using Luxena.Base.Metamodel;


namespace Luxena.Travel.Domain
{

	public partial class TktFile : GdsFile
	{
		public override GdsFileType FileType => GdsFileType.TktFile;

		[ReadOnly]
		public virtual string OfficeCode { get; set; }

		[ReadOnly]
		public virtual string OfficeIata { get; set; }
	}

}