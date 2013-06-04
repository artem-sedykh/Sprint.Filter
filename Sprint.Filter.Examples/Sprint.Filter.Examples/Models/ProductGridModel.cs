using Sprint.Grid.Impl;
using System.Web.Helpers;

namespace Sprint.Filter.Examples.Models
{
    public sealed class ProductGridModel : GridModel<Product>
    {
        public ProductGridModel(string gridKey)
            : base(gridKey)
        {
            Columns.For((c, html) => c.ID, "Id")
                   .Title("ID")
                   .SortColumn(c => c.ID, SortDirection.Ascending)
                   .HeaderAttributes(new {style = "width:2%;"});

            Columns.For((c, html) => c.Name, "Name")
                   .Title("Name")
                   .SortColumn(c => c.Name)
                   .HeaderAttributes(new { style = "width:40%;" });            

            Columns.For((c, html) => c.Category.Name, "CategoryName")
                   .Title("CategoryName")
                   .SortColumn(c => c.Category.Name)
                   .HeaderAttributes(new { style = "width:38%;" });

            Columns.For((c, html) => c.UnitPrice.HasValue ? c.UnitPrice.Value.ToString("G29") : null, "UnitPrice")
                   .Title("UnitPrice")
                   .SortColumn(c => c.UnitPrice)
                   .HeaderAttributes(new { style = "width:20%;" });

            PageSize = 25;
        }
    }
}