using Microsoft.EntityFrameworkCore;
using System;
using Pamaxie.Data;

namespace Pamaxie.Database.Sql
{
    public class SqlDbContext : DbContext
    {
        //Tables of the Registered Users and their settings
        public DbSet<User> Users { get; set; }

        //Tables of all our Registered Applications
        public DbSet<Application> Applications { get; set; }

        //Tables related to things for URL Filters.
        public DbSet<DomainName> AdvertisingUrls { get; set; }
        public DbSet<DomainName> AgressiveUrls { get; set; }
        public DbSet<DomainName> BankingUrls { get; set; }
        public DbSet<DomainName> BitcoinUrls { get; set; }
        public DbSet<DomainName> CryptoJackingUrls { get; set; }
        public DbSet<DomainName> DDosUrls { get; set; }
        public DbSet<DomainName> DrugUrls { get; set; }
        public DbSet<DomainName> GamblingUrls { get; set; }
        public DbSet<DomainName> HackingUrls { get; set; }
        public DbSet<DomainName> MarketingUrls { get; set; }
        public DbSet<DomainName> MixedAdultUrls { get; set; }
        public DbSet<DomainName> PhishingUrls { get; set; }
        public DbSet<DomainName> PornographicUrls { get; set; }
        public DbSet<DomainName> RedirectorUrls { get; set; }
        public DbSet<DomainName> WarezUrls { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Environment.GetEnvironmentVariable("PamaxieSqlDb") ?? string.Empty);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(k => k.GoogleUserId);
        }
    }
}