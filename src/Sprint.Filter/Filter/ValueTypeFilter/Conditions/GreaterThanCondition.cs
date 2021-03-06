﻿// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class GreaterThanCondition<TProperty> : IValueTypeCondition<TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        public GreaterThanCondition() { }

        public GreaterThanCondition(string title)
        {
            Title = title;
        }

        public IFilterValue<TProperty?> Value { get; set; }

        public string Title { get; }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> property)
        {
            if (Value?.LeftValue == null)
                return null;

            var val = Expression.Constant(Value.LeftValue, property.ReturnType);

            return Expression.Lambda<Func<TModel, bool>>(Expression.GreaterThan(property.Body, val), property.Parameters[0]);
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty?>> left, Expression<Func<TModel, TProperty?>> right)
        {
            return For(left);
        }
    }
}
