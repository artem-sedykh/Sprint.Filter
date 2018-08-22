// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using Helpers;
    using System.Linq.Expressions;

    public sealed class IsIntersectionCondition<TProperty> : IValueTypeCondition<TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        public IsIntersectionCondition() { }

        public IsIntersectionCondition(string title)
        {
            Title = title;
        }

        public IFilterValue<TProperty?> Value { get; set; }

        public string Title { get; }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> property)
        {
            throw new NotSupportedException("Not Supported Method");
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> left, Expression<Func<TModel, TProperty?>> right)
        {
            if (Value != null && (Value.RightValue != null || Value.LeftValue != null))
            {
                if (Value.LeftValue != null && Value.RightValue != null)
                    return ExpressionHelper.IsIntersection(left, right, Value.LeftValue, Value.RightValue);

                return Value.LeftValue != null
                           ? ExpressionHelper.IsIntersectionWithBegin(right, Value.LeftValue)
                    // ReSharper disable PossibleNullReferenceException
                           : ExpressionHelper.IsIntersectionWithEnd(left, Value.RightValue);
                // ReSharper restore PossibleNullReferenceException
            }

            return null;
        }
    }
}
