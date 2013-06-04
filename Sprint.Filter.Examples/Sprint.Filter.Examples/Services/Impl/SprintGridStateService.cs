using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Script.Serialization;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Services.Impl
{
    public class SprintGridStateService : ISprintGridStateService
    {
        private readonly NorthwindDataContext _dc;

        public SprintGridStateService(NorthwindDataContext dc)
        {
            _dc = dc;
        }

        public IQueryable<SavedFilter> GetStates(string key)
        {
            return _dc.SavedFilters.Where(x => x.FilterKey == key);
        }

        public void Create(GridStateView model)
        {
            var bf = new BinaryFormatter();
            var options = GetActionGridOptions(model.ActionGridRawOptions);
           
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, options);
                var data = ms.ToArray();

                if (model.GridStateId.HasValue)
                {
                    var gridState =
                        _dc.SavedFilters.First(x => x.Id == model.GridStateId && x.FilterKey == model.GridKey);

                    gridState.Name = model.Name;

                    gridState.Data = data;

                    _dc.SaveChanges();
                }
                else
                {
                    var gridState = new SavedFilter()
                    {
                       Data = data,
                       FilterKey = model.GridKey,
                       Name = model.Name                       
                    };

                    _dc.SavedFilters.Add(gridState);

                    _dc.SaveChanges();

                    model.GridStateId = gridState.Id;
                }                
            }         
        }

        public ActionGridOptions GetActionGridOptions(int id, string gridKey)
        {
            var state = _dc.SavedFilters.First(x => x.Id == id && x.FilterKey==gridKey);

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(state.Data))
            {
                var options = bf.Deserialize(ms) as ActionGridOptions;
                if (options != null)
                    options.LoadFilterId = state.Id;
                return options;
            }
        }

        public void Delete(int stateId, string gridKey)
        {
            var gridState = _dc.SavedFilters.First(x => x.Id == stateId && x.FilterKey == gridKey);

            _dc.SavedFilters.Remove(gridState);

            _dc.SaveChanges();
        }

        public static ActionGridOptions GetActionGridOptions(string rawOptions)
        {
            var scriptSerializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            //register FilterValueJavaScriptConverter
            scriptSerializer.RegisterConverters(new List<JavaScriptConverter> { new FilterValueJavaScriptConverter() });

            return string.IsNullOrWhiteSpace(rawOptions)
                         ? null
                         : scriptSerializer.Deserialize<ActionGridOptions>(rawOptions);
        }
    }
}