using Microsoft.EntityFrameworkCore;
using testapi.Model;

namespace testapi.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;

        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSalary> UserSalaries { get; set; }
        public virtual DbSet<UserJobInfo> UserJobInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    _config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure()
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<User>().ToTable("Users", "TutorialAppSchema").HasKey(u => u.UserId);

            modelBuilder
                .Entity<UserSalary>()
                .ToTable("UserSalary", "TutorialAppSchema")
                .HasKey(u => u.UserId);

            modelBuilder
                .Entity<UserJobInfo>()
                .ToTable("UserJobInfo", "TutorialAppSchema")
                .HasKey(u => u.UserId);
        }
    }
}
