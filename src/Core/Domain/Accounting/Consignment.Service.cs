using System;
using System.Linq;

using Luxena.Travel.Reports;


namespace Luxena.Travel.Domain
{

	partial class Consignment
	{
		public class Service : Entity2Service<Consignment>
		{

			#region Read

			public byte[] GetLastFileBy(object id)
			{
				return By(id).As(a => a.LastIssuedConsignment()).As(a => a.Content);
			}

			#endregion


			#region Modify

			public Service()
			{
				Calculating += r =>
				{
					if (r.Number.No())
					{
						if (db.Configuration.Consignment_NumberMode == InvoiceNumberMode.ByOrderNumber)
						{
							var order = r.Order;

							if (order == null)
							{
								r.Number = NewSequence();
							}
							else
							{
								string number;
								do
								{
									order.ConsignmentLastIndex = (order.ConsignmentLastIndex ?? 0) + 1;
									number = order.Number.TrimStart("O.") + "-" + order.ConsignmentLastIndex;
								} while (Exists(a => a.Number == number));

								r.Number = number;

								db.Save(order);
							}
						}
						else
						{
							r.Number = NewSequence();
						}
					}
				};

				Deleting += r =>
					r.OrderItems.ForEach(a => a.Consignment = null);
			}


			public Consignment Create(Order order)
			{
				var r = new Consignment
				{
					Supplier = db.Configuration.Company,
					Acquirer = order.ShipTo ?? order.Customer,
					GrandTotal = order.Total.Clone(),
					Vat = order.Vat.Clone(),
					Discount = order.Discount.Clone(),
				};

				Save(r);

				foreach (var item in order.Items)
				{
					r.AddOrderItem(item);
				}

				r.IssueDate = r.OrderItems.Where(a => a.Product != null).Max(a => (DateTime?)a.Product.IssueDate) ?? order.IssueDate;

				r.TotalSupplied = r.GetTotalSupplied(db);

				return r;
			}

			#endregion


			public void Issue(Consignment consignment)
			{
				var bytes = (db.Resolve<ConsignmentForm>() ?? new ConsignmentForm())
					.Build(db, consignment);

				var item = new IssuedConsignment
				{
					TimeStamp = DateTime.Now.AsUtc(),
					Content = bytes,
					IssuedBy = db.Security.Person,
					Number = consignment.Number
				};

				consignment.AddIssuedConsignment(item);

				db.Save(item);
			}

		}
	}

}