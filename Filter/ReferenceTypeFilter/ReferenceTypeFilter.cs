// ReSharper disable CheckNamespace
namespace Sprint.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Helpers;

    public sealed class ReferenceTypeFilter<TModel,TProperty>:IReferenceTypeFilter<TModel, TProperty> where TProperty : class, IComparable, IComparable<TProperty>, IEquatable<TProperty>
    {
        private readonly IDictionary<string, IReferenceTypeCondition<TProperty>> _conditions;
        private bool _isVisible;
        private Type _returnModelType;
        private readonly string _typeName;
        private Func<IFilterValue<TProperty>,
            Func<IFilterValue<TProperty>, LambdaExpressionDecorator<Func<TModel, bool>>>,
            string, object> _expressionBuilder;

        private IFilterValue<TProperty> _defaultFilterValue;
        private IFilterValue<TProperty> _initFilterValue;
        private Lazy<IFilterValue<TProperty>> _defaultLazyFilterValue;
        private Func<IFilterValue<TProperty>, IEnumerable<SelectListItem>> _valueResolver;
        private Func<IReferenceTypeCondition<TProperty>, IFilterValue<TProperty>, Expression<Func<TModel, bool>>> _conditionInvoker;

        private IFilterValue<TProperty> _filterValue;
 
        public ReferenceTypeFilter()
        {
            _conditions = new Dictionary<string, IReferenceTypeCondition<TProperty>>();

            _isVisible = true;

            _returnModelType = typeof(TModel);

            _typeName = typeof (TProperty).FullName;
        }

        string IFilterView.ConditionKey
        {
            get
            {
                var value = GetFilterValue();
                return value != null ? value.ConditionKey : null;
            }
        }

        IEnumerable<object> IFilterView.Values
        {
            get
            {
                var value = GetFilterValue();

                return value != null ? value.Values : null;
            }
        }

        object IFilterView.LeftValue
        {
            get
            {
                var value = GetFilterValue();

                return value != null ? value.LeftValue : null;
            }
        }

        object IFilterView.RightValue
        {
            get
            {
                var value = GetFilterValue();

                return value != null ? value.RightValue : null;   
            }
        }

        string IFilterView.TypeName
        {
            get { return _typeName; }            
        }

        string IFilterView.ValueFormat { get; set; }

        string IFilterView.Title { get; set; }

        string IFilterView.TemplateName { get; set; }

        Func<IEnumerable<SelectListItem>> IFilterView.ValueResolver
        {
            get { return ValueResolver; }
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

        public void SetValueResolver(Func<IFilterValue<TProperty>, IEnumerable<SelectListItem>> valueResolver)
        {
            _valueResolver = valueResolver;
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
            if (_returnModelType == typeof(T))
            {
                var value = GetFilterValue();

                if (value == null)
                    return null;

                if (_expressionBuilder != null)
                {
                    var selectedCondition = GetCondition(value);

                    var expr = _expressionBuilder(value,
                        _conditionInvoker != null ? (filterValue => new LambdaExpressionDecorator<Func<TModel, bool>>(_conditionInvoker(selectedCondition.Value, filterValue))) : (Func<IFilterValue<TProperty>, LambdaExpressionDecorator<Func<TModel, bool>>>)null,
                        selectedCondition.Key) as Expression<Func<T, bool>>;

                    return expr != null ? expr.ExpandLambdaExpressionDecorators() : null;
                }

                if (_conditionInvoker != null)
                {
                    var selectedCondition = GetCondition(value);

                    var expr = _conditionInvoker(selectedCondition.Value, GetFilterValue());

                    if (expr != null)                    
                        return Expression.Lambda<Func<T, bool>>(expr.Body, expr.Parameters);                    
                }
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
            Func<IFilterValue<TProperty>, LambdaExpressionDecorator<Func<TModel, bool>>>,
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

        bool IFilterView.HasChanged()
        {
            var filterValue = GetFilterValue();

            if (filterValue == null)
                return false;

            var defaultValue = _defaultFilterValue ?? (_defaultLazyFilterValue != null ? _defaultLazyFilterValue.Value : null);

            return filterValue.Equals(defaultValue);
        }

        private KeyValuePair<string, IReferenceTypeCondition<TProperty>> GetCondition(IFilterValue filterValue)
        {            
            return _conditions.FirstOrDefault(x => x.Key == filterValue.ConditionKey);
        }

        private IEnumerable<SelectListItem> ValueResolver()
        {
            if (_valueResolver == null)
                return null;

            var value = GetFilterValue();

            return _valueResolver(value);
        }
    }
}
