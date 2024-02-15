using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using java.io;
using java.util;

using Luxena.Base.Metamodel;
using Luxena.Travel.Domain;
using Luxena.Travel.Parsers;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;
using org.apache.poi.ss.util;


namespace Luxena.Travel.Export
{

	public class AviaDocumentExcelBuilder// : DomainService
	{

		public Domain.Domain db { get; set; }


		public AviaDocumentExcelBuilder(string exportConfig)
		{
			var serializer = new XmlSerializer(typeof(ExportStructure));

			using (var reader = XmlReader.Create(exportConfig.ResolvePath(), new XmlReaderSettings()))
				_structure = (ExportStructure)serializer.Deserialize(reader);
		}


		public byte[] Make(IList<Product> products)
		{

			_products = products;

			var needRates = _structure.Fields.Any(a => 
				a.PropertyName.EndsWith("_USD", "_EUR", "_RUB") ||
				(a.ChildFields?.Any(b => b.PropertyName.EndsWith("_USD", "_EUR", "_RUB")) ?? false)
			);


			if (needRates)
			{
				var minDate = _products.Min(a => a.IssueDate);
				var maxDate = _products.Max(a => a.IssueDate);

				var rates = db.CurrencyDailyRate.Query.Where(a => a.Date >= minDate && a.Date <= maxDate).ToDictionary(a => a.Date);

				_products.ForEach(a => a.SetRate(rates.By(a.IssueDate)));
			}


			var vatRate = db.Configuration.VatRate;
			_products.ForEach(a => a.SetVatRate(vatRate));


			return ExportDocuments();

		}


		private void CreateStyles()
		{
			Font font = _workbook.createFont();
			font.setBoldweight(Font.__Fields.BOLDWEIGHT_BOLD);

			_headerStyle = _workbook.createCellStyle();
			_headerStyle.setFont(font);
			_headerStyle.setAlignment(CellStyle.__Fields.ALIGN_CENTER);
			_headerStyle.setVerticalAlignment(CellStyle.__Fields.VERTICAL_CENTER);
			_headerStyle.setWrapText(true);
		}


		private byte[] ExportDocuments()
		{

			_workbook = new HSSFWorkbook();

			CreateStyles();

			_sheet = _workbook.createSheet(DomainRes.AllProduct_Caption_List);

			InitMaxHeaderDepth();


			for (var i = 0; i < _maxHeaderDepth + 1; i++)
			{
				CreateHeaderRows(i);
			}


			foreach (var field in _structure.Fields)
			{
				CreateHeaderCells(field, 0);
			}

			var rowIndex = _maxHeaderDepth;


			foreach (var product in _products)
			{
				rowIndex++;
				CreateDataCells(product, CreateDataRow(rowIndex));
			}


			using (var stream = new ByteArrayOutputStream())
			{
				_workbook.write(stream);

				return stream.toByteArray();
			}

		}


		private void InitMaxHeaderDepth()
		{
			foreach (var field in _structure.Fields)
			{
				CalculateDepth(field, 0);
			}
		}

		private static void SetCellValue(ref Cell cell, object fieldValue, Type fieldType)
		{
			if (fieldValue == null)
				return;

			if (fieldType == typeof(int) || fieldType == typeof(decimal))
				cell.setCellValue((double)((decimal)fieldValue));
			else if (fieldType == typeof(DateTime))
			{
				var dateTime = (DateTime)fieldValue;
				cell.setCellValue(new GregorianCalendar(dateTime.Year, dateTime.Month - 1, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second).getTime());
			}
			else if (fieldType == typeof(bool))
				cell.setCellValue((bool)fieldValue);
			else
				cell.setCellValue(fieldValue.ToString());
		}

		private void SetCellStyle(ref Cell cell, string fieldFormat)
		{
			if (fieldFormat.No()) return;

			HSSFCellStyle cellStyle;

			if (!_cellStyleDictionary.ContainsKey(fieldFormat))
			{
				DataFormat format = _workbook.createDataFormat();
				cellStyle = _workbook.createCellStyle();
				cellStyle.setDataFormat(format.getFormat(fieldFormat));
				_cellStyleDictionary.Add(fieldFormat, cellStyle);
			}

			if (_cellStyleDictionary.TryGetValue(fieldFormat, out cellStyle))
				cell.setCellStyle(cellStyle);
		}

