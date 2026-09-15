using Business.CaseManagement.Interface;
using Domain.CaseManagement.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infra.CaseManagement.Repositories
{
    public class SupportCaseRepository : ISupportCase
    {
        private readonly CaseDbContext _context;
        public SupportCaseRepository(CaseDbContext context)
        {
            _context = context;
        }
        public async Task<SupportCase> AddAsync(SupportCase supportCase)
        {
            await _context.SupportCase.AddAsync(supportCase);
            await _context.SaveChangesAsync();
            return supportCase;
        }

        public async Task<List<SupportCase>> GetAllAsync()
        {
            return await _context.SupportCase.ToListAsync();
        }

        public async Task<SupportCase> GetByIdAsync(int id)
        {
            return await _context.SupportCase.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<SupportCase>> Search(int id)
        {
            return await _context.SupportCase.Where(s => s.Id == id).ToListAsync();
        }

        public async Task<SupportCase> UpdateAsync(SupportCase supportCase)
        {
            _context.SupportCase.Update(supportCase);
            await _context.SaveChangesAsync();
            return supportCase;
        }
    }
}
