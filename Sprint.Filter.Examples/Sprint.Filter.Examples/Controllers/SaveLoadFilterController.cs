using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sprint.Filter.Examples.Attributes;
using Sprint.Filter.Examples.Filters;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Controllers
{
    public class SaveLoadFilterController : Controller
    {
        private readonly NorthwindDataContext _dc;

        public const string FilterKey = "ProductFilterKeySaveLoadFilter";

        public SaveLoadFilterController(NorthwindDataContext dc)
        {
            _dc = dc;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Save/Load filter";

            return View();
        }

        public ActionResult Grid([SessionModelBinder(FilterKey)]FilterOptions options)
        {
            var filter = new ProductFilter(_dc);

            var query = _dc.Products.OrderBy(x => x.ID).AsQueryable();

            query = filter.Init(options).ApplyFilters(query);

            ViewData["filterOptions"] = options;            

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
