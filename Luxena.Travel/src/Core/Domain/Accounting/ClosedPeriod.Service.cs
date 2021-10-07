using System;
using System.Linq;

using Luxena.Base.Domain;


namespace Luxena.Travel.Domain
{

	partial class ClosedPeriod
	{
		public class Service : Entity2Service<ClosedPeriod>
		{

			public bool IsPeriodOverlap(object id, DateTime dateFrom, DateTime dateTo)
			{
				return Exists(p => 
					p.Id != id && (
						(p.DateFrom <= dateFrom && p.DateTo >= dateFrom) || 
						(p.DateFrom <= dateTo && p.DateTo >= dateTo)
					)
				);
			}

			public ClosedPeriod Last()
			{
				return Query.OrderByDescending(p => p.DateFrom).FirstOrDefault();
			}


			public bool IsOpened(DateTime date, bool allowRestricted)
			{
				date = date.Date;
				var periodStates = allowRestricted ? _denyClosed : _denyClosedAndRestricted;

				var result = !Exists(a => a.DateFrom <= date && date <= a.DateTo && periodStates.Contains(a.PeriodState));
				return result;
			}

			public bool IsOpened(DateTime date)
			{
				return IsOpened(date, db.IsGranted(UserRole.Supervisor));
			}

			public void Assert(DateTime date, bool allowRestricted)
			{
				if (!IsOpened(date, allowRestricted))
					throw new DomainException(Exceptions.Document_Closed);
			}

			public void Assert(DateTime date)
			{
				if (!IsOpened(date))
					throw new DomainException(Exceptions.Document_Closed);
			}

			private static readonly PeriodState[] _denyClosed = { PeriodState.Closed };

			private static readonly PeriodState[] _denyClosedAndRestricted = { PeriodState.Closed, PeriodState.Restricted };
			
		}

	}

}