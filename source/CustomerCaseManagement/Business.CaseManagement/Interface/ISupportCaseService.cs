using Application.CaseManagement.DTO;
using Domain.CaseManagement.Entity;

namespace Application.CaseManagement.Interface
{
    public interface ISupportCaseService
    {
        Task<SupportCaseResponse> AddAsync(SupportCaseRequest supportCase);
        Task<SupportCaseResponse> GetByIdAsync(int id);
        Task<List<SupportCaseResponse>> GetAllAsync();
        Task<SupportCaseResponse> UpdateAsync(SupportCaseRequest supportCase);
        Task<SupportCaseResponse> GetByRefIdAsync(int refId);
    }
}
