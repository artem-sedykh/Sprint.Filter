// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class IsNotNullOrEmptyCondition : IReferenceTypeCondition<string>
    {
        public IsNotNullOrEmptyCondition() { }

        public IsNotNullOrEmptyCondition(string title)
        {
            Title = title;
        }

        public IFilterValue<string> Value { get; set; }

        public string Title { get; }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, string>> property)
        {
            var nullConstant = Expression.Constant(null, property.ReturnType);

            var zeroConstant = Expression.Constant(0, typeof(int));

            return Expression.Lambda<Func<TModel, bool>>(Expression.Not(Expression.OrElse(Expression.Equal(property.Body, nullConstant), Expression.Equal(Expression.Property(property.Body, "Length"), zeroConstant))), property.Parameters[0]);
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, string>> left, Expression<Func<TModel, string>> right)
        {
            return For(left);
        }
    }
}
