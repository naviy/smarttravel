using System.Collections.Generic;
using System.Linq;


namespace Luxena.Travel.Domain
{

	public abstract partial class Identity : Entity3D
	{

		public virtual IList<Preferences> Preferences => _preferences;

		public virtual T GetPreferences<T>()
		{
			if (_preferences == null)
				return default(T);

			foreach (var preferences in _preferences.OfType<T>())
				return preferences;

			return default (T);
		}

		protected void CopyPreferences(Identity src)
		{
			foreach (var preferences in _preferences)
			{
				var newPreferences = (Preferences) preferences.Clone();

				preferences.Identity = src;

				src._preferences.Add(newPreferences);
			}
		}

		private readonly IList<Preferences> _preferences = new List<Preferences>();
	}

}