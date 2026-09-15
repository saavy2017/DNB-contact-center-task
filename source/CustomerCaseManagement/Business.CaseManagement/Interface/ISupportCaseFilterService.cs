using Application.CaseManagement.DTO;
using Domain.CaseManagement.Entity;

namespace Application.CaseManagement.Interface
{
    public interface ISupportCaseFilterService
    {
        Task<List<SupportCaseResponse>> Search(SupportCaseFilterRequest request);
    }
}
