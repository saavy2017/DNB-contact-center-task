using Domain.CaseManagement.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.CaseManagement.Interface
{
    public interface ISupportCaseRepository
    {
        Task<SupportCase> AddAsync(SupportCase supportCase);
        Task<SupportCase> GetByIdAsync(int id);
        Task<List<SupportCase>> GetAllAsync();
        Task<SupportCase> UpdateAsync(SupportCase supportCase);
        Task<List<SupportCase>> Search(int id);
    }
}
