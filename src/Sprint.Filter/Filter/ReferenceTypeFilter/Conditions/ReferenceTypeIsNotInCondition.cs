// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public sealed class ReferenceTypeIsNotInCondition<TProperty> : IReferenceTypeCondition<TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        public ReferenceTypeIsNotInCondition() { }

        public ReferenceTypeIsNotInCondition(string title)
        {
            Title = title;
        }

        public IFilterValue<TProperty> Value { get; set; }

        public string Title { get; }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty>> property)
        {
            if (Value?.Values != null && Value.Values.Any(x => x!=null))
            {
                var values = Value.Values.Where(x => x != null).Select(x => x).ToList();

                var collectionType = values.GetType();

                var parametr = property.Parameters[0];

                var propertyBody = property.Body;

                var method = collectionType.GetMethod("Contains", new[] { property.ReturnType });

                var value = Expression.Constant(values, collectionType);

                var containsMethodExp = Expression.Call(value, method ?? throw new InvalidOperationException(), propertyBody);

                return Expression.Lambda<Func<TModel, bool>>(Expression.Not(containsMethodExp), parametr);
            }

            return null;
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, TProperty>> left, Expression<Func<TModel, TProperty>> right)
        {
            return For(left);
        }
    }
}
