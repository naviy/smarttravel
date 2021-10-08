using System;
using System.Collections;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Base.Data.NHibernate;
using Luxena.Base.Metamodel;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

using Property = Luxena.Base.Metamodel.Property;


namespace Luxena.Travel.Domain
{
	public class GenericDao : Base.Data.NHibernate.GenericDao
	{
		public GenericDao(TransactionManager transManager, global::NHibernate.Cfg.Configuration configuration)
			: base(transManager, configuration)
		{
		}

		public Domain db { get; set; }


		protected override void SetPropertyProjection(Class clazz, ColumnConfig config, ICriteria criteria, ProjectionList projections)
		{
			var property = clazz.GetProperty(config.Name);

			if (property.Type.Is<Money>())
			{
				projections.Add(Projections.Property(property.DataPath + "." + AmountProperty));

				var currencyDataPath = GetCurrencyDataPath(property, criteria);

				projections.Add(Projections.Property(currencyDataPath + "." + Class.Of<Currency>().EntityNameProperty.Name));
				projections.Add(Projections.Property(currencyDataPath + "." + Class.Of<Currency>().IdentifierProperty.Name));
			}
			else
				base.SetPropertyProjection(clazz, config, criteria, projections);
		}

		public override void SetCriteriaRestrictions(Class clazz, ICriteria criteria, RangeRequest request)
		{
			if (request.NamedFilters != null)
				foreach (var filter in request.NamedFilters)
				{
					if (filter == "Payments")
					{
						if (!db.Configuration.SeparateDocumentAccess) continue;

						var person = db.Security.Person;

						var owners = db.DocumentAccess.Query
							.Where(a => a.Person == person)
							.Select(a => a.Owner.Id)
							.ToList();

						if (criteria.GetCriteriaByAlias("_Order") == null)
							criteria.CreateAlias("Order", "_Order", JoinType.LeftOuterJoin);

						criteria.Add(Restrictions.Or(
							Restrictions.Eq("_Order.IsPublic", true),
							Restrictions.Or(
								Restrictions.In("Owner.Id", owners),
								Restrictions.In("_Order.Owner.Id", owners))
							)
						);
					}
					else if (filter == "Orders")
					{
						if (!db.Configuration.SeparateDocumentAccess) continue;

						var person = db.Security.Person;

						var owners = (ICollection) TransactionManager.Session.QueryOver<DocumentAccess>()
							.Select(a => a.Owner.Id)
							.Where(a => a.Person == person)
							.List<object>();

						criteria.Add(Restrictions.Or(
							Restrictions.Eq("IsPublic", true),
							Restrictions.In("Owner.Id", owners)
						));
					}
				}

			base.SetCriteriaRestrictions(clazz, criteria, request);
		}

		protected override ICriterion SetGeneralFilterRestriction(string query, Class clazz, Property property, ICriteria criteria)
		{
			if (property.Type.Is<Money>())
			{
				return null;

				/*var currencyDataPath = GetCurrencyDataPath(property, criteria) + "." + Class.Of<Currency>().ReferenceProperty.Name;

				ICriterion criterion = Restrictions.Like(currencyDataPath, query, MatchMode.Anywhere);

				decimal amount;

				if (decimal.TryParse(query, out amount))
					criterion = Restrictions.Or(criterion, Restrictions.Eq(property.DataPath + "." + AmountProperty, amount));

				return criterion;*/
			}

			return base.SetGeneralFilterRestriction(query, clazz, property, criteria);
		}

