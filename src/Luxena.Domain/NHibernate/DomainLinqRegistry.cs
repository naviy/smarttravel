using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;


namespace Luxena.Domain.NHibernate
{

	public class DomainLinqRegistry : DefaultLinqToHqlGeneratorsRegistry
	{

		public void Register<T, TResult>(Expression<Func<T, TResult>> property, Expression<Func<T, TResult>> expression)
		{
			RegisterGenerator(ReflectionHelper.GetProperty(property), new ExpressionPropertyGenerator(expression));
		}

		public void RegisterEntity<TEntity>()
		{
			RegisterEntity(typeof(TEntity));
		}

		public void RegisterEntity(Type entityType)
		{
			var formulaProperties = entityType
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(a => a.FieldType.IsSubclassOf(typeof(Formula)));
			foreach (var formulaProperty in formulaProperties)
			{

				var name = formulaProperty.Name.As(a => a.EndsWith("Formula") ? a.Substring(0, a.Length - 7) : a);
				var property = entityType.GetProperty(name);
				if (property == null)
					throw new Exception("DomainLinqToHqlGeneratorsRegistry.RegisterProperties: Can't find property for formula-property " + formulaProperty.Name);

				var expression = ((Formula)formulaProperty.GetValue(null)).GetExpression();

				RegisterGenerator(property, new ExpressionPropertyGenerator(expression));
			}

		}


		public class ExpressionPropertyGenerator : BaseHqlGeneratorForProperty
		{
			public ExpressionPropertyGenerator(Expression expression)
			{
				_expression = expression;
			}

			public override HqlTreeNode BuildHql(MemberInfo member, Expression expression, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
			{
				return visitor.Visit(_expression);
			}

			private readonly Expression _expression;
		}


	}

}
