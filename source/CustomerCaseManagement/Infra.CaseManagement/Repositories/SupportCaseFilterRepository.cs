using Application.CaseManagement.DTO;
using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.CaseManagement.Repositories
{
    public class SupportCaseFilterRepository : ISupportCaseFilterRepository
    {
        private readonly CaseDbContext _context;
        public SupportCaseFilterRepository(CaseDbContext context)
        {
            _context = context;
        }
        public async Task<List<SupportCase>> Search(SupportCase request)
        {
            //Search with necessary fields and can be extended later

            var result = await _context.SupportCase.AsNoTracking().Where(x => (request.ReferenceNumber == 0 || x.ReferenceNumber == request.ReferenceNumber) &&
                        (string.IsNullOrEmpty(request.CustomerEmail) || x.CustomerEmail.ToLower().Contains(request.CustomerEmail.ToLower())) &&
                        (string.IsNullOrEmpty(request.Priority) || x.Priority.ToLower() == request.Priority.ToLower()) &&
                        (string.IsNullOrEmpty(request.Status) || x.Status.ToLower() == request.Status.ToLower())).OrderBy(s => s.ReferenceNumber)
                .Select(x => new SupportCase
                {
                    ReferenceNumber = x.ReferenceNumber,
                    CustomerName = x.CustomerName,
                    CustomerEmail = x.CustomerEmail,
                    Subject = x.Subject,
                    Description = x.Description,
                    Priority = x.Priority,
                    Status = x.Status
                }).ToListAsync();
            return result;
        }
    }
}
