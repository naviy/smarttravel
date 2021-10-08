using System;
using System.Collections.Generic;
using System.Linq;

using java.io;
using java.util;

using Luxena.Base.Metamodel;
using Luxena.Travel.Domain;
using Luxena.Travel.Export;
using Luxena.Travel.Parsers;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;
using org.apache.poi.ss.util;

using StringReader = System.IO.StringReader;


namespace Luxena.Travel.Reports
{

	public class AviaDocumentExcelExporter2011
	{
		private readonly IDictionary<string, HSSFCellStyle> _cellStyleDictionary;
		private readonly IList<ExportField> _dataFields;
		private readonly IList<Row> _headerRows;
		private readonly string[] _notShowIfVoidProperties;
		private readonly string[] _percentProperties;
		private int _currentColumnIndex;
		private IEnumerable<AviaDocument> _documents;
		private HSSFCellStyle _headerStyle;
		private int _maxHeaderDepth;
		private Sheet _sheet;
		private ExportStructure _structure;
		private HSSFWorkbook _workbook;

		public AviaDocumentExcelExporter2011()
		{
			_cellStyleDictionary = new Dictionary<string, HSSFCellStyle>();
			_headerRows = new List<Row>();
			_dataFields = new List<ExportField>();
			_notShowIfVoidProperties = new[]
			{
				"Itinerary", "Departure", "SegmentClasses", "Fare", "EqualFare", "CancelFee", "FeesTotal", "Total", "Vat", "CommissionPercent", "Commission", "CancelCommissionPercent", "TotalToTransfer", "ServiceFee", "Discount", "GrandTotal",
				"PaymentType", "Customer.Id", "Intermediary.Id", "Note", "Handling"
			};
			_percentProperties = new[] { "CancelCommissionPercent", "CommissionPercent" };
		}

		public AviaDocumentExcelExporter2011(ExportStructure structure)
		{
			_cellStyleDictionary = new Dictionary<string, HSSFCellStyle>();
			_headerRows = new List<Row>();
			_dataFields = new List<ExportField>();
			_notShowIfVoidProperties = new[]
			{
				"Itinerary", "Departure", "SegmentClasses", "Fare", "EqualFare", "CancelFee", "FeesTotal", "Total", "Vat", "CommissionPercent", "Commission", "CancelCommissionPercent", "TotalToTransfer", "ServiceFee", "Discount", "GrandTotal",
				"PaymentType", "Customer.Id", "Intermediary.Id", "Note", "Handling"
			};
			_percentProperties = new[] { "CancelCommissionPercent", "CommissionPercent" };
			_structure = structure;
		}

		private void CalculateDepth(ExportField field, int childDepth)
		{
			var childFields = field.ChildFields;
			if (childFields == null) return;

			childDepth++;
			if (_maxHeaderDepth < childDepth)
				_maxHeaderDepth = childDepth;

			foreach (var field2 in childFields)
				CalculateDepth(field2, childDepth);
		}