		private static Money GetMoneyByFormula(ExportField field, Product r)
		{
			var operators = new[] { '+', '-' };

			IList<string> propertiesSequence = new List<string>();
			IList<char> operatorsSequence = new List<char>();

			var propertyName = string.Empty;
			field.Formula = field.Formula.Replace(" ", "");

			for (var i = 0; i < field.Formula.Length; i++)
			{
				if (operators.Contains(field.Formula[i]))
				{
					operatorsSequence.Add(field.Formula[i]);
					propertiesSequence.Add(propertyName);
					propertyName = string.Empty;
				}
				else
					propertyName += field.Formula[i];

				if (i == field.Formula.Length - 1)
					propertiesSequence.Add(propertyName);
			}

			var property = Class.Of(r).TryGetProperty(propertiesSequence[0]);
			if (property == null)
				return null;

			var result = (Money)property.GetValue(r);

			if (propertiesSequence.Count != operatorsSequence.Count + 1 || propertiesSequence.Count <= 1)
				return result;

			for (var i = 0; i < propertiesSequence.Count - 1; i++)
			{
				property = Class.Of(r).TryGetProperty(propertiesSequence[i + 1]);

				var val = (Money)property?.GetValue(r);
				if (val == null)
					continue;

				if (operatorsSequence[i] == '+')
					result += val;
				else if (operatorsSequence[i] == '-')
					result -= val;
			}

			return result;
		}

		private Row CreateDataRow(int rowIndex)
		{
			var row = _sheet.createRow(rowIndex);
			row.setHeight(300);
			return row;
		}

		private void CreateDataCells(Product r, Row row)
		{

			var cls = r.GetClass();
			var sign = r.IsRefund ? -1 : 1;
			//if (r as AviaTicket != null)
			//	cls = Class.Of(typeof (AviaTicket));
			//else if (r as AviaRefund != null)
			//{
			//	cls = Class.Of(typeof (AviaRefund));
			//	sign = -1;
			//}
			//else if (r as AviaMco != null)
			//	cls = Class.Of(typeof (AviaMco));
			//else
			//cls = Class.Of(typeof (AviaDocument));



			var fieldIndex = 0;

			foreach (var field in _dataFields)
			{

				var localFieldIndex = fieldIndex;
				fieldIndex += field.DataFieldCount;


				object value = field.ValueConst;

				if (value != null)
				{
					var cell = row.createCell(localFieldIndex);
					SetCellValue(ref cell, value, typeof(string));
					continue;
				}


				var propertiesNames = field.PropertyName.Split('.');

				var property = cls.TryGetProperty(propertiesNames[0]);

				if (property == null)
					continue;


				if (propertiesNames.Length == 1)
				{
					value = field.Formula.Yes() ? GetMoneyByFormula(field, r) : property.GetValue(r);
				}
				else
				{

					var obj = property.GetValue(r);
					if (obj == null)
						continue;

					var propertyClazz = Class.Of(obj);
					property = propertyClazz.TryGetProperty(propertiesNames[1]);

					if (property == null)
						continue;

					value = property.GetValue(obj);

				}


				var fieldType = property.Type;

				if ((value == null && fieldType != typeof(Money)) || r.IsVoid && _notShowIfVoidProperties.Contains(field.PropertyName))
				{
					continue;
				}


				if (fieldType == typeof(Money))
				{

					var cellAmount = row.createCell(localFieldIndex);

					SetCellStyle(ref cellAmount, field.ExcelFormat.No() ? _structure.MoneyDefaultFormat : field.ExcelFormat);

					SetCellValue(ref cellAmount, ((Money)value)?.Amount * sign ?? 0.0m, typeof(decimal));

					if (_structure.DisplayCurrency)
					{
						var cellCurrency = row.createCell(++localFieldIndex);
						SetCellValue(ref cellCurrency, value == null ? "" : ((Money)value).Currency, typeof(string));
					}
					else if (_structure.DefaultCurrency != null && value != null && !((Money)value).Currency.Equals(_structure.DefaultCurrency))
					{
						SetCellValue(ref cellAmount, ((Money)value).Amount + " " + ((Money)value).Currency,
							typeof(string));
					}

				}

				else if (field.PropertyName == "Type")
				{

					var cell = row.createCell(localFieldIndex);
					if (r.IsVoid)
						value = "Void";
					var type = value.ToString();

					if (_structure.DocumentTypeMapping != null)
						_structure.DocumentTypeMapping.TryGetValue(value.ToString(), out type);

					SetCellValue(ref cell, type, typeof(string));

				}

				else if (field.PropertyName == "Remarks" && r.IsAviaDocument)
				{

					var doc = (AviaDocument)r;

					if (doc.Origin == ProductOrigin.AmadeusAir)
					{
						var remarks = GetAirRemarks(doc.Remarks);

						row.createCell(localFieldIndex).setCellValue(remarks[0]);
						row.createCell(++localFieldIndex).setCellValue(remarks[1]);
						row.createCell(++localFieldIndex).setCellValue(remarks[2]);
						row.createCell(++localFieldIndex).setCellValue(remarks[3]);
					}
					else
					{
						row.createCell(localFieldIndex).setCellValue(doc.Remarks);
					}

				}

				else
				{

					var cell = row.createCell(localFieldIndex);
					SetCellStyle(ref cell, field.ExcelFormat);

					if (_percentProperties.Contains(field.PropertyName))
					{
						value = ((decimal?)value == 0 ? null : (decimal?)value) / 100;
					}

					SetCellValue(ref cell, value, fieldType);

				}

			}

		}



