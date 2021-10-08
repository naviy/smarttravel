using System;
using System.Diagnostics;
using System.Globalization;

// ReSharper disable once MemberHidesStaticFromOuterClass

namespace Luxena
{
	public static partial class CodePatternExtentions
	{

		public static CultureInfo DefaultCulture = CultureInfo.InvariantCulture;


		#region Object As...

		#region Object Base

		public static ObjectValue As(this object value)
		{
			return new ObjectValue(value);
		}

		public static object As(this object value, Type type)
		{
			if (type == typeof(object))
				return value;

			var v = new ObjectValue(value);

			if (type == typeof(int))
				return v.Int;
			if (type == typeof(int?))
				return v.Intn;

			if (type == typeof(DateTime))
				return v.DateTime;
			if (type == typeof(DateTime?))
				return v.DateTimen;


			if (type == typeof(bool))
				return v.Bool;
			if (type == typeof(bool?))
				return v.Booln;

			if (type == typeof(byte))
				return v.Byte;
			if (type == typeof(byte?))
				return v.Byten;

			if (type == typeof(decimal))
				return v.Decimal;
			if (type == typeof(decimal?))
				return v.Decimaln;

			if (type == typeof(double))
				return v.Double;
			if (type == typeof(double?))
				return v.Doublen;

			if (type == typeof(float))
				return v.Float;
			if (type == typeof(float?))
				return v.Floatn;

			if (type == typeof(long))
				return v.Long;
			if (type == typeof(long?))
				return v.Longn;

			if (type == typeof(sbyte))
				return v.SByte;
			if (type == typeof(sbyte?))
				return v.SByten;

			if (type == typeof(short))
				return v.Short;
			if (type == typeof(short?))
				return v.Shortn;

			if (type == typeof(uint))
				return v.UInt;
			if (type == typeof(uint?))
				return v.UIntn;

			if (type == typeof(ulong))
				return v.ULong;
			if (type == typeof(ulong?))
				return v.ULongn;

			if (type == typeof(ushort))
				return v.UShort;
			if (type == typeof(ushort?))
				return v.UShortn;

			return value;
		}

		public partial struct ObjectValue
		{
			public readonly object Value;

			public ObjectValue(object value)
				: this()
			{
				Value = value;
			}
		}

		static bool IsEmpty(object value)
		{
			string s;
			return value == null || Convert.IsDBNull(value) || ((s = value as string) != null && string.IsNullOrEmpty(s));
		}

		#endregion


		#region Object As Bool

		[DebuggerStepThrough]
		private static bool TryBool(object value, out bool result)
		{
			if (IsEmpty(value))
			{
				result = false;
				return false;
			}
			result = Convert.ToBoolean(value);
			return true;
		}


