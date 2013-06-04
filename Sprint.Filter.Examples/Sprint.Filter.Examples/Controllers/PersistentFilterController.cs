using System.Linq;
using System.Web.Mvc;
using Sprint.Filter.Examples.Attributes;
using Sprint.Filter.Examples.Filters;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Controllers
{
    public class PersistentFilterController : Controller
    {
        private readonly NorthwindDataContext _dc;
        public const string FilterKey = "ProductFilterKey";

        public PersistentFilterController(NorthwindDataContext dc)
        {
            _dc = dc;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Multiple Conditions Filter";

            ViewBag.Information = "Apply filter and press F5. FilterOptions saved in Session.";

            return View();
        }

        public ActionResult Grid([SessionModelBinder(FilterKey)]FilterOptions options)
        {
            var filter = new ProductFilter(_dc);

            var query = _dc.Products.OrderBy(x => x.ID).AsQueryable();

            query = filter.Init(options).ApplyFilters(query);

            return View("Products", query);
        }

        public ActionResult Filter([SessionModelBinder(FilterKey)]FilterOptions options)
        {
            var filter = new ProductFilter(_dc);
            filter.Init(options);

            return View("AjaxFilterCollection", filter);
        }

        public ActionResult ClearFilter()
        {
            Session.Remove(FilterKey);
            return RedirectToAction("Index");
        }
    }
}
