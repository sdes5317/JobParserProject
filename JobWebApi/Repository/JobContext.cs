using JobWebApi.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace JobWebApi.Repository
{
    public class JobContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<JobDetail> JobDetails { get; set; }

        public JobContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySql");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySQL(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobDetail>(builder =>
            {
                builder.ForMySQLHasCollation("utf8_unicode_ci");
                builder.HasKey(jobDetail => jobDetail.Id);
                builder.HasIndex(jobDetail => jobDetail.CreationTime);
                builder.Property(jobDetail => jobDetail.CreationTime);
            });
        }
    }
}