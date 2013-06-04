using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sprint.Filter.Examples.Filters;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Controllers
{
    public class MultipleConditionsFilterController : Controller
    {
        private readonly NorthwindDataContext _dc;

        public MultipleConditionsFilterController(NorthwindDataContext dc)
        {
            _dc = dc;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Multiple Conditions Filter";

            return View();
        }

        public ActionResult Grid(FilterOptions options)
        {
            var filter = new ProductFilter(_dc);

            var query = _dc.Products.OrderBy(x => x.ID).AsQueryable();

            query = filter.Init(options).ApplyFilters(query);

            return View("Products", query);
        }

        public ActionResult Filter(FilterOptions options)
        {
            var filter = new ProductFilter(_dc);
            filter.Init(options);

            return View("AjaxFilterCollection", filter);
        }

        public ActionResult ClearFilter()
        {
            return RedirectToAction("Index");
        }
    }
}
