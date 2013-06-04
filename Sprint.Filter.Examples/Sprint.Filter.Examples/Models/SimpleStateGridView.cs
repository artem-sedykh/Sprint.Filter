using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sprint.Filter.Examples.Models
{
    public class SimpleStateGridView
    {
        [HiddenInput(DisplayValue = false)]
        public string Key { get; set; }

        [Display(Name = @"Name:"), Required]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string FilterRawOptions { get; set; }        

        public override string ToString()
        {
            return "Save as:";
        }
    }
}