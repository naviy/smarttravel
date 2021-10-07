using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Domain;
using Luxena.Base.Metamodel;


namespace Luxena.Travel.Domain
{

	public class AppStateService : DomainService
	{
		public AppStateChangesHolding AppStateChangesHolding { get { return db.Resolve(ref _appStateChangesHolding); } }
		private AppStateChangesHolding _appStateChangesHolding;

		public void RegisterEntity(Entity2 document)
		{
			Person person = null;

			var product = document as Product;
			if (product != null)
				person = product.Seller;
			else
			{
				var voiding = document as AviaDocumentVoiding;
				if (voiding != null)
					person = voiding.Document.Seller;
			}

			foreach (var user in db.User.ListBy(person))
				AppStateChangesHolding.AddAviaDocument(user, document);
		}

		public void RegisterTask(Task task)
		{
			if (task.AssignedTo == null || task.AssignedTo.GetClass().Type != typeof(Person))
				return;

			var person = db.Person.By(task.AssignedTo.Id);

			foreach (var user in db.User.ListBy(person))
				AppStateChangesHolding.AddTask(user, task);
		}

		public void RegisterUserRolesChange(User user)
		{
			AppStateChangesHolding.AddUserRolesChange(user);
		}

		public IList<Entity2> GetImportedDocuments(User user)
		{
			return AppStateChangesHolding.GetImportedDocuments(user)
				.Select(a => (Entity2) db.Load(Class.Of(a.Type).Type, a.Id))
				.Where(a => a != null)
				.ToList();
		}

		public IList<Task> GetAssignedTasks(User user)
		{
			return AppStateChangesHolding.GetAssignedTasks(user)
				.Select(a => db.Task.By(a.Id))
				.Where(a => a != null)
				.ToList();
		}

		public bool IsUserRolesChanged(User user)
		{
			return AppStateChangesHolding.IsUserRolesChanged(user);
		}

		public void ClearUserData(User user)
		{
			AppStateChangesHolding.ClearUserData(user);
		}

	}

}