namespace Sprint.Filter
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    [Serializable]
    public sealed class FilterValue<TModel> : IFilterValue<TModel>
    {
        public FilterValue()
        {
            TypeName = typeof(TModel).FullName;
            Values=new List<TModel>();
        }

        public string ConditionKey { get; set; }

        public IEnumerable<TModel> Values { get; set; }

        public TModel LeftValue { get; set; }

        public TModel RightValue { get; set; }

        public string TypeName { get; set; }

        IEnumerable<object> IFilterValue.Values
        {
            get { return Values != null ? Values.Cast<object>() : null; }
        }

        object IFilterValue.LeftValue
        {
            get { return LeftValue; }
        }

        object IFilterValue.RightValue
        {
            get { return RightValue; }
        }
    }
}
