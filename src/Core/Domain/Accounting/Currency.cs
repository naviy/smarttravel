using System.Linq;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	[RU("Валюта", "Валюты")]
	[SupervisorPrivileges]
	public partial class Currency : Entity3
	{
		public Currency()
		{
		}

		public Currency(string code)
		{
			Code = code;
		}

		[Patterns.Code, EntityName]
		public virtual string Code { get; set; }

		[RU("Числовой код")]
		public virtual int NumericCode { get; set; }

		[RU("Кириллический код")]
		public virtual string CyrillicCode { get; set; }


		public override bool Equals(object obj)
		{
			var currency = obj as Currency;

			return currency != null && Equals(currency.Code, Code);
		}

		public override int GetHashCode()
		{
			return HashCodeUtility.GetHashCode(Code);
		}

		public override string ToString()
		{
			return Code;
		}

		public override Entity Resolve(Domain db)
		{
			var savedCurrency = Code.No()
				? db.Currency.By(c => c.NumericCode == NumericCode, cacheable: true)
				: db.Currency.By(c => c.Code == Code, cacheable: true);

			return savedCurrency ?? db.Save(this);
		}


		public class Service : Entity3Service<Currency>
		{

			public override RangeResponse Suggest(RangeRequest prms)
			{
				var limit = prms.Limit;
				if (limit == 0) limit = 50;

				if (prms.Query == "*")
				{
					return new RangeResponse(Query
						.OrderBy(a => a.Code)
						.Select(a => EntityReference.FromArray(a.Id, a.Code, typeof(Currency).Name))
						.Take(limit)
						.ToArray()
					);
				}

				var d1 = Query
					.Where(a => a.Code.StartsWith(prms.Query))
					.OrderBy(a => a.Code)
					.Select(a => EntityReference.FromArray(a.Id, a.Code, typeof(Currency).Name))
					.Take(limit)
					.ToArray();

				return new RangeResponse(d1);
			}

		}

	}

}