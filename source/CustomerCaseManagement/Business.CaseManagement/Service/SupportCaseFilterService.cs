using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Microsoft.Extensions.Logging;

namespace Application.CaseManagement.Service
{
    public class SupportCaseFilterService : ISupportCaseFilterService
    {
        private readonly ISupportCaseFilterRepository _supportCaseFilterRepository;
        private readonly ILogger<SupportCaseFilterService> _logger;
        public SupportCaseFilterService(ISupportCaseFilterRepository supportCaseFilterRepository, ILogger<SupportCaseFilterService> logger)
        {
            _supportCaseFilterRepository = supportCaseFilterRepository;
            _logger = logger;
        }

        public async Task<List<SupportCaseResponse>> Search(SupportCaseFilterRequest request)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Search: An error occurred while searching for support cases.");
                throw;
            }
        }
    }
}
