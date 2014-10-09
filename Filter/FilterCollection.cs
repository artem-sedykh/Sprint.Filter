namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using LinqKit;

    public class FilterCollection : IFilterCollection
    {
        private readonly IDictionary<String, IFilter> _filters;
        private IFilterOptions _filtersOptions;

        public FilterCollection()
        {
            _filters = new Dictionary<string, IFilter>();
        }

        /// <summary>
        /// Filters for viewmodel.
        /// </summary>
        IDictionary<string, IFilterView> IFilterCollectionView.Filters
        {
            get { return _filters.ToDictionary(f => f.Key, f => f.Value as IFilterView); }
        }

        /// <summary>
        /// Apply filters for current sequence.
        /// </summary>        
        ///<typeparam name="TModel">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">Sequence.</param>
        /// <returns>filtered sequence.</returns>
        public IEnumerable<TModel> ApplyFilters<TModel>(IEnumerable<TModel> source)
        {
            var predicate = BuildExpression<TModel>();

            return (source != null && predicate != null) ? source.AsQueryable().Where(predicate) : source;
        }

        /// <summary>
        /// Apply filters for current query.
        /// </summary>        
        ///<typeparam name="TModel">The type of the elements of <paramref name="query"/>.</typeparam>
        /// <param name="query">Query.</param>
        /// <returns>filtered query.</returns>
        public IQueryable<TModel> ApplyFilters<TModel>(IQueryable<TModel> query)
        {
            var predicate = BuildExpression<TModel>();

            return (query != null && predicate != null) ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Creates expression tree.
        /// </summary>
        /// <typeparam name="TModel">Type.</typeparam>
        /// <returns>Expression tree.</returns>
        public Expression<Func<TModel, bool>> BuildExpression<TModel>()
        {
            var modelType = typeof(TModel);

            var filters = _filters.Values.Where(x => x.ReturnModelType == modelType);

            var predicate = filters.AsParallel()
                .Select(filter => filter.BuildExpression<TModel>())
                .Where(expr => expr != null)
                .Aggregate<Expression<Func<TModel, bool>>, Expression<Func<TModel, bool>>>(null, (current, expression) => current == null ? expression : current.And(expression));            

            return predicate.Expand();
        }

        /// <summary>
        /// Create an expression tree for filter.
        /// </summary>
        /// <typeparam name="TModel">Type.</typeparam>
        /// <param name="filterKey">Filter key</param>
        /// <returns>Expression tree.</returns>
        public Expression<Func<TModel, bool>> BuildExpressionFor<TModel>(string filterKey)
        {
            return (_filters.ContainsKey(filterKey) && _filters[filterKey].ReturnModelType == typeof(TModel)) ? _filters[filterKey].BuildExpression<TModel>() : null;
        }

        /// <summary>
        /// Add filter for reference type.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Type target property.</typeparam>
        /// <param name="key">Filter key</param>
        /// <param name="filter">Reference type filter.</param>
        /// <returns>Reference type filter configuration.</returns>
        public IReferenceTypeFilterConfiguration<TModel, TProperty> Add<TModel, TProperty>(string key, IReferenceTypeFilter<TModel, TProperty> filter) where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
        {
            var configuration = new ReferenceTypeFilterConfiguration<TModel, TProperty>(filter);

            _filters[key] = filter;

            return configuration;
        }

        /// <summary>
        /// Add filter for value type.
        /// </summary>
        /// <typeparam name="TModel">Model type.</typeparam>
        /// <typeparam name="TProperty">Type target property.</typeparam>
        /// <param name="key">Filter key</param>
        /// <param name="filter">Value type filter.</param>
        /// <returns>Value type filter configuration.</returns>
        public IValueTypeFilterConfiguration<TModel, TProperty> Add<TModel, TProperty>(string key, IValueTypeFilter<TModel, TProperty> filter) where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
        {
            var configuration = new ValueTypeFilterConfiguration<TModel, TProperty>(filter);

            _filters[key] = filter;

            return configuration;
        }

        /// <summary>
        /// Initialize filters.
        /// </summary>
        /// <param name="options">Filter collection options.</param>
        /// <returns>Current filter collection.</returns>
        public IFilterCollection Init(IFilterOptions options)
        {
            _filtersOptions = options;

            if (_filtersOptions != null && _filtersOptions.Filters != null)
            {
                foreach (var filterOptions in _filtersOptions.Filters)
                {
                    if (!_filters.ContainsKey(filterOptions.Key)) continue;

                    var filter = _filters[filterOptions.Key];
                    
                    filter.Init(filterOptions.Value);
                }
            }

            return this;
        }

        /// <summary>
        /// Get registered filter by key.
        /// </summary>
        /// <param name="key">Filter key.</param>
        /// <returns>filter</returns>
        public IFilter this[string key]
        {
            get { return _filters[key]; }
        }


        /// <summary>
        /// Filters.
        /// </summary>
        public IDictionary<string, IFilter> Filters
        {
            get { return _filters; }
        }

        /// <summary>
        /// The name of the action method.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Name of the controller that contains the action method.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// An object containing the parameters for a route.        
        /// </summary>  
        public object RouteValues { get; set; }

        /// <summary>
        /// Gets or sets the DOM-member, which must be updated with the reply from the server.
        /// </summary>        
        /// <returns>
        /// DOM-element ID, which should be updated.
        /// </returns>
        public string UpdateTargetId { get; set; }

        /// <summary>
        /// Current filter options.
        /// </summary>
        public IFilterOptions FilterOptions
        {
            get { return _filtersOptions; }
        }
    }
}
