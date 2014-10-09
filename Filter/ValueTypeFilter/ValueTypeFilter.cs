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

    public sealed class ValueTypeFilter<TModel,TProperty>:IValueTypeFilter<TModel, TProperty> where TProperty : struct, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly IDictionary<string, IValueTypeCondition<TProperty>> _conditions;
        private bool _isVisible;
        private Type _returnModelType;
        private readonly string _typeName;
        private Lazy<IEnumerable<SelectListItem>> _lazyDictionary;
        private IQueryable<SelectListItem> _dictionary;
        private Func<
             IFilterValue<TProperty?>,
             Func<IFilterValue<TProperty?>, Expression<Func<TModel, bool>>>,
             string,
             Object> _expressionBuilder;
        private IFilterValue<TProperty?> _defaultFilterValue;
        private IFilterValue<TProperty?> _initFilterValue;
        private Lazy<IFilterValue<TProperty?>> _defaultLazyFilterValue;
        private IFilterValue<TProperty?> _filterValue;
        private Func<IValueTypeCondition<TProperty>,IFilterValue<TProperty?>, Expression<Func<TModel, bool>>> _conditionInvoker;

        public ValueTypeFilter()
        {
            _conditions = new Dictionary<string, IValueTypeCondition<TProperty>>();

            _isVisible = true;

            _returnModelType = typeof(TModel);

            _typeName = typeof (TProperty).FullName;
        }

        string IFilterView.ConditionKey { get { return GetFilterValue().ConditionKey; } }

        IEnumerable<object> IFilterView.Values { get { return GetFilterValue().Values.Cast<object>(); } }

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

        public void Init(IFilterValue<TProperty?> value)
        {
            _filterValue = null;

            if (value == null) return;

            _initFilterValue = value;            
        }

        void IFilter.Init(IFilterValue value)
        {
            Init(value as IFilterValue<TProperty?>);         
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
            if (_returnModelType == typeof(T))
            {
                var value = GetFilterValue();

                if(value == null)
                    return null;

                if(_expressionBuilder != null)
                {
                    var selectedCondition = GetCondition();

                    var expr = _expressionBuilder(value,
                        filterValue => _conditionInvoker(selectedCondition.Value, filterValue), 
                        selectedCondition.Key) as Expression<Func<T, bool>>;

                    return expr != null ? expr.Expand() : null;
                }

                if(_conditionInvoker != null)
                {
                    var selectedCondition = GetCondition();

                    var expr = _conditionInvoker(selectedCondition.Value, GetFilterValue());

                    if(expr != null)
                    {
                        expr = expr.Expand();

                        return Expression.Lambda<Func<T, bool>>(expr.Body, expr.Parameters);
                    }                    
                }             
            }

            return null;
        }

        public void SetDefaultValue(Func<IFilterValue<TProperty?>> filterValue)
        {
            _filterValue = null;

            _defaultLazyFilterValue = new Lazy<IFilterValue<TProperty?>>(filterValue);
        }

        public void SetDefaultValue(IFilterValue<TProperty?> filterValue)
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

        public void For(Expression<Func<TModel, TProperty?>> property)
        {
            _conditionInvoker = (condition, value) =>
            {
                if (condition == null)
                    return null;

                condition.Value = value;

                return condition.For(property);
            };
        }

        public void For(Expression<Func<TModel, TProperty?>> left, Expression<Func<TModel, TProperty?>> right)
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
             IFilterValue<TProperty?>,
             Func<IFilterValue<TProperty?>, Expression<Func<TModel, bool>>>,
             string,
             Expression<Func<T, bool>>> builder)
        {
            _expressionBuilder = builder;

            _returnModelType = typeof(T);
        }

        public void Conditions(Action<IDictionary<string, IValueTypeCondition<TProperty>>> conditions)
        {
            conditions(_conditions);
        }

        private IFilterValue<TProperty?> GetFilterValue()
        {
            if (_filterValue != null)
                return _filterValue;

            var value = (_initFilterValue ?? _defaultFilterValue) ??
                        (_defaultLazyFilterValue != null ? _defaultLazyFilterValue.Value : null);            

            _filterValue = value;

            return value;
        }

        private KeyValuePair<string, IValueTypeCondition<TProperty>> GetCondition()
        {
            var key = GetFilterValue().ConditionKey;

            return _conditions.FirstOrDefault(x => x.Key == key);
        }
    }
}
