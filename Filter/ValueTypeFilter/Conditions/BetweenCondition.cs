// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class BetweenCondition<TProperty> : IValueTypeCondition<TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly string _title;

        public BetweenCondition() { }

        public BetweenCondition(string title)
        {
            _title = title;
        }

        public IFilterValue<TProperty?> Value { get; set; }

        public string Title
        {
            get { return _title; }
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> property)
        {
            if (Value == null)
                return null;

            var leftExpression = default(BinaryExpression);

            var rightExpression = default(BinaryExpression);

            if (Value.LeftValue != null)
                leftExpression = Expression.GreaterThanOrEqual(property.Body, Expression.Constant(Value.LeftValue, property.ReturnType));

            if (Value.RightValue != null)
                rightExpression = Expression.LessThanOrEqual(property.Body, Expression.Constant(Value.RightValue, property.ReturnType));

            if (leftExpression != default(BinaryExpression) && rightExpression != default(BinaryExpression))
                return Expression.Lambda<Func<TModel, bool>>(Expression.AndAlso(leftExpression, rightExpression), property.Parameters[0]);

            if (leftExpression != default(BinaryExpression))
                return Expression.Lambda<Func<TModel, bool>>(leftExpression, property.Parameters[0]);

            if (rightExpression != default(BinaryExpression))
                return Expression.Lambda<Func<TModel, bool>>(rightExpression, property.Parameters[0]);

            return null;
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> left, Expression<Func<TModel, TProperty?>> right)
        {
            return For(left);
        }
    }
}
