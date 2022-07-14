using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;


namespace Luxena.Travel.Domain
{

	partial class DocumentAccess
	{

		public class Service : Entity2Service<DocumentAccess>
		{

			public DocumentAccessRestriction GetAccessRestriction()
			{
				bool fullDocumentControl;

				return GetAccessRestriction(out fullDocumentControl);
			}

			public DocumentAccessRestriction GetAccessRestriction(out bool fullDocumentControl)
			{
				fullDocumentControl = false;

				if (db.IsGranted(UserRole.Administrator, UserRole.Supervisor))
				{
					fullDocumentControl = true;

					return DocumentAccessRestriction.FullAccess;
				}

				if (!db.Configuration.SeparateDocumentAccess)
					return DocumentAccessRestriction.FullAccess;

				if (DocumentAccessList.Count == 0)
					return DocumentAccessRestriction.NoAccess;

				if (DocumentAccessList.Count == 1 && DocumentAccessList[0].Owner == null)
				{
					fullDocumentControl = DocumentAccessList[0].FullDocumentControl;

					return DocumentAccessRestriction.FullAccess;
				}

				return DocumentAccessRestriction.RestrictedAccess;
			}

			public bool HasAccess(Party documentOwner)
			{
				bool fullDocumentControl;

				return HasAccess(documentOwner, out fullDocumentControl);
			}

			public bool HasAccess(Party documentOwner, out bool fullDocumentControl)
			{
				if (db.IsGranted(UserRole.Administrator, UserRole.Supervisor))
				{
					fullDocumentControl = true;
					return true;
				}

				if (!db.Configuration.SeparateDocumentAccess)
				{
					fullDocumentControl = false;
					return true;
				}

				if (DocumentAccessList.Count == 1 && DocumentAccessList[0].Owner == null)
				{
					fullDocumentControl = DocumentAccessList[0].FullDocumentControl;
					return true;
				}

				var documentAccess = DocumentAccessList.By(d => Equals(d.Owner, documentOwner));

				if (documentAccess == null)
				{
					fullDocumentControl = false;
					return false;
				}

				fullDocumentControl = documentAccess.FullDocumentControl;

				return true;
			}

			public IList<Party> GetMappedOwners()
			{
				return DocumentAccessList
					.Where(d => d.Owner != null)
					.Select(d => d.Owner)
					.ToList();
			}

			public IList<Party> GetDocumentOwners()
			{
				IList<Party> owners;

				var accessRestriction = GetAccessRestriction();

				if (accessRestriction == DocumentAccessRestriction.NoAccess)
				{
					owners = new List<Party>();
				}
				else
				{

					Party owner = null;

					owners = Session.QueryOver<DocumentOwner>()
						.Select(o => o.Owner)
						.JoinAlias(o => o.Owner, () => owner)
						.Where(o => o.IsActive)
						.OrderBy(o => o.IsDefault).Desc
						.OrderBy(o => owner.Name).Asc
						.Cacheable()
						.List<Party>();

					if (accessRestriction == DocumentAccessRestriction.RestrictedAccess)
					{
						var documentOwners = GetMappedOwners();

						for (var i = owners.Count - 1; i >= 0; i--)
							if (!documentOwners.Contains(owners[i]))
								owners.RemoveAt(i);
					}
				}

				if (owners.Count == 0)
					owners.Add(db.Configuration.Company);

				return owners;
			}


			public PropertyFilter CreateDocumentOwnerFilter(string propertyName)
			{
				var owners = GetMappedOwners();

				if (owners.Count == 0)
					return null;

				return new PropertyFilter
				{
					Property = propertyName,
					Conditions = new[]
					{
						new PropertyFilterCondition
						{
							Operator = FilterOperator.IsIdInOrIsNull,//IsIdIn,
							Value = owners.Convert(p => p.Id)
						}
					}
				};
			}


			private IList<DocumentAccess> DocumentAccessList
			{
				get
				{
					return _documentAccessList ?? (_documentAccessList =
						ListBy(a => a.Person == db.Security.Person)
					);
				}
			}

			private IList<DocumentAccess> _documentAccessList;

		}

	}

}