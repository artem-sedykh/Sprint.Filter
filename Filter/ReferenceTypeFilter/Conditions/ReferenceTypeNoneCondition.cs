// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class ReferenceTypeNoneCondition<TProperty> : IReferenceTypeCondition<TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly string _title;

        public ReferenceTypeNoneCondition() { }

        public ReferenceTypeNoneCondition(string title)
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
            return null;
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty>> left, Expression<Func<TModel, TProperty>> right)
        {
            return For(left);
        }
    }
}
