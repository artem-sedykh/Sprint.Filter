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

    public interface IReferenceTypeFilterConfiguration<TModel, TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        /// <summary>
        /// Set value format.
        /// </summary>
        /// <param name="valueFormat">value format. 
        /// For example: {0:MM/dd/yyyy}</param>
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetValueFormat(string valueFormat);

        /// <summary>
        /// Add conditions
        /// </summary>
        /// <param name="conditions">Conditions</param>  
        IReferenceTypeFilterConfiguration<TModel, TProperty> Conditions(Action<IDictionary<string, IReferenceTypeCondition<TProperty>>> conditions);

        /// <summary>
        /// Set default filter value.
        /// </summary>
        /// <param name="filterValue">Filter value.</param>      
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(IFilterValue<TProperty> filterValue);

        /// <summary>
        /// Set default filter value.
        /// </summary>
        /// <param name="filterValue">Filter value.</param>      
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetDefaultFilterValue(Func<IFilterValue<TProperty>> filterValue);

        /// <summary>
        /// Set filter template.
        /// </summary>
        /// <param name="tempaleName">Template name.</param>   
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetTemplate(string tempaleName);

        /// <summary>
        /// Set dictionary of available values.
        /// </summary>
        /// <param name="dictionary">Available filter values.</param>   
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetDictionary(Func<IEnumerable<SelectListItem>> dictionary);

        /// <summary>
        /// Set dictionary of available values.
        /// </summary>
        /// <param name="dictionary">Available filter values.</param>   
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetDictionary(IQueryable<SelectListItem> dictionary);

        /// <summary>
        /// Set filter display name.
        /// </summary>
        /// <param name="title">Display name.</param>
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetTitle(string title);

        /// <summary>
        /// Set aditional data.
        /// </summary>                
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetAdditionalData(object data);

        IReferenceTypeFilterConfiguration<TModel, TProperty> Visible(bool isVisible);

        /// <summary>
        /// Set filter type.
        /// </summary>
        /// <param name="type">filter type.</param>   
        IReferenceTypeFilterConfiguration<TModel, TProperty> SetType(FilterType type);

        /// <summary>
        /// set target property.
        /// </summary>
        /// <param name="property">Target property.</param>  
        IReferenceTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty>> property);

        /// <summary>
        /// Set target property.
        /// </summary>
        /// <param name="begin">Target property.</param>
        /// <param name="end">Target property.</param>    
        IReferenceTypeFilterConfiguration<TModel, TProperty> For(Expression<Func<TModel, TProperty>> begin, Expression<Func<TModel, TProperty>> end);

        /// <summary>
        /// Allows you to convert current expression into a more complex expression.
        /// For example:
        /// .ConditionBuilder((expression, conditionKey) =>expression!=null?Linq.Expr&#60;Task, bool>(x => x.Users.AsQueryable().Any(expression)):null));
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="builder">Builder.</param>     
        IReferenceTypeFilterConfiguration<TModel, TProperty> ConditionBuilder<T>(Func<
            IFilterValue<TProperty>,
            Func<IFilterValue<TProperty>, LambdaExpressionDecorator<Func<TModel, bool>>>,
            string,
            Expression<Func<T, bool>>> builder);
    }
}
