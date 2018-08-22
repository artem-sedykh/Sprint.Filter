using Sprint.Linq;

namespace Sprint.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ExpressionHelper
    {
        private static readonly ExpressionVisitor Visitor = new LambdaExpressionDecoratorVisitor();

        internal static Expression<Func<TModel, bool>> ExpandLambdaExpressionDecorators<TModel>(this Expression<Func<TModel, bool>> expression)
        {
            if(expression == null)
                return null;

            return (Expression<Func<TModel, bool>>)Visitor.Visit(expression);
        }

        /// <summary>
        /// Building a strongly typed lambda expression to calculate the intersections of intervals.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <typeparam name="TProperty">Type field.</typeparam>
        /// <param name="firstIntervalBegin">The left-most boundary of the first interval.</param>
        /// <param name="firstIntervalEnd">The extreme right edge of the first interval.</param>
        /// <param name="secondIntervalBegin">The left-most boundary of the second interval.</param>
        /// <param name="secondIntervalEnd">The extreme right edge of the second interval.</param>
        /// <returns>Expression tree.</returns>
        public static Expression<Func<TSource, bool>> IsIntersection<TSource, TProperty>(Expression<Func<TSource, TProperty?>> firstIntervalBegin, Expression<Func<TSource, TProperty?>> firstIntervalEnd, TProperty? secondIntervalBegin, TProperty? secondIntervalEnd) where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
        {
            if (firstIntervalBegin == null)
                throw new ArgumentNullException(nameof(firstIntervalBegin));

            if (firstIntervalEnd == null)
                throw new ArgumentNullException(nameof(firstIntervalEnd));

            var type = typeof(TProperty?);
            var parametr = Expression.Parameter(typeof(TSource), "p");
            var expressionFirstIntervalBegin = Expression.Invoke(firstIntervalBegin, parametr);
            var expressionFirstIntervalEnd = Expression.Invoke(firstIntervalEnd, parametr);

            var expressionSecondIntervalBegin = Expression.Constant(secondIntervalBegin, type);
            var expressionSecondIntervalEnd = Expression.Constant(secondIntervalEnd, type);

            var expression =
                Expression.OrElse(
                    Expression.OrElse(
                        Expression.AndAlso(
                            Expression.GreaterThanOrEqual(expressionFirstIntervalBegin,
                                                          expressionSecondIntervalBegin),
                            Expression.LessThanOrEqual(expressionFirstIntervalBegin, expressionSecondIntervalEnd)),
                        Expression.AndAlso(
                            Expression.GreaterThanOrEqual(expressionFirstIntervalEnd, expressionSecondIntervalBegin),
                            Expression.LessThanOrEqual(expressionFirstIntervalEnd, expressionSecondIntervalEnd))),
                    Expression.AndAlso(
                        Expression.LessThanOrEqual(expressionFirstIntervalBegin, expressionSecondIntervalBegin),
                        Expression.GreaterThanOrEqual(expressionFirstIntervalEnd, expressionSecondIntervalEnd)));
            //LinqKit fix for EF
            return Expression.Lambda<Func<TSource, bool>>(expression, parametr).Expand();
        }

        /// <summary>
        /// Building a strongly typed lambda expression to calculate the intersections of intervals.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <typeparam name="TProperty">Type field.</typeparam>        
        /// <param name="firstIntervalEnd">The extreme right edge of the first interval.</param>
        /// <param name="secondIntervalBegin">The left-most boundary of the second interval.</param>
        /// <returns>Expression tree.</returns>
        public static Expression<Func<TSource, bool>> IsIntersectionWithBegin<TSource, TProperty>(Expression<Func<TSource, TProperty>> firstIntervalEnd, TProperty secondIntervalBegin)
        {
            if (firstIntervalEnd == null)
                throw new ArgumentNullException(nameof(firstIntervalEnd));

            var type = typeof(TProperty);
            var parametr = Expression.Parameter(typeof(TSource), "p");
            var expressionFirstIntervalEnd = Expression.Invoke(firstIntervalEnd, parametr);

            var expressionSecondIntervalBegin = Expression.Constant(secondIntervalBegin, type);
            return Expression.Lambda<Func<TSource, bool>>(Expression.GreaterThanOrEqual(expressionFirstIntervalEnd, expressionSecondIntervalBegin), parametr).Expand();
        }

        /// <summary>
        /// Building a strongly typed lambda expression to calculate the intersections of intervals.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <typeparam name="TProperty">Type field.</typeparam>    
        /// <param name="firstIntervalBegin">the leftmost boundary of the first interval.</param>
        /// <param name="secondIntervalEnd">The extreme right edge of the second interval.</param>
        /// <returns>Expression tree.</returns>
        public static Expression<Func<TSource, bool>> IsIntersectionWithEnd<TSource, TProperty>(Expression<Func<TSource, TProperty>> firstIntervalBegin, TProperty secondIntervalEnd)
        {
            if (firstIntervalBegin == null)
                throw new ArgumentNullException(nameof(firstIntervalBegin));

            var type = typeof(TProperty);
            var parametr = Expression.Parameter(typeof(TSource), "p");
            var expressionFirstIntervalBegin = Expression.Invoke(firstIntervalBegin, parametr);

            var expressionSecondIntervalEnd = Expression.Constant(secondIntervalEnd, type);
            return Expression.Lambda<Func<TSource, bool>>(Expression.LessThanOrEqual(expressionFirstIntervalBegin, expressionSecondIntervalEnd), parametr).Expand();
        }
    }

    internal class LambdaExpressionDecoratorVisitor : ExpressionVisitor
    {
        private static readonly Type ExpressionDecoratorType = typeof(LambdaExpressionDecorator<>);

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Convert && node.Operand.Type.IsGenericType && node.Operand.Type.GetGenericTypeDefinition() == ExpressionDecoratorType)
            {
                var decorator = Expression.Lambda<Func<object>>(node.Operand).Compile()();

                var expressionProperty = decorator.GetType().GetProperty("Expression", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (expressionProperty != null)
                {
                    var expr = (Expression)expressionProperty.GetValue(decorator, null);

                    return expr;
                }
            }

            return base.VisitUnary(node);
        }
    }
}
