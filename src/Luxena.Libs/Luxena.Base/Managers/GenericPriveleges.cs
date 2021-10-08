using System;


namespace Luxena
{


	[AttributeUsage(AttributeTargets.Class)]
	public class GenericPrivilegesAttribute : Attribute
	{

		public object[] List { get { return _list; } set { _list = value ?? new object[0]; } }

		public object[] Create { get { return _create; } set { _create = value ?? new object[0]; } }

		public object[] Copy { get { return _copy; } set { _copy = value ?? new object[0]; } }

		public object[] View { get { return _view; } set { _view = value ?? new object[0]; } }

		public object[] Update { get { return _update; } set { _update = value ?? new object[0]; } }

		public object[] Delete { get { return _delete; } set { _delete = value ?? new object[0]; } }

		public object[] Replace { get { return _replace; } set { _replace = value ?? new object[0]; } }


		private object[] _list;
		private object[] _create;
		private object[] _copy;
		private object[] _view;
		private object[] _update;
		private object[] _delete;
		private object[] _replace;
	}



}