		private void CreateDataCells(AviaDocument aviaDocument, Row row)
		{
			Class class2;
			var num = 1;
			if (aviaDocument is AviaTicket)
			{
				class2 = Class.Of(typeof(AviaTicket));
			}
			else if (aviaDocument is AviaRefund)
			{
				class2 = Class.Of(typeof(AviaRefund));
				num = -1;
			}
			else if (aviaDocument is AviaMco)
			{
				class2 = Class.Of(typeof(AviaMco));
			}
			else
			{
				class2 = Class.Of(typeof(AviaDocument));
			}

			var num2 = 0;

			foreach (var field in _dataFields)
			{
				var i = num2;
				num2 += field.DataFieldCount;
				object valueConst = field.ValueConst;
				if (valueConst != null)
				{
					var cell = row.createCell(i);
					SetCellValue(ref cell, valueConst, typeof(string));
				}
				else
				{
					string[] strArray = field.PropertyName.Split(new[] { '.' });
					Property property = class2.TryGetProperty(strArray[0]);
					if (property != null)
					{
						if (strArray.Length == 1) { valueConst = !string.IsNullOrEmpty(field.Formula) ? GetMoneyByFormula(field, aviaDocument) : property.GetValue(aviaDocument); }
						else
						{
							var obj3 = property.GetValue(aviaDocument);
							if (obj3 == null) { goto Label_04FB; }
							property = Class.Of(obj3).TryGetProperty(strArray[1]);
							if (property == null) { goto Label_04FB; }
							valueConst = property.GetValue(obj3);
						}
						Type fieldType = property.Type;
						if ((valueConst != null) && (!aviaDocument.IsVoid || !_notShowIfVoidProperties.Contains(field.PropertyName)))
						{
							if (fieldType == typeof(Money))
							{
								var cell2 = row.createCell(i);
								SetCellStyle(ref cell2, string.IsNullOrEmpty(field.ExcelFormat) ? _structure.MoneyDefaultFormat : field.ExcelFormat);
								SetCellValue(ref cell2, ((Money)valueConst).Amount * num, typeof(decimal));
								if (_structure.DisplayCurrency)
								{
									var cell = row.createCell(++i);
									SetCellValue(ref cell, ((Money)valueConst).Currency, typeof(string));
								}
								else if (!(_structure.DefaultCurrency == null || ((Money)valueConst).Currency.Equals(_structure.DefaultCurrency)))
								{
									SetCellValue(ref cell2, ((Money)valueConst).Amount + " " + ((Money)valueConst).Currency, typeof(string));
								}
							}
							else
							{
								Cell cell;
								if (field.PropertyName == "Type")
								{
									cell = row.createCell(i);
									if (aviaDocument.IsVoid) { valueConst = "Void"; }
									string str = valueConst.ToString();
									if (_structure.DocumentTypeMapping != null) { _structure.DocumentTypeMapping.TryGetValue(valueConst.ToString(), out str); }
									SetCellValue(ref cell, str, typeof(string));
								}
								else if (field.PropertyName == "Remarks")
								{
									if (aviaDocument.Origin == ProductOrigin.AmadeusAir)
									{
										string[] airRemarks = GetAirRemarks(aviaDocument.Remarks);
										row.createCell(i).setCellValue(airRemarks[0]);
										row.createCell(++i).setCellValue(airRemarks[1]);
										row.createCell(++i).setCellValue(airRemarks[2]);
										row.createCell(++i).setCellValue(airRemarks[3]);
									}
									else
									{
										row.createCell(i).setCellValue(aviaDocument.Remarks);
									}
								}
								else
								{
									cell = row.createCell(i);
									SetCellStyle(ref cell, field.ExcelFormat);
									if (_percentProperties.Contains(field.PropertyName))
									{
										var nullable = (decimal?)valueConst;
										valueConst = (((nullable.GetValueOrDefault() == 0M) && nullable.HasValue) ? (nullable = null) : ((decimal?)valueConst)) / 100M;
									}
									SetCellValue(ref cell, valueConst, fieldType);
								}
							}
						}
					}
				Label_04FB:
					;
				}
			}
		}

		private Row CreateDataRow(int rowIndex)
		{
			Row row = _sheet.createRow(rowIndex);
			row.setHeight(300);
			return row;
		}

		private void CreateHeaderCell(int r1, ExportField field)
		{
			Property property = null;
			field.DataFieldCount = 1;
			if ((string.IsNullOrEmpty(field.PropertyName) || (field.PropertyName.Split(new[] { '.' }).Length != 1)) || ((((property = Class.Of(typeof(AviaDocument)).TryGetProperty(field.PropertyName)) != null) || ((property = Class.Of(typeof(AviaTicket)).TryGetProperty(field.PropertyName)) != null)) || (((property = Class.Of(typeof(AviaRefund)).TryGetProperty(field.PropertyName)) != null) || ((property = Class.Of(typeof(AviaMco)).TryGetProperty(field.PropertyName)) != null))))
			{
				if ((property != null) && (property.Type == typeof(Money)))
				{
					Cell cell = _headerRows[r1].createCell(_currentColumnIndex);
					cell.setCellStyle(_headerStyle);
					_sheet.setColumnWidth(_currentColumnIndex, _structure.MoneyDefaultAmountWidth * 0x100);
					cell.setCellValue(field.Caption);
					if (_structure.DisplayCurrency)
					{
						_currentColumnIndex++;
						_headerRows[r1].createCell(_currentColumnIndex);
						_sheet.setColumnWidth(_currentColumnIndex, _structure.MoneyDefaultCurrencyWidth * 0x100);
						_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex - 1, _currentColumnIndex));
						field.DataFieldCount = 2;
					}
				}
				else if (field.PropertyName == "Remarks")
				{
					var cell2 = _headerRows[r1].createCell(_currentColumnIndex);
					_sheet.setColumnWidth(_currentColumnIndex, field.Width * 0x100);
					cell2.setCellStyle(_headerStyle);
					cell2.setCellValue("RM");
					_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));

