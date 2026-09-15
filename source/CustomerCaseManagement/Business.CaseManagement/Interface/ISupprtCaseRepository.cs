using Domain.CaseManagement.Entity;

namespace Application.CaseManagement.Interface
{
    public interface ISupportCaseRepository
    {
        Task<SupportCase> AddAsync(SupportCase supportCase);
        Task<SupportCase> GetByIdAsync(int id);
        Task<List<SupportCase>> GetAllAsync();
        Task<SupportCase> UpdateAsync(SupportCase supportCase);
        Task<SupportCase> GetByRefIdAsync(int refId);
    }
}
