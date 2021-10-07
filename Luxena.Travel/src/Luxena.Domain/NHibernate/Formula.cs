using System;
using System.Linq.Expressions;


namespace Luxena.Domain
{

	public abstract class Formula
	{
		public static Formula<T1, T2> New<T1, T2>(Expression<Func<T1, T2>> expression)
		{
			return new Formula<T1, T2>(expression);
		}

		public abstract Expression GetExpression();

	}

	public class Formula<T1, T2> : Formula
	{

		public Formula(Expression<Func<T1, T2>> expression)
		{
			if (expression == null) throw new ArgumentNullException("expression");

			Expression = expression;
			Eval = expression.Compile();
		}

		public Expression<Func<T1, T2>> Expression { get; private set; }

		public Func<T1, T2> Eval { get; private set; }

		public override Expression GetExpression() { return Expression; }

	}

}
