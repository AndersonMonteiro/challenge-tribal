using Microsoft.EntityFrameworkCore;
using Tribal.CreditLineRequests.Domain.CreditLines.Entities;

namespace Tribal.CreditLine.Data
{
    public class DataContext : DbContext
    {
        public DbSet<CreditLineRequest> CreditLineRequests { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }
}
