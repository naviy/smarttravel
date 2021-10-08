using System;

using Common.Logging;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Services
{

	public class BirthdayTask : ITask
	{

		// bool ITask.IsStarted { get; set; }

		public Domain.Domain db { get; set; }


		public void Execute()
		{
			db.Commit(() =>
			{
				var count = db.Configuration.BirthdayTaskResponsible == null ? 0 : GenerateTasks();
				if (count != 0)
					_log.Info(string.Format("Generated {0} bithday tasks.", count));
			});
		}

		private int GenerateTasks()
		{
			var variables = db.SystemVariables;

			var from = variables.BirthdayTaskTimestamp.Date.AddDays(DatePeriod);
			var to = DateTime.Today.AddDays(DatePeriod - 1);

			var persons = db.Task.GetPersonsWithBirthday(from, to);

			foreach (var person in persons)
				db.Task.Save(CreateTask(person, from));

			variables.BirthdayTaskTimestamp = DateTime.Now.AsUtc();

			return persons.Count;
		}

		private Task CreateTask(Person person, DateTime from)
		{
			if (person.Birthday == null)
				throw new InvalidOperationException();

			var birthday = person.Birthday.Value;

			var dueDate = new DateTime(from.Year, birthday.Month, birthday.Day);

			if (dueDate < from)
				dueDate = dueDate.AddYears(1);

			return new Task
			{
				Number = db.Task.NewSequence(),
				Subject = CommonRes.BirthdayTask_Subject,
				DueDate = dueDate,
				RelatedTo = person,
				AssignedTo = db.Configuration.BirthdayTaskResponsible,
				Status = TaskStatus.Open
			};
		}

		private const int DatePeriod = 7;
		private static readonly ILog _log = LogManager.GetLogger(typeof(BirthdayTask));
	}
}