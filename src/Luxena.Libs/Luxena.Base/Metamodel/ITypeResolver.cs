using System;


namespace Luxena.Base.Metamodel
{
	public interface ITypeResolver
	{
		Type Resolve(object obj);
	}
}