using System;


namespace Luxena
{

	public class Lazy<TOwner, TValue>
		where TOwner: class
	{

		public Lazy(Func<TOwner, TValue> defaultGetter)
		{
			DefaultGetter = defaultGetter;
		}

		public Func<TOwner, TValue> DefaultGetter { get; private set; }

		public TValue NewValue { get; private set; }

		public bool IsDirty { get; private set; }


		public TValue Get(TOwner owner)
		{
			return IsDirty ? NewValue : DefaultGetter(owner);
		}

		public void Set(TValue value)
		{
			if (Equals(value, NewValue)) return;

			NewValue = value;
			IsDirty = true;
		}


		public static implicit operator bool(Lazy<TOwner, TValue> me)
		{
			return me.IsDirty;
		}

		public static implicit operator TValue(Lazy<TOwner, TValue> me)
		{
			return me.NewValue;
		}

		//public static implicit operator Lazy<TOwner, TValue>(Func<TOwner, TValue> defaultGetter)
		//{
		//	return new Lazy<TOwner, TValue>(defaultGetter);
		//}

	}

}
