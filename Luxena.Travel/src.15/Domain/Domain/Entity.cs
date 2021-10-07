using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace Luxena.Travel.Domain
{

	using Luxena.Domain;


	[EntityBase]
	public abstract partial class Entity : Domain.Entity<string>
	{

		[ConcurrencyCheck]
		public int Version { get; set; }


		//protected override string NewId()
		//{
		//	return Guid.NewGuid().ToString("N");
		//}

		public override string GetEmptyId() => "";

		public override bool IsNew() => Version == 0 || Id.No();

		public override bool GetIsSaving() => Version >= 0;

		protected override bool HasId() => Id.Yes();

		public static string NewId() => Guid.NewGuid().ToString("N");


		public override void Calculate()
		{
			base.Calculate();

			if (IsSaving() && Id.No())
			{
				Id = NewId();
				Version = 0;
			}
		}


		public virtual LocalizationText Localization(string lang) => null;


		public static bool LookupStep<TEntity, TLookup>(
			LookupParams<TEntity, TLookup> p,
			ref IEnumerable<TLookup> data,
			Func<IQueryable<TEntity>> getQuery
		)
		{
			var d2 = p.GetList(getQuery());

			data = data.AsConcat(d2);

			p.TakeCount -= d2.Count;
			p.SkipCount = Math.Max(0, p.SkipCount - p.Count ?? 0);

			return p.TakeCount <= 0;
		}

	}


	[EntityBase]
	public abstract partial class Entity2 : Entity
	{

		[RU("Дата создания")]
		[DateTime2, ReadOnly, Utility]
		public virtual DateTimeOffset CreatedOn { get; set; }

		[RU("Создано пользователем")]
		[ReadOnly, Utility]
		public virtual string CreatedBy { get; set; }

		[RU("Дата изменения")]
		[DateTime2, ReadOnly, Utility]
		public virtual DateTimeOffset? ModifiedOn { get; set; }

		[RU("Изменено пользователем")]
		[ReadOnly, Utility]
		public virtual string ModifiedBy { get; set; }


		//protected override Domain.Entity Clone()
		//{
		//	var c = (Entity2)base.Clone();

		//	c.CreatedOn = DateTime.Now;
		//	c.CreatedBy = null;

		//	c.ModifiedOn = null;
		//	c.ModifiedBy = null;

		//	return c;
		//}


		protected virtual TEntity Clone2<TEntity>()
		{
			return default(TEntity);
		}

		public override void Calculate()
		{
			base.Calculate();

			if (IsSaving())
			{
				if (IsNew())
				{
					CreatedOn = DateTime.Now;
					CreatedBy = db.UserName ?? "SANDBOX";
				}
				else
				{
					ModifiedOn = DateTime.Now;
					ModifiedBy = db.UserName ?? "SANDBOX";
				}
			}
		}

		protected override void Flush(bool isDeleted)
		{
			Version++;
			base.Flush(isDeleted);
		}

	}


	[EntityBase]
	public abstract partial class Entity3 : Entity2
	{

		[Patterns.Name]
		[EntityName]//, Required]
		public virtual string Name { get; set; }


		public override string ToString()
		{
			return Name;
		}


		public static IEnumerable<TLookup> Lookup<TEntity, TLookup>(LookupParams<TEntity, TLookup> p)
			where TEntity : Entity3
		{
			return Lookup3(p);
		}

		public static IEnumerable<TLookup> Lookup3<TEntity, TLookup>(LookupParams<TEntity, TLookup> p)
			where TEntity : Entity3
		{
			if (p.Filter.No())
				return p.GetList();

			IEnumerable<TLookup> data = null;

			Lookup3(p, ref data);

			return data;
		}

		public static bool Lookup3<TEntity, TLookup>(
			LookupParams<TEntity, TLookup> p,
			ref IEnumerable<TLookup> data
		)
			where TEntity : Entity3
		{
			var filter = p.Filter;
			var filter2 = " " + filter;

			var b = LookupStep(p, ref data, () => p.Where(a =>
				a.Name.StartsWith(filter)
			));

			b = b || LookupStep(p, ref data, () => p.Where(a =>
				!a.Name.StartsWith(filter) && a.Name.Contains(filter2)
			));

			b = b || LookupStep(p, ref data, () => p.Where(a =>
				!a.Name.StartsWith(filter) && a.Name.Contains(filter) && !a.Name.Contains(filter2)
			));

			//p.Count = p.SkipCount > 0 ? (int?)null : p.Query.Count(a => a.Name.Contains(filter));
			//p.Count = p.Query.Count(a => a.Name.Contains(filter));
			p.Count = null;

			return b;
		}

	}


	public abstract class Entity3Lookup : INameContainer
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public string GetName() { return Name; }

		//public static IQueryable<AirlineLookup> SelectAndOrderByName(IQueryable<Organization> query)
		//{
		//	return query
		//		.Select(a => new AirlineLookup
		//		{
		//			Id = a.Id,
		//			Name = a.Name,
		//		})
		//		.OrderBy(a => a.Name);
		//}
	}

	[EntityBase]
	public abstract partial class Entity3D : Entity3
	{

		[Patterns.Description]
		public virtual string Description { get; set; }

	}


	public static class EntityExtensions
	{

		public static TEntity ByName<TEntity>(this IQueryable<TEntity> query, string name)
			where TEntity : Entity3
		{
			return name.No() ? null : query.FirstOrDefault(a => a.Name == name);
		}

		public static string NameById<TEntity>(this IQueryable<TEntity> query, string id)
			where TEntity : Entity3
		{
			return id.No() ? null : query.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();
		}

	}

}