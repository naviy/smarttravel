using Luxena.Base.Serialization;

namespace Luxena.Travel.Domain
{

	[DataContract]
	public class AviaDocumentProcessArgs
	{
		public bool AllowSaveAndContinue { get; set; }

		public bool AllowEditParty { get; set; }

		public bool HasAccessToDocumentList { get; set; }
	}

}