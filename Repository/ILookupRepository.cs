using CSRMGMT.Areas.Admin.ViewModels;

namespace CSRMGMT.Repository
{
    public interface ILookupRepository
    {
        Task<IEnumerable<ListValueModel>> GetClientType();
        Task<IEnumerable<ListValueModel>> GetProjectCategory();
    }
}
