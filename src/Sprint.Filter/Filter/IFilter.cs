namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IFilter : IFilterView
    {
        /// <summary>
        /// Apply filter for current sequence.
        /// </summary>
        ///<typeparam name="TModel">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Sequence.</param>
        /// <returns>filtered sequence.</returns>
        IEnumerable<TModel> ApplyFilter<TModel>(IEnumerable<TModel> source);

        /// <summary>
        /// Apply filter for current query.
        /// </summary>        
        ///<typeparam name="TModel">The type of the elements of <paramref name="query"/>.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>filtered query.</returns>
        IQueryable<TModel> ApplyFilter<TModel>(IQueryable<TModel> query);

        /// <summary>
        /// Create expression tree.
        /// </summary>
        /// <typeparam name="TModel">Type.</typeparam>
        /// <returns>Expression tree.</returns>
        Expression<Func<TModel, bool>> BuildExpression<TModel>();

        /// <summary>
        /// hide/show filter.
        /// </summary>
        void Visible(bool isVisible);

        /// <summary>
        /// Init current filter.
        /// </summary>
        /// <param name="value">Filter value.</param>
        void Init(IFilterValue value);

        /// <summary>
        /// Target model type.
        /// </summary>
        Type ReturnModelType { get; }
    }
}
