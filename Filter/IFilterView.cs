namespace Sprint.Filter
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System;

    public interface IFilterView
    {
        /// <summary>
        /// Filter display name
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// render template name
        /// </summary>
        string TemplateName { get; set; }

        Func<IEnumerable<SelectListItem>> ValueResolver { get; }

        /// <summary>
        /// list of conditions
        /// </summary>
        /// <returns>Returns a list of available filter conditions</returns>
        IEnumerable<SelectListItem> GetConditions();

        /// <summary>
        /// Additional data
        /// You can use HtmlHelper.AnonymousObjectToHtmlAttributes(Model.AdditionalData) to get the values
        /// </summary>
        object AdditionalData { get; set; }

        /// <summary>
        /// hide/show filter
        /// </summary>
        bool IsVisible { get; }

        /// <summary>
        /// Filter type
        /// </summary>
        FilterType FilterType { get; set; }

        /// <summary>
        /// Selected condition key
        /// </summary>
        string ConditionKey { get; }

        /// <summary>
        /// The list of values
        /// </summary>
        IEnumerable<object> Values { get; }

        /// <summary>
        /// Left value
        /// </summary>
        object LeftValue { get; }

        /// <summary>
        /// Right value
        /// </summary>
        object RightValue { get; }

        /// <summary>
        /// full type name (TProperty)
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Display value format
        /// </summary>
        string ValueFormat { get; set; }

        bool HasChanged();
    }
}