using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{
	[DataContract]
	public class DayStats
	{
		public DateTime Date { get; set; }
		public string DateText { get; set; }
		public long Unprocessed { get; set; }
		public long Total { get; set; }
		public long Void { get; set; }
		public string ReportUrl { get; set; }
	}
}