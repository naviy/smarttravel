using System;

using Luxena.Travel.Domain;

using CellBorderStyle = NPOI.SS.UserModel.BorderStyle;


namespace Luxena.Travel.Reports
{
	public class CashOrderFormRussian : ICashOrderForm
	{
		public Domain.Domain db { get; set; }

		public byte[] Print(CashInOrderPayment payment)
		{
			var company = db.Configuration.Company.NameForDocuments;
			var accountant = db.Configuration.AccountantDisplayString;

			var dateShort = payment.Date.ToString("dd.MM.yyyy");
			var dateMiddle = payment.Date.ToString("dd MMMM yyyy г.");

			var user = db.Security.User;
			var cashier = user == null ? null : user.Person.NameForDocuments;

			var reason = payment.GetReason();

			var amount = payment.Amount.ToWords();

			var including = "НДС " + ToString(payment.Vat == null ? 0 : payment.Vat.Amount) + " руб.";

			var xls = new XlsBuilder();

			xls.NewSheet("ПКО");

			xls.Widths(7.86, 4.71, 7.86, 7.43, 6.43, 9.43, 0.25, 6.14, 3.14, 0.83, 0.83, 7.14, 3.43, 12.43);

			var font6 = xls.NewFont().Size(6);
			var font7 = xls.NewFont().Size(7);
			var font7Bold = xls.NewFont().Size(7).Bold();
			var font8 = xls.NewFont().Size(8);
			var font8Bold = xls.NewFont().Size(8).Bold();

			xls.AddRow(11.25)
				.NewStyle(font6).Right()
				.AddCell("Унифицированная форма КО-1").Merge(9);

			xls.AddRow(11.25)
				.AddCell("Утверждена постановлением Госкомстата России от 18.08.98 №88").Merge(9)
				.Skip(2)
				.NewStyle(font7).Center()
				.AddCell(company).Merge(3);

			xls.AddRow(7.5)
				.Skip(11)
				.NewStyle(font6).Center().Top().BorderTop(CellBorderStyle.THIN)
				.AddCell("организация").Merge(3);

			xls.AddRow(12.75)
				.Skip(7)
				.NewStyle(font7).Center().Middle().Border(CellBorderStyle.THIN, CellBorderStyle.THIN, CellBorderStyle.NONE)
				.AddCell("Коды").Merge(2)
				.Skip(2)
				.NewStyle(font8Bold).Center().Middle()
				.AddCell("КВИТАНЦИЯ").Merge(3);

			xls.AddRow(18.75)
				.NewStyle(font8).Right().Middle()
				.AddCell("Форма по ОКУД").Merge(6)
				.Skip()
				.NewStyle(font8Bold).Center().Middle().Border(CellBorderStyle.MEDIUM, CellBorderStyle.MEDIUM, CellBorderStyle.THIN)
				.AddCell("0310001").Merge(2)
				.Skip(2)
				.NewStyle(font7).Center().BorderBottom(CellBorderStyle.THIN)
				.AddCell("к приходному кассовому ордеру\n№ " + payment.DocumentNumber).Merge(3);

			xls.AddRow(18.75)
				.NewStyle(font8Bold)
				.AddCell(company).Merge(5)
				.NewStyle(font8).Right().Middle()
				.AddCell("по ОКПО")
				.Skip()
				.NewStyle(font8Bold).Center().Middle().Border(CellBorderStyle.THIN, CellBorderStyle.MEDIUM)
				.AddCell().Merge(2)
				.Skip(2)
				.NewStyle(font7).Right()
				.AddCell("от")
				.NewStyle(font7Bold).BorderBottom(CellBorderStyle.THIN)
				.AddCell(dateMiddle).Merge(2);

			xls.AddRow(7.5)
				.NewStyle(font6).Center().Top().BorderTop(CellBorderStyle.THIN)
				.AddCell("организация").Merge(5)
				.Skip(2)
				.NewStyle(font8Bold).Center().Middle().Border(CellBorderStyle.THIN, CellBorderStyle.MEDIUM, CellBorderStyle.NONE)
				.AddCell().Merge(2, 2);

			xls.AddRow(11.25)
				.NewStyle(font8Bold)
				.AddCell().Merge(5)
				.Skip(2)
				.NewStyle(font8Bold).Center().Middle().Border(CellBorderStyle.NONE, CellBorderStyle.MEDIUM, CellBorderStyle.MEDIUM)
				.AddCell().Merge(2)
				.Skip(2)
				.NewStyle(font7).AddCell("Принято от").Merge(3);

			xls.AddRow(8.25)
				.NewStyle(font6).Center().Top().BorderTop(CellBorderStyle.THIN)
				.AddCell("подразделение").Merge(5).Skip(6)
				.NewStyle(font7).Top()
				.AddCell(payment.ReceivedFrom).Merge(3, 2);

			xls.AddRow(20.25)
				.NewStyle(font8Bold).Right()
				.AddCell("ПРИХОДНЫЙ КАССОВЫЙ ОРДЕР").Indention(1).Merge(4)
				.NewStyle(font7).Center().Border(CellBorderStyle.THIN)
				.AddCell("Номер документа").Merge(3)
				.AddCell("Дата составления").Merge(2);

			xls.AddRow(11.25)
				.Skip(4)
				.NewStyle(font8).Center().Middle().Border(CellBorderStyle.MEDIUM)
				.AddCell(payment.DocumentNumber).Merge(3)
				.AddCell(dateShort).Merge(2)
				.Skip(2)
				.NewStyle(font7)
				.AddCell("Основание:").Merge(3, 2);

			xls.AddRow(4.5);

			xls.AddRow(11.25)
				.NewStyle(font7).Center().Middle().Border(CellBorderStyle.THIN, CellBorderStyle.THIN, CellBorderStyle.NONE)
				.AddCell("Дебет").Merge(1, 2)
				.AddCell("Кредит").Merge(4)
				.AddCell("Сумма").Merge(2, 2)
				.NewStyle(font6).Center().Middle().Border(CellBorderStyle.THIN, CellBorderStyle.THIN, CellBorderStyle.NONE)
				.AddCell("Код целевого назначения").Merge(1, 2)
				.AddCell().Merge(1, 2)
				.Skip(2)
				.NewStyle(font7).Top()
				.AddCell(reason).Merge(3, 2);

			xls.AddRow(35.25)
				.NewStyle(font6).Center().Middle().Border(CellBorderStyle.THIN, CellBorderStyle.THIN, CellBorderStyle.NONE)
				.AddCell()
				.AddCell()
				.AddCell("код структурного подразделения")
				.AddCell("корреспонди-рующий счет, субсчет")
				.AddCell("код аналити-ческого учета")
				.AddCell().Merge(2)
				.AddCell()
				.AddCell();

			xls.AddRow(18.75)
				.NewStyle(font7).Center().Middle().Border(CellBorderStyle.MEDIUM, CellBorderStyle.NONE, CellBorderStyle.MEDIUM, CellBorderStyle.MEDIUM)
				.AddCell("50.01")
				.NewStyle(font7).Center().Middle().Border(CellBorderStyle.MEDIUM, CellBorderStyle.NONE, CellBorderStyle.MEDIUM, CellBorderStyle.THIN)
				.AddCell()
				.AddCell()
				.AddCell()
				.AddCell()
				.AddCell(ToString(payment.Amount.Amount)).Merge(2)
				.AddCell()
				.NewStyle(font7).Center().Middle().Border(CellBorderStyle.MEDIUM, CellBorderStyle.MEDIUM, CellBorderStyle.MEDIUM, CellBorderStyle.THIN)
				.AddCell()
				.Skip(2)
				.NewStyle(font7)
				.AddCell("Сумма")
				.NewStyle(font7Bold).BorderBottom(CellBorderStyle.THIN)
				.AddCell(ShortAmount(payment.Amount.Amount)).Merge(2);

			xls.AddRow(11.25)
				.Skip(12)
				.NewStyle(font6).Center().Top().BorderTop(CellBorderStyle.THIN)
				.AddCell("цифрами").Merge(2);

			xls.AddRow(13.50)
				.NewStyle(font7).Top()
				.AddCell("Принято от")
				.AddCell(payment.ReceivedFrom).Merge(8)
				.Skip(2)
				.AddCell(amount).Merge(3, 2);

			xls.AddRow(26.25)
				.NewStyle(font7).Top()
				.AddCell("Основание:")
				.AddCell(reason).Merge(8, 3);

			xls.AddRow(18.75)
				.Skip(11)
				.NewStyle(font7)
				.AddCell("В том числе").Merge(3);

			xls.AddRow(22.50)
				.Skip(11)
				.NewStyle(font7).Top()
				.AddCell(including).Merge(3);

			xls.AddRow(22.50)
				.NewStyle(font7).Top()
				.AddCell("Сумма")
				.AddCell(amount).Merge(8)
				.Skip(3)
				.NewStyle(font7Bold).BorderBottom(CellBorderStyle.THIN)
				.AddCell(dateMiddle).Merge(2);

			xls.AddRow(18.75)
				.NewStyle(font7).Top()
				.AddCell("В том числе")
				.AddCell(including).Merge(8).Skip(2)
				.NewStyle(font7Bold)
				.AddCell("М.П. (штампа)").Merge(3);

			xls.AddRow(18.75)
				.NewStyle(font7).Top()
				.AddCell("Приложение")
				.AddCell(payment.Note).Merge(8).Skip(2)
				.NewStyle(font7Bold)
				.AddCell("Главный бухгалтер").Merge(3);

			xls.AddRow(12.75)
				.NewStyle(font7Bold)
				.AddCell("Главный бухгалтер").Merge(2)
				.NewStyle(font7Bold).BorderBottom(CellBorderStyle.THIN)
				.AddCell().Skip()
				.AddCell(accountant).Merge(4)
				.Skip(5)
				.AddCell(accountant);

			xls.AddRow(11.25)
				.Skip(2)
				.NewStyle(font6).Center().Top().BorderTop(CellBorderStyle.THIN)
				.AddCell("подпись").Skip()
				.AddCell("расшифровка подписи").Merge(4).Skip(3)
				.AddCell("подпись").Skip()
				.AddCell("расшифровка подписи");

			xls.AddRow(12.75)
				.Skip(11)
				.NewStyle(font7Bold)
				.AddCell("Кассир").Merge(3);

			xls.AddRow(12.75)
				.NewStyle(font7Bold).AddCell("Получил кассир").Merge(2)
				.NewStyle(font7Bold).BorderBottom(CellBorderStyle.THIN)
				.AddCell().Skip()
				.AddCell(cashier).Merge(4)
				.Skip(5)
				.AddCell(cashier);

			xls.AddRow(7.5).Skip(2)
				.NewStyle(font6).Center().Top().BorderTop(CellBorderStyle.THIN)
				.AddCell("подпись").Skip()
				.AddCell("расшифровка подписи").Merge(4).Skip(3)
				.AddCell("подпись").Skip()
				.AddCell("расшифровка подписи");

			return xls.GetBytes();
		}

		private static string ShortAmount(decimal amount)
		{
			var roubles = Decimal.Floor(amount);

			return roubles.ToString("N0") + " руб. " + ((amount - roubles) * 100).ToString("00") + " коп.";
		}

		private static string ToString(decimal value)
		{
			return value.ToString("N2");
		}
	}
}