using System;
using System.Runtime.CompilerServices;


namespace LxnBase
{

	public static class Log
	{

		[AlternateSignature]
		public static extern object log(object msg1);

		[AlternateSignature]
		public static extern object log(object msg1, object msg2);

		[AlternateSignature]
		public static extern object log(object msg1, object msg2, object msg3);

		[AlternateSignature]
		public static extern object log(object msg1, object msg2, object msg3, object msg4);

		public static object log(object msg1, object msg2, object msg3, object msg4, object msg5)
		{
			if (!Script.IsUndefined(msg5))
			{
				Script.Eval("$log(msg1, msg2, msg3, msg4, msg5)");
				return msg5;
			}
			if (!Script.IsUndefined(msg4))
			{
				Script.Eval("$log(msg1, msg2, msg3, msg4)");
				return msg4;
			}
			if (!Script.IsUndefined(msg3))
			{ 
				Script.Eval("$log(msg1, msg2, msg3)");
				return msg3;
			}
			if (!Script.IsUndefined(msg2))
			{
				Script.Eval("$log(msg1, msg2)");
				return msg2;
			}

			Script.Eval("$log(msg1)");
			return msg1;
		}


		[AlternateSignature]
		public static extern object loga(object msg1);

		[AlternateSignature]
		public static extern object loga(object msg1, object msg2);

		[AlternateSignature]
		public static extern object loga(object msg1, object msg2, object msg3);

		[AlternateSignature]
		public static extern object loga(object msg1, object msg2, object msg3, object msg4);

		public static object loga(object msg1, object msg2, object msg3, object msg4, object msg5)
		{
			if (!Script.IsUndefined(msg5))
			{
				Script.Eval("$log(msg1, msg2, msg3, msg4, msg5)");
				return msg5;
			}
			if (!Script.IsUndefined(msg4))
			{
				Script.Eval("$log(msg1, msg2, msg3, msg4)");
				return msg4;
			}
			if (!Script.IsUndefined(msg3))
			{
				Script.Eval("$log(msg1, msg2, msg3)");
				return msg3;
			}
			if (!Script.IsUndefined(msg2))
			{
				Script.Eval("$log(msg1, msg2)");
				return msg2;
			}

			Script.Eval("$log(msg1)");
			return msg1;
		}

		[AlternateSignature]
		public static extern object logb();
		public static object logb(object msg)
		{
			Script.Eval("$logb(msg)");
			return msg;
		}

		public static void loge()
		{
			Script.Eval("$loge()");
		}

		public static object alert(object msg)
		{
			Script.Eval("$log(msg)");
			Script.Alert(Script.IsValue(msg) ? msg.ToString() : Script.IsNull(msg) ? "null" : "undefined");
			return msg;
		}

		public static void trace()
		{
			Script.Eval("console.trace()");
		}

	}

}
