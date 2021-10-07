//using System;

//using Luxena.Domain.Contracts;



//namespace Luxena.Travel.Domain
//{


//	public partial class UserVisitDto : Entity3DContract
//	{

//		public User.Reference User { get; set; }

//		public DateTime StartDate { get; set; }

//		public string IP { get; set; }

//		public string SessionId { get; set; }

//		public string Request { get; set; }
//	}



//	public partial class UserVisitContractService : Entity3DContractService<UserVisit, UserVisit.Service, UserVisitDto>
//	{
//		public UserVisitContractService()
//		{
//			ContractFromEntity += (r, c) =>
//			{
//				c.User = r.User;
//				c.StartDate = r.StartDate;
//				c.IP = r.IP;
//				c.SessionId = r.SessionId;
//				c.Request = r.Request;
//			};

//			EntityFromContract += (r, c) =>
//			{
//				r.User = c.User + db;
//				r.StartDate = c.StartDate + db;
//				r.IP = c.IP + db;
//				r.SessionId = c.SessionId + db;
//				r.Request = c.Request + db;
//			};
//		}
//	}



//}