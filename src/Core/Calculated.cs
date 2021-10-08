using System;


namespace Luxena.Travel
{
	public class Calculated<T>
	{
		public Calculated(Action action)
		{
			_action = action;
			Pending = true;
		}

		public T Get()
		{
			if (Pending)
			{
				_action();

				if (Pending)
					throw new Exception("The action of the calculated doesn't set value");
			}

			return _value;
		}

		public Calculated<T> Set(T value)
		{
			_value = value;
			Pending = false;

			return this;
		}

		public Calculated<T> Reset()
		{
			Pending = true;

			return this;
		}

		public bool Pending { get; private set; }

		private readonly Action _action;
		private T _value;
	}
}