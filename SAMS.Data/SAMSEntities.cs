using Microsoft.EntityFrameworkCore;
using SAMS.Infrastructure.Entities;

namespace SAMS.Data
{
    public class SAMSEntities : DbContext
    {
        public SAMSEntities(DbContextOptions<SAMSDbContext> options) : base(options) { }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }

        //add-migration -Context SAMSDbContext per1312
        //update-database -Context SAMSDbContext
    }
}
