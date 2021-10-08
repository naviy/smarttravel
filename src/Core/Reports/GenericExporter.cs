using System;
using System.Collections.Generic;

using Luxena.Base.Data;
using Luxena.Base.Domain;
using Luxena.Base.Metamodel;
using Luxena.Base.Services;

using java.io;
using java.util;

using Luxena.Travel.Domain;

using org.apache.poi.hssf.usermodel;
using org.apache.poi.ss.usermodel;
using org.apache.poi.ss.util;

using Currency = Luxena.Travel.Domain.Currency;


namespace Luxena.Travel.Reports
{
	public class GenericExporter : IGenericExporter
	{
		public byte[] Export(Class clazz, string[] visibleProperties, RecordConfig config, object[] list)
		{
			if (list == null || list.Length == 0 || visibleProperties.Length == 0 || clazz == null)
				return null;

			_clazz = clazz;

			foreach (var columnConfig in config.Columns)
				_recordProperties.Add(_clazz.GetProperty(columnConfig.Name));
			foreach (var propertyName in visibleProperties)
				_visibleProperties.Add(_clazz.GetProperty(propertyName));

			_list = list;

			return Export();
		}

		private byte[] Export()
		{
			_workbook = new HSSFWorkbook();
			_sheet = _workbook.createSheet(_clazz.ListCaption.Replace('/', ' '));
			CreateStyles();

			var headerRow = _sheet.createRow(0);
			headerRow.setHeight(500);

			var currentColumnIndex = 0;

			foreach (var property in _visibleProperties)
			{
				var cell = headerRow.createCell(currentColumnIndex);
				cell.setCellStyle(_headerStyle);

				if (property.Type == typeof (Money))
				{
					SetColumnWidth(currentColumnIndex, typeof (decimal));
					currentColumnIndex++;
					headerRow.createCell(currentColumnIndex);
					_sheet.addMergedRegion(new CellRangeAddress(0, 0, currentColumnIndex - 1, currentColumnIndex));
					SetColumnWidth(currentColumnIndex, typeof (Currency));
				}
				else
					SetColumnWidth(currentColumnIndex, property.Type);

				cell.setCellValue(property.Caption);

				currentColumnIndex++;
			}

			for (var i = 0; i < _list.Length; i++)
			{
				var row = _sheet.createRow(i + 1);
				row.setHeight(400);

				currentColumnIndex = 0;
				var propertyIndex = 0;

				foreach (var recordProperty in _recordProperties)
				{
					if (_visibleProperties.Contains(recordProperty))
					{
						var cell = row.createCell(currentColumnIndex);

						if (recordProperty.Type == typeof (Money))
						{
							var money = (object[]) ((object[]) _list[i])[propertyIndex];

							if (money != null)
							{
								SetCellValue(ref cell, money[0], typeof (decimal));
								var cellCurrency = row.createCell(currentColumnIndex + 1);
								SetCellValue(ref cellCurrency, money[1], typeof (string));
							}

							currentColumnIndex++;
						}
						else if (recordProperty.Type.Is<Entity2>() || recordProperty.Type.Is<Invoice>()) // TODO: make invoice Entity
						{
							if (((object[]) _list[i])[propertyIndex] != null)
								SetCellValue(ref cell, ((object[]) ((object[]) _list[i])[propertyIndex])[1], typeof (string));
						}
						else
							SetCellValue(ref cell, ((object[]) _list[i])[propertyIndex], recordProperty.Type);

						currentColumnIndex++;
					}

					propertyIndex++;
				}
			}

			using (var stream = new ByteArrayOutputStream())
			{
				_workbook.write(stream);

				return stream.toByteArray();
			}
		}

		private void SetCellValue(ref Cell cell, object value, Type type)
		{
			if (value == null)
				return;

			if (type == typeof (int) || type == typeof (decimal))
			{
				cell.setCellStyle(_moneyStyle);
				cell.setCellValue((double) ((decimal) value));
			}
			else if (type == typeof (DateTime))
			{
				var epoch = new DateTime(1970, 1, 1);
				var ts = ((DateTime) value).ToUniversalTime().Subtract(epoch);
				var date = new Date((long) ts.TotalMilliseconds);

				cell.setCellStyle(_dateStyle);
				cell.setCellValue(new Date(date.getTime()));
			}
			else if (type == typeof (bool))
				if ((bool) value)
					cell.setCellValue("1");
				else
					cell.setCellValue("0");
			else
			{
				cell.setCellStyle(_textStyle);
				cell.setCellValue(value.ToString());
			}
		}

		private void SetColumnWidth(int columnIndex, Type type)
		{
			int width;
			if (type == typeof (DateTime))
				width = 10;
			else if (type == typeof (decimal))
				width = 10;
			else if (type == typeof (Currency))
				width = 6;
			else if (type == typeof (String))
				width = 40;
			else
				width = 20;

			_sheet.setColumnWidth(columnIndex, width*256);
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

			DataFormat format = _workbook.createDataFormat();
			_dateStyle = _workbook.createCellStyle();
			_dateStyle.setDataFormat(format.getFormat("DD.MM.YYYY"));

			_moneyStyle = _workbook.createCellStyle();
			_moneyStyle.setDataFormat(format.getFormat("#,##0.00"));

			_textStyle = _workbook.createCellStyle();
			_textStyle.setWrapText(true);
		}

		private Class _clazz;
		private readonly IList<Property> _visibleProperties = new List<Property>();
		private readonly IList<Property> _recordProperties = new List<Property>();
		private object[] _list;


		private HSSFWorkbook _workbook;
		private Sheet _sheet;
		private HSSFCellStyle _headerStyle;
		private CellStyle _dateStyle;
		private CellStyle _moneyStyle;
		private CellStyle _textStyle;
	}
}