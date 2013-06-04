using System.Linq;
using System.Web.Mvc;
using Sprint.Filter.Examples.Attributes;
using Sprint.Filter.Examples.Filters;
using Sprint.Filter.Examples.Models;
using Sprint.Filter.Examples.Specifications;
using Sprint.Grid;
using Sprint.Grid.Impl;

namespace Sprint.Filter.Examples.Controllers
{
    public class SprintGridWithFilterController : Controller
    {
        public const string ProductGridKey = "product";

        private readonly NorthwindDataContext _dc;

        public SprintGridWithFilterController(NorthwindDataContext dc)
        {            
            _dc = dc;
        }

        public ActionResult Index([SessionModelBinder(ProductGridKey)]ActionGridOptions options)
        {
            ViewData["gridKey"] = ProductGridKey;

            return View(options);
        }

        public ActionResult Grid([SessionModelBinder(ProductGridKey)]ActionGridOptions options)
        {
            var filter = new ProductFilter(_dc);
            
            var query = _dc.Products.Where(ProductSpecification.Search(options.SearchString).Predicate).OrderBy(x => x.ID).AsQueryable();

            query = filter.Init(options).ApplyFilters(query);

            var model = new ProductGridModel(ProductGridKey);

            return View(new ActionGridView<Product>(model, query).Init(options));
        }
        
        public ActionResult Filter([SessionModelBinder(ProductGridKey)] ActionGridOptions options)
        {
            var filter = new ProductFilter(_dc) {UpdateTargetId = ProductGridKey};

            filter.Init(options);

            return View("FilterCollection", filter);
        }

        public ActionResult GridSetting(ActionGridOptions options, string gridKey)
        {
            var gridModel = new ProductGridModel(ProductGridKey);
            gridModel.Init(options);
            var model = new GridSettingView(gridKey)
            {
                Columns = gridModel.Columns.OrderBy(x => x.Value.Order).ToDictionary(x => x.Key, x => x.Value as IGridColumn),
                PageSize = gridModel.PageSize
            };

            return View("GridSetting", model);
        }
    }
}
