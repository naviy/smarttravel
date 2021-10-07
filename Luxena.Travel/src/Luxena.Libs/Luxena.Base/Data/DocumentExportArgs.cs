using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{
	[DataContract]
	public class DocumentExportArgs
	{
		public DocumentExportMode Mode { get; set; }

		public RangeRequest Request { get; set; }

		public object[] SelectedDocuments { get; set; }
	}

	[DataContract]
	public enum DocumentExportMode
	{
		All,
		Selected,
		ExceptSelected
	}
}