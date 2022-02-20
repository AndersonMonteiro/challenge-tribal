using Microsoft.EntityFrameworkCore;
using Tribal.CreditLineRequests.Domain.CreditLines.Entities;

namespace Tribal.CreditLine.Data
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"User ID=postgres;Password=123qwe!;Server=creditline-db;Port=5432;Database=creditline;Integrated Security=true;Pooling=true;");
        }
    }
}
