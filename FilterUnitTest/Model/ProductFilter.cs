using Sprint.Filter;
using Sprint.Filter.Conditions;

namespace FilterUnitTest.Model
{
    public class ProductFilter : FilterCollection
    {
        public ProductFilter()
        {             
            Add("CategoryId", new ValueTypeFilter<Product, int>())
                .For(x => x.CategoryId)
                .Conditions(c => c["isin"] = new ValueTypeIsInCondition<int>("IsIn"))                
                .SetTitle("Categories:")
                .SetTemplate("FastFilterWithListBox");

            Add("UnitPrice", new ValueTypeFilter<Product, decimal>())
                .For(x => x.UnitPrice)
                .Conditions(c => c["between"] = new BetweenCondition<decimal>("Between"))
                .SetTitle("UnitPrice:")
                .SetValueFormat("{0:G29}")
                .SetTemplate("FastNumberFilter");

            Add("Name", new ReferenceTypeFilter<Product, string>())
                .For(x => x.Name)
                .Conditions(c => c["contains"] = new ContainsCondition("Contains"))
                .SetTitle("Name:")
                .SetTemplate("FastStringFilter");
        }
    }
}
