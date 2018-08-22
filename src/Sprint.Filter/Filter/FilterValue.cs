namespace Sprint.Filter
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    [Serializable]
    public sealed class FilterValue<TModel> : IFilterValue<TModel>
    {      
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_typeName != null ? _typeName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ConditionKey != null ? ConditionKey.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Values != null ? Values.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ EqualityComparer<TModel>.Default.GetHashCode(LeftValue);
                hashCode = (hashCode*397) ^ EqualityComparer<TModel>.Default.GetHashCode(RightValue);
                return hashCode;
            }
        }

        private readonly string _typeName;

        public FilterValue()
        {
            var type = typeof (TModel);

            _typeName = (Nullable.GetUnderlyingType(type) ?? type).FullName;
            
            Values = new List<TModel>();
        }

        public string ConditionKey { get; set; }

        public IEnumerable<TModel> Values { get; set; }

        public TModel LeftValue { get; set; }

        public TModel RightValue { get; set; }

        public string TypeName => _typeName;

        IEnumerable<object> IFilterValue.Values => Values?.Cast<object>();

        object IFilterValue.LeftValue => LeftValue;

        object IFilterValue.RightValue => RightValue;

        public override bool Equals(object obj)
        {
            return obj is FilterValue<TModel> filterValue && Equals(filterValue);
        }

        public bool Equals(FilterValue<TModel> filterValue)
        {
            if (filterValue == null)
                return false;

            if ((filterValue.Values == null && Values != null) || (filterValue.Values != null && Values == null))
                return false;

            var equal = TypeName == filterValue.TypeName
                        && ConditionKey == filterValue.ConditionKey
                        && EqualProperty(LeftValue, filterValue.LeftValue)
                        && EqualProperty(RightValue, filterValue.RightValue);


            if (filterValue.Values != null && Values != null)
                equal = equal && Values.SequenceEqual(filterValue.Values);

            return equal;
        }

        public static bool EqualProperty(TModel property1, TModel property2)
        {
            if (property1 == null && property2 != null || (property2 == null && property1 != null))
                return false;

            return property1 == null || property1.Equals(property2);
        }

    }
}