		[DebuggerStepThrough]
		public static bool As(this object value, bool defaults)
		{
			bool result;
			return TryBool(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static bool? As(this object value, bool? defaults)
		{
			bool result;
			return TryBool(value, out result) ? result : defaults;
		}

		public partial struct ObjectValue
		{
			public bool Bool
			{
				[DebuggerStepThrough]
				get
				{
					bool result;
					return CodePatternExtentions.TryBool(Value, out result) && result;
				}
			}

			public bool? Booln
			{
				[DebuggerStepThrough]
				get
				{
					bool result;
					return CodePatternExtentions.TryBool(Value, out result) ? (bool?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryBool(out bool result)
			{
				return CodePatternExtentions.TryBool(Value, out result);
			}
		}


		[DebuggerStepThrough]
		public static bool Bool(this ObjectValue value, bool defaults)
		{
			bool result;
			return TryBool(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static bool? Booln(this ObjectValue value, bool? defaults)
		{
			bool result;
			return TryBool(value.Value, out result) ? result : defaults;
		}

		#endregion


		#region Object As Byte

		[DebuggerStepThrough]
		private static bool TryByte(object value, out byte result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToByte(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryByte(object value, IFormatProvider provider, out byte result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToByte(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static byte As(this object value, byte defaults)
		{
			byte result;
			return TryByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte As(this object value, IFormatProvider provider, byte defaults)
		{
			byte result;
			return TryByte(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? As(this object value, byte? defaults)
		{
			byte result;
			return TryByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? As(this object value, IFormatProvider provider, byte? defaults)
		{
			byte result;
			return TryByte(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			public byte Byte
			{
				[DebuggerStepThrough]
				get
				{
					byte result;
					return CodePatternExtentions.TryByte(Value, out result) ? result : (byte)0;
				}
			}

			public byte? Byten
			{
				[DebuggerStepThrough]
				get
				{
					byte result;
					return CodePatternExtentions.TryByte(Value, out result) ? (byte?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryByte(out byte result)
			{
				return CodePatternExtentions.TryByte(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryByte(IFormatProvider provider, out byte result)
			{
				return CodePatternExtentions.TryByte(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static byte Byte(this ObjectValue value, byte defaults)
		{
			byte result;
			return TryByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte Byte(this ObjectValue value, IFormatProvider provider, byte defaults)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte Byte(this ObjectValue value, IFormatProvider provider)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? result : (byte)0;
		}


		[DebuggerStepThrough]
		public static byte? Byten(this ObjectValue value, byte? defaults)
		{
			byte result;
			return TryByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? Byten(this ObjectValue value, IFormatProvider provider, byte? defaults)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? Byten(this ObjectValue value, IFormatProvider provider)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? (byte?)result : null;
		}

		#endregion


		#region Object As DateTime

		[DebuggerStepThrough]
		private static bool TryDateTime(object value, out DateTime result)
		{
			if (IsEmpty(value)) { result = System.DateTime.MinValue; return false; }
			result = Convert.ToDateTime(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryDateTime(object value, IFormatProvider provider, out DateTime result)
		{
			if (IsEmpty(value)) { result = System.DateTime.MinValue; return false; }
			result = Convert.ToDateTime(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static DateTime As(this object value, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime As(this object value, IFormatProvider provider, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value, provider, out result) ? result : defaults;
		}


		[DebuggerStepThrough]
		public static DateTime? As(this object value, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime? As(this object value, IFormatProvider provider, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			public DateTime DateTime
			{
				[DebuggerStepThrough]
				get
				{
					DateTime result;
					return CodePatternExtentions.TryDateTime(Value, out result) ? result : DateTime.MinValue;
				}
			}

			public DateTime? DateTimen
			{
				[DebuggerStepThrough]
				get
				{
					DateTime result;
					return CodePatternExtentions.TryDateTime(Value, out result) ? (DateTime?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryDateTime(out DateTime result)
			{
				return CodePatternExtentions.TryDateTime(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryDateTime(IFormatProvider provider, out DateTime result)
			{
				return CodePatternExtentions.TryDateTime(Value, out result);
			}
		}


		[DebuggerStepThrough]
		public static DateTime DateTime(this ObjectValue value, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime DateTime(this ObjectValue value, IFormatProvider provider, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime DateTime(this ObjectValue value, IFormatProvider provider)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? result : System.DateTime.MinValue;
		}


		[DebuggerStepThrough]
		public static DateTime? DateTimen(this ObjectValue value, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime? DateTimen(this ObjectValue value, IFormatProvider provider, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime? DateTimen(this ObjectValue value, IFormatProvider provider)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? (DateTime?)result : null;
		}

		#endregion


		#region Object As Decimal

		[DebuggerStepThrough]
		private static bool TryDecimal(object value, out decimal result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToDecimal(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryDecimal(object value, IFormatProvider provider, out decimal result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToDecimal(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static decimal As(this object value, decimal defaults)
		{
			decimal result;
			return TryDecimal(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal As(this object value, IFormatProvider provider, decimal defaults)
		{
			decimal result;
			return TryDecimal(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? As(this object value, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? As(this object value, IFormatProvider provider, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			public decimal Decimal
			{
				[DebuggerStepThrough]
				get
				{
					decimal result;
					return CodePatternExtentions.TryDecimal(Value, out result) ? result : 0;
				}
			}

			public decimal? Decimaln
			{
				[DebuggerStepThrough]
				get
				{
					decimal result;
					return CodePatternExtentions.TryDecimal(Value, out result) ? (decimal?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryDecimal(out decimal result)
			{
				return CodePatternExtentions.TryDecimal(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryDecimal(IFormatProvider provider, out decimal result)
			{
				return CodePatternExtentions.TryDecimal(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static decimal Decimal(this ObjectValue value, decimal defaults)
		{
			decimal result;
			return TryDecimal(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal Decimal(this ObjectValue value, IFormatProvider provider, decimal defaults)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal Decimal(this ObjectValue value, IFormatProvider provider)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static decimal? Decimaln(this ObjectValue value, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? Decimaln(this ObjectValue value, IFormatProvider provider, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? Decimaln(this ObjectValue value, IFormatProvider provider)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? (decimal?)result : null;
		}

		#endregion


		#region Object As Double

		[DebuggerStepThrough]
		private static bool TryDouble(object value, out double result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToDouble(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryDouble(object value, IFormatProvider provider, out double result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToDouble(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static double As(this object value, double defaults)
		{
			double result;
			return TryDouble(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double As(this object value, IFormatProvider provider, double defaults)
		{
			double result;
			return TryDouble(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? As(this object value, double? defaults)
		{
			double result;
			return TryDouble(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? As(this object value, IFormatProvider provider, double? defaults)
		{
			double result;
			return TryDouble(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{

			public double Double
			{
				[DebuggerStepThrough]
				get
				{
					double result;
					return CodePatternExtentions.TryDouble(Value, out result) ? result : 0;
				}
			}

			public double? Doublen
			{
				[DebuggerStepThrough]
				get
				{
					double result;
					return CodePatternExtentions.TryDouble(Value, out result) ? (double?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryDouble(out double result)
			{
				return CodePatternExtentions.TryDouble(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryDouble(IFormatProvider provider, out double result)
			{
				return CodePatternExtentions.TryDouble(Value, provider, out result);
			}

		}


		[DebuggerStepThrough]
		public static double Double(this ObjectValue value, double defaults)
		{
			double result;
			return TryDouble(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double Double(this ObjectValue value, IFormatProvider provider, double defaults)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double Double(this ObjectValue value, IFormatProvider provider)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static double? Doublen(this ObjectValue value, double? defaults)
		{
			double result;
			return TryDouble(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? Doublen(this ObjectValue value, IFormatProvider provider, double? defaults)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? Doublen(this ObjectValue value, IFormatProvider provider)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? (double?)result : null;
		}

		#endregion


		#region Object As Int

		[DebuggerStepThrough]
		private static bool TryInt(object value, out int result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt32(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryInt(object value, IFormatProvider provider, out int result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt32(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static int As(this object value, int defaults)
		{
			int result;
			return TryInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int As(this object value, IFormatProvider provider, int defaults)
		{
			int result;
			return TryInt(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? As(this object value, int? defaults)
		{
			int result;
			return TryInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? As(this object value, IFormatProvider provider, int? defaults)
		{
			int result;
			return TryInt(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			public int Int
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt(Value, out result) ? result : 0;
				}
			}

			public int? Intn
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt(Value, out result) ? (int?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryInt(out int result)
			{
				return CodePatternExtentions.TryInt(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt(IFormatProvider provider, out int result)
			{
				return CodePatternExtentions.TryInt(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static int Int(this ObjectValue value, int defaults)
		{
			int result;
			return TryInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int(this ObjectValue value, IFormatProvider provider, int defaults)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int(this ObjectValue value, IFormatProvider provider)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static int? Intn(this ObjectValue value, int? defaults)
		{
			int result;
			return TryInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Intn(this ObjectValue value, IFormatProvider provider, int? defaults)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Intn(this ObjectValue value, IFormatProvider provider)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? (int?)result : null;
		}

		#endregion


		#region Object As Float

		[DebuggerStepThrough]
		private static bool TryFloat(object value, out float result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = (float)Convert.ToDouble(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryFloat(object value, IFormatProvider provider, out float result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = (float)Convert.ToDouble(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static float As(this object value, float defaults)
		{
			float result;
			return TryFloat(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float As(this object value, IFormatProvider provider, float defaults)
		{
			float result;
			return TryFloat(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? As(this object value, float? defaults)
		{
			float result;
			return TryFloat(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? As(this object value, IFormatProvider provider, float? defaults)
		{
			float result;
			return TryFloat(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			public float Float
			{
				[DebuggerStepThrough]
				get
				{
					float result;
					return CodePatternExtentions.TryFloat(Value, out result) ? result : 0;
				}
			}

			public float? Floatn
			{
				[DebuggerStepThrough]
				get
				{
					float result;
					return CodePatternExtentions.TryFloat(Value, out result) ? (float?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryFloat(out float result)
			{
				return CodePatternExtentions.TryFloat(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryFloat(IFormatProvider provider, out float result)
			{
				return CodePatternExtentions.TryFloat(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static float Float(this ObjectValue value, float defaults)
		{
			float result;
			return TryFloat(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float Float(this ObjectValue value, IFormatProvider provider, float defaults)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float Float(this ObjectValue value, IFormatProvider provider)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static float? Floatn(this ObjectValue value, float? defaults)
		{
			float result;
			return TryFloat(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? Floatn(this ObjectValue value, IFormatProvider provider, float? defaults)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? Floatn(this ObjectValue value, IFormatProvider provider)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? (float?)result : null;
		}

		#endregion


		#region Object As Int16

		[DebuggerStepThrough]
		private static bool TryInt16(object value, out Int16 result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt16(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryInt16(object value, IFormatProvider provider, out Int16 result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt16(value, provider);
			return true;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryInt16(out Int16 result)
			{
				return CodePatternExtentions.TryInt16(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt16(IFormatProvider provider, out Int16 result)
			{
				return CodePatternExtentions.TryInt16(Value, provider, out result);
			}

			public Int16 Int16
			{
				[DebuggerStepThrough]
				get
				{
					Int16 result;
					return CodePatternExtentions.TryInt16(Value, out result) ? result : (Int16)0;
				}
			}

			public Int16? Int16n
			{
				[DebuggerStepThrough]
				get
				{
					Int16 result;
					return CodePatternExtentions.TryInt16(Value, out result) ? (Int16?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static Int16 Int16(this ObjectValue value, Int16 defaults)
		{
			Int16 result;
			return TryInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16 Int16(this ObjectValue value, IFormatProvider provider, Int16 defaults)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16 Int16(this ObjectValue value, IFormatProvider provider)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? result : (Int16)0;
		}


		[DebuggerStepThrough]
		public static Int16? Int16n(this ObjectValue value, Int16? defaults)
		{
			Int16 result;
			return TryInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16? Int16n(this ObjectValue value, IFormatProvider provider, Int16? defaults)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16? Int16n(this ObjectValue value, IFormatProvider provider)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? (Int16?)result : null;
		}

		#endregion


		#region Object As Int32

		[DebuggerStepThrough]
		private static bool TryInt32(object value, out int result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt32(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryInt32(object value, IFormatProvider provider, out int result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt32(value, provider);
			return true;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryInt32(out int result)
			{
				return CodePatternExtentions.TryInt32(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt32(IFormatProvider provider, out int result)
			{
				return CodePatternExtentions.TryInt32(Value, provider, out result);
			}

			public int Int32
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt32(Value, out result) ? result : 0;
				}
			}

			public int? Int32n
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt32(Value, out result) ? (int?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static int Int32(this ObjectValue value, int defaults)
		{
			int result;
			return TryInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int32(this ObjectValue value, IFormatProvider provider, int defaults)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int32(this ObjectValue value, IFormatProvider provider)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static int? Int32n(this ObjectValue value, int? defaults)
		{
			int result;
			return TryInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Int32n(this ObjectValue value, IFormatProvider provider, int? defaults)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Int32n(this ObjectValue value, IFormatProvider provider)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? (int?)result : null;
		}

		#endregion


		#region Object As Int64

		[DebuggerStepThrough]
		private static bool TryInt64(object value, out long result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt64(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryInt64(object value, IFormatProvider provider, out long result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt64(value, provider);
			return true;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryInt64(out long result)
			{
				return CodePatternExtentions.TryInt64(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt64(IFormatProvider provider, out long result)
			{
				return CodePatternExtentions.TryInt64(Value, provider, out result);
			}

			public long Int64
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryInt64(Value, out result) ? result : 0;
				}
			}

			public long? Int64n
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryInt64(Value, out result) ? (long?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static long Int64(this ObjectValue value, long defaults)
		{
			long result;
			return TryInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Int64(this ObjectValue value, IFormatProvider provider, long defaults)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Int64(this ObjectValue value, IFormatProvider provider)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static long? Int64n(this ObjectValue value, long? defaults)
		{
			long result;
			return TryInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Int64n(this ObjectValue value, IFormatProvider provider, long? defaults)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Int64n(this ObjectValue value, IFormatProvider provider)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? (long?)result : null;
		}

		#endregion


		#region Object As Long

		[DebuggerStepThrough]
		private static bool TryLong(object value, out long result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt64(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryLong(object value, IFormatProvider provider, out long result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt64(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static long As(this object value, long defaults)
		{
			long result;
			return TryLong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long As(this object value, IFormatProvider provider, long defaults)
		{
			long result;
			return TryLong(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? As(this object value, long? defaults)
		{
			long result;
			return TryLong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? As(this object value, IFormatProvider provider, long? defaults)
		{
			long result;
			return TryLong(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryLong(out long result)
			{
				return CodePatternExtentions.TryLong(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryLong(IFormatProvider provider, out long result)
			{
				return CodePatternExtentions.TryLong(Value, provider, out result);
			}

			public long Long
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryLong(Value, out result) ? result : 0;
				}
			}

			public long? Longn
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryLong(Value, out result) ? (long?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static long Long(this ObjectValue value, long defaults)
		{
			long result;
			return TryLong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Long(this ObjectValue value, IFormatProvider provider, long defaults)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Long(this ObjectValue value, IFormatProvider provider)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static long? Longn(this ObjectValue value, long? defaults)
		{
			long result;
			return TryLong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Longn(this ObjectValue value, IFormatProvider provider, long? defaults)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Longn(this ObjectValue value, IFormatProvider provider)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? (long?)result : null;
		}

		#endregion


		#region Object As SByte

		[DebuggerStepThrough]
		private static bool TrySByte(object value, out sbyte result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToSByte(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TrySByte(object value, IFormatProvider provider, out sbyte result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToSByte(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static sbyte As(this object value, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte As(this object value, IFormatProvider provider, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? As(this object value, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? As(this object value, IFormatProvider provider, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TrySByte(out sbyte result)
			{
				return CodePatternExtentions.TrySByte(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TrySByte(IFormatProvider provider, out sbyte result)
			{
				return CodePatternExtentions.TrySByte(Value, provider, out result);
			}

			public sbyte SByte
			{
				[DebuggerStepThrough]
				get
				{
					sbyte result;
					return CodePatternExtentions.TrySByte(Value, out result) ? result : (sbyte)0;
				}
			}

			public sbyte? SByten
			{
				[DebuggerStepThrough]
				get
				{
					sbyte result;
					return CodePatternExtentions.TrySByte(Value, out result) ? (sbyte?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static sbyte SByte(this ObjectValue value, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte SByte(this ObjectValue value, IFormatProvider provider, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte SByte(this ObjectValue value, IFormatProvider provider)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? result : (sbyte)0;
		}


		[DebuggerStepThrough]
		public static sbyte? SByten(this ObjectValue value, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? SByten(this ObjectValue value, IFormatProvider provider, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? SByten(this ObjectValue value, IFormatProvider provider)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? (sbyte?)result : null;
		}

		#endregion


		#region Object As Short

		[DebuggerStepThrough]
		private static bool TryShort(object value, out short result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt16(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryShort(object value, IFormatProvider provider, out short result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToInt16(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static short As(this object value, short defaults)
		{
			short result;
			return TryShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short As(this object value, IFormatProvider provider, short defaults)
		{
			short result;
			return TryShort(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? As(this object value, short? defaults)
		{
			short result;
			return TryShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? As(this object value, IFormatProvider provider, short? defaults)
		{
			short result;
			return TryShort(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryShort(out short result)
			{
				return CodePatternExtentions.TryShort(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryShort(IFormatProvider provider, out short result)
			{
				return CodePatternExtentions.TryShort(Value, provider, out result);
			}

			public short Short
			{
				[DebuggerStepThrough]
				get
				{
					short result;
					return CodePatternExtentions.TryShort(Value, out result) ? result : (short)0;
				}
			}

			public short? Shortn
			{
				[DebuggerStepThrough]
				get
				{
					short result;
					return CodePatternExtentions.TryShort(Value, out result) ? (short?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static short Short(this ObjectValue value, short defaults)
		{
			short result;
			return TryShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short Short(this ObjectValue value, IFormatProvider provider, short defaults)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short Short(this ObjectValue value, IFormatProvider provider)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? result : (short)0;
		}


		[DebuggerStepThrough]
		public static short? Shortn(this ObjectValue value, short? defaults)
		{
			short result;
			return TryShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? Shortn(this ObjectValue value, IFormatProvider provider, short? defaults)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? Shortn(this ObjectValue value, IFormatProvider provider)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? (short?)result : null;
		}

		#endregion


		#region Object As String

		[DebuggerStepThrough]
		private static bool TryString(object value, out string result)
		{
			if (value == null) { result = null; return false; }
			result = value.ToString();
			return !string.IsNullOrWhiteSpace(result);
		}


		[DebuggerStepThrough]
		public static string As(this object value, string defaults)
		{
			string result;
			return TryString(value, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryString(out string result)
			{
				return CodePatternExtentions.TryString(Value, out result);
			}

			public string String
			{
				[DebuggerStepThrough]
				get
				{
					string result;
					return CodePatternExtentions.TryString(Value, out result) ? result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static string String(this ObjectValue value, string defaults)
		{
			string result;
			return TryString(value.Value, out result) ? result : defaults;
		}

		#endregion


		#region Object As UInt

		[DebuggerStepThrough]
		private static bool TryUInt(object value, out uint result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt32(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryUInt(object value, IFormatProvider provider, out uint result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt32(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static uint As(this object value, uint defaults)
		{
			uint result;
			return TryUInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint As(this object value, IFormatProvider provider, uint defaults)
		{
			uint result;
			return TryUInt(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? As(this object value, uint? defaults)
		{
			uint result;
			return TryUInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? As(this object value, IFormatProvider provider, uint? defaults)
		{
			uint result;
			return TryUInt(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryUInt(out uint result)
			{
				return CodePatternExtentions.TryUInt(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt(IFormatProvider provider, out uint result)
			{
				return CodePatternExtentions.TryUInt(Value, provider, out result);
			}

			public uint UInt
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt(Value, out result) ? result : 0;
				}
			}

			public uint? UIntn
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt(Value, out result) ? (uint?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static uint UInt(this ObjectValue value, uint defaults)
		{
			uint result;
			return TryUInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt(this ObjectValue value, IFormatProvider provider, uint defaults)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt(this ObjectValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static uint? UIntn(this ObjectValue value, uint? defaults)
		{
			uint result;
			return TryUInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UIntn(this ObjectValue value, IFormatProvider provider, uint? defaults)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UIntn(this ObjectValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? (uint?)result : null;
		}

		#endregion


		#region Object As UInt16

		[DebuggerStepThrough]
		private static bool TryUInt16(object value, out UInt16 result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt16(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryUInt16(object value, IFormatProvider provider, out UInt16 result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt16(value, provider);
			return true;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryUInt16(out UInt16 result)
			{
				return CodePatternExtentions.TryUInt16(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt16(IFormatProvider provider, out UInt16 result)
			{
				return CodePatternExtentions.TryUInt16(Value, provider, out result);
			}

			public UInt16 UInt16
			{
				[DebuggerStepThrough]
				get
				{
					UInt16 result;
					return CodePatternExtentions.TryUInt16(Value, out result) ? result : (UInt16)0;
				}
			}

			public UInt16? UInt16n
			{
				[DebuggerStepThrough]
				get
				{
					UInt16 result;
					return CodePatternExtentions.TryUInt16(Value, out result) ? (UInt16?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static UInt16 UInt16(this ObjectValue value, UInt16 defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16 UInt16(this ObjectValue value, IFormatProvider provider, UInt16 defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16 UInt16(this ObjectValue value, IFormatProvider provider)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? result : (UInt16)0;
		}


		[DebuggerStepThrough]
		public static UInt16? UInt16n(this ObjectValue value, UInt16? defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16? UInt16n(this ObjectValue value, IFormatProvider provider, UInt16? defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16? UInt16n(this ObjectValue value, IFormatProvider provider)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? (UInt16?)result : null;
		}

		#endregion


		#region Object As UInt32

		[DebuggerStepThrough]
		private static bool TryUInt32(object value, out uint result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt32(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryUInt32(object value, IFormatProvider provider, out uint result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt32(value, provider);
			return true;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryUInt32(out uint result)
			{
				return CodePatternExtentions.TryUInt32(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt32(IFormatProvider provider, out uint result)
			{
				return CodePatternExtentions.TryUInt32(Value, provider, out result);
			}

			public uint UInt32
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt32(Value, out result) ? result : 0;
				}
			}

			public uint? UInt32n
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt32(Value, out result) ? (uint?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static uint UInt32(this ObjectValue value, uint defaults)
		{
			uint result;
			return TryUInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt32(this ObjectValue value, IFormatProvider provider, uint defaults)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt32(this ObjectValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static uint? UInt32n(this ObjectValue value, uint? defaults)
		{
			uint result;
			return TryUInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UInt32n(this ObjectValue value, IFormatProvider provider, uint? defaults)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UInt32n(this ObjectValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? (uint?)result : null;
		}

		#endregion


		#region Object As UInt64

		[DebuggerStepThrough]
		private static bool TryUInt64(object value, out ulong result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt64(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryUInt64(object value, IFormatProvider provider, out ulong result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt64(value, provider);
			return true;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryUInt64(out ulong result)
			{
				return CodePatternExtentions.TryUInt64(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt64(IFormatProvider provider, out ulong result)
			{
				return CodePatternExtentions.TryUInt64(Value, provider, out result);
			}

			public ulong UInt64
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryUInt64(Value, out result) ? result : 0;
				}
			}

			public ulong? UInt64n
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryUInt64(Value, out result) ? (ulong?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static ulong UInt64(this ObjectValue value, ulong defaults)
		{
			ulong result;
			return TryUInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong UInt64(this ObjectValue value, IFormatProvider provider, ulong defaults)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong UInt64(this ObjectValue value, IFormatProvider provider)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static ulong? UInt64n(this ObjectValue value, ulong? defaults)
		{
			ulong result;
			return TryUInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? UInt64n(this ObjectValue value, IFormatProvider provider, ulong? defaults)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? UInt64n(this ObjectValue value, IFormatProvider provider)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? (ulong?)result : null;
		}

		#endregion


		#region Object As ULong

		[DebuggerStepThrough]
		private static bool TryULong(object value, out ulong result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt64(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryULong(object value, IFormatProvider provider, out ulong result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt64(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static ulong As(this object value, ulong defaults)
		{
			ulong result;
			return TryULong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong As(this object value, IFormatProvider provider, ulong defaults)
		{
			ulong result;
			return TryULong(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? As(this object value, ulong? defaults)
		{
			ulong result;
			return TryULong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? As(this object value, IFormatProvider provider, ulong? defaults)
		{
			ulong result;
			return TryULong(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryULong(out ulong result)
			{
				return CodePatternExtentions.TryULong(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryULong(IFormatProvider provider, out ulong result)
			{
				return CodePatternExtentions.TryULong(Value, provider, out result);
			}

			public ulong ULong
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryULong(Value, out result) ? result : 0;
				}
			}

			public ulong? ULongn
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryULong(Value, out result) ? (ulong?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static ulong ULong(this ObjectValue value, ulong defaults)
		{
			ulong result;
			return TryULong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong ULong(this ObjectValue value, IFormatProvider provider, ulong defaults)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong ULong(this ObjectValue value, IFormatProvider provider)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static ulong? ULongn(this ObjectValue value, ulong? defaults)
		{
			ulong result;
			return TryULong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? ULongn(this ObjectValue value, IFormatProvider provider, ulong? defaults)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? ULongn(this ObjectValue value, IFormatProvider provider)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? (ulong?)result : null;
		}

		#endregion


		#region Object As UShort

		[DebuggerStepThrough]
		private static bool TryUShort(object value, out ushort result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt16(value, DefaultCulture);
			return true;
		}

		[DebuggerStepThrough]
		private static bool TryUShort(object value, IFormatProvider provider, out ushort result)
		{
			if (IsEmpty(value)) { result = 0; return false; }
			result = Convert.ToUInt16(value, provider);
			return true;
		}


		[DebuggerStepThrough]
		public static ushort As(this object value, ushort defaults)
		{
			ushort result;
			return TryUShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort As(this object value, IFormatProvider provider, ushort defaults)
		{
			ushort result;
			return TryUShort(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? As(this object value, ushort? defaults)
		{
			ushort result;
			return TryUShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? As(this object value, IFormatProvider provider, ushort? defaults)
		{
			ushort result;
			return TryUShort(value, provider, out result) ? result : defaults;
		}


		public partial struct ObjectValue
		{
			[DebuggerStepThrough]
			public bool TryUShort(out ushort result)
			{
				return CodePatternExtentions.TryUShort(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUShort(IFormatProvider provider, out ushort result)
			{
				return CodePatternExtentions.TryUShort(Value, provider, out result);
			}

			public ushort UShort
			{
				[DebuggerStepThrough]
				get
				{
					ushort result;
					return CodePatternExtentions.TryUShort(Value, out result) ? result : (ushort)0;
				}
			}

			public ushort? UShortn
			{
				[DebuggerStepThrough]
				get
				{
					ushort result;
					return CodePatternExtentions.TryUShort(Value, out result) ? (ushort?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static ushort UShort(this ObjectValue value, ushort defaults)
		{
			ushort result;
			return TryUShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort UShort(this ObjectValue value, IFormatProvider provider, ushort defaults)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort UShort(this ObjectValue value, IFormatProvider provider)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? result : (ushort)0;
		}


		[DebuggerStepThrough]
		public static ushort? UShortn(this ObjectValue value, ushort? defaults)
		{
			ushort result;
			return TryUShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? UShortn(this ObjectValue value, IFormatProvider provider, ushort? defaults)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? UShortn(this ObjectValue value, IFormatProvider provider)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? (ushort?)result : null;
		}

		#endregion


		#endregion


		#region String As...

		#region String Base

		public static StringValue As(this string value)
		{
			return new StringValue(value);
		}

		public static object As(this string value, Type type)
		{
			if (type == typeof(string))
				return value;

			var v = new StringValue(value);

			if (type == typeof(int))
				return v.Int;
			if (type == typeof(int?))
				return v.Intn;

			if (type == typeof(DateTime))
				return v.DateTime;
			if (type == typeof(DateTime?))
				return v.DateTimen;


			if (type == typeof(bool))
				return v.Bool;
			if (type == typeof(bool?))
				return v.Booln;

			if (type == typeof(byte))
				return v.Byte;
			if (type == typeof(byte?))
				return v.Byten;

			if (type == typeof(decimal))
				return v.Decimal;
			if (type == typeof(decimal?))
				return v.Decimaln;

			if (type == typeof(double))
				return v.Double;
			if (type == typeof(double?))
				return v.Doublen;

			if (type == typeof(float))
				return v.Float;
			if (type == typeof(float?))
				return v.Floatn;

			if (type == typeof(long))
				return v.Long;
			if (type == typeof(long?))
				return v.Longn;

			if (type == typeof(sbyte))
				return v.SByte;
			if (type == typeof(sbyte?))
				return v.SByten;

			if (type == typeof(short))
				return v.Short;
			if (type == typeof(short?))
				return v.Shortn;

			if (type == typeof(TimeSpan))
				return v.TimeSpan;
			if (type == typeof(TimeSpan?))
				return v.TimeSpann;

			if (type == typeof(uint))
				return v.UInt;
			if (type == typeof(uint?))
				return v.UIntn;

			if (type == typeof(ulong))
				return v.ULong;
			if (type == typeof(ulong?))
				return v.ULongn;

			if (type == typeof(ushort))
				return v.UShort;
			if (type == typeof(ushort?))
				return v.UShortn;

			return value;
		}

		public partial struct StringValue
		{
			public readonly string Value;

			public StringValue(string value)
				: this()
			{
				Value = value;
			}
		}

		#endregion


		#region String As Bool

		[DebuggerStepThrough]
		private static bool TryBool(string value, out bool result)
		{
			return bool.TryParse(value, out result);
		}

		[DebuggerStepThrough]
		public static bool As(this string value, bool defaults)
		{
			bool result;
			return TryBool(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static bool? As(this string value, bool? defaults)
		{
			bool result;
			return TryBool(value, out result) ? result : defaults;
		}

		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryBool(out bool result)
			{
				return CodePatternExtentions.TryBool(Value, out result);
			}

			public bool Bool
			{
				[DebuggerStepThrough]
				get
				{
					bool result;
					return CodePatternExtentions.TryBool(Value, out result) && result;
				}
			}

			public bool? Booln
			{
				[DebuggerStepThrough]
				get
				{
					bool result;
					return CodePatternExtentions.TryBool(Value, out result) ? (bool?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static bool Bool(this StringValue value, bool defaults)
		{
			bool result;
			return TryBool(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static bool? Booln(this StringValue value, bool? defaults)
		{
			bool result;
			return TryBool(value.Value, out result) ? result : defaults;
		}

		#endregion


		#region String As Byte

		[DebuggerStepThrough]
		private static bool TryByte(string value, out byte result)
		{
			return byte.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryByte(string value, IFormatProvider provider, out byte result)
		{
			return byte.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static byte As(this string value, byte defaults)
		{
			byte result;
			return TryByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte As(this string value, IFormatProvider provider, byte defaults)
		{
			byte result;
			return TryByte(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? As(this string value, byte? defaults)
		{
			byte result;
			return TryByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? As(this string value, IFormatProvider provider, byte? defaults)
		{
			byte result;
			return TryByte(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			public byte Byte
			{
				[DebuggerStepThrough]
				get
				{
					byte result;
					return CodePatternExtentions.TryByte(Value, out result) ? result : (byte)0;
				}
			}

			public byte? Byten
			{
				[DebuggerStepThrough]
				get
				{
					byte result;
					return CodePatternExtentions.TryByte(Value, out result) ? (byte?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryByte(out byte result)
			{
				return CodePatternExtentions.TryByte(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryByte(IFormatProvider provider, out byte result)
			{
				return CodePatternExtentions.TryByte(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static byte Byte(this StringValue value, byte defaults)
		{
			byte result;
			return TryByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte Byte(this StringValue value, IFormatProvider provider, byte defaults)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte Byte(this StringValue value, IFormatProvider provider)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? result : (byte)0;
		}


		[DebuggerStepThrough]
		public static byte? Byten(this StringValue value, byte? defaults)
		{
			byte result;
			return TryByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? Byten(this StringValue value, IFormatProvider provider, byte? defaults)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static byte? Byten(this StringValue value, IFormatProvider provider)
		{
			byte result;
			return TryByte(value.Value, provider, out result) ? (byte?)result : null;
		}

		#endregion


		#region String As DateTime

		[DebuggerStepThrough]
		private static bool TryDateTime(string value, out DateTime result)
		{
			return System.DateTime.TryParse(
				value, DefaultCulture,
				DateTimeStyles.AllowWhiteSpaces,// | DateTimeStyles.,
				out result
			);
		}

		[DebuggerStepThrough]
		private static bool TryDateTime(string value, IFormatProvider provider, out DateTime result)
		{
			return System.DateTime.TryParse(
				value, provider,
				DateTimeStyles.AllowWhiteSpaces,// | DateTimeStyles.,
				out result
			);
		}


		[DebuggerStepThrough]
		public static DateTime As(this string value, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime As(this string value, IFormatProvider provider, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value, provider, out result) ? result : defaults;
		}


		[DebuggerStepThrough]
		public static DateTime? As(this string value, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime? As(this string value, IFormatProvider provider, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			public DateTime DateTime
			{
				[DebuggerStepThrough]
				get
				{
					DateTime result;
					return CodePatternExtentions.TryDateTime(Value, out result) ? result : DateTime.MinValue;
				}
			}

			public DateTime? DateTimen
			{
				[DebuggerStepThrough]
				get
				{
					DateTime result;
					return CodePatternExtentions.TryDateTime(Value, out result) ? (DateTime?)result : null;
				}
			}

			public DateTime ToDateTime(string format)
			{
				DateTime result;
				return DateTime.TryParseExact(Value, format, DefaultCulture, DateTimeStyles.AllowWhiteSpaces, out result) ? result : DateTime.MinValue;
			}

			public DateTime? ToDateTimen(string format)
			{
				DateTime result;
				return DateTime.TryParseExact(Value, format, DefaultCulture, DateTimeStyles.AllowWhiteSpaces, out result) ? (DateTime?)result : null;
			}

			public TimeSpan? ToTimeSpann(string format)
			{
				TimeSpan result;
				return TimeSpan.TryParseExact(Value, format, DefaultCulture, TimeSpanStyles.None, out result) ? (TimeSpan?)result : null;
			}


			[DebuggerStepThrough]
			public bool TryDateTime(out DateTime result)
			{
				return CodePatternExtentions.TryDateTime(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryDateTime(IFormatProvider provider, out DateTime result)
			{
				return CodePatternExtentions.TryDateTime(Value, out result);
			}
		}

		[DebuggerStepThrough]
		public static DateTime DateTime(this StringValue value, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime DateTime(this StringValue value, IFormatProvider provider, DateTime defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime DateTime(this StringValue value, IFormatProvider provider)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? result : System.DateTime.MinValue;
		}


		[DebuggerStepThrough]
		public static DateTime? DateTimen(this StringValue value, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime? DateTimen(this StringValue value, IFormatProvider provider, DateTime? defaults)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static DateTime? DateTimen(this StringValue value, IFormatProvider provider)
		{
			DateTime result;
			return TryDateTime(value.Value, provider, out result) ? (DateTime?)result : null;
		}

		#endregion


		#region String As Decimal

		[DebuggerStepThrough]
		private static bool TryDecimal(string value, out decimal result)
		{
			return decimal.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryDecimal(string value, IFormatProvider provider, out decimal result)
		{
			return decimal.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static decimal As(this string value, decimal defaults)
		{
			decimal result;
			return TryDecimal(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal As(this string value, IFormatProvider provider, decimal defaults)
		{
			decimal result;
			return TryDecimal(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? As(this string value, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? As(this string value, IFormatProvider provider, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			public decimal Decimal
			{
				[DebuggerStepThrough]
				get
				{
					decimal result;
					return CodePatternExtentions.TryDecimal(Value, out result) ? result : 0;
				}
			}

			public decimal? Decimaln
			{
				[DebuggerStepThrough]
				get
				{
					decimal result;
					return CodePatternExtentions.TryDecimal(Value, out result) ? (decimal?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryDecimal(out decimal result)
			{
				return CodePatternExtentions.TryDecimal(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryDecimal(IFormatProvider provider, out decimal result)
			{
				return CodePatternExtentions.TryDecimal(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static decimal Decimal(this StringValue value, decimal defaults)
		{
			decimal result;
			return TryDecimal(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal Decimal(this StringValue value, IFormatProvider provider, decimal defaults)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal Decimal(this StringValue value, IFormatProvider provider)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static decimal? Decimaln(this StringValue value, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? Decimaln(this StringValue value, IFormatProvider provider, decimal? defaults)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static decimal? Decimaln(this StringValue value, IFormatProvider provider)
		{
			decimal result;
			return TryDecimal(value.Value, provider, out result) ? (decimal?)result : null;
		}

		#endregion


		#region String As Double

		[DebuggerStepThrough]
		private static bool TryDouble(string value, out double result)
		{
			result = 0;
			if (value.No()) return false;

			var sep = DefaultCulture.NumberFormat.NumberDecimalSeparator;

			if (sep == ".")
				value = value.Replace(',', '.');
			else if (sep == ",")
				value = value.Replace('.', ',');

			return double.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryDouble(string value, IFormatProvider provider, out double result)
		{
			return double.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static double As(this string value, double defaults)
		{
			double result;
			return TryDouble(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double As(this string value, IFormatProvider provider, double defaults)
		{
			double result;
			return TryDouble(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? As(this string value, double? defaults)
		{
			double result;
			return TryDouble(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? As(this string value, IFormatProvider provider, double? defaults)
		{
			double result;
			return TryDouble(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{

			public double Double
			{
				[DebuggerStepThrough]
				get
				{
					double result;
					return CodePatternExtentions.TryDouble(Value, out result) ? result : 0;
				}
			}

			public double? Doublen
			{
				[DebuggerStepThrough]
				get
				{
					double result;
					return CodePatternExtentions.TryDouble(Value, out result) ? (double?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryDouble(out double result)
			{
				return CodePatternExtentions.TryDouble(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryDouble(IFormatProvider provider, out double result)
			{
				return CodePatternExtentions.TryDouble(Value, provider, out result);
			}

		}


		[DebuggerStepThrough]
		public static double Double(this StringValue value, double defaults)
		{
			double result;
			return TryDouble(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double Double(this StringValue value, IFormatProvider provider, double defaults)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double Double(this StringValue value, IFormatProvider provider)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static double? Doublen(this StringValue value, double? defaults)
		{
			double result;
			return TryDouble(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? Doublen(this StringValue value, IFormatProvider provider, double? defaults)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static double? Doublen(this StringValue value, IFormatProvider provider)
		{
			double result;
			return TryDouble(value.Value, provider, out result) ? (double?)result : null;
		}

		#endregion


		#region String As Int

		[DebuggerStepThrough]
		private static bool TryInt(string value, out int result)
		{
			return int.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryInt(string value, IFormatProvider provider, out int result)
		{
			return int.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static int As(this string value, int defaults)
		{
			int result;
			return TryInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int As(this string value, IFormatProvider provider, int defaults)
		{
			int result;
			return TryInt(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? As(this string value, int? defaults)
		{
			int result;
			return TryInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? As(this string value, IFormatProvider provider, int? defaults)
		{
			int result;
			return TryInt(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			public int Int
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt(Value, out result) ? result : 0;
				}
			}

			public int? Intn
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt(Value, out result) ? (int?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryInt(out int result)
			{
				return CodePatternExtentions.TryInt(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt(IFormatProvider provider, out int result)
			{
				return CodePatternExtentions.TryInt(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static int Int(this StringValue value, int defaults)
		{
			int result;
			return TryInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int(this StringValue value, IFormatProvider provider, int defaults)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int(this StringValue value, IFormatProvider provider)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static int? Intn(this StringValue value, int? defaults = null)
		{
			int result;
			return TryInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Intn(this StringValue value, IFormatProvider provider, int? defaults)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Intn(this StringValue value, IFormatProvider provider)
		{
			int result;
			return TryInt(value.Value, provider, out result) ? (int?)result : null;
		}

		#endregion


		#region String As Float

		[DebuggerStepThrough]
		private static bool TryFloat(string value, out float result)
		{
			return float.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryFloat(string value, IFormatProvider provider, out float result)
		{
			return float.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static float As(this string value, float defaults)
		{
			float result;
			return TryFloat(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float As(this string value, IFormatProvider provider, float defaults)
		{
			float result;
			return TryFloat(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? As(this string value, float? defaults)
		{
			float result;
			return TryFloat(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? As(this string value, IFormatProvider provider, float? defaults)
		{
			float result;
			return TryFloat(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			public float Float
			{
				[DebuggerStepThrough]
				get
				{
					float result;
					return CodePatternExtentions.TryFloat(Value, out result) ? result : 0;
				}
			}

			public float? Floatn
			{
				[DebuggerStepThrough]
				get
				{
					float result;
					return CodePatternExtentions.TryFloat(Value, out result) ? (float?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryFloat(out float result)
			{
				return CodePatternExtentions.TryFloat(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryFloat(IFormatProvider provider, out float result)
			{
				return CodePatternExtentions.TryFloat(Value, provider, out result);
			}
		}


		[DebuggerStepThrough]
		public static float Float(this StringValue value, float defaults)
		{
			float result;
			return TryFloat(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float Float(this StringValue value, IFormatProvider provider, float defaults)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float Float(this StringValue value, IFormatProvider provider)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static float? Floatn(this StringValue value, float? defaults)
		{
			float result;
			return TryFloat(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? Floatn(this StringValue value, IFormatProvider provider, float? defaults)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static float? Floatn(this StringValue value, IFormatProvider provider)
		{
			float result;
			return TryFloat(value.Value, provider, out result) ? (float?)result : null;
		}

		#endregion


		#region String As Enum

		[DebuggerStepThrough]
		private static bool TryEnum<T>(string value, out T result) where T : struct
		{
			return Enum.TryParse(value, out result);
		}

		[DebuggerStepThrough]
		private static bool TryEnum<T>(string value, bool ignoreCase, out T result) where T : struct
		{
			return Enum.TryParse(value, ignoreCase, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public T Enum<T>() where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, out result) ? result : default(T);
			}

			[DebuggerStepThrough]
			public T? Enumn<T>() where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, out result) ? (T?)result : null;
			}


			[DebuggerStepThrough]
			public T Enum<T>(T defaults) where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, out result) ? result : defaults;
			}

			[DebuggerStepThrough]
			public T Enum<T>(bool ignoreCase, T defaults) where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, ignoreCase, out result) ? result : defaults;
			}

			[DebuggerStepThrough]
			public T Enum<T>(bool ignoreCase) where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, ignoreCase, out result) ? result : default(T);
			}


			[DebuggerStepThrough]
			public T? Enumn<T>(T? defaults) where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, out result) ? result : defaults;
			}

			[DebuggerStepThrough]
			public T? Enumn<T>(bool ignoreCase, T? defaults) where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, ignoreCase, out result) ? result : defaults;
			}

			[DebuggerStepThrough]
			public T? Enumn<T>(bool ignoreCase) where T : struct
			{
				T result;
				return CodePatternExtentions.TryEnum(Value, ignoreCase, out result) ? (T?)result : null;
			}


			[DebuggerStepThrough]
			public bool TryEnum<T>(out T result) where T : struct
			{
				return CodePatternExtentions.TryEnum(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryEnum<T>(bool ignoreCase, out T result) where T : struct
			{
				return CodePatternExtentions.TryEnum(Value, ignoreCase, out result);
			}

		}


		#endregion


		#region String As Int16

		[DebuggerStepThrough]
		private static bool TryInt16(string value, out Int16 result)
		{
			return System.Int16.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryInt16(string value, IFormatProvider provider, out Int16 result)
		{
			return System.Int16.TryParse(value, NumberStyles.Number, provider, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryInt16(out Int16 result)
			{
				return CodePatternExtentions.TryInt16(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt16(IFormatProvider provider, out Int16 result)
			{
				return CodePatternExtentions.TryInt16(Value, provider, out result);
			}

			public Int16 Int16
			{
				[DebuggerStepThrough]
				get
				{
					Int16 result;
					return CodePatternExtentions.TryInt16(Value, out result) ? result : (Int16)0;
				}
			}

			public Int16? Int16n
			{
				[DebuggerStepThrough]
				get
				{
					Int16 result;
					return CodePatternExtentions.TryInt16(Value, out result) ? (Int16?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static Int16 Int16(this StringValue value, Int16 defaults)
		{
			Int16 result;
			return TryInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16 Int16(this StringValue value, IFormatProvider provider, Int16 defaults)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16 Int16(this StringValue value, IFormatProvider provider)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? result : (Int16)0;
		}


		[DebuggerStepThrough]
		public static Int16? Int16n(this StringValue value, Int16? defaults)
		{
			Int16 result;
			return TryInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16? Int16n(this StringValue value, IFormatProvider provider, Int16? defaults)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static Int16? Int16n(this StringValue value, IFormatProvider provider)
		{
			Int16 result;
			return TryInt16(value.Value, provider, out result) ? (Int16?)result : null;
		}

		#endregion


		#region String As Int32

		[DebuggerStepThrough]
		private static bool TryInt32(string value, out int result)
		{
			return int.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryInt32(string value, IFormatProvider provider, out int result)
		{
			return int.TryParse(value, NumberStyles.Number, provider, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryInt32(out int result)
			{
				return CodePatternExtentions.TryInt32(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt32(IFormatProvider provider, out int result)
			{
				return CodePatternExtentions.TryInt32(Value, provider, out result);
			}

			public int Int32
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt32(Value, out result) ? result : 0;
				}
			}

			public int? Int32n
			{
				[DebuggerStepThrough]
				get
				{
					int result;
					return CodePatternExtentions.TryInt32(Value, out result) ? (int?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static int Int32(this StringValue value, int defaults)
		{
			int result;
			return TryInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int32(this StringValue value, IFormatProvider provider, int defaults)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int Int32(this StringValue value, IFormatProvider provider)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static int? Int32n(this StringValue value, int? defaults)
		{
			int result;
			return TryInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Int32n(this StringValue value, IFormatProvider provider, int? defaults)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static int? Int32n(this StringValue value, IFormatProvider provider)
		{
			int result;
			return TryInt32(value.Value, provider, out result) ? (int?)result : null;
		}

		#endregion


		#region String As Int64

		[DebuggerStepThrough]
		private static bool TryInt64(string value, out long result)
		{
			return long.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryInt64(string value, IFormatProvider provider, out long result)
		{
			return long.TryParse(value, NumberStyles.Number, provider, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryInt64(out long result)
			{
				return CodePatternExtentions.TryInt64(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryInt64(IFormatProvider provider, out long result)
			{
				return CodePatternExtentions.TryInt64(Value, provider, out result);
			}

			public long Int64
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryInt64(Value, out result) ? result : 0;
				}
			}

			public long? Int64n
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryInt64(Value, out result) ? (long?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static long Int64(this StringValue value, long defaults)
		{
			long result;
			return TryInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Int64(this StringValue value, IFormatProvider provider, long defaults)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Int64(this StringValue value, IFormatProvider provider)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static long? Int64n(this StringValue value, long? defaults)
		{
			long result;
			return TryInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Int64n(this StringValue value, IFormatProvider provider, long? defaults)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Int64n(this StringValue value, IFormatProvider provider)
		{
			long result;
			return TryInt64(value.Value, provider, out result) ? (long?)result : null;
		}

		#endregion


		#region String As Long

		[DebuggerStepThrough]
		private static bool TryLong(string value, out long result)
		{
			return long.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryLong(string value, IFormatProvider provider, out long result)
		{
			return long.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static long As(this string value, long defaults)
		{
			long result;
			return TryLong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long As(this string value, IFormatProvider provider, long defaults)
		{
			long result;
			return TryLong(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? As(this string value, long? defaults)
		{
			long result;
			return TryLong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? As(this string value, IFormatProvider provider, long? defaults)
		{
			long result;
			return TryLong(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryLong(out long result)
			{
				return CodePatternExtentions.TryLong(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryLong(IFormatProvider provider, out long result)
			{
				return CodePatternExtentions.TryLong(Value, provider, out result);
			}

			public long Long
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryLong(Value, out result) ? result : 0;
				}
			}

			public long? Longn
			{
				[DebuggerStepThrough]
				get
				{
					long result;
					return CodePatternExtentions.TryLong(Value, out result) ? (long?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static long Long(this StringValue value, long defaults)
		{
			long result;
			return TryLong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Long(this StringValue value, IFormatProvider provider, long defaults)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long Long(this StringValue value, IFormatProvider provider)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static long? Longn(this StringValue value, long? defaults)
		{
			long result;
			return TryLong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Longn(this StringValue value, IFormatProvider provider, long? defaults)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static long? Longn(this StringValue value, IFormatProvider provider)
		{
			long result;
			return TryLong(value.Value, provider, out result) ? (long?)result : null;
		}

		#endregion


		#region String As SByte

		[DebuggerStepThrough]
		private static bool TrySByte(string value, out sbyte result)
		{
			return sbyte.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TrySByte(string value, IFormatProvider provider, out sbyte result)
		{
			return sbyte.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static sbyte As(this string value, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte As(this string value, IFormatProvider provider, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? As(this string value, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? As(this string value, IFormatProvider provider, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TrySByte(out sbyte result)
			{
				return CodePatternExtentions.TrySByte(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TrySByte(IFormatProvider provider, out sbyte result)
			{
				return CodePatternExtentions.TrySByte(Value, provider, out result);
			}

			public sbyte SByte
			{
				[DebuggerStepThrough]
				get
				{
					sbyte result;
					return CodePatternExtentions.TrySByte(Value, out result) ? result : (sbyte)0;
				}
			}

			public sbyte? SByten
			{
				[DebuggerStepThrough]
				get
				{
					sbyte result;
					return CodePatternExtentions.TrySByte(Value, out result) ? (sbyte?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static sbyte SByte(this StringValue value, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte SByte(this StringValue value, IFormatProvider provider, sbyte defaults)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte SByte(this StringValue value, IFormatProvider provider)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? result : (sbyte)0;
		}


		[DebuggerStepThrough]
		public static sbyte? SByten(this StringValue value, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? SByten(this StringValue value, IFormatProvider provider, sbyte? defaults)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static sbyte? SByten(this StringValue value, IFormatProvider provider)
		{
			sbyte result;
			return TrySByte(value.Value, provider, out result) ? (sbyte?)result : null;
		}

		#endregion


		#region String As Short

		[DebuggerStepThrough]
		private static bool TryShort(string value, out short result)
		{
			return short.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryShort(string value, IFormatProvider provider, out short result)
		{
			return short.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static short As(this string value, short defaults)
		{
			short result;
			return TryShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short As(this string value, IFormatProvider provider, short defaults)
		{
			short result;
			return TryShort(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? As(this string value, short? defaults)
		{
			short result;
			return TryShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? As(this string value, IFormatProvider provider, short? defaults)
		{
			short result;
			return TryShort(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryShort(out short result)
			{
				return CodePatternExtentions.TryShort(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryShort(IFormatProvider provider, out short result)
			{
				return CodePatternExtentions.TryShort(Value, provider, out result);
			}

			public short Short
			{
				[DebuggerStepThrough]
				get
				{
					short result;
					return CodePatternExtentions.TryShort(Value, out result) ? result : (short)0;
				}
			}

			public short? Shortn
			{
				[DebuggerStepThrough]
				get
				{
					short result;
					return CodePatternExtentions.TryShort(Value, out result) ? (short?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static short Short(this StringValue value, short defaults)
		{
			short result;
			return TryShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short Short(this StringValue value, IFormatProvider provider, short defaults)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short Short(this StringValue value, IFormatProvider provider)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? result : (short)0;
		}


		[DebuggerStepThrough]
		public static short? Shortn(this StringValue value, short? defaults)
		{
			short result;
			return TryShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? Shortn(this StringValue value, IFormatProvider provider, short? defaults)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static short? Shortn(this StringValue value, IFormatProvider provider)
		{
			short result;
			return TryShort(value.Value, provider, out result) ? (short?)result : null;
		}

		#endregion


		#region String As TimeSpan

		[DebuggerStepThrough]
		private static bool TryTimeSpan(string value, out TimeSpan result)
		{
			return System.TimeSpan.TryParse(value, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryTimeSpan(string value, IFormatProvider provider, out TimeSpan result)
		{
			return System.TimeSpan.TryParse(value, provider, out result);
		}


		[DebuggerStepThrough]
		public static TimeSpan As(this string value, TimeSpan defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static TimeSpan As(this string value, IFormatProvider provider, TimeSpan defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value, provider, out result) ? result : defaults;
		}


		[DebuggerStepThrough]
		public static TimeSpan? As(this string value, TimeSpan? defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static TimeSpan? As(this string value, IFormatProvider provider, TimeSpan? defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			public TimeSpan TimeSpan
			{
				[DebuggerStepThrough]
				get
				{
					TimeSpan result;
					return CodePatternExtentions.TryTimeSpan(Value, out result) ? result : TimeSpan.MinValue;
				}
			}

			public TimeSpan? TimeSpann
			{
				[DebuggerStepThrough]
				get
				{
					TimeSpan result;
					return CodePatternExtentions.TryTimeSpan(Value, out result) ? (TimeSpan?)result : null;
				}
			}

			[DebuggerStepThrough]
			public bool TryTimeSpan(out TimeSpan result)
			{
				return CodePatternExtentions.TryTimeSpan(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryTimeSpan(IFormatProvider provider, out TimeSpan result)
			{
				return CodePatternExtentions.TryTimeSpan(Value, out result);
			}
		}


		[DebuggerStepThrough]
		public static TimeSpan TimeSpan(this StringValue value, TimeSpan defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static TimeSpan TimeSpan(this StringValue value, IFormatProvider provider, TimeSpan defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static TimeSpan TimeSpan(this StringValue value, IFormatProvider provider)
		{
			TimeSpan result;
			return TryTimeSpan(value.Value, provider, out result) ? result : System.TimeSpan.MinValue;
		}


		[DebuggerStepThrough]
		public static TimeSpan? TimeSpann(this StringValue value, TimeSpan? defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static TimeSpan? TimeSpann(this StringValue value, IFormatProvider provider, TimeSpan? defaults)
		{
			TimeSpan result;
			return TryTimeSpan(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static TimeSpan? TimeSpann(this StringValue value, IFormatProvider provider)
		{
			TimeSpan result;
			return TryTimeSpan(value.Value, provider, out result) ? (TimeSpan?)result : null;
		}

		#endregion


		#region String As UInt

		[DebuggerStepThrough]
		private static bool TryUInt(string value, out uint result)
		{
			return uint.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryUInt(string value, IFormatProvider provider, out uint result)
		{
			return uint.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static uint As(this string value, uint defaults)
		{
			uint result;
			return TryUInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint As(this string value, IFormatProvider provider, uint defaults)
		{
			uint result;
			return TryUInt(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? As(this string value, uint? defaults)
		{
			uint result;
			return TryUInt(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? As(this string value, IFormatProvider provider, uint? defaults)
		{
			uint result;
			return TryUInt(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryUInt(out uint result)
			{
				return CodePatternExtentions.TryUInt(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt(IFormatProvider provider, out uint result)
			{
				return CodePatternExtentions.TryUInt(Value, provider, out result);
			}

			public uint UInt
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt(Value, out result) ? result : 0;
				}
			}

			public uint? UIntn
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt(Value, out result) ? (uint?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static uint UInt(this StringValue value, uint defaults)
		{
			uint result;
			return TryUInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt(this StringValue value, IFormatProvider provider, uint defaults)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt(this StringValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static uint? UIntn(this StringValue value, uint? defaults)
		{
			uint result;
			return TryUInt(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UIntn(this StringValue value, IFormatProvider provider, uint? defaults)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UIntn(this StringValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt(value.Value, provider, out result) ? (uint?)result : null;
		}

		#endregion


		#region String As UInt16

		[DebuggerStepThrough]
		private static bool TryUInt16(string value, out UInt16 result)
		{
			return System.UInt16.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryUInt16(string value, IFormatProvider provider, out UInt16 result)
		{
			return System.UInt16.TryParse(value, NumberStyles.Number, provider, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryUInt16(out UInt16 result)
			{
				return CodePatternExtentions.TryUInt16(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt16(IFormatProvider provider, out UInt16 result)
			{
				return CodePatternExtentions.TryUInt16(Value, provider, out result);
			}

			public UInt16 UInt16
			{
				[DebuggerStepThrough]
				get
				{
					UInt16 result;
					return CodePatternExtentions.TryUInt16(Value, out result) ? result : (UInt16)0;
				}
			}

			public UInt16? UInt16n
			{
				[DebuggerStepThrough]
				get
				{
					UInt16 result;
					return CodePatternExtentions.TryUInt16(Value, out result) ? (UInt16?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static UInt16 UInt16(this StringValue value, UInt16 defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16 UInt16(this StringValue value, IFormatProvider provider, UInt16 defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16 UInt16(this StringValue value, IFormatProvider provider)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? result : (UInt16)0;
		}


		[DebuggerStepThrough]
		public static UInt16? UInt16n(this StringValue value, UInt16? defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16? UInt16n(this StringValue value, IFormatProvider provider, UInt16? defaults)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static UInt16? UInt16n(this StringValue value, IFormatProvider provider)
		{
			UInt16 result;
			return TryUInt16(value.Value, provider, out result) ? (UInt16?)result : null;
		}

		#endregion


		#region String As UInt32

		[DebuggerStepThrough]
		private static bool TryUInt32(string value, out uint result)
		{
			return uint.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryUInt32(string value, IFormatProvider provider, out uint result)
		{
			return uint.TryParse(value, NumberStyles.Number, provider, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryUInt32(out uint result)
			{
				return CodePatternExtentions.TryUInt32(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt32(IFormatProvider provider, out uint result)
			{
				return CodePatternExtentions.TryUInt32(Value, provider, out result);
			}

			public uint UInt32
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt32(Value, out result) ? result : 0;
				}
			}

			public uint? UInt32n
			{
				[DebuggerStepThrough]
				get
				{
					uint result;
					return CodePatternExtentions.TryUInt32(Value, out result) ? (uint?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static uint UInt32(this StringValue value, uint defaults)
		{
			uint result;
			return TryUInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt32(this StringValue value, IFormatProvider provider, uint defaults)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint UInt32(this StringValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static uint? UInt32n(this StringValue value, uint? defaults)
		{
			uint result;
			return TryUInt32(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UInt32n(this StringValue value, IFormatProvider provider, uint? defaults)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static uint? UInt32n(this StringValue value, IFormatProvider provider)
		{
			uint result;
			return TryUInt32(value.Value, provider, out result) ? (uint?)result : null;
		}

		#endregion


		#region String As UInt64

		[DebuggerStepThrough]
		private static bool TryUInt64(string value, out ulong result)
		{
			return ulong.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryUInt64(string value, IFormatProvider provider, out ulong result)
		{
			return ulong.TryParse(value, NumberStyles.Number, provider, out result);
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryUInt64(out ulong result)
			{
				return CodePatternExtentions.TryUInt64(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUInt64(IFormatProvider provider, out ulong result)
			{
				return CodePatternExtentions.TryUInt64(Value, provider, out result);
			}

			public ulong UInt64
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryUInt64(Value, out result) ? result : 0;
				}
			}

			public ulong? UInt64n
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryUInt64(Value, out result) ? (ulong?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static ulong UInt64(this StringValue value, ulong defaults)
		{
			ulong result;
			return TryUInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong UInt64(this StringValue value, IFormatProvider provider, ulong defaults)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong UInt64(this StringValue value, IFormatProvider provider)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static ulong? UInt64n(this StringValue value, ulong? defaults)
		{
			ulong result;
			return TryUInt64(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? UInt64n(this StringValue value, IFormatProvider provider, ulong? defaults)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? UInt64n(this StringValue value, IFormatProvider provider)
		{
			ulong result;
			return TryUInt64(value.Value, provider, out result) ? (ulong?)result : null;
		}

		#endregion


		#region String As ULong

		[DebuggerStepThrough]
		private static bool TryULong(string value, out ulong result)
		{
			return ulong.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryULong(string value, IFormatProvider provider, out ulong result)
		{
			return ulong.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static ulong As(this string value, ulong defaults)
		{
			ulong result;
			return TryULong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong As(this string value, IFormatProvider provider, ulong defaults)
		{
			ulong result;
			return TryULong(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? As(this string value, ulong? defaults)
		{
			ulong result;
			return TryULong(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? As(this string value, IFormatProvider provider, ulong? defaults)
		{
			ulong result;
			return TryULong(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryULong(out ulong result)
			{
				return CodePatternExtentions.TryULong(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryULong(IFormatProvider provider, out ulong result)
			{
				return CodePatternExtentions.TryULong(Value, provider, out result);
			}

			public ulong ULong
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryULong(Value, out result) ? result : 0;
				}
			}

			public ulong? ULongn
			{
				[DebuggerStepThrough]
				get
				{
					ulong result;
					return CodePatternExtentions.TryULong(Value, out result) ? (ulong?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static ulong ULong(this StringValue value, ulong defaults)
		{
			ulong result;
			return TryULong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong ULong(this StringValue value, IFormatProvider provider, ulong defaults)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong ULong(this StringValue value, IFormatProvider provider)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? result : 0;
		}


		[DebuggerStepThrough]
		public static ulong? ULongn(this StringValue value, ulong? defaults)
		{
			ulong result;
			return TryULong(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? ULongn(this StringValue value, IFormatProvider provider, ulong? defaults)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ulong? ULongn(this StringValue value, IFormatProvider provider)
		{
			ulong result;
			return TryULong(value.Value, provider, out result) ? (ulong?)result : null;
		}

		#endregion


		#region String As UShort

		[DebuggerStepThrough]
		private static bool TryUShort(string value, out ushort result)
		{
			return ushort.TryParse(value, NumberStyles.Number, DefaultCulture, out result);
		}

		[DebuggerStepThrough]
		private static bool TryUShort(string value, IFormatProvider provider, out ushort result)
		{
			return ushort.TryParse(value, NumberStyles.Number, provider, out result);
		}


		[DebuggerStepThrough]
		public static ushort As(this string value, ushort defaults)
		{
			ushort result;
			return TryUShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort As(this string value, IFormatProvider provider, ushort defaults)
		{
			ushort result;
			return TryUShort(value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? As(this string value, ushort? defaults)
		{
			ushort result;
			return TryUShort(value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? As(this string value, IFormatProvider provider, ushort? defaults)
		{
			ushort result;
			return TryUShort(value, provider, out result) ? result : defaults;
		}


		public partial struct StringValue
		{
			[DebuggerStepThrough]
			public bool TryUShort(out ushort result)
			{
				return CodePatternExtentions.TryUShort(Value, out result);
			}

			[DebuggerStepThrough]
			public bool TryUShort(IFormatProvider provider, out ushort result)
			{
				return CodePatternExtentions.TryUShort(Value, provider, out result);
			}

			public ushort UShort
			{
				[DebuggerStepThrough]
				get
				{
					ushort result;
					return CodePatternExtentions.TryUShort(Value, out result) ? result : (ushort)0;
				}
			}

			public ushort? UShortn
			{
				[DebuggerStepThrough]
				get
				{
					ushort result;
					return CodePatternExtentions.TryUShort(Value, out result) ? (ushort?)result : null;
				}
			}
		}


		[DebuggerStepThrough]
		public static ushort UShort(this StringValue value, ushort defaults)
		{
			ushort result;
			return TryUShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort UShort(this StringValue value, IFormatProvider provider, ushort defaults)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort UShort(this StringValue value, IFormatProvider provider)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? result : (ushort)0;
		}


		[DebuggerStepThrough]
		public static ushort? UShortn(this StringValue value, ushort? defaults)
		{
			ushort result;
			return TryUShort(value.Value, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? UShortn(this StringValue value, IFormatProvider provider, ushort? defaults)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? result : defaults;
		}

		[DebuggerStepThrough]
		public static ushort? UShortn(this StringValue value, IFormatProvider provider)
		{
			ushort result;
			return TryUShort(value.Value, provider, out result) ? (ushort?)result : null;
		}

		#endregion


		#endregion

	}



}
