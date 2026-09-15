using Business.CaseManagement.Interface;
using Domain.CaseManagement.Entity;

namespace Business.CaseManagement.Service
{
    public class SupportCaseService : ISupportCase
    {
        private readonly ISupportCase _supportCaseRepository;
        public SupportCaseService(ISupportCase supportCaseRepository)
        {
            _supportCaseRepository = supportCaseRepository;
        }
        public async Task<SupportCase> AddAsync(SupportCase supportCase)
        {
            return await _supportCaseRepository.AddAsync(supportCase);
        }

        public async Task<SupportCase> GetByIdAsync(int id)
        {
            return await _supportCaseRepository.GetByIdAsync(id);   
        }

        public async Task<List<SupportCase>> GetAllAsync()
        {
            return await _supportCaseRepository.GetAllAsync();
        }

        public async Task<SupportCase> UpdateAsync(SupportCase supportCase)
        {
            return await _supportCaseRepository.UpdateAsync(supportCase);
        }

        public async Task<List<SupportCase>> Search(int id)
        {
            return await _supportCaseRepository.Search(id);
        }
    }
}
