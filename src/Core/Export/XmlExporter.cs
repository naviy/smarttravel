using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

using Common.Logging;

using Luxena.Base.Metamodel;
using Luxena.Travel.Domain;




namespace Luxena.Travel.Export
{


	public class XmlExporter : IExporter, IAviaDocumentExporter, IPartyExporter
	{

		private static readonly IDictionary<Type, Type> _contracts = new Dictionary<Type, Type>
		{
			{ typeof(AviaTicket), typeof(AviaTicketContract) },
			{ typeof(AviaRefund), typeof(AviaRefundContract) },
			{ typeof(AviaMco), typeof(AviaMcoContract) },
			{ typeof(Insurance), typeof(InsuranceContract) },
			{ typeof(Order), typeof(OrderContract) },
			{ typeof(Invoice), typeof(InvoiceContract) },
			{ typeof(CashInOrderPayment), typeof(PaymentContract) },
			{ typeof(CashOutOrderPayment), typeof(PaymentContract) },
			{ typeof(CheckPayment), typeof(PaymentContract) },
			{ typeof(ElectronicPayment), typeof(PaymentContract) },
			{ typeof(WireTransfer), typeof(PaymentContract) },
			{ typeof(Person), typeof(PersonContract) },
			{ typeof(Organization), typeof(OrganizationContract) },
		};


		public string ExportPath { get; set; }


		//===


		public static LuxenaXmlFile CreateGdsFile(object obj, out Type contractType)
		{
			contractType = null;

			if (obj == null) return null;

			var file = new LuxenaXmlFile();

			var cls = obj.GetClass();

			string className, reference;
			if (_contracts.TryGetValue(cls.Type, out contractType))
			{
				if (cls.Type.IsSubclassOf(typeof(Party)))
				{
					reference = ((Party)obj).Id.AsString();
					className = "Party";
				}
				else
				{
					reference = _illegalPattern.Replace(cls.EntityNameProperty == null ? obj.ToString() : cls.EntityNameProperty.GetString(obj), "-");
					className = cls.Type.Name;
				}
			}
			else
			{
				if (cls.Type.IsSubclassOf(typeof(Product)))
				{
					contractType = typeof(ProductContract);
					reference = ((Product)obj).Id.AsString();
					className = "Product";
				}
				else
					return null;
			}

			file.CreatedOn = file.TimeStamp = DateTime.Now;
			file.Name = $"{file.CreatedOn:yyyy-MM-dd_HH-mm-ss}_{className}_{reference}";


			var contract = Activator.CreateInstance(contractType, obj);

			var serializer = new DataContractSerializer(contract.GetType());

			using (var stream = new MemoryStream())
			{
				var writer = new CustomXmlWriter(stream);
				serializer.WriteObject(writer, contract);
				writer.Flush();

				stream.Seek(0, SeekOrigin.Begin);

				using (var streamReader = new StreamReader(stream))
				{
					file.Content = streamReader.ReadToEnd();
				}
			}

			return file;
		}


		public void Export(GdsFile file, Type contractType)
		{
			if (file == null) return;

			try
			{
				var path = Path.Combine(ExportPath, file.Name + ".xml");

				for (var i = 1; System.IO.File.Exists(path); ++i)
					path = Path.Combine(ExportPath, $"{file.Name}_{i:00}.xml");


				_log.Info(Path.GetFileName(path));

				System.IO.File.AppendAllText(path, file.Content);
			}
			catch (Exception ex)
			{
				_log.Error(ex.FullMessage(), ex);
			}
		}


		public void Export(object obj)
		{
			Type contractType;
			var file = CreateGdsFile(obj, out contractType);
			Export(file, contractType);
		}


		//===


		private static readonly Regex _illegalPattern = new Regex('[' + Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars())) + ']', RegexOptions.Compiled);

		private readonly ILog _log = LogManager.GetLogger(typeof(XmlExporter));

	}


}