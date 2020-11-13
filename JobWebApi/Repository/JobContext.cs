using System;
using System.Text.RegularExpressions;
using JobWebApi.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.EntityFrameworkCore.Extensions;

namespace JobWebApi.Repository
{
    public class JobContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<JobDetail> JobDetails { get; set; }

        public JobContext()
        {
            var rawConnectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            //We will get this
            //Database=localdb;Data Source=127.0.0.1:50754;User Id=azure;Password=6#vWHD_$
            //Need change to this 
            //"Server=127.0.0.1; Port=50726; Database=localdb; Uid=azure; Pwd=6#vWHD_$; Character Set=utf8"
            _connectionString = rawConnectionString
                .Replace("Data Source=127.0.0.1:", "Port=")
                .Replace("User Id", "Uid")
                .Replace("Password", "Pwd") + ";Server=127.0.0.1; Character Set=utf8";
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

        private string GetMySqlConnectionString()
        {
            var rawConnectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb") 
                                      ?? throw new NullReferenceException("MYSQLCONNSTR_localdb Parameter is null !" +
                                                                          "Please Check MySql In Azure is Open !");
            //We will get this
            //Database=localdb;Data Source=127.0.0.1:50754;User Id=azure;Password=6#vWHD_$
            //Need change to this 
            //"Server=127.0.0.1; Port=50726; Database=localdb; Uid=azure; Pwd=6#vWHD_$; Character Set=utf8"
            return rawConnectionString
                .Replace("Data Source=127.0.0.1:", "Port=")
                .Replace("User Id", "Uid")
                .Replace("Password", "Pwd") + ";Server=127.0.0.1; Character Set=utf8";
        }
    }
}