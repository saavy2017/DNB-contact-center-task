using Application.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infra.CaseManagement.Repositories
{
    public class SupportCaseRepository : ISupportCaseRepository
    {
        private readonly CaseDbContext _context;
        public SupportCaseRepository(CaseDbContext context)
        {
            _context = context;
        }
        public async Task<SupportCase> AddAsync(SupportCase supportCase)
        {
            var maxId = await _context.SupportCase.MaxAsync(x => (int?)x.Id) ?? 0;
            supportCase.ReferenceNumber = maxId + 1;
            await _context.SupportCase.AddAsync(supportCase);
            await _context.SaveChangesAsync();
            return supportCase;
        }

        public async Task<List<SupportCase>> GetAllAsync()
        {
            return await _context.SupportCase.AsNoTracking().OrderBy(s => s.ReferenceNumber).ToListAsync();
        }

        public async Task<SupportCase> GetByIdAsync(int id)
        {
            return await _context.SupportCase.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<SupportCase> GetByRefIdAsync(int refId)
        {
            return await _context.SupportCase.AsNoTracking().FirstOrDefaultAsync(s => s.ReferenceNumber == refId);
        }

        public async Task<List<SupportCase>> Search(int id)
        {
            return await _context.SupportCase.AsNoTracking().Where(s => s.Id == id).ToListAsync();
        }

        public async Task<SupportCase> UpdateAsync(SupportCase supportCase)
        {
            _context.SupportCase.Update(supportCase);
            await _context.SaveChangesAsync();
            return supportCase;
        }
    }
}
