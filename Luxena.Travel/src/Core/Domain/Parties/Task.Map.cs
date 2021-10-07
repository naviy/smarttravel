using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Mapping
{
	public class TaskMap : Entity2Mapping<Task>
	{
		public TaskMap()
		{
			Property(x => x.Number, m =>
			{
				m.NotNullable(true);
				m.Unique(true);
				m.Length(10);
			});
			Property(x => x.Subject, m =>
			{
				m.Length(200);
				m.NotNullable(true);
			});
			Property(x => x.Description, m => m.Type(NHibernateUtil.StringClob));
			ManyToOne(x => x.RelatedTo);
			ManyToOne(x => x.Order);
			ManyToOne(x => x.AssignedTo);
			Property(x => x.Status, m => m.NotNullable(true));
			Property(x => x.DueDate, m => m.Type<UtcKindDateType>());

			BagAggregate(x => x.Comments, i => i.Task, o => o.CreatedOn);
		}
	}
}