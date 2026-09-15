using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;

namespace Infra.CaseManagement.Repositories
{
    public class SupportCaseFilterRepository : ISupportCaseFilterRepository
    {
        private readonly CaseDbContext _context;
        public SupportCaseFilterRepository(CaseDbContext context)
        {
            _context = context;
        }
        public Task<List<SupportCase>> Search(SupportCase request)
        {
            //Search with necessary fields and can be extended later

            var result = _context.SupportCase.AsQueryable().Where(x => (request.ReferenceNumber == 0 || x.ReferenceNumber == request.ReferenceNumber) &&
                        (string.IsNullOrEmpty(request.CustomerEmail) || x.CustomerEmail.Contains(request.CustomerEmail)) &&
                        (string.IsNullOrEmpty(request.Priority) || x.Priority == request.Priority) &&
                        (string.IsNullOrEmpty(request.Status) || x.Status == request.Status))
                .Select(x => new SupportCase
                {
                    ReferenceNumber = x.ReferenceNumber,
                    CustomerName = x.CustomerName,
                    CustomerEmail = x.CustomerEmail,
                    Subject = x.Subject,
                    Description = x.Description,
                    Priority = x.Priority,
                    Status = x.Status
                }).ToList();
            return Task.FromResult(result);
        }
    }
}
