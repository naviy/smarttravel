using System;


namespace Luxena.Base.Text
{
	public interface IErrorTranslator
	{
		Exception Translate(Exception exception);
	}
}