using System.Linq;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Services
{
    public interface ISimpleStateService
    {
        void Create(SimpleStateGridView model);

        IQueryable<SavedFilter> GetSavedFilters(string key);

        IFilterOptions GetState(int id, string key);

        void Delete(int id, string key);
    }
}