		private void CalculateDepth(ExportField field, int childDepth)
		{
			var childs = field.ChildFields;
			if (childs == null) return;

			childDepth++;

			if (_maxHeaderDepth < childDepth)
				_maxHeaderDepth = childDepth;

			foreach (var child in childs)
			{
				CalculateDepth(child, childDepth);
			}
		}

		private void CreateHeaderRows(int rowIndex)
		{
			var row = _sheet.createRow(rowIndex);
			row.setHeight(500);
			_headerRows.Add(row);
		}

		private void CreateHeaderCells(ExportField field, int rowFrom)
		{

			if ((field.ChildFields == null) || (field.ChildFields.Length == 0))
			{
				CreateHeaderCell(rowFrom, field);
				_sheet.addMergedRegion(new CellRangeAddress(rowFrom, _maxHeaderDepth, _currentColumnIndex - 1, _currentColumnIndex - 1));
			}
			else
			{
				var colFrom = _currentColumnIndex;

				foreach (var child in field.ChildFields)
				{
					CreateHeaderCell(rowFrom, colFrom, field.Caption);

					CreateHeaderCells(child, (rowFrom + 1));
				}

				var rowTo = field.ChildFields.First().Caption.No() ? rowFrom + 1 : rowFrom;

				_sheet.addMergedRegion(new CellRangeAddress(rowFrom, rowTo, colFrom, _currentColumnIndex - 1));
			}

		}


