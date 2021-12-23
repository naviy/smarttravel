using System;
using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel.SubSystems.Conversion;

using Common.Logging;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Export
{

	

	//===g






	[Convertible]
	public class GdsFileExportDestination
	{

		public string OwnerId { get; set; }
		public string InboxPath { get; set; }
		public string OrderAssignedToPersonId { get; set; }
		public string OrderCustomerId { get; set; }
		public bool UseAppendingToOrder { get; set; }

	}






	public class GdsFileExporter
	{

		//---g



		public Domain.Domain db { get; set; }

		public List<GdsFileExportDestination> Destinations { get; set; }// = new List<GdsFileExportDestination>();



		//---g



		public bool Export(GdsFile file, IEnumerable<Entity2> documents)
		{

			if (file == null || Destinations == null) 
				return false;


			//_log.Info("START EXPORT 1");

			var docs = documents
				.Select(a => a as AviaDocument ?? a.As<AviaDocumentVoiding>()?.Document)
				.Where(a => a?.Owner != null)
				.ToList();


			if (docs.No()) 
				return false;


			var appendedToOrder = false;


			foreach (var dest in Destinations)
			{

				var dest1 = dest;

				if (docs.By(a => a.Owner.Id.AsString() == dest1.OwnerId) == null) 
					continue;


				var path = file.SaveToInboxFolder(dest.InboxPath);
				_log.Info(path);


				if (dest.UseAppendingToOrder && !appendedToOrder)
				{
					AddToOrder(dest, docs);
				}

				appendedToOrder = true;

			}


			return true;

		}



		public bool Export(AviaDocument document)
		{

			var priorOwner = db.OldValue(document, a => a.Owner);

			if (priorOwner == null || document.Owner == null || Equals(document.Owner, priorOwner))
				return false;


			//_log.Info("START EXPORT 2");


			var exported = false;


			foreach (var dest in Destinations)
			{

				if (document.Owner.Id.AsString() == dest.OwnerId)
				{
					var file = GetFile(document);
					exported = Export(file, new[] { document });
				}

				else if (priorOwner.Id.AsString() == dest.OwnerId)
				{

					document.Order?.Remove(db, document);

					var file = (GdsFile)GetFile(document).Clone();
					file.Name = file.Name.Replace(".", "_DEL.");
					file.Content = "DELETE " + document.Number;

					var path = file.SaveToInboxFolder(dest.InboxPath);
					_log.Info(path);

					document.GdsFileIsExported = true;
					exported = true;

				}

			}


			return exported;

		}



		private static GdsFile GetFile(AviaDocument document)
		{

			var file = document.OriginalDocument;

			if (file != null) 
				return file;


			Type contractType;
			file = XmlExporter.CreateGdsFile(document, out contractType);
			if (file == null) return null;

			file.Name += ".xml";


			return file;

		}



		private void AddToOrder(GdsFileExportDestination dest, IEnumerable<AviaDocument> docs)
		{

			var owner = db.Party.Load(dest.OwnerId);
			var customer = dest.OrderCustomerId.Yes() ? db.Party.Load(dest.OrderCustomerId) : owner;


			var order = db.Order.By(a => a.Customer == customer && a.IssueDate == DateTime.Today && a.CreatedBy == "SYSTEM");
			
			if (order == null)
			{
				order = new Order
				{
					Owner = owner,
					CreatedBy = "SYSTEM",
					AssignedTo = db.Person.Load(dest.OrderAssignedToPersonId)
				};
				order.SetCustomer(db, customer);

				db.Save(order);
			}


			var emptyMoney = new Money(db.Configuration.DefaultCurrency, 0);

			foreach (var doc in docs)
			{

				if (doc.Order != null) 
					continue;

				doc.GdsFileIsExported = true;


				order.Add(db, doc, ServiceFeeMode.AlwaysJoin);

				doc.MustBeUnprocessed = false;


				doc.ServiceFee |= emptyMoney;
				doc.Discount |= emptyMoney;


				if (doc.IsRefund)
				{
					doc.ServiceFeePenalty |= emptyMoney;
					doc.RefundServiceFee |= emptyMoney;
				}

				doc.GrandTotal |= doc.GetGrandTotal();


				db.Save(doc);

			}

		}



		//---g



		private readonly ILog _log = LogManager.GetLogger(typeof(GdsFileExporter));



		//---g

	}






	//===g



}
