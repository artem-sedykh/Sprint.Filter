// ReSharper disable CheckNamespace
namespace Sprint.Filter
// ReSharper restore CheckNamespace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using LinqKit;

    public sealed class ReferenceTypeFilter<TModel,TProperty>:IReferenceTypeFilter<TModel, TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly IDictionary<string, IReferenceTypeCondition<TProperty>> _conditions;
        private bool _isVisible;
        private Type _returnModelType;
        private readonly string _typeName;
        private Lazy<IEnumerable<SelectListItem>> _lazyDictionary;
        private IQueryable<SelectListItem> _dictionary;
        private Func<IFilterValue<TProperty>,
            Func<IFilterValue<TProperty>, Expression<Func<TModel, bool>>>,
            string, object> _expressionBuilder;

        private IFilterValue<TProperty> _defaultFilterValue;
        private IFilterValue<TProperty> _initFilterValue;
        private Lazy<IFilterValue<TProperty>> _defaultLazyFilterValue;
        private Func<IReferenceTypeCondition<TProperty>, IFilterValue<TProperty>, Expression<Func<TModel, bool>>> _conditionInvoker;        
        private IFilterValue<TProperty> _filterValue;
 
        public ReferenceTypeFilter()
        {
            _conditions = new Dictionary<string, IReferenceTypeCondition<TProperty>>();

            _isVisible = true;

            _returnModelType = typeof(TModel);

            _typeName = typeof (TProperty).FullName;
        }

        string IFilterView.ConditionKey { get { return GetFilterValue().ConditionKey; } }

        IEnumerable<object> IFilterView.Values { get { return GetFilterValue().Values; } }

        object IFilterView.LeftValue
        {
            get { return GetFilterValue().LeftValue; }
        }

        object IFilterView.RightValue { get { return GetFilterValue().RightValue; } }

        string IFilterView.TypeName
        {
            get { return _typeName; }            
        }

        string IFilterView.ValueFormat { get; set; }

        string IFilterView.Title { get; set; }

        string IFilterView.TemplateName { get; set; }

        IEnumerable<SelectListItem> IFilterView.Dictionary
        {
            get { return _dictionary ?? (_lazyDictionary != null ? _lazyDictionary.Value : null); }
        }

        IEnumerable<SelectListItem> IFilterView.GetConditions()
        {
            return _conditions != null ? _conditions.Select(x => new SelectListItem { Value = x.Key, Text = x.Value.Title }) : null;
        }

        object IFilterView.AdditionalData { get; set; }

        public bool IsVisible
        {
            get { return _isVisible; }
        }

        public FilterType FilterType { get; set; }

        public void Init(IFilterValue<TProperty> value)
        {
            _filterValue = null;

            if (value == null) return;

            _initFilterValue = value;            
        }

        void IFilter.Init(IFilterValue value)
        {
            Init(value as IFilterValue<TProperty>);
        }

        public IEnumerable<T> ApplyFilter<T>(IEnumerable<T> query)
        {
            var expression = BuildExpression<T>();

            return expression != null ? query.AsQueryable().Where(expression) : query;
        }

        public IQueryable<T> ApplyFilter<T>(IQueryable<T> query)
        {
            var expression = BuildExpression<T>();

            return expression != null ? query.Where(expression) : query;
        }

        public Expression<Func<T, bool>> BuildExpression<T>()
        {
            if (_returnModelType == typeof(T) && _conditionInvoker != null)
            {
                var selectedCondition = GetCondition();

                var expression = (_expressionBuilder != null 
                    ? _expressionBuilder(GetFilterValue(), filterValue => _conditionInvoker(selectedCondition.Value, filterValue), selectedCondition.Key)
                    : _conditionInvoker(selectedCondition.Value, GetFilterValue())) as Expression<Func<T, bool>>;

                return expression != null ? expression.Expand() : null;
            }

            return null;
        }

        public void SetDefaultValue(Func<IFilterValue<TProperty>> filterValue)
        {
            _filterValue = null;

            _defaultLazyFilterValue = new Lazy<IFilterValue<TProperty>>(filterValue);
        }

        public void SetDefaultValue(IFilterValue<TProperty> filterValue)
        {
            _filterValue = null;

            _defaultFilterValue = filterValue;
        }

        public void SetDictionary(Func<IEnumerable<SelectListItem>> dictionary)
        {
            _lazyDictionary = new Lazy<IEnumerable<SelectListItem>>(dictionary);
        }

        public void SetDictionary(IQueryable<SelectListItem> dictionary)
        {
            _dictionary = dictionary;
        }

        public void Visible(bool isVisible)
        {
            _isVisible = isVisible;
        }

        public Type ReturnModelType
        {
            get { return _returnModelType; }
        }

        public void For(Expression<Func<TModel, TProperty>> property)
        {
            _conditionInvoker = (condition, value) =>
            {
                if (condition == null)
                    return null;

                condition.Value = value;                

                return condition.For(property);
            };      
        }

        public void For(Expression<Func<TModel, TProperty>> left, Expression<Func<TModel, TProperty>> right)
        {
            _conditionInvoker = (condition, value) =>
            {
                if (condition == null)
                    return null;

                condition.Value = value;

                return condition.For(left,right);
            };      
        }

        public void ConditionBuilder<T>(Func<
            IFilterValue<TProperty>,
            Func<IFilterValue<TProperty>, Expression<Func<TModel, bool>>>,
            string,
            Expression<Func<T, bool>>> builder)
        {
            _expressionBuilder = builder;

            _returnModelType = typeof(T);
        }

        public void Conditions(Action<IDictionary<string, IReferenceTypeCondition<TProperty>>> conditions)
        {
            conditions(_conditions);
        }

        private IFilterValue<TProperty> GetFilterValue()
        {
            if (_filterValue != null)
                return _filterValue;

            var value = (_initFilterValue ?? _defaultFilterValue) ??
                        (_defaultLazyFilterValue != null ? _defaultLazyFilterValue.Value : null);

            value = value ?? (new FilterValue<TProperty> { ConditionKey = _conditions.Keys.FirstOrDefault() });            

            _filterValue = value;

            return value;
        }

        private KeyValuePair<string, IReferenceTypeCondition<TProperty>> GetCondition()
        {
            var key = GetFilterValue().ConditionKey;

            return _conditions.FirstOrDefault(x => x.Key == key);
        }
    }
}
