using System.Collections.Generic;

using Luxena.Base.Data;
using Luxena.Base.Domain;

namespace Luxena.Travel.Domain
{
	public class AppStateChangesHolding
	{
		public void AddAviaDocument(User user, Entity2 document)
		{
			AddEntity(_importedDocuments, user, document);
		}

		public void AddTask(User user, Task task)
		{
			AddEntity(_assignedTasks, user, task);
		}

		public void AddUserRolesChange(User user)
		{
			if (!_userRolesChanges.Contains(user.Name))
				_userRolesChanges.Add(user.Name);
		}

		public IList<EntityReference> GetImportedDocuments(User user)
		{
			return GetEntityList(_importedDocuments, user);
		}

		public IList<EntityReference> GetAssignedTasks(User user)
		{
			return GetEntityList(_assignedTasks, user);
		}

		public bool IsUserRolesChanged(User user)
		{
			var res = _userRolesChanges.Contains(user.Name);

			_userRolesChanges.Remove(user.Name);

			return res;
		}

		public void ClearUserData(User user)
		{
			_importedDocuments.Remove(user.Name);

			_assignedTasks.Remove(user.Name);

			_userRolesChanges.Remove(user.Name);

		}

		private static void AddEntity(IDictionary<string, List<EntityReference>> dictionary, User user, Entity2 entity)
		{
			if (!dictionary.ContainsKey(user.Name))
				dictionary.Add(user.Name, new List<EntityReference>());

			dictionary[user.Name].Add(entity);
		}

		private static IList<EntityReference> GetEntityList(IDictionary<string, List<EntityReference>> dictionary, User user)
		{
			List<EntityReference> items = null;

			if (dictionary.ContainsKey(user.Name))
			{
				items = dictionary[user.Name];
				dictionary[user.Name] = new List<EntityReference>();
			}

			return items ?? new List<EntityReference>();
		}

		private readonly IDictionary<string, List<EntityReference>> _importedDocuments =
			new Dictionary<string, List<EntityReference>>();

		private readonly IDictionary<string, List<EntityReference>> _assignedTasks =
			new Dictionary<string, List<EntityReference>>();

		private static readonly IList<string> _userRolesChanges = new List<string>();
	}
}