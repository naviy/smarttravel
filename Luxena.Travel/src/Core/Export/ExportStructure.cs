using System;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Export
{
	[Serializable]
	public class ExportStructure
	{
		public ExportStructure()
		{
		}

		public ExportStructure(ExportField[] fields)
		{
			Fields = fields;
			MoneyDefaultAmountWidth = 11;
			MoneyDefaultCurrencyWidth = 6;
			MoneyDefaultFormat = "#,##0.00";
		}

		public ExportField[] Fields { get; set; }
		
		public bool DisplayCurrency { get; set; }
		public Currency DefaultCurrency { get; set; }
		public int MoneyDefaultCurrencyWidth { get; set; }
		public int MoneyDefaultAmountWidth { get; set; }
		public string MoneyDefaultFormat { get; set; }

		public SerializableDictionary<string, string> DocumentTypeMapping { get; set; }
	}
}
