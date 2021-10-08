using System.Collections.Generic;
using System.Web;


namespace Luxena.Castle
{
	public class WebScopeAccessor : IScopeAccessor
	{
		public Stack<Scope> CurrentStack
		{
			get
			{
				var current = HttpContext.Current;

				if (current == null)
					return null;

				var stack = (Stack<Scope>) current.Items[Key];

				if (stack == null)
				{
					stack = new Stack<Scope>();

					current.Items[Key] = stack;
				}

				return stack;
			}
		}

		private const string Key = "castle.webScopeAccessor";
	}
}