using Application.CaseManagement.DTO;
using Domain.CaseManagement.Entity;

namespace Application.CaseManagement.Interface
{
    public interface ISupportCaseService
    {
        Task<SupportCaseRequest> AddAsync(SupportCaseRequest supportCase);
        Task<SupportCaseRequest> GetByIdAsync(int id);
        Task<List<SupportCaseRequest>> GetAllAsync();
        Task<SupportCaseRequest> UpdateAsync(SupportCaseRequest supportCase);
        Task<List<SupportCaseRequest>> Search(int id);
        Task<SupportCaseRequest> GetByRefIdAsync(int refId);
    }
}
