using System;

using Luxena.Base.Domain;


namespace Luxena.Travel.Domain
{

	public class SystemVariables : IModifyAware, IEntity
	{
		public virtual object Id { get; set; }

		public virtual int Version { get; set; }

		public virtual DateTime? ModifiedOn { get; set; }

		public virtual string ModifiedBy { get; set; }

		public virtual DateTime BirthdayTaskTimestamp { get; set; }

		public override string ToString()
		{
			return string.Empty;
		}
	}
	

	partial class Domain
	{
		public SystemVariables SystemVariables => ResolveSingleton(ref _systemVariables);
		private SystemVariables _systemVariables;
	}

}