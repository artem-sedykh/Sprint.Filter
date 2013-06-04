using System;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Filters
{
    public class ProductFilter : FilterCollection
    {
        public ProductFilter(NorthwindDataContext dc)
        {
            if (dc == null)
                throw new ArgumentNullException("dc");

            var categories = dc.Categories.OrderBy(x => x.Name).Select(x =>
                new SelectListItem
                {
                    Value = SqlFunctions.StringConvert((double)x.ID).Trim(),
                    Text = x.Name
                });

            Add("CategoryId", new ValueTypeFilter<Product, int>())
                .For(x => x.CategoryID)
                .Conditions(FilterConditions.MultipleValueConditions)
                .SetDictionary(categories)
                .SetTitle("Categories:")
                .SetTemplate("FilterWithListBox");

            Add("UnitPrice", new ValueTypeFilter<Product, decimal>())
                .For(x => x.UnitPrice)
                .Conditions(FilterConditions.NumericConditions)
                .SetTitle("UnitPrice:")
                .SetValueFormat("{0:G29}")
                .SetTemplate("NumberFilter");

            Add("Name", new ReferenceTypeFilter<Product, string>())
                .For(x => x.Name)
                .Conditions(FilterConditions.StringConditions)
                .SetTitle("Name:")
                .SetTemplate("StringFilter");

            Action = "Grid";

            UpdateTargetId = "products";
        }
    }
}