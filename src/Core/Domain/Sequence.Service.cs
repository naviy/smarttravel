using System;

using NHibernate;


namespace Luxena.Travel.Domain
{

	public class SequenceService : DomainService
	{
		public string Next<T>()
		{
			return Next(typeof(T).Name, null);
		}

		public string Next(string name)
		{
			return Next(name, null);
		}

		public string Next<T>(Func<string, bool> inUseCallback)
		{
			return Next(typeof (T).Name, inUseCallback);
		}



		public string Next(string name, Func<string, bool> inUseCallback)
		{
			var id = FindSequenceId(name);

			var now = DateTime.Now;

			Session
				.CreateSQLQuery(@"
update lt_sequence set 
	current = case when extract(year from timestamp) < :year then 1 else current + 1 end, 
	timestamp = :timestamp 
	where id = :id"
				)
				.SetInt32("year", now.Year)
				.SetDateTime("timestamp", now)
				.SetString("id", id)
				.ExecuteUpdate();

			var data = Session
				.CreateSQLQuery("select format, current from lt_sequence where id = :id")
				.AddScalar("format", NHibernateUtil.String)
				.AddScalar("current", NHibernateUtil.Int64)
				.SetString("id", id)
				.UniqueResult<object[]>();

			if (inUseCallback == null)
				return string.Format(data[0].ToString(), now, data[1]);

			var format = (string)data[0];
			var current = (long)data[1] - 1;

			var start = current;

			while (!inUseCallback(string.Format(format, now, current)) && current > 0)
				current--;

			if (current == start)
				while (inUseCallback(string.Format(format, now, ++current))) {
				}
			else
				current++;

			if (current != start + 1)
			{
				Session.CreateSQLQuery(@"update lt_sequence set current = :current where id = :id")
					.SetInt64("current", current)
					.SetString("id", id)
					.ExecuteUpdate();
			}

			return string.Format(format, now, current);
		}

		private string FindSequenceId(string name)
		{
			var id = Session
				.CreateSQLQuery("select id from lt_sequence where name = :name")
				.SetString("name", name)
				.UniqueResult<object>();

			if (id == null)
				throw new Exception($"Generator for '{name}' is not defined");

			return (string)id;
		}


		#region With Descriminator

		public string Next<T>(string discriminator, Func<string, bool> inUseCallback)
		{
			return Next(typeof(T).Name, discriminator, inUseCallback);
		}

		public string Next(string name, string discriminator, Func<string, bool> inUseCallback)
		{
			var id = FindSequenceId(name, discriminator);

			var now = DateTime.Now;

			Session
				.CreateSQLQuery(@"
update lt_sequence set 
	current = case when extract(year from timestamp) < :year then 1 else current + 1 end, 
	timestamp = :timestamp 
 where id = :id"
				)
				.SetInt32("year", now.Year)
				.SetDateTime("timestamp", now)
				.SetString("id", id)
				.ExecuteUpdate();

			var data = Session
				.CreateSQLQuery("select format, current from lt_sequence where id = :id")
				.AddScalar("format", NHibernateUtil.String)
				.AddScalar("current", NHibernateUtil.Int64)
				.SetString("id", id)
				.UniqueResult<object[]>();

			if (inUseCallback == null)
				return string.Format(data[0].ToString(), now, data[1]);

			var format = (string)data[0];
			var current = (long)data[1] - 1;

			var start = current;

			while (!inUseCallback(string.Format(format, now, current)) && current > 0)
				current--;

			if (current == start)
				while (inUseCallback(string.Format(format, now, ++current)))
				{
				}
			else
				current++;

			if (current != start + 1)
			{
				Session.CreateSQLQuery(@"update lt_sequence set current = :current where id = :id")
					.SetInt64("current", current)
					.SetString("id", id)
					.ExecuteUpdate();
			}

			return string.Format(format, now, current);
		}

		private string FindSequenceId(string name, string discriminator)
		{
			var result = Session
				.CreateSQLQuery("select id, discriminator from lt_sequence where name = :name")
				.SetString("name", name)
				.List<object[]>();

			object[] data = null;

			if (result.Count == 1)
			{
				if (result[0][1] == null)
					data = result[0];
			}
			else
			{
				foreach (var objects in result)
					if ((string)objects[1] == discriminator || objects[1] == null && data == null)
						data = objects;
			}

			if (data == null)
				throw new Exception($"Generator for '{name}' is not defined");

			return (string)data[0];
		}

		#endregion

	}

}