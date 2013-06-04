using Sprint.Filter.Examples.Models;
using Sprint.Specifications;

namespace Sprint.Filter.Examples.Specifications
{
    public class ProductSpecification
    {
        public static ISpecification<Product> Search(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return new Specification<Product>(p => true);

            searchString = searchString.ToUpper();

            return new Specification<Product>(p => p.Name.Contains(searchString) || p.Category.Name.Contains(searchString));
        }
    }
}