using System;
using System.Runtime.Serialization;


namespace Luxena.Base.Metamodel
{
	public class MetamodelException : Exception
	{
		public MetamodelException()
		{
		}

		public MetamodelException(string message)
			: base(message)
		{
		}

		public MetamodelException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public MetamodelException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}