					var cell3 = _headerRows[r1].createCell(++_currentColumnIndex);
					_sheet.setColumnWidth(_currentColumnIndex, field.Width * 0x100);
					cell3.setCellStyle(_headerStyle);
					cell3.setCellValue("RM1");
					_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));

					var cell4 = _headerRows[r1].createCell(++_currentColumnIndex);
					_sheet.setColumnWidth(_currentColumnIndex, field.Width * 0x100);
					cell4.setCellStyle(_headerStyle);
					cell4.setCellValue("RM2");
					_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));

					var cell5 = _headerRows[r1].createCell(++_currentColumnIndex);
					_sheet.setColumnWidth(_currentColumnIndex, field.Width * 0x100);
					cell5.setCellStyle(_headerStyle);
					cell5.setCellValue("RM3");
					_sheet.addMergedRegion(new CellRangeAddress(r1, _maxHeaderDepth, _currentColumnIndex, _currentColumnIndex));
					field.DataFieldCount = 4;
				}
				else
				{
					Cell cell6 = _headerRows[r1].createCell(_currentColumnIndex);
					_sheet.setColumnWidth(_currentColumnIndex, field.Width * 0x100);
					cell6.setCellStyle(_headerStyle);
					cell6.setCellValue(field.Caption);
				}
				_dataFields.Add(field);
				_currentColumnIndex++;
			}
		}

		private void CreateHeaderCell(int r1, int c1, string text)
		{
			Cell cell = _headerRows[r1].createCell(c1);
			cell.setCellValue(text);
			cell.setCellStyle(_headerStyle);
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
				var num = _currentColumnIndex;
				foreach (ExportField field2 in field.ChildFields)
				{
					CreateHeaderCell(rowFrom, num, field.Caption);
					CreateHeaderCells(field2, rowFrom + 1);
				}
				var lastRow = string.IsNullOrEmpty(Enumerable.First(field.ChildFields).Caption) ? (rowFrom + 1) : rowFrom;
				_sheet.addMergedRegion(new CellRangeAddress(rowFrom, lastRow, num, _currentColumnIndex - 1));
			}
		}

		private void CreateHeaderRows(int rowIndex)
		{
			var item = _sheet.createRow(rowIndex);
			item.setHeight(500);
			_headerRows.Add(item);
		}

		private void CreateStyles()
		{
			Font font = _workbook.createFont();
			font.setBoldweight(700);
			_headerStyle = _workbook.createCellStyle();
			_headerStyle.setFont(font);
			_headerStyle.setAlignment(2);
			_headerStyle.setVerticalAlignment(1);
			_headerStyle.setWrapText(true);
		}

		public byte[] Export(IEnumerable<AviaDocument> documents)
		{
			_documents = documents;
			return ExportDocuments();
		}

		private byte[] ExportDocuments()
		{
			_workbook = new HSSFWorkbook();

			CreateStyles();

			_sheet = _workbook.createSheet(DomainRes.AviaDocument_Caption_List);

			InitMaxHeaderDepth();

			for (var i = 0; i < (_maxHeaderDepth + 1); i++)
				CreateHeaderRows(i);

			foreach (var field in _structure.Fields)
				CreateHeaderCells(field, 0);

			var rowIndex = _maxHeaderDepth;

			foreach (var document in _documents)
			{
				rowIndex++;
				CreateDataCells(document, CreateDataRow(rowIndex));
			}

			using (var stream = new ByteArrayOutputStream())
			{
				_workbook.write(stream);
				return stream.toByteArray();
			}

		}

		private static string[] GetAirRemarks(string remark)
		{
			var enumerator = new LinesEnumerator(new StringReader(remark));

			var list = new List<List<string>>(4)
			{
				new List<string>(),
				new List<string>(),
				new List<string>(),
				new List<string>()
			};

			while (enumerator.MoveNext())
			{
				if (enumerator.Current.StartsWith("ID1/"))
					list[1].Add(enumerator.Current.Substring(4));
				else if (enumerator.Current.StartsWith("ID2/"))
					list[2].Add(enumerator.Current.Substring(4));
				else if (enumerator.Current.StartsWith("ID3/"))
					list[3].Add(enumerator.Current.Substring(4));
				else
					list[0].Add(enumerator.Current);
			}

			return new[]
			{
				string.Join("#", list[0].ToArray()), 
				string.Join("#", list[1].ToArray()), 
				string.Join("#", list[2].ToArray()), 
				string.Join("#", list[3].ToArray())
			};
		}

		private static Money GetMoneyByFormula(ExportField field, AviaDocument aviaDocument)
		{
			int num;
			char[] source = { '+', '-' };
			var list = new List<string>();
			var list2 = new List<char>();
			var item = string.Empty;
			field.Formula = field.Formula.Replace(" ", "");
			for (num = 0; num < field.Formula.Length; num++)
			{
				if (source.Contains(field.Formula[num]))
				{
					list2.Add(field.Formula[num]);
					list.Add(item);
					item = string.Empty;
				}
				else
				{
					item = item + field.Formula[num];
				}

				if (num == (field.Formula.Length - 1))
					list.Add(item);
			}

			var property = Class.Of(aviaDocument).TryGetProperty(list[0]);
			if (property == null) return null;

			var money = (Money)property.GetValue(aviaDocument);

			if ((list.Count != (list2.Count + 1)) || (list.Count <= 1))
				return money;

			for (num = 0; num < (list.Count - 1); num++)
			{
				property = Class.Of(aviaDocument).TryGetProperty(list[num + 1]);
				if (property == null) continue;

				var money2 = (Money)property.GetValue(aviaDocument);
				if (money2 == null) continue;

				if (list2[num] == '+')
					money += money2;
				else if (list2[num] == '-')
					money -= money2;
			}
			return money;
		}

		private void InitMaxHeaderDepth()
		{
			foreach (ExportField field in _structure.Fields) { CalculateDepth(field, 0); }
		}

		private void SetCellStyle(ref Cell cell, string fieldFormat)
		{
			if (string.IsNullOrEmpty(fieldFormat)) return;

			HSSFCellStyle style;
			if (!_cellStyleDictionary.ContainsKey(fieldFormat))
			{
				var format = _workbook.createDataFormat();
				style = _workbook.createCellStyle();
				style.setDataFormat(format.getFormat(fieldFormat));
				_cellStyleDictionary.Add(fieldFormat, style);
			}

			if (_cellStyleDictionary.TryGetValue(fieldFormat, out style))
				cell.setCellStyle(style);
		}

		private static void SetCellValue(ref Cell cell, object fieldValue, Type fieldType)
		{
			if (fieldValue == null) return;

			if ((fieldType == typeof(int)) || (fieldType == typeof(decimal)))
			{
				cell.setCellValue((double)((decimal)fieldValue));
			}
			else if (fieldType == typeof(DateTime))
			{
				var time = (DateTime)fieldValue;
				cell.setCellValue(new GregorianCalendar(time.Year, time.Month - 1, time.Day, time.Hour, time.Minute, time.Second).getTime());
			}
			else if (fieldType == typeof(bool))
			{
				cell.setCellValue((bool)fieldValue);
			}
			else
			{
				cell.setCellValue(fieldValue.ToString());
			}
		}

		public void SetStructure(ExportStructure structure)
		{
			_structure = structure;
		}
	}

}