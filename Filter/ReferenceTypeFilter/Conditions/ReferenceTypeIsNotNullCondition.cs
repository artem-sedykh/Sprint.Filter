// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class ReferenceTypeIsNotNullCondition<TProperty> : IReferenceTypeCondition<TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly string _title;

        public ReferenceTypeIsNotNullCondition() { }

        public ReferenceTypeIsNotNullCondition(string title)
        {
            _title = title;
        }

        public IFilterValue<TProperty> Value { get; set; }

        public string Title
        {
            get { return _title; }
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty>> property)
        {
            var val = Expression.Constant(null, property.ReturnType);

            return Expression.Lambda<Func<TModel, bool>>(Expression.NotEqual(property.Body, val), property.Parameters[0]);
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty>> left, Expression<Func<TModel, TProperty>> right)
        {
            return For(left);
        }
    }
}
