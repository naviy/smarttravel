using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;


namespace Luxena.Travel.Domain.Mapping
{
	public class TaskCommentMap : Entity2Mapping<TaskComment>
	{
		public TaskCommentMap()
		{
			ManyToOne(x => x.Task, m => m.NotNullable(true));
			Property(x => x.Text, m => { m.Type(NHibernateUtil.StringClob); m.NotNullable(true); });
		}
	}
}
