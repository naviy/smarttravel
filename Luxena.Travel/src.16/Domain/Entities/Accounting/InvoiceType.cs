namespace Luxena.Travel.Domain
{

	public enum InvoiceType
	{

		[RU("Счет")]
		Invoice = 0,

		[RU("Квитанция")]
		Receipt = 1,

		[RU("Акт выполненных работ")]
		CompletionCertificate = 2,

	}

}