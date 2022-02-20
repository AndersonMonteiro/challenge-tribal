using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fintech.CreditLineRequests.Domain.CreditLines.Entities;
using Fintech.CreditLineRequests.Domain.CreditLines.Enums;
using Fintech.CreditLineRequests.Domain.CreditLines.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fintech.CreditLine.Data.Repositories
{
    public class CreditLineRequestRepository : ICreditLineRequestRepository
    {
        private DataContext _context;
        
        public CreditLineRequestRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Create(CreditLineRequest creditLineRequest)
        {
            await _context.CreditLineRequests.AddAsync(creditLineRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CreditLineRequest>> GetByRequestByInterval(DateTime initialDateTime)
        {
            return await _context.CreditLineRequests.Where(cl => cl.CreatedDate >= initialDateTime).ToListAsync();
        }

        public async Task<List<CreditLineRequest>> GetByStatus(CreditLineRequestsStatus status)
        {
            return await _context.CreditLineRequests.Where(cl => cl.Status == status).ToListAsync();
        }
    }
}
