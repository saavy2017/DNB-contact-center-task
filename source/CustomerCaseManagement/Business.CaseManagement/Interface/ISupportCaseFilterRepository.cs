using Application.CaseManagement.DTO;
using Domain.CaseManagement.Entity;

namespace Application.CaseManagement.Interface
{
    public interface ISupportCaseFilterRepository
    {
        Task<List<SupportCase>> Search(SupportCase request);
    
    }
}
