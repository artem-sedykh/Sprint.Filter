// ReSharper disable CheckNamespace
namespace Sprint.Filter
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public interface IValueTypeCondition<TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        IFilterValue<TProperty?> Value { get; set; }

        string Title { get; }

        Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> property);

        Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> left, Expression<Func<TModel, TProperty?>> right);
    }
}
