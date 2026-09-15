using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;

namespace Application.CaseManagement.Service
{
    public class SupportCaseFilterService : ISupportCaseFilterService
    {
        private readonly ISupportCaseFilterRepository _supportCaseFilterRepository;
        public SupportCaseFilterService(ISupportCaseFilterRepository supportCaseFilterRepository)
        {
            _supportCaseFilterRepository = supportCaseFilterRepository;
        }

        public async Task<List<SupportCaseResponse>> Search(SupportCaseFilterRequest request)
        {
            var supportCase = new SupportCase
            {
                ReferenceNumber = request?.ReferenceNumber ?? 0,
                CustomerEmail = request?.CustomerEmail,
                Priority = request?.Priority,
                Status = request?.Status
            };
            var supportCases = await _supportCaseFilterRepository.Search(supportCase);
            return supportCases.Select(sc => new SupportCaseResponse
            {
                ReferenceNumber = sc.ReferenceNumber,
                CustomerName = sc.CustomerName,
                CustomerEmail = sc.CustomerEmail,
                Subject = sc.Subject,
                Description = sc.Description,
                Priority = sc.Priority,
                Status = sc.Status
            }).ToList();
        }

    }
}
