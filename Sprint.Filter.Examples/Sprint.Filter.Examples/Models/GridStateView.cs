using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sprint.Filter.Examples.Attributes;

namespace Sprint.Filter.Examples.Models
{
    public class GridStateView
    {
        [HiddenInput(DisplayValue = false)]
        public string GridKey { get; set; }

        [Display(Name = "What filter you want to change?"), DropDownList("States", OptionLabel = "The new filter", Css = "dropdown grid-state-list")]
        public int? GridStateId { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> States { get; set; }

        [Display(Name = @"Name:"), Required, StringLength(100)]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ActionGridRawOptions { get; set; }

        public override string ToString()
        {
            return "Save as:";
        }
    }
}