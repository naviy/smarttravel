using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Travel.Export;




namespace Luxena.Travel.Domain
{



	//===g





	partial class Order
	{

		//---g




		public class Service : Entity2Service<Order>
		{

			//---g



			#region Read

			public Order ByNumber(string number)
			{
				return By(o => o.Number == number && !o.IsVoid);
			}

			#endregion



			//---g



			#region Permissions

			public OperationStatus CanChangeVat()
			{
				return db.Configuration.AllowAgentSetOrderVat || db.IsGranted(UserRole.Administrator, UserRole.Supervisor);
			}

			public override OperationStatus CanDelete(Order r)
			{
				var status = base.CanDelete(r);
				if (!status) return status;

				if (r.Payments.Count > 0 || r.Invoices.Count > 0 && !db.IsGranted(UserRole.Supervisor, UserRole.Administrator))
					status.DisableInfo = Exceptions.Order_Deletepaid_Error;

				return status;
			}

			public bool CanRestore(Order order, out IList<Product> products)
			{
				products = new List<Product>(
					order.Items
						.Select(a => a.Product)
						.Where(a => a?.Order != null && !Equals(a.Order, order))
						.Distinct()
				);

				return products.Count == 0;
			}

			#endregion



			//---g




			public Service()
			{


				Validating += r =>
				{
				};



				Calculating += r =>
				{

					if (r.Number.No())
						r.Number = NewSequence();


					r.AssignedTo |= db.Security.Person;


					if (r.IssueDate == DateTime.MinValue)
						r.IssueDate = DateTime.Today;


					if (r.Owner == null)
					{
						var owners = db.DocumentAccess.GetDocumentOwners();

						if (owners.Count == 1)
							r.Owner = owners[0];
					}

				};



				Updating += r =>
				{

					//var oldCustomer = OldValue(r, a => a.Customer);
					//if (!Equals(oldCustomer, r.Customer))
					if (IsDirty(r, a => a.Customer))
					{

						r.Items
							.Select(a => a.Product)
							.Where(a => a != null && a.SetCustomer(db, r.Customer))
							.ForEach(a => db.Save(r, a))
						;

						r.Payments.ForEach(a =>
						{
							if (a.Payer == null)
								a.SetPayer(r.Customer);
						});

					}

				};



				Modifing += r =>
				{
					r.Refresh();
					r.EnsureRefresh();

					if (!IsDirty(r, a => a.InvoiceLastIndex) && !IsDirty(r, a => a.ConsignmentLastIndex))
						db.OnCommit(r, Export);
				};



				Deleting += r => r.ClearOrderReferences(db);


			}



			public void CalcFinanceData(IList<OrderItem> items, Currency currency, out Money total, out Money discount, out Money vat)
			{

				var givenVat = new Money(currency);
				var taxedTotal = new Money(currency);

				discount = new Money(currency);
				total = new Money(currency);
				vat = new Money(currency);


				foreach (var item in items)
				{

					if (item.HasVat)
					{
						givenVat += item.GivenVat ?? new Money(currency);
						taxedTotal += item.TaxedTotal ?? new Money(currency);
					}


					discount += item.Discount ?? new Money(currency);
					total += item.GrandTotal;

				}


				if (taxedTotal != null && taxedTotal.Amount > 0)
				{
					vat = (taxedTotal - discount) * db.Configuration.VatRate / (100 + db.Configuration.VatRate);
				}


				if (givenVat != null && givenVat.Amount > 0)
					vat += givenVat;


				if (vat != null && vat.Amount < 0)
					vat.Amount = 0;

			}



			public void SetIsVoid(object orderId, bool value)
			{
				By(orderId).SetIsVoid(db, value);
			}



			//---g



			public void Export(Order r)
			{
				db.Resolve<IExporter>().Do(a => a.Export(r));
			}



			//---g

		}




		//---g

	}






	//===g



}