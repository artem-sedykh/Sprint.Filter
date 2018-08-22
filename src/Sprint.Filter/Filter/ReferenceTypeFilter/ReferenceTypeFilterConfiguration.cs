// ReSharper disable CheckNamespace
namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    internal sealed class ReferenceTypeFilterConfiguration<TModel, TProperty> : IReferenceTypeFilterConfiguration<TModel, TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly IReferenceTypeFilter<TModel, TProperty> _filter;

        internal ReferenceTypeFilterConfiguration(IReferenceTypeFilter<TModel, TProperty> filter)
        {
            _filter = filter;

        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetValueFormat(string valueFormat)
        {
            _filter.ValueFormat = valueFormat;

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> Conditions(Action<IDictionary<string, IReferenceTypeCondition<TProperty>>> conditions)
        {
            _filter.Conditions(conditions);

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(IFilterValue<TProperty> filterValue)
        {
            _filter.SetDefaultValue(filterValue);

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(Func<IFilterValue<TProperty>> filterValue)
        {
            _filter.SetDefaultValue(filterValue);

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetTemplate(string tempaleName)
        {
            _filter.TemplateName = tempaleName;

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetValueResolver(Func<IFilterValue<TProperty>, IEnumerable<SelectListItem>> valueResolver)
        {
            _filter.SetValueResolver(valueResolver);

            return this;
        }     

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetTitle(string title)
        {
            _filter.Title = title;

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetAdditionalData(object data)
        {
            _filter.AdditionalData = data;

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> Visible(bool isVisible)
        {
            _filter.Visible(isVisible);

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> SetType(FilterType type)
        {
            _filter.FilterType = type;

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty>> property)
        {
            _filter.For(property);

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty>> begin, Expression<Func<TModel, TProperty>> end)
        {
            _filter.For(begin, end);

            return this;
        }

        public IReferenceTypeFilterConfiguration<TModel, TProperty> ConditionBuilder<T>(Func<
            IFilterValue<TProperty>,
            Func<IFilterValue<TProperty>, LambdaExpressionDecorator<Func<TModel, bool>>>,
            string,
            Expression<Func<T, bool>>> builder)
        {
            _filter.ConditionBuilder(builder);

            return this;
        }
    }
}
