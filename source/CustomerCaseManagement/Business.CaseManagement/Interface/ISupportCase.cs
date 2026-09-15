using Domain.CaseManagement.Entity;

namespace Business.CaseManagement.Interface
{
    public interface ISupportCase
    {
        Task<SupportCase> AddAsync(SupportCase supportCase);
        Task<SupportCase> GetByIdAsync(int id);
        Task<List<SupportCase>> GetAllAsync();
        Task<SupportCase> UpdateAsync(SupportCase supportCase);
        Task<List<SupportCase>> Search(int id);
    }
}
