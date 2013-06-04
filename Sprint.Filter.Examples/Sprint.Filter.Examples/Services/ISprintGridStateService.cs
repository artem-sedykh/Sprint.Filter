using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sprint.Filter.Examples.Models;

namespace Sprint.Filter.Examples.Services
{
    public interface ISprintGridStateService
    {
        IQueryable<SavedFilter> GetStates(string key);

        void Create(GridStateView model);

        ActionGridOptions GetActionGridOptions(int id, string gridKey);

        void Delete(int stateId, string gridKey);
    }
}