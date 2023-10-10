using Microsoft.EntityFrameworkCore;

namespace SAMS.Data
{
    public class SAMSDbContext : SAMSEntities
    {
        public SAMSDbContext(DbContextOptions<SAMSDbContext> options) : base(options) { }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
