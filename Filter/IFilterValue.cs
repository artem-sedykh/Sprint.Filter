namespace Sprint.Filter
{
    using System.Collections.Generic;
    using Mvc;

    [FilterValueModelBinder]      
    public interface IFilterValue
    {
        string ConditionKey { get; }

        IEnumerable<object> Values { get; }

        object LeftValue { get; }

        object RightValue { get; }

        string TypeName { get; }
    }

    public interface IFilterValue<out TModel> : IFilterValue
    {
        new IEnumerable<TModel> Values { get; }

        new TModel LeftValue { get; }

        new TModel RightValue { get; }        
    }
}
