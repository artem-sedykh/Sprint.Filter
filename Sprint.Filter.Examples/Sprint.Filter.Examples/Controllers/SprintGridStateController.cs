using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Sprint.Filter.Examples.Attributes;
using Sprint.Filter.Examples.Models;
using Sprint.Filter.Examples.Services;

namespace Sprint.Filter.Examples.Controllers
{
    public class SprintGridStateController : Controller
    {
        private readonly ISprintGridStateService _sprintGridStateService;

        public SprintGridStateController(ISprintGridStateService sprintGridStateService)
        {
            _sprintGridStateService = sprintGridStateService;
        }

        [HttpGet]
        public ActionResult Create(string gridKey)
        {
            var items = _sprintGridStateService.GetStates(gridKey).Select(x => new SelectListItem
                {
                    Value = SqlFunctions.StringConvert((double) x.Id).Trim(),
                    Text = x.Name
                }).ToList();

            var model = new GridStateView
                {
                    GridKey = gridKey,
                    States = items
                };

            return View("CreateModal", model);
        }

        [HttpPost, ValidateModelState]
        public JsonResult Create(GridStateView model)
        {
            _sprintGridStateService.Create(model);

            return new JsonResult
                {
                    Data = model.GridStateId,
                    ContentType = "text/html",
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
        }

        [HttpGet]
        public ActionResult StateList(string gridKey, int stateId)
        {
            var query = _sprintGridStateService.GetStates(gridKey);

            var items = query.Select(x => new SelectListItem
            {
                Value = SqlFunctions.StringConvert((double)x.Id).Trim(),
                Selected = x.Id == stateId,
                Text = x.Name
            }).ToList();

            ViewData["gridKey"] = gridKey;

            return View("StateList", items);
        }

        [HttpGet]
        public JsonResult GetState(int id, string gridKey)
        {
            return new JsonResult
            {
                Data = _sprintGridStateService.GetActionGridOptions(id, gridKey),
                ContentType = "text/html",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult DeleteState(int id, string gridKey)
        {
            _sprintGridStateService.Delete(id, gridKey);

            return new EmptyResult();
        }

        [HttpGet]
        public JsonResult Clear(string gridKey)
        {
            Session.Remove(gridKey);

            return new JsonResult
            {
                Data = new ActionGridOptions(),
                ContentType = "text/html",
                ContentEncoding = System.Text.Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
