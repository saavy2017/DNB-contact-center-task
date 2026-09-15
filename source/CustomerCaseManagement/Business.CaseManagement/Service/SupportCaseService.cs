using Business.CaseManagement.Interface;
using Domain.CaseManagement.Entity;

namespace Business.CaseManagement.Service
{
    public class SupportCaseService : ISupportCase
    {
        public async Task<SupportCase> AddAsync(SupportCase supportCase)
        {
            throw new NotImplementedException();
        }

        public async Task<SupportCase> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SupportCase>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<SupportCase> UpdateAsync(SupportCase supportCase)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SupportCase>> Search(int id)
        {
            throw new NotImplementedException();
        }
    }
}
