// ReSharper disable CheckNamespace
namespace Sprint.Filter
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    internal sealed class ValueTypeFilterConfiguration<TModel, TProperty> : IValueTypeFilterConfiguration<TModel, TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly IValueTypeFilter<TModel, TProperty> _filter;

        internal ValueTypeFilterConfiguration(IValueTypeFilter<TModel, TProperty> filter)
        {
            _filter = filter;

        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetValueFormat(string valueFormat)
        {
            _filter.ValueFormat = valueFormat;

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> Conditions(Action<IDictionary<string, IValueTypeCondition<TProperty>>> conditions)
        {
            _filter.Conditions(conditions);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(IFilterValue<TProperty?> filterValue)
        {
            _filter.SetDefaultValue(filterValue);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(Func<IFilterValue<TProperty?>> filterValue)
        {
            _filter.SetDefaultValue(filterValue);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetTemplate(string tempaleName)
        {
            _filter.TemplateName = tempaleName;

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetDictionary(Func<IEnumerable<SelectListItem>> dictionary)
        {
            _filter.SetDictionary(dictionary);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetDictionary(IQueryable<SelectListItem> dictionary)
        {
            _filter.SetDictionary(dictionary);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetTitle(string title)
        {
            _filter.Title = title;

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetAdditionalData(object data)
        {
            _filter.AdditionalData = data;

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> Visible(bool isVisible)
        {
            _filter.Visible(isVisible);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> SetType(FilterType type)
        {
            _filter.FilterType = type;

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty?>> property)
        {
            _filter.For(property);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty?>> begin, Expression<Func<TModel, TProperty?>> end)
        {
            _filter.For(begin, end);

            return this;
        }

        public IValueTypeFilterConfiguration<TModel, TProperty> ConditionBuilder<T>(Func<Expression<Func<TModel, bool>>, string, Expression<Func<T, bool>>> builder)
        {
            _filter.ConditionBuilder(builder);

            return this;
        }
    }
}
