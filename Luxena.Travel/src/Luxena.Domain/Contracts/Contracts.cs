using System.Diagnostics;

using Luxena.Domain.Entities;


namespace Luxena.Domain.Contracts
{

	public class Contracts<TDomain, TContracts>
		where TDomain : Domain<TDomain>
		where TContracts : Contracts<TDomain, TContracts>
	{

		public TDomain Domain { [DebuggerStepThrough] get { return db; } [DebuggerStepThrough] set { db = value; } }
		protected internal TDomain db;


		[DebuggerStepThrough]
		protected TContractService ResolveService<TContractService>()
			where TContractService : EntityContractService<TDomain, TContracts>, new()
		{
			var service = new TContractService { Domain = db, Contracts = (TContracts)this };
			return service;
		}

		[DebuggerStepThrough]
		protected TContractService ResolveService<TContractService>(ref TContractService service)
			where TContractService : EntityContractService<TDomain, TContracts>, new()
		{
			return service ?? (service = ResolveService<TContractService>());
		}

		[DebuggerStepThrough]
		public static implicit operator TDomain(Contracts<TDomain, TContracts> dc)
		{
			return dc == null ? null : dc.Domain;
		}
	
	}
	
}
