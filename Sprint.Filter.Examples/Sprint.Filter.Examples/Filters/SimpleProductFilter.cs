using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sprint.Filter.Conditions;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Filters
{
    public class SimpleProductFilter : FilterCollection
    {
        public SimpleProductFilter(NorthwindDataContext dc)
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
                .Conditions(conditions=>conditions["isin"]= new ValueTypeIsInCondition<int>("IsIn"))
                .SetDictionary(categories)
                .SetTitle("Categories:")
                .SetTemplate("FastFilterWithListBox");

            Add("UnitPrice", new ValueTypeFilter<Product, decimal>())
                .For(x => x.UnitPrice)
                .Conditions(conditions => conditions["between"] = new BetweenCondition<decimal>("Between"))
                .SetTitle("UnitPrice:")
                .SetValueFormat("{0:G29}")
                .SetTemplate("FastNumberFilter");

            Add("Name", new ReferenceTypeFilter<Product, string>())
                .For(x => x.Name)
                .Conditions(conditions=>conditions["contains"] = new ContainsCondition("Contains"))
                .SetTitle("Name:")
                .SetTemplate("FastStringFilter");

            Action = "Grid";

            UpdateTargetId = "products";
        }
    }
}