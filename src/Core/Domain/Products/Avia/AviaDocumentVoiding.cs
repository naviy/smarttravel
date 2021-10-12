using System;




namespace Luxena.Travel.Domain
{



	//===g






	public partial class AviaDocumentVoiding : Entity2
	{

		//---g



		[EntityName]
		public virtual AviaDocument Document { get; set; }

		public virtual bool IsVoid { get; set; }

		public virtual DateTime TimeStamp { get; set; }

		public virtual GdsOriginator Originator { get; set; }

		public virtual ProductOrigin Origin { get; set; }

		public virtual string AgentOffice { get; set; }

		public virtual string AgentCode { get; set; }

		public virtual Person Agent { get; set; }

		public virtual string IataOffice { get; set; }



		//---g



		public override object Clone()
		{
			var voiding = (AviaDocumentVoiding) base.Clone();

			voiding.Document = (AviaDocument) Document.Clone();

			return voiding;
		}



		public override string ToString()
		{
			return (IsVoid ? "Void" : "Restore") + " " + Document;
		}



		public override Entity Resolve(Domain db)
		{

			base.Resolve(db);

			var r = this;


			r.Document = db.AviaDocument.FindToVoid(r);

			r.Agent = db.GdsAgent.PersonBy(r.Origin, r.AgentOffice, r.AgentCode);

			r.Document.AddVoiding(r);


			if (r.IsVoid)
				r.Document.Order?.Remove(db, r.Document);


			return r;

		}



		//---g



		public class Service : Entity2Service<AviaDocumentVoiding>
		{

		}




		//---g

	}






	//===g



}