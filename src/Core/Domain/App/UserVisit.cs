using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

using Luxena.Base.Metamodel;
using Luxena.Domain;



namespace Luxena.Travel.Domain
{


	[RU("Заход пользователя", "Заходы пользователей")]
	[AdminOnlyPrivileges(Create = null, Update = null, Copy = null, Delete = null)]
	public partial class UserVisit : Entity
	{

		[ReadOnly, Required]
		public virtual User User { get; set; }

		[RU("Время захода")]
		[EntityDate, DateTime2, ReadOnly, Required, Utility]
		public virtual DateTime StartDate { get; set; }

		[RU("IP"), ReadOnly, Required]
		public virtual string IP { get; set; }

		[RU("SessionId"), ReadOnly, Required]
		public virtual string SessionId { get; set; }

		[RU("Подробнее о запросе"), Text(10), ReadOnly]
		public virtual string Request { get; set; }


		public override object Clone()
		{
			var clone = (UserVisit)base.Clone();
			return clone;
		}

	}



}