//using System;

//using Luxena.Base.Domain;

//namespace Luxena.Travel.Domain
//{
//	public partial class SystemShutdown : ICreateAware, IEntity
//	{
//		public virtual object Id { get; set; }

//		public virtual string CreatedBy { get; set; }

//		public virtual DateTime CreatedOn { get; set; }

//		public virtual DateTime LaunchPlannedOn { get; set; }

//		public virtual string Note { get; set; }

//		public override string ToString()
//		{
//			return string.Format("{0} ({1})", CreatedOn.ToString("g"), CreatedBy);
//		}
//	}
//}