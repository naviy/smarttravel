using LxnBase.UI;

using Luxena.Travel.Controls;


namespace Luxena.Travel
{

	public abstract class EntityListTab : AutoListTabExt2
	{

		protected EntityListTab(string tabId, ListArgs args) : base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			dsm = new SemanticDomain(this);
		}

		protected SemanticDomain dsm;
		protected SemanticEntity Entity;

		protected virtual string ClassName { get { return Entity._className; } }


		protected override void HandleInsertPress()
		{
			Create(ClassName);
		}

	}


	public class Entity3ListTab : EntityListTab
	{

		public Entity3ListTab(string tabId, ListArgs args) : base(tabId, args) { }

		protected override void CreateColumnConfigs()
		{
			Entity3Semantic se = (Entity3Semantic)Entity;

			AddColumns(new object[]
			{
				se.Name,

				se.CreatedOn.ToColumn(true, 90),
				se.CreatedBy.ToColumn(true, 90),
				se.ModifiedOn.ToColumn(true, 90),
				se.ModifiedBy.ToColumn(true, 90),
			});
		}
	}

	public class Entity3DListTab : EntityListTab
	{

		public Entity3DListTab(string tabId, ListArgs args) : base(tabId, args) { }

		protected override void CreateColumnConfigs()
		{
			Entity3DSemantic se = (Entity3DSemantic)Entity;

			AddColumns(new object[]
			{
				se.Name,
				se.Description,

				se.CreatedOn.ToColumn(true, 90),
				se.CreatedBy.ToColumn(true, 90),
				se.ModifiedOn.ToColumn(true, 90),
				se.ModifiedBy.ToColumn(true, 90),
			});
		}
	}

}