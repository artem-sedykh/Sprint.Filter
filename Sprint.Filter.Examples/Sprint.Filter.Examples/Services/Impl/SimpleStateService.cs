using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Services.Impl
{
    public class SimpleStateService : ISimpleStateService
    {
        private readonly NorthwindDataContext _dc;

        public SimpleStateService(NorthwindDataContext dc)
        {
            _dc = dc;
        }

        public void Create(SimpleStateGridView model)
        {
            var state = new SavedFilter
            {
                Name = model.Name,
                FilterKey = model.Key,
                Data = Encoding.UTF8.GetBytes(model.FilterRawOptions)
            };

            _dc.SavedFilters.Add(state);
            _dc.SaveChanges();
        }

        public static FilterOptions GetFilterOptions(byte[] data)
        {
            var filterRawOptions = Encoding.UTF8.GetString(data);
            var scriptSerializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            scriptSerializer.RegisterConverters(new List<JavaScriptConverter> { new FilterValueJavaScriptConverter() });

            return string.IsNullOrWhiteSpace(filterRawOptions)
                          ? null
                          : scriptSerializer.Deserialize<FilterOptions>(HttpUtility.HtmlDecode(filterRawOptions));
        }

        public IQueryable<SavedFilter> GetSavedFilters(string key)
        {
            return _dc.SavedFilters.Where(x => x.FilterKey == key);
        }

        public IFilterOptions GetState(int id, string key)
        {
            var state = _dc.SavedFilters.First(x => x.Id == id && x.FilterKey == key);
            var options = GetFilterOptions(state.Data);
            options.LoadFilterId = id;

            return options;
        }

        public void Delete(int id, string key)
        {
            var state = _dc.SavedFilters.First(x => x.Id == id && x.FilterKey == key);

            _dc.SavedFilters.Remove(state);

            _dc.SaveChanges();
        }
    }
}