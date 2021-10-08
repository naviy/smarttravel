using System;
using System.Collections;
using System.Collections.Generic;

using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	partial class Task
	{
		public class Service : Entity2Service<Task>
		{

			public override OperationStatus CanUpdate(Task r)
			{
				return db.Granted(
					r.AssignedTo == db.Security.Person || db.IsGranted(UserRole.Administrator, UserRole.Supervisor)
				);
			}

			public virtual object ChangeStatus(object[] ids, TaskStatus status, RangeRequest prms)
			{
				var tasks = ListByIds(ids);

				foreach (var task in tasks)
				{
					if (!CanUpdate(task))
						throw new OperationDeniedException(Base.Resources.UpdateOperationDenied_Msg);

					task.Status = status;
				}

				db.Flush();

				var data = new ArrayList();

				foreach (var task in tasks)
					data.Add(new ObjectSerializer().Serialize(task));


				if (prms == null)
					return data[0];

				prms.PositionableObjectId = tasks[0].Id;

				return new object[]
				{
					data,
					db.GetRange<Task>(prms)
				};
			}


			public IList<Person> GetPersonsWithBirthday(DateTime from, DateTime to)
			{
				const string hql = @"
					from
						Person p
					where
						(month(p.Birthday) = month(:from) and day(p.Birthday) >= day(:from) or month(p.Birthday) > month(:from))
						and
						(month(p.Birthday) = month(:to) and day(p.Birthday) <= day(:to) or month(p.Birthday) < month(:to))
					order by
						p.Birthday, p.Name
				";

				if (from.Year == to.Year && from.Date <= to.Date)
				{
					return Session.CreateQuery(hql)
						.SetDateTime("from", from)
						.SetDateTime("to", to)
						.SetReadOnly(true)
						.List<Person>();
				}

				if (@from.Year >= to.Year)
					return new List<Person>();

				var persons = Session.CreateQuery(hql)
					.SetDateTime("from", @from)
					.SetDateTime("to", new DateTime(@from.Year, 12, 31))
					.SetReadOnly(true)
					.List<Person>();

				var list = Session.CreateQuery(hql)
					.SetDateTime("from", new DateTime(to.Year, 1, 1))
					.SetDateTime("to", to)
					.SetReadOnly(true)
					.List<Person>();

				foreach (var person in list)
					persons.Add(person);

				return persons;
			}


			public Service()
			{
				Calculating += r =>
				{
					if (r.Number.No())
						r.Number = NewSequence();

					if (r.AssignedTo != null && IsDirty(r, a => a.AssignedTo))
						db.OnCommit(r, db.AppState.RegisterTask);
				};
			}
		}

	}

}