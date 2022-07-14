using System;
using System.Linq;

using Luxena.Base;
using Luxena.Base.Data;
using Luxena.Travel.Export;
using Luxena.Travel.Reports;

using EntityReference = Luxena.Base.Data.EntityReference;




namespace Luxena.Travel.Domain
{



	//===g






	partial class Invoice
	{

		//---g



		public class Service : EntityService<Invoice>
		{


			#region Read

			public byte[] GetFile(object id)
			{
				return By(id).As(a => a.Content);
			}

			public override RangeResponse Suggest(RangeRequest request)
			{
				return Suggest(request, InvoiceType.Invoice, Query);
			}

			public static RangeResponse Suggest(RangeRequest request, InvoiceType type, IQueryable<Invoice> query)
			{
				var list = query
					.Where(a => a.Type == type && !a.Order.IsVoid && a.Number.Contains(request.Query))
					.OrderBy(a => a.Number)
					.Take(request.Limit)
					.ToList();

				if (list.Count == 0)
				{
					list = query
						.Where(a => a.Type == type && a.Number.Contains(request.Query))
						.OrderBy(a => a.Number)
						.Take(request.Limit)
						.ToList();
				}

				return new RangeResponse(list.Select(a => new object[]
				{
					a.Id,
					a.Number,
					typeof(Invoice).Name,
					(EntityReference)a.Order,
					(EntityReference)(a.Order.BillTo ?? a.Order.Customer),
					(MoneyDto)a.Order.TotalDue,
					(MoneyDto)a.Order.VatDue,
					(EntityReference)a.Order.Owner,
				}).ToArray());
			}

			public RangeResponse Suggest(RangeRequest prms, string paymentType)
			{
				return Suggest(prms, typeof(WireTransfer).Name == paymentType ? InvoiceType.Invoice : InvoiceType.Receipt, Query);
			}

			#endregion


			#region Modify

			public Service()
			{
				Deleting += r =>
				{

					var person = db.Security.Person;

					bool fullAccess;

					if (!db.DocumentAccess.HasAccess(r.Owner, out fullAccess) && !r.Order.IsPublic || 
						!fullAccess && !Equals(r.IssuedBy, person)
					)
					{
						throw new OperationDeniedException(Resources.DeleteOperationDenied_Msg);
					}


					if (r.Id == null)
						return;


					var payments = db.Payment.ListBy(a => a.Invoice.Id == r.Id);

					payments.ForEach(a => a.SetInvoice(null));

					db.Save(payments);

				};
			}



			public Invoice Issue(Order order, string number, DateTime issueDate, object ownerId, object bankAccountId, int? formNumber, bool showPaid)
			{

				db.AssertUpdate(order);


				if (number.No())
				{
					if (db.Configuration.Invoice_NumberMode == InvoiceNumberMode.ByOrderNumber)
					{
						do
						{
							order.InvoiceLastIndex = (order.InvoiceLastIndex ?? 0) + 1;
							number = order.Number.Replace('O', 'I') + (order.InvoiceLastIndex > 1 ? "-" + order.InvoiceLastIndex : null);
						}
						while (Exists(a => a.Number == number));
						db.Save(order);
					}
					else
					{
						number = NewSequence()
							+ order.AssignedTo?.InvoiceSuffix.As(a => "-" + a);
					}
				}


				var issuedBy = db.Security.Person;
				var owner = db.Party.By(ownerId);
				var bankAccount = db.BankAccount.By(bankAccountId);


				var printer = db.Resolve<IInvoicePrinter>() ?? new InvoicePrinter { db = db };

				var content = printer.Build(
					order, number, issueDate, issuedBy, owner, bankAccount, formNumber, showPaid, out var fileExtension
				);


				var invoice = new Invoice
				{
					Number = number,
					IssueDate = issueDate,
					TimeStamp = DateTime.Now.AsUtc(),
					Type = InvoiceType.Invoice,
					Content = content,
					IssuedBy = issuedBy,
					Total = order.TotalDue.Clone(),
					Vat = order.VatDue.Clone(),
					FileExtension = fileExtension,
				};


				order.AddPrintedDocument(invoice);

				Save(invoice);
				Export(invoice);


				return invoice;

			}



			public Invoice IssueCompletionCertificate(Order order, string number, DateTime issueDate, object ownerId, object bankAccountId, bool showPaid)
			{

				db.AssertUpdate(order);


				if (number.No())
				{
					var lastInvoiceNumber = order.Invoices.Where(a => a.Type == InvoiceType.Invoice).OrderByDescending(a => a.Number).Select(a => a.Number).One();

					if (lastInvoiceNumber.Yes())
						number = "A" + lastInvoiceNumber;
					else
						number = db.Sequence.Next("CompletionCertificate");
				}


				var issuedBy = db.Security.Person;
				var owner = db.Party.By(ownerId);
				var bankAccount = db.BankAccount.By(bankAccountId);


				var printer = db.Resolve<ICompletionCertificatePrinter>() ?? new CompletionCertificatePrinter { db = db };

				var bytes = printer.Build(
					order, number, issueDate, issuedBy, owner, bankAccount, showPaid, out var fileExtension
				);


				var invoice = new Invoice
				{
					Number = number,
					IssueDate = issueDate,
					TimeStamp = DateTime.Now.AsUtc(),
					Type = InvoiceType.CompletionCertificate,
					Content = bytes,
					IssuedBy = issuedBy,
					Total = order.TotalDue.Clone(),
					Vat = order.VatDue.Clone(),
					FileExtension = fileExtension,
				};

				order.AddPrintedDocument(invoice);


				Save(invoice);
				Export(invoice);


				return invoice;

			}



			public Invoice IssueReceipt(Order order)
			{
				db.AssertUpdate(order);

				var r = new Invoice
				{
					Number = db.Sequence.Next("Receipt"),
					IssueDate = DateTime.Now.AsUtc(),
					TimeStamp = DateTime.Now.AsUtc(),
					Type = InvoiceType.Receipt,
					IssuedBy = db.Security.Person,
					Total = order.TotalDue.Clone(),
					Vat = order.VatDue.Clone(),
				};

				//r.Content = db.Resolve<IReceiptPrinter>()?.Build(order, r);
				r.Content = (db.Resolve<IReceiptPrinter>() ?? new ReceiptPrinter { db = db }).Build(order, r);

				order.AddPrintedDocument(r);

				Save(r);
				Export(r);

				return r;
			}



			#endregion


			#region Operations

			public void Export(Invoice invoice)
			{
				db.Resolve<IEntityExporter>().Do(a => a.Export(invoice));
				db.Resolve<IExporter>().Do(a => a.Export(invoice));
			}

			#endregion

		}



		//---g

	}






	//===g



}