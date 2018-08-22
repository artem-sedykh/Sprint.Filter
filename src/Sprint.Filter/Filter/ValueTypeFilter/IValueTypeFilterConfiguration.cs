using Sprint.Helpers;
// ReSharper disable CheckNamespace


namespace Sprint.Filter
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Linq.Expressions;

    public interface IValueTypeFilterConfiguration<TModel, TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        /// <summary>
        /// Set value format.
        /// </summary>
        /// <param name="valueFormat">value format.
        /// For example: {0:MM/dd/yyyy}</param>
        IValueTypeFilterConfiguration<TModel, TProperty> SetValueFormat(string valueFormat);

        /// <summary>
        /// Add conditions
        /// </summary>
        /// <param name="conditions">Conditions</param>
        IValueTypeFilterConfiguration<TModel, TProperty> Conditions(Action<IDictionary<string, IValueTypeCondition<TProperty>>> conditions);

        /// <summary>
        /// Set default filter value.
        /// </summary>
        /// <param name="filterValue">Filter value.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(IFilterValue<TProperty?> filterValue);

        /// <summary>
        /// Set default filter value.
        /// </summary>
        /// <param name="filterValue">Filter value.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(Func<IFilterValue<TProperty?>> filterValue);

        /// <summary>
        /// Set filter template.
        /// </summary>
        /// <param name="tempaleName">Template name.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> SetTemplate(string tempaleName);

        IValueTypeFilterConfiguration<TModel, TProperty> SetValueResolver(Func<IFilterValue<TProperty?>, IEnumerable<SelectListItem>> valueResolver);

        /// <summary>
        /// Set filter display name.
        /// </summary>
        /// <param name="title">Display name.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> SetTitle(string title);

        /// <summary>
        /// Set aditional data.
        /// </summary>
        IValueTypeFilterConfiguration<TModel, TProperty> SetAdditionalData(object data);

        IValueTypeFilterConfiguration<TModel, TProperty> Visible(bool isVisible);

        /// <summary>
        /// Set filter type.
        /// </summary>
        /// <param name="type">filter type.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> SetType(FilterType type);

        /// <summary>
        /// set target property.
        /// </summary>
        /// <param name="property">Target property.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty?>> property);

        /// <summary>
        /// Set target property.
        /// </summary>
        /// <param name="begin">Target property.</param>
        /// <param name="end">Target property.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty?>> begin, Expression<Func<TModel, TProperty?>> end);


        /// <summary>
        /// Allows you to convert current expression into a more complex expression.
        /// For example:
        /// .ConditionBuilder((expression, conditionKey) =>expression!=null?Linq.Expr&#60;Task, bool>(x => x.Users.AsQueryable().Any(expression)):null));
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="builder">Builder.</param>
        IValueTypeFilterConfiguration<TModel, TProperty> ConditionBuilder<T>(Func<
             IFilterValue<TProperty?>,
             Func<IFilterValue<TProperty?>, LambdaExpressionDecorator<Func<TModel, bool>>>,
             string,
             Expression<Func<T, bool>>> builder);
    }
}
