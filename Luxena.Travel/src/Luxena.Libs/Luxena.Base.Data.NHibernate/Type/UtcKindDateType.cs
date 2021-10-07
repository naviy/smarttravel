using System;
using System.Collections.Generic;
using System.Data;

using NHibernate;
using NHibernate.Type;


namespace Luxena.Base.Data.NHibernate.Type
{
	[Serializable]
	public class UtcKindDateType : DateType
	{
		public UtcKindDateType() 
		{
			SetParameterValues(new Dictionary<string, string> { { BaseValueParameterName, BaseDateValue.AsUtc().ToString() } });
		}

		public override string Name
		{
			get { return "UtcKindDate"; }
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

		public override string ObjectToSQLString(object value, global::NHibernate.Dialect.Dialect dialect)
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

			return ((DateTime) obj).AsUtc();
		}
	}
}