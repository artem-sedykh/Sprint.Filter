// ReSharper disable CheckNamespace
namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Helpers;

    public interface IReferenceTypeFilter<TModel, TProperty> : IFilter where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        void For(Expression<Func<TModel, TProperty>> property);

        void For(Expression<Func<TModel, TProperty>> left, Expression<Func<TModel, TProperty>> right);        

        void ConditionBuilder<T>(Func<
            IFilterValue<TProperty>,
            Func<IFilterValue<TProperty>, LambdaExpressionDecorator<Func<TModel, bool>>>, 
            string,
            Expression<Func<T, bool>>> builder);

        void Conditions(Action<IDictionary<string, IReferenceTypeCondition<TProperty>>> conditions);
        
        void SetDefaultValue(Func<IFilterValue<TProperty>> filterValue);

        void SetDefaultValue(IFilterValue<TProperty> filterValue);

        void Init(IFilterValue<TProperty> filterValue);

        void SetValueResolver(Func<IFilterValue<TProperty>, IEnumerable<SelectListItem>> valueResolver);
    }
}
