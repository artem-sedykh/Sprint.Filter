namespace Sprint.Filter
{
    using System.Collections.Generic;

    public interface IFilterOptions
    {
        long LoadFilterId { get; set; }

        IDictionary<string, IFilterValue> Filters { get; }

        FilterType FilterType { get; }
    }
}
