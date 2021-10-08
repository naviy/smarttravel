namespace Luxena.Travel.Domain
{

	public partial class SirenaFile : GdsFile
	{
		public override GdsFileType FileType
		{
			get { return GdsFileType.SirenaFile; }
		}
	}

}