		private void CreateHeaderCell(int r1, ExportField field)
		{
			Property property = null;
			field.DataFieldCount = 1;

			if (field.PropertyName.Yes())
			{
				var propertiesNames = field.PropertyName.Split('.');

				if (propertiesNames.Length == 1)
				{
					if ((property = Class.Of(typeof(AviaDocument)).TryGetProperty(field.PropertyName)) == null)
						if ((property = Class.Of(typeof(AviaTicket)).TryGetProperty(field.PropertyName)) == null)
							if ((property = Class.Of(typeof(AviaRefund)).TryGetProperty(field.PropertyName)) == null)
								if ((property = Class.Of(typeof(AviaMco)).TryGetProperty(field.PropertyName)) == null)
									return;
				}
			}

			if ((property != null) && property.Type == typeof(Money))
			{
				var cellAmount = _headerRows[r1].createCell(_currentColumnIndex);
				cellAmount.setCellStyle(_headerStyle);
				_sheet.setColumnWidth(_currentColumnIndex, _structure.MoneyDefaultAmountWidth * 256);
				cellAmount.setCellValue(field.Caption);

				if (_structure.DisplayCurrency)
				{
					_currentColumnIndex++;
					_headerRows[r1].createCell(_currentColumnIndex);
					_sheet.setColumnWidth(_currentColumnIndex, _structure.MoneyDefaultCurrencyWidth * 256);

					_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex - 1, _currentColumnIndex));

					field.DataFieldCount = 2;
				}
			}
			else if (field.PropertyName == "Remarks")
			{
				var cellRm = _headerRows[r1].createCell(_currentColumnIndex);
				_sheet.setColumnWidth(_currentColumnIndex, field.Width * 256);
				cellRm.setCellStyle(_headerStyle);
				cellRm.setCellValue("RM");
				_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));
				var cellRm1 = _headerRows[r1].createCell(++_currentColumnIndex);
				_sheet.setColumnWidth(_currentColumnIndex, field.Width * 256);
				cellRm1.setCellStyle(_headerStyle);
				cellRm1.setCellValue("RM1");
				_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));
				var cellRm2 = _headerRows[r1].createCell(++_currentColumnIndex);
				_sheet.setColumnWidth(_currentColumnIndex, field.Width * 256);
				cellRm2.setCellStyle(_headerStyle);
				cellRm2.setCellValue("RM2");
				_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));
				var cellRm3 = _headerRows[r1].createCell(++_currentColumnIndex);
				_sheet.setColumnWidth(_currentColumnIndex, field.Width * 256);
				cellRm3.setCellStyle(_headerStyle);
				cellRm3.setCellValue("RM3");
				_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));

				field.DataFieldCount = 4;
			}
			else
			{
				var cell = _headerRows[r1].createCell(_currentColumnIndex);
				_sheet.setColumnWidth(_currentColumnIndex, field.Width * 256);
				cell.setCellStyle(_headerStyle);
				cell.setCellValue(field.Caption);
			}

			_dataFields.Add(field);

			_currentColumnIndex++;
		}

		private void CreateHeaderCell(int r1, int c1, string text)
		{
			var cell = _headerRows[r1].createCell(c1);
			cell.setCellValue(text);
			cell.setCellStyle(_headerStyle);
		}

		private static string[] GetAirRemarks(string remark)
		{
			var lines = new LinesEnumerator(new System.IO.StringReader(remark));
			var remarks = new List<List<string>>(4)
			{
				new List<string>(),
				new List<string>(),
				new List<string>(),
				new List<string>()
			};

			while (lines.MoveNext())
			{
				if (lines.Current.StartsWith("ID1/"))
					remarks[1].Add(lines.Current.Substring(4));
				else if (lines.Current.StartsWith("ID2/"))
					remarks[2].Add(lines.Current.Substring(4));
				else if (lines.Current.StartsWith("ID3/"))
					remarks[3].Add(lines.Current.Substring(4));
				else
					remarks[0].Add(lines.Current);
			}

			const string separator = "#";

			var strings = new string[4];
			strings[0] = string.Join(separator, remarks[0].ToArray());
			strings[1] = string.Join(separator, remarks[1].ToArray());
			strings[2] = string.Join(separator, remarks[2].ToArray());
			strings[3] = string.Join(separator, remarks[3].ToArray());

			return strings;
		}


		private IList<Product> _products;

		private HSSFWorkbook _workbook;
		private Sheet _sheet;
		private HSSFCellStyle _headerStyle;
		private readonly IDictionary<string, HSSFCellStyle> _cellStyleDictionary = new Dictionary<string, HSSFCellStyle>();

		private readonly ExportStructure _structure;
		private int _maxHeaderDepth;

		private readonly IList<Row> _headerRows = new List<Row>();
		private int _currentColumnIndex;
		private readonly IList<ExportField> _dataFields = new List<ExportField>();

		private readonly string[] _notShowIfVoidProperties =
		{
			"Itinerary", "Departure", "SegmentClasses", "Fare", "EqualFare", "CancelFee", "FeesTotal", "Total", "Vat",
			"CommissionPercent", "Commission", "CancelCommissionPercent", "TotalToTransfer", "ServiceFee", "Discount", "GrandTotal", "PaymentType", "Customer.Id",
			"Intermediary.Id", "Note", "Handling"
		};

		private readonly string[] _percentProperties = { "CancelCommissionPercent", "CommissionPercent" };

	}

}