using System.Collections;

using LxnBase;


namespace Luxena.Travel
{

	public partial class SemanticDomain
	{

		public SemanticDomain(object owner)
		{
			// ReSharper disable once SuspiciousTypeConversion.Global
			Dictionary domain = (Dictionary)(object)this;
			ArrayList views = new ArrayList();

			foreach (string entityName in domain.Keys)
			{
				SemanticEntity entity = (SemanticEntity)domain[entityName];
				entity._domain = this;

				entity.EditForm = owner as IEditForm;
				entity.GridForm = owner as IGridForm;
				Dictionary entityDic = (Dictionary)(object)entity;
				
				foreach (string memberName in entityDic.Keys)
				{
					SemanticMember member = entityDic[memberName] as SemanticMember;
					if (member == null) continue;

					//if (memberName.CharAt(0) > 'a')
					//	Log.log(entityName, memberName);

					member._domain = this;
					member._entity = entity;
					member._name = memberName;

					member.EditForm = entity.EditForm;
					member.GridForm = entity.GridForm;
					
					views.Add(member);
				}

				views.Add(entity);
			}

			foreach (SemanticEntity view in views)
			{
				view.Initialize();
			}
		}

	}

}