		protected override void SetPropertyFilterRestriction(Class clazz, PropertyFilter filter, PropertyFilterCondition filterCondition, ICriteria criteria)
		{
			var property = clazz.GetProperty(filter.Property);

			if (property.Type.Is<Money>())
			{
				ICriterion criterion = null;

				switch (filter.InternalPath)
				{
					case CurrencyProperty:

						var currencyDataPath = GetCurrencyDataPath(property, criteria) + "." + Class.Of<Currency>().EntityNameProperty.Name;

						switch (filterCondition.Operator)
						{
							case FilterOperator.Equals:
								criterion = Restrictions.Eq(currencyDataPath, filterCondition.Value);
								break;
							case FilterOperator.IsNull:
								criterion = Restrictions.IsNull(currencyDataPath);
								break;
							case FilterOperator.StartsWith:
								criterion = Restrictions.Like(currencyDataPath, filterCondition.Value + "%");
								break;
							case FilterOperator.Contains:
								criterion = Restrictions.Like(currencyDataPath, "%" + filterCondition.Value + "%");
								break;
							case FilterOperator.EndsWith:
								criterion = Restrictions.Like(currencyDataPath, "%" + filterCondition.Value);
								break;
							default:
								throw new ArgumentException($"Invalid property '{currencyDataPath}' filter operator", "filterCondition");
						}
					
						break;

					case AmountProperty:

						var amountDataPath = (property.DataPath != property.Name ? GetAmountDataPath(property, criteria) : property.DataPath) + "." + AmountProperty;

						switch (filterCondition.Operator)
						{
							case FilterOperator.Equals:
								criterion = Restrictions.Eq(amountDataPath, Convert.ChangeType(filterCondition.Value, typeof (decimal)));
								break;
							case FilterOperator.IsNull:
								criterion = Restrictions.IsNull(amountDataPath);
								break;
							case FilterOperator.Less:
								criterion = Restrictions.Lt(amountDataPath, Convert.ChangeType(filterCondition.Value, typeof (decimal)));
								break;
							case FilterOperator.LessOrEquals:
								criterion = Restrictions.Le(amountDataPath, Convert.ChangeType(filterCondition.Value, typeof (decimal)));
								break;
							case FilterOperator.GreaterOrEquals:
								criterion = Restrictions.Ge(amountDataPath, Convert.ChangeType(filterCondition.Value, typeof (decimal)));
								break;
							case FilterOperator.Greater:
								criterion = Restrictions.Gt(amountDataPath, Convert.ChangeType(filterCondition.Value, typeof (decimal)));
								break;
							default:
								throw new ArgumentException($"Invalid property '{amountDataPath}' filter operator", nameof(filterCondition));
						}

						break;
				}

				if (criterion == null)
					throw new Exception($"Invalid property '{property.Name + "." + filter.InternalPath}' data path");

				if (filterCondition.Not)
					criterion = Restrictions.Not(criterion);

				criteria.Add(criterion);
			}
			else
				base.SetPropertyFilterRestriction(clazz, filter, filterCondition, criteria);
		}

		protected override object GetPropertyValue(object value, ColumnConfig config)
		{
			if (value != null && value.GetType().Is<Money>())
			{
				var money = (Money) value;

				return new [] { money.Amount, money.Currency.As(a => a.Code), money.Currency.As(a => a.Id) };
			}

			return base.GetPropertyValue(value, config);
		}

		protected override object GetPropertyValue(IEnumerator items, Property property, ColumnConfig config)
		{
			if (property.Type.Is<Money>())
			{
				if (items.Current == null)
				{
					items.MoveNext();
					items.MoveNext();
					items.MoveNext();

					return null;
				}

				var data = new object[3];

				data[0] = Convert.ToDecimal(items.Current);
				items.MoveNext();
				data[1] = items.Current;
				items.MoveNext();
				data[2] = items.Current;
				items.MoveNext();

				return data;
			}

			return base.GetPropertyValue(items, property, config);
		}

		private static string GetCurrencyDataPath(Property property, ICriteria criteria)
		{
			string alias = $"_{property.DataPath}_{CurrencyProperty}";

			if (criteria.GetCriteriaByAlias(alias) == null)
				criteria.CreateAlias(property.DataPath + "." + CurrencyProperty, alias, JoinType.LeftOuterJoin);

			return alias;
		}

		private static string GetAmountDataPath(Property property, ICriteria criteria)
		{
			var alias = $"_{property.Name}";

			if (criteria.GetCriteriaByAlias(alias) == null)
				criteria.CreateAlias(property.DataPath, alias, JoinType.LeftOuterJoin);

			return alias;
		}

		private const string CurrencyProperty = "Currency";
		private const string AmountProperty = "Amount";
	}
}