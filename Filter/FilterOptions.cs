namespace Sprint.Filter
{
    using System.Collections.Generic;

    public class FilterOptions : IFilterOptions
    {
        public FilterOptions()
        {
            Filters = new Dictionary<string, IFilterValue>();
        }

        public long LoadFilterId { get; set; }

        public IDictionary<string, IFilterValue> Filters { get; set; }

        public FilterType FilterType { get; set; }
    }
}
