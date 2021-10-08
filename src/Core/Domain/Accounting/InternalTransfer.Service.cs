namespace Luxena.Travel.Domain
{

	partial class InternalTransfer
	{
		
		public class Service : Entity2Service<InternalTransfer>
		{

			public Service()
			{
				Calculating += r =>
				{
					if (r.Number.No())
						r.Number = NewSequence();

//					var old = new
//					{
//						FromOrder = OldValue(r, a => a.FromOrder),
//						FromParty = OldValue(r, a => a.FromParty),
//						ToOrder = OldValue(r, a => a.ToOrder),
//						ToParty = OldValue(r, a => a.ToParty),
//						Amount = OldValue(r, a => a.Amount),
//					};
//
//					if (!Equals(r.FromOrder, old.FromOrder))
//					{
//						if (old.FromOrder != null)
//						{
//							old.FromOrder.RemoveOutgoingTransfer(r);
//							old.FromOrder.Refresh();
//							db.SaveOnCommit(old.FromOrder);
//						}
//
//						if (r.FromOrder != null)
//						{
//							r.FromOrder.AddOutgoingTransfer(r);
//							r.FromParty = r.FromOrder.Customer;
//							r.FromOrder.Refresh();
//							db.SaveOnCommit(r.FromOrder);
//						}
//					}
//
//					if (!Equals(r.ToOrder, old.ToOrder))
//					{
//						if (old.ToOrder != null)
//						{
//							old.ToOrder.RemoveIncomingTransfer(r);
//							old.ToOrder.Refresh();
//							db.SaveOnCommit(old.ToOrder);
//						}
//
//						if (r.ToOrder != null)
//						{
//							r.ToOrder.AddIncomingTransfer(r);
//							r.ToParty = r.ToOrder.Customer;
//							r.ToOrder.Refresh();
//							db.SaveOnCommit(r.ToOrder);
//						}
//					}
//
//					if (r.Amount != old.Amount)
//					{
//						if (r.FromOrder != null)
//						{
//							r.FromOrder.Refresh();
//							db.SaveOnCommit(r.FromOrder);
//						}
//
//						if (r.ToOrder != null)
//						{
//							r.ToOrder.Refresh();
//							db.SaveOnCommit(r.ToOrder);
//						}
//					}
				};

				Deleting += r =>
				{
					r.FromOrder = null;
					r.ToOrder = null;

//					if (r.FromOrder != null)
//					{
//						r.FromOrder.RemoveOutgoingTransfer(r);
//						r.FromOrder.Refresh();
//						db.SaveOnCommit(r.FromOrder);
//					}
//
//					if (r.ToOrder != null)
//					{
//						r.ToOrder.RemoveIncomingTransfer(r);
//						r.ToOrder.Refresh();
//						db.SaveOnCommit(r.ToOrder);
//					}
				};
			}

		}

	}

}