using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Models
{
    public class ApplicatonDbContext:DbContext
    {
        public ApplicatonDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
