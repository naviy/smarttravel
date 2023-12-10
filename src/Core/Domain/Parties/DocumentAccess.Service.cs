using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;




namespace Luxena.Travel.Domain
{


	partial class DocumentAccess
	{

		//---g



		public class Service : Entity2Service<DocumentAccess>
		{

			//---g



			public DocumentAccessRestriction GetAccessRestriction()
			{
				return GetAccessRestriction(out _);
			}



			public DocumentAccessRestriction GetAccessRestriction(out bool fullDocumentControl)
			{

				fullDocumentControl = false;


				if (db.IsGranted(UserRole.Administrator, UserRole.Supervisor))
				{
					fullDocumentControl = true;

					return DocumentAccessRestriction.FullAccess;
				}


				if (db.Configuration.SeparateDocumentAccess)
				{

					if (DocumentAccessList.Count == 0)
					{
						return DocumentAccessRestriction.NoAccess;
					}


					if (DocumentAccessList.Count == 1 && DocumentAccessList[0].Owner == null)
					{
						fullDocumentControl = DocumentAccessList[0].FullDocumentControl;

						return DocumentAccessRestriction.FullAccess;
					}


					return DocumentAccessRestriction.RestrictedAccessByOwner;

				}


				if (db.Configuration.SeparateDocumentAccessByAgent)
				{
					return DocumentAccessRestriction.RestrictedAccessByAgent;
				}


				return DocumentAccessRestriction.FullAccess;

			}



			public bool HasAccess(Party documentOwner)
			{
				return HasAccess(documentOwner, out _);
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

					if (accessRestriction == DocumentAccessRestriction.RestrictedAccessByOwner)
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



			public PropertyFilter[] CreateDocumentAgentFilters(string propertyName)
			{

				var otherAgents = OtherAgents;

				if (otherAgents.Count == 0)
					return null;


				return new[]
				{
					new PropertyFilter
					{
						Property = propertyName,
						Conditions = new[]
						{
							new PropertyFilterCondition
							{
								Operator = FilterOperator.IsIdNotInOrIsNull,
								Value = otherAgents.Convert(p => p.Id)
							}
						}
					},

					new PropertyFilter
					{
						Property = "IsProcessed",
						Conditions = new[]
						{
							new PropertyFilterCondition
							{
								Operator = FilterOperator.Equals,
								Value = true
							}
						}
					},

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




			private IList<Person> OtherAgents
			{
				get
				{
					return _otherUserPersons ?? (_otherUserPersons = db.User
						.ListBy(a => a.Person != db.Security.Person)
						.Where(a => a.Person != null && a.Active)
						.ToList(a => a.Person)
					);
				}
			}



			//---g



			private IList<DocumentAccess> _documentAccessList;
			private IList<Person> _otherUserPersons;



			//---g

		}



		//---g

	}



}