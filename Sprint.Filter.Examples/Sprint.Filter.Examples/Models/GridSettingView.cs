using System.Collections.Generic;
using System.Web.Mvc;
using Sprint.Grid;

namespace Sprint.Filter.Examples.Models
{
    public sealed class GridSettingView
    {
        private readonly string _gridKey;

        public GridSettingView(string gridKey)
        {
            _gridKey = gridKey;
        }

        [HiddenInput(DisplayValue = false)]
        public string GridKey
        {
            get { return _gridKey; }
        }

        public IDictionary<string, IGridColumn> Columns { get; set; }

        public int PageSize { get; set; }
    }
}