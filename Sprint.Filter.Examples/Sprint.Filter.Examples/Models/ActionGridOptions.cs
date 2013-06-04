using System;
using System.Collections.Generic;
using Sprint.Grid;

namespace Sprint.Filter.Examples.Models
{
    [Serializable]
    public sealed class ActionGridOptions : IGridOptions, IFilterOptions
    {
        public ActionGridOptions()
        {
            Filters = new Dictionary<string, IFilterValue>();
        }

        public string SearchString { get; set; }

        public long LoadFilterId { get; set; }

        public IDictionary<string, IFilterValue> Filters { get; set; }

        public FilterType FilterType { get; set; }

        public Dictionary<string, Dictionary<string, object>> ColOpt { get; set; }

        public Dictionary<string, string> PageOpt { get; set; }
    }
}