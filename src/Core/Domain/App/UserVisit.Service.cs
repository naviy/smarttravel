using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;



namespace Luxena.Travel.Domain
{


	partial class UserVisit
	{

		public class Service : EntityService<UserVisit>
		{

			public Service()
			{
				//Modifing += r =>
				//{
				//};
			}

			public void Add(string ip, string userAgent)
			{
				var user = db.Security.User;

				var visit = db.UserVisit.New();

				visit.User = user;
				visit.StartDate = DateTime.Now;
				visit.SessionId = user.SessionId;
				visit.IP = ip;
				visit.Request = userAgent;

				db.Save(visit);

			}

		}

	}



}