using System;
using System.Linq;
using System.Reflection;
using System.Web.OData;
using System.Web.OData.Query;

using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Core.UriParser.TreeNodeKinds;
using Microsoft.OData.Edm;


namespace Luxena.Domain.Web
{

	public static class ODataExtensions
	{

		public static IQueryable Where(this IQueryable query, ODataQueryOptions queryOptions)
		{

			var selectExpand = queryOptions.SelectExpand;
			_selectExpandProperty.SetValue(queryOptions, null);

			query = queryOptions.ApplyTo(query, new ODataQuerySettings());

			_selectExpandProperty.SetValue(queryOptions, selectExpand);

			return query;
		}

		public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query, ODataQueryOptions queryOptions)
		{

			var selectExpand = queryOptions.SelectExpand;
			_selectExpandProperty.SetValue(queryOptions, null);

			query = (IQueryable<TEntity>)queryOptions.ApplyTo(query);

			_selectExpandProperty.SetValue(queryOptions, selectExpand);

			return query;
		}

		public static IQueryable Select(this IQueryable query, ODataQueryOptions queryOptions)
		{

			_countProperty.SetValue(queryOptions, null);
			_filterProperty.SetValue(queryOptions, null);
			_orderByProperty.SetValue(queryOptions, null);
			_skipProperty.SetValue(queryOptions, null);
			_topExpandProperty.SetValue(queryOptions, null);

			query = queryOptions.ApplyTo(query);

			return query;
		}


		public static IQueryable<T> Limit<T>(this IQueryable<T> query, ODataQueryOptions queryOptions)
		{
			if (!(query is IOrderedQueryable<T>))
				return query;

			int skipCount = queryOptions.Skip.As(a => a.Value);
			int takeCount = queryOptions.Top.As(a => a.Value);

			if (skipCount > 0 && takeCount > 0)
				query = query.Skip(skipCount);

			if (takeCount > 0)
				query = query.Take(takeCount);

			return query;
		}


		public static string GetIdFilterValue(this ODataQueryOptions queryOptions, string idPropertyName = "Id")
		{
			var expr = queryOptions.Filter?.FilterClause.Expression as BinaryOperatorNode;
			if (expr == null || expr.OperatorKind != BinaryOperatorKind.Equal)
				return null;

			var propNode = expr.Left.Unconvert();
			var propName = (propNode as SingleValuePropertyAccessNode)?.Property.Name;

			if (propName == idPropertyName)
				return (expr.Right.Unconvert() as ConstantNode)?.Value?.ToString();

			return null;
		}

		public static void ClearFilter(this ODataQueryOptions queryOptions)
		{
			_filterProperty.SetValue(queryOptions, null);
		}

		public static TParams GetFilterParams<TParams>(this ODataQueryOptions queryOptions)
			where TParams : class, new()
		{
			var prms = new TParams();

			if (queryOptions.Filter != null)
			{
				var expr = queryOptions.Filter.FilterClause.Expression;
				var model = queryOptions.Context.Model;
				ParseFilterExpression(model, prms, expr);
			}

			return prms;
		}

		public static void ParseFilterExpression<TParams>(IEdmModel model, TParams prms, SingleValueNode expr)
		{
			var binaryOperatorNode = expr as BinaryOperatorNode;
			if (binaryOperatorNode != null)
				ParseFilterExpression(model, prms, binaryOperatorNode);
			else
			{
				var singleValueFunctionCallNode = expr as SingleValueFunctionCallNode;
				if (singleValueFunctionCallNode != null)
				{
					ParseFilterExpression(model, prms, singleValueFunctionCallNode);
				}
			}
		}

		public static void ParseFilterExpression<TParams>(IEdmModel model, TParams prms, BinaryOperatorNode expr)
		{
			if (expr.OperatorKind == BinaryOperatorKind.Equal)
			{
				//string propName;

				//var propExpr = expr.Left as SingleValuePropertyAccessNode;

				//if (propExpr != null)
				//	propName = propExpr.Property.Name;
				//else
				//	propName = ((SingleNavigationNode)((SingleValuePropertyAccessNode)((ConvertNode)expr.Left).Source).Source).NavigationProperty.Name;

				var propNode = Unconvert(expr.Left);

				var propName = ((SingleValuePropertyAccessNode)propNode).Property.Name;

				//var valueExpr = expr.Right as ConstantNode ?? (ConstantNode)Unconvert(expr.Right);
				var valueExpr = (ConstantNode)Unconvert(expr.Right);

				var value = valueExpr.Value;

				var enumValue = value as ODataEnumValue;

				if (enumValue != null)
				{
					var annotation = model.GetAnnotationValue<ClrTypeAnnotation>(valueExpr.TypeReference.Definition);
					var enumType = annotation.ClrType;
					value = Enum.Parse(enumType, enumValue.Value);
				}

				typeof(TParams).GetProperty(propName).SetValue(prms, value);
			}
			else if (expr.OperatorKind == BinaryOperatorKind.And || expr.OperatorKind == BinaryOperatorKind.Or)
			{
				ParseFilterExpression(model, prms, Unconvert(expr.Left));
				ParseFilterExpression(model, prms, Unconvert(expr.Right));
			}
			else
				throw new NotImplementedException();
		}

		public static void ParseFilterExpression<TParams>(IEdmModel model, TParams prms, SingleValueFunctionCallNode expr)
		{
			if (expr.Name == "contains")
			{
				var parameters = expr.Parameters.ToArray();
				var propName = ((SingleValuePropertyAccessNode)parameters[0]).Property.Name;

				var value = ((ConstantNode)parameters[1]).Value;

				typeof(TParams).GetProperty(propName).SetValue(prms, value);
			}
			else
				throw new NotImplementedException();
		}



		public static SingleValueNode Unconvert(this SingleValueNode node)
		{
			var convertNode = node as ConvertNode;
			return convertNode != null ? convertNode.Source : node;
		}

		

		private static readonly PropertyInfo _selectExpandProperty = typeof(ODataQueryOptions).GetProperty("SelectExpand");
		private static readonly PropertyInfo _countProperty = typeof(ODataQueryOptions).GetProperty("Count");
		private static readonly PropertyInfo _filterProperty = typeof(ODataQueryOptions).GetProperty("Filter");
		private static readonly PropertyInfo _orderByProperty = typeof(ODataQueryOptions).GetProperty("OrderBy");
		private static readonly PropertyInfo _skipProperty = typeof(ODataQueryOptions).GetProperty("Skip");
		private static readonly PropertyInfo _topExpandProperty = typeof(ODataQueryOptions).GetProperty("Top");

	}

}
