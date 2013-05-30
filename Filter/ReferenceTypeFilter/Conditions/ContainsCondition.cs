// ReSharper disable CheckNamespace
namespace Sprint.Filter.Conditions
// ReSharper restore CheckNamespace
{
    using System;
    using System.Linq.Expressions;

    public sealed class ContainsCondition : IReferenceTypeCondition<string>
    {
        private readonly string _title;

        public ContainsCondition() { }

        public ContainsCondition(string title)
        {
            _title = title;
        }

        public IFilterValue<string> Value { get; set; }

        public string Title
        {
            get { return _title; }
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, string>> property)
        {
            if (Value == null || Value.LeftValue == null)
                return null;

            var val = Expression.Constant(Value.LeftValue, property.ReturnType);

            var method = property.ReturnType.GetMethod("Contains", new[] { property.ReturnType });

            var containsMethodExp = Expression.Call(property.Body, method, new Expression[] { val });

            return Expression.Lambda<Func<TModel, bool>>(containsMethodExp, property.Parameters[0]);
        }

        public Expression<Func<TModel, bool>> For<TModel>(Expression<Func<TModel, string>> left, Expression<Func<TModel, string>> right)
        {
            return For(left);
        }
    }
}
