using System;
using System.Html;
using System.Html.Data;
using System.Serialization;

using Ext.state;

namespace Luxena.Travel
{
	public class LocalStorageProvider : Provider
	{
		public static bool IsSupported()
		{
			return !Script.IsNullOrUndefined(_storage);
		}

		public void set(string name, object value)
		{
			if (Script.IsNullOrUndefined(value))
			{
				clear(name);

				return;
			}

			_storage.SetItem(name, Json.Stringify(value));

			base.set(name, value);
		}

		public void clear(string name)
		{
			_storage.RemoveItem(name);

			base.clear(name);
		}

		public object Get(string name)
		{
			return Json.Parse((string) _storage.GetItem(name));
		}

		private static readonly Storage _storage = Window.LocalStorage;
	}
}