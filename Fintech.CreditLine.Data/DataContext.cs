using Microsoft.EntityFrameworkCore;
using Fintech.CreditLineRequests.Domain.CreditLines.Entities;

namespace Fintech.CreditLine.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base()
        {
        }
        public DbSet<CreditLineRequest> CreditLineRequests { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
