using System;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public partial class Sequence : Entity
	{

		[EntityName]
		public string Name { get; set; }

		public string Discriminator { get; set; }

		public long Current { get; set; }

		public string Format { get; set; }

		public DateTimeOffset Timestamp { get; set; }


		public string GenerateNext(Func<string, bool> match = null)
		{
			var now = DateTime.Now;

			Current = Timestamp.Year < now.Year ? 1 : Current + 1;
			Timestamp = now;

			if (match != null)
			{
				Current = Current - 1;
				var start = Current;

				// Возвращаемся к первому подходящему
				while (!match(string.Format(Format, now, Current)) && Current > 0)
					Current--;

				// Ищем следующий
				if (Current == start)
					while (match(string.Format(Format, now, ++Current))) { }
				else
					Current++;
			}
			
			return string.Format(Format, now, Current);
		}

	}


	partial class Domain
	{

		public DbSet<Sequence> Sequences { get; set; }
		

		public string NewSequence<T>(string discriminator = null, Func<string, bool> match = null)
		{
			return NewSequence(typeof(T).Name, discriminator, match);
		}
		
		public string NewSequence(string name, string discriminator = null, Func<string, bool> match = null)
		{
			Sequence seq;

			if (discriminator.Yes())
				seq =
					db.Sequences.By(a => a.Name == name && a.Discriminator == discriminator) ?? 
					db.Sequences.By(a => a.Name == name && a.Discriminator == null);
			else
				seq = db.Sequences.By(a => a.Name == name);

			if (seq == null)
				throw new Exception($"Sequence for '{name}' and '{discriminator}' is not defined");


			//seq.Save(db);

			return seq.GenerateNext(match);
		}

	}

}
