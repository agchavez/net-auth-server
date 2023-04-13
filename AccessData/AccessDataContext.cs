using AccessData.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessData
{
    public class AccessDataContext: DbContext
    {

        public AccessDataContext(DbContextOptions<AccessDataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Access> Accesses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Access>().ToTable("user_access");
            modelBuilder.Entity<User>().ToTable("user");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Access>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accesses)
                .HasForeignKey(a => a.UserId);
        }

    }
}