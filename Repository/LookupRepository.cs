using CSRMGMT.Areas.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CSRMGMT.Repository
{
    public class LookupRepository:ILookupRepository
    {
        private AppdbContext _dbContext;
        public LookupRepository(AppdbContext context)
        {
            this._dbContext = context;
        }
        public async Task<IEnumerable<ListValueModel>> GetClientType()
        {
            return await (from i in _dbContext.LookupMaster
                          where i.IsActive==true &&  i.MasterName=="ClientType"
                          select new ListValueModel { ID = i.Id, Name = i.Name, Description = "" }
                       ).OrderBy(i => i.Name).ToListAsync();
        }
        public async Task<IEnumerable<ListValueModel>> GetProjectCategory()
        {

            return await (from i in _dbContext.LookupMaster
                          where i.IsActive == true && i.MasterName == "ProjectCategory"
                          select new ListValueModel { ID = i.Id, Name = i.Name, Description = "" }
                       ).OrderBy(i => i.Name).ToListAsync();
        }
    }
}
