// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class ValueTypeNoneCondition<TProperty> : IValueTypeCondition<TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        public ValueTypeNoneCondition() { }

        public ValueTypeNoneCondition(string title)
        {
            Title = title;
        }

        public IFilterValue<TProperty?> Value { get; set; }

        public string Title { get; }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> property)
        {
            return null;
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> left, Expression<Func<TModel, TProperty?>> right)
        {
            return For(left);
        }
    }
}
