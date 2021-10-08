using System.Collections.Generic;


namespace Luxena.Castle
{
	public interface IScopeAccessor
	{
		Stack<Scope> CurrentStack { get; }
	}
}