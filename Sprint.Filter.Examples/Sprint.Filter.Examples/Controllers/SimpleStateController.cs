using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sprint.Filter.Examples.Models;
using Sprint.Filter.Examples.Services;

namespace Sprint.Filter.Examples.Controllers
{
    public class SimpleStateController : Controller
    {
        private readonly ISimpleStateService _simpleStateService;

        public SimpleStateController(ISimpleStateService simpleStateService)
        {
            _simpleStateService = simpleStateService;
        }

        [HttpGet]
        public ActionResult Create(string key)
        {
            var model = new SimpleStateGridView
                {
                    Key=key
                };

            return View("CreateModal", model);
        }

        [HttpPost]
        public ActionResult Create(SimpleStateGridView model)
        {
            _simpleStateService.Create(model);

            return new EmptyResult();
        }

        public ActionResult GetStateList(int id,string key)
        {
            var query = _simpleStateService.GetSavedFilters(key);            
            ViewData["filterKey"] = key;
            var items = query.Select(x => new SelectListItem
                {
                    Value = SqlFunctions.StringConvert((double) x.Id).Trim(),
                    Selected = x.Id==id,
                    Text = x.Name
                }).ToList();

            return View("StateList", items);
        }

        [HttpGet]
        public JsonResult GetState(int id, string key)
        {
            var options = _simpleStateService.GetState(id, key);

            return new JsonResult
            {
                Data = options,
                ContentType = "text/html",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public ActionResult DeleteState(int id,string key)
        {
            _simpleStateService.Delete(id, key);

            return new EmptyResult();
        }
    }
}