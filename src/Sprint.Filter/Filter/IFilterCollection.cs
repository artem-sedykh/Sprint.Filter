namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFilterCollection : IFilterCollectionView
    {
        /// <summary>
        /// Filters.
        /// </summary>
        new IDictionary<string, IFilter> Filters { get; }

        /// <summary>
        /// Apply filters for current sequence.
        /// </summary>        
        ///<typeparam name="TModel">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Sequence.</param>
        /// <returns>filtered sequence.</returns>
        IEnumerable<TModel> ApplyFilters<TModel>(IEnumerable<TModel> source);

        /// <summary>
        /// Apply filters for current query.
        /// </summary>        
        ///<typeparam name="TModel">The type of the elements of <paramref name="query"/>.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>filtered query.</returns>
        IQueryable<TModel> ApplyFilters<TModel>(IQueryable<TModel> query);

        /// <summary>
        /// Creates expression tree.
        /// </summary>
        /// <typeparam name="TModel">Type.</typeparam>
        /// <returns>Expression tree.</returns>
        Expression<Func<TModel, bool>> BuildExpression<TModel>();

        /// <summary>
        /// Create an expression tree for filter.
        /// </summary>
        /// <typeparam name="TModel">Type.</typeparam>
        /// <param name="filterKey">Filter key</param>
        /// <returns>Expression tree.</returns>
        Expression<Func<TModel, bool>> BuildExpressionFor<TModel>(string filterKey);

        /// <summary>
        /// Add filter for reference type.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Type target property.</typeparam>
        /// <param name="key">Filter key</param>
        /// <param name="filter">Reference type filter.</param>
        /// <returns>Reference type filter configuration.</returns>
        IReferenceTypeFilterConfiguration<TModel, TProperty> Add<TModel, TProperty>(string key, IReferenceTypeFilter<TModel, TProperty> filter) where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>;

        /// <summary>
        /// Add filter for value type.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Type target property.</typeparam>
        /// <param name="key">Filter key</param>
        /// <param name="filter">Value type filter.</param>
        /// <returns>Value type filter configuration.</returns>
        IValueTypeFilterConfiguration<TModel, TProperty> Add<TModel, TProperty>(string key, IValueTypeFilter<TModel, TProperty> filter) where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>;

        /// <summary>
        /// Initialize filters.
        /// </summary>
        /// <param name="options">Filter collection options.</param>
        /// <returns>Current filter collection.</returns>
        IFilterCollection Init(IFilterOptions options);

        /// <summary>
        /// Get registered filter by key.
        /// </summary>
        /// <param name="key">Filter key.</param>
        /// <returns>filter</returns>
        IFilter this[string key] { get; }
    }
}
