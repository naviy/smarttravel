using System;
using System.Data;

using NHibernate;
using NHibernate.Dialect;
using NHibernate.Type;


namespace Luxena.Base.Data.NHibernate.Type
{
	[Serializable]
	public class UtcKindDateTimeType : DateTimeType
	{
		public override string Name
		{
			get { return "UtcKindDateTime"; }
		}

		public override object FromStringValue(string xml)
		{
			return AsUtc(base.FromStringValue(xml));
		}

		public override object Get(IDataReader rs, int index)
		{
			return AsUtc(base.Get(rs, index));
		}

		public override int GetHashCode(object x, EntityMode entityMode)
		{
			return base.GetHashCode(AsUtc(x), entityMode);
		}

		public override bool IsEqual(object x, object y)
		{
			return base.IsEqual(AsUtc(x), AsUtc(y));
		}

		public override string ObjectToSQLString(object value, Dialect dialect)
		{
			return base.ObjectToSQLString(AsUtc(value), dialect);
		}

		public override void Set(IDbCommand st, object value, int index)
		{
			base.Set(st, AsUtc(value), index);
		}

		public override string ToString(object val)
		{
			return base.ToString(AsUtc(val));
		}

		private static object AsUtc(object obj)
		{
			if (!(obj is DateTime))
				return obj;

			return ((DateTime)obj).AsUtc();
		}
	}
}