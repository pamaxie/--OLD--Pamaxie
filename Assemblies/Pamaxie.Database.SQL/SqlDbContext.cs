using System;
using Microsoft.EntityFrameworkCore;
using Pamaxie.Data;

namespace Pamaxie.Database.Sql
{
    public class SqlDbContext : DbContext
    {
        //Tables of the Registered Users and their settings
        public virtual DbSet<User> Users { get; set; }
        
        //Tables of all our Registered Applications
        public DbSet<Application> Applications { get; set; }

        //Tables related to things for URL Filters.
        public DbSet<AdvertisingUrl> AdvertisingUrls { get; set; }
        public DbSet<AgressiveUrl> AgressiveUrls { get; set; }
        public DbSet<BankingUrl> BankingUrls { get; set; }
        public DbSet<BitcoinUrl> BitcoinUrls { get; set; }
        public DbSet<CryptoJackingUrl> CryptoJackingUrls { get; set; }
        public DbSet<DDosUrl> DDosUrls { get; set; }
        public DbSet<DrugUrl> DrugUrls { get; set; }
        public DbSet<GamblingUrl> GamblingUrls { get; set; }
        public DbSet<HackingUrl> HackingUrls { get; set; }
        public DbSet<MarketingUrl> MarketingUrls { get; set; }
        public DbSet<MixedAdultUrl> MixedAdultUrls { get; set; }
        public DbSet<PhishingUrl> PhishingUrls { get; set; }
        public DbSet<PornographicUrl> PornographicUrls { get; set; }
        public DbSet<RedirectorUrl> RedirectorUrls { get; set; }
        public DbSet<WarezUrl> WarezUrls { get; set; }


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