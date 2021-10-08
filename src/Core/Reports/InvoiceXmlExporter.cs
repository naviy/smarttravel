using System;
using System.IO;
using System.Xml;

using Luxena.Travel.Domain;

using File = System.IO.File;


namespace Luxena.Travel.Reports
{
	public class InvoiceXmlExporter : IEntityExporter
	{
		public string ExportPath { get; set; }

		public string ArchivePath { get; set; }

		public string DateFormat { get; set; }

		public void Export(object obj)
		{
			if (ExportPath.No() || DateFormat.No() || !(obj is Invoice))
				return;

			var invoice = (Invoice)obj;

			_document = new XmlDocument();

			XmlNode declarationNode = _document.CreateXmlDeclaration("1.0", "UTF-8", null);
			_document.AppendChild(declarationNode);

			XmlNode invoiceNode = _document.CreateElement("Invoice");
			_document.AppendChild(invoiceNode);

			XmlNode node = _document.CreateElement("Id");
			node.InnerText = invoice.Id.ToString();
			invoiceNode.AppendChild(node);

			node = _document.CreateElement("Number");
			node.InnerText = invoice.Number;
			invoiceNode.AppendChild(node);

			node = _document.CreateElement("IssuedOn");
			node.InnerText = invoice.IssueDate.ToString(DateFormat);
			invoiceNode.AppendChild(node);

			invoiceNode.AppendChild(GetContrAgentNode("IssuedBy", invoice.IssuedBy));

			/*node = _document.CreateElement("Status");
			string status = "Opened";
			if (invoice.Status == OrderStatus.Closed)
				status = "Closed";
			else if (invoice.Status == OrderStatus.Voided)
				status = "Voided";
			node.InnerText = status;
			invoiceNode.AppendChild(node);*/

			invoiceNode.AppendChild(GetContrAgentNode("Owner", invoice.Owner));

			invoiceNode.AppendChild(GetContrAgentNode("Customer", invoice.Customer));

			invoiceNode.AppendChild(GetContrAgentNode("ShipTo", invoice.ShipTo));

			invoiceNode.AppendChild(GetContrAgentNode("BillTo", invoice.BillTo, invoice.BillToName));

			node = _document.CreateElement("InvoiceItems");

			foreach (var invoiceItem in invoice.Order.Items)
				node.AppendChild(GetInvoiceItemNode(invoiceItem));

			invoiceNode.AppendChild(node);

			invoiceNode.AppendChild(GetMoneyNode("Discount", invoice.Order.Discount));

			invoiceNode.AppendChild(GetMoneyNode("Vat", invoice.Order.Vat));

			invoiceNode.AppendChild(GetMoneyNode("GrandTotal", invoice.Total));

			var fileName = string.Format("{0:yyyy-MM-dd_HH-mm-ss_}{1}{2}", DateTime.Now, "Invoice", ".xml");
			var fullName = Path.Combine(ExportPath.ResolvePath(), fileName);

			var i = 0;

			while (File.Exists(fullName))
			{
				i++;
				fileName = string.Format("{0:yyyy-MM-dd_HH-mm-ss_}{1}_{2}{3}", DateTime.Now, "Invoice", i, ".xml");
				fullName = Path.Combine(ExportPath.ResolvePath(), fileName);
			}

			using (var streamWriter = new StreamWriter(fullName))
				_document.Save(streamWriter);

			if (ArchivePath.Yes())
				File.Copy(fullName, Path.Combine(ArchivePath.ResolvePath(), fileName));
		}

		private XmlNode GetContrAgentNode(string propertyName, Party contrAgent, string name = null)
		{
			XmlNode node = _document.CreateElement(propertyName);
			if (contrAgent != null)
			{
				XmlNode childNode = _document.CreateElement("Id");
				childNode.InnerText = contrAgent.Id.ToString();
				node.AppendChild(childNode);
				childNode = _document.CreateElement("Type");
				childNode.InnerText = (contrAgent is Person) ? "Person" : "Organization";
				node.AppendChild(childNode);
				childNode = _document.CreateElement("Name");
				childNode.InnerText = contrAgent.NameForDocuments;
				node.AppendChild(childNode);
			}
			else if (!string.IsNullOrWhiteSpace(name))
			{
				node.InnerText = name;
			}

			return node;
		}

		private XmlNode GetInvoiceItemNode(OrderItem item)
		{
			XmlNode node = _document.CreateElement("InvoiceItem");

			XmlNode childNode = _document.CreateElement("Id");
			childNode.InnerText = item.Id.ToString();
			node.AppendChild(childNode);
			childNode = _document.CreateElement("Text");
			childNode.InnerText = item.Text;
			node.AppendChild(childNode);
			node.AppendChild(GetMoneyNode("Total", item.GrandTotal));
			node.AppendChild(GetMoneyNode("Taxable", item.TaxedTotal));
			childNode = _document.CreateElement("VatIncluded");
			childNode.InnerText = item.HasVat.ToString();
			node.AppendChild(childNode);

			if (item.Product != null && !item.IsServiceFee)
			{
				var aviaTicket = item.Product as AviaTicket;

				if (aviaTicket != null && aviaTicket.Departure != null)
				{
					childNode = _document.CreateElement("Departure");
					childNode.InnerText = aviaTicket.Departure.Value.ToString(DateFormat);
					node.AppendChild(childNode);
				}
			}

			return node;
		}

		private XmlNode GetMoneyNode(string propertyName, Money money)
		{
			XmlNode node = _document.CreateElement(propertyName);
			if (money == null)
				return node;

			node.InnerText = string.Format("{0:F2}", money.Amount);
			return node;
		}

		private XmlDocument _document;
	}
}
