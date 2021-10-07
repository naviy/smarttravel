using System;
using System.Text.RegularExpressions;

using Luxena.Base.Data;
using Luxena.Base.Text;

using NHibernate.Exceptions;

using Npgsql;


namespace Luxena.Travel.Config
{
	public class PostgreSqlErrorTranlator : IErrorTranslator
	{
		public Exception Translate(Exception exception)
		{
			var topException = exception;

			while (exception != null)
			{
				var npgsqlException = exception as NpgsqlException;

				if (npgsqlException != null)
				{
					switch (npgsqlException.Code)
					{
						case "23503":
							return new ForeignKeyViolation(Exceptions.DeleteConstrainedFailed_Msg);

						case "23505":
						{
							exception = topException;

							while (exception != npgsqlException)
							{
								var adoException = exception as GenericADOException;

								if (adoException != null)
								{
									var clazz = _operationErrorClass.Match(adoException.Message).Groups[1].Value;
									var constraint = _uniqueConstraintName.Match(npgsqlException.BaseMessage).Groups[1].Value;

									return new UniqueKeyViolation(Exceptions.ResourceManager.GetString(clazz + "_" + constraint));
								}

								exception = exception.InnerException;
							}
						}
							break;
					}
				}

				exception = exception.InnerException;
			}

			return null;
		}

		private static readonly Regex _uniqueConstraintName = new Regex(".*\\\"(.*)\\\"", RegexOptions.Compiled);
		private static readonly Regex _operationErrorClass = new Regex("could not (?:insert|update): \\[(?:(?:\\w+\\.)*)(\\w+)#\\w+]", RegexOptions.Compiled);
	}
}