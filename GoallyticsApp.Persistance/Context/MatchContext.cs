using GoallyticsApp.Domain.Entities.AuthEntities;
using GoallyticsApp.Domain.Entities.MatchEntities;
using GoallyticsApp.Persistance.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Context
{
    public class MatchContext : DbContext
    {
        public MatchContext(DbContextOptions<MatchContext> options) : base(options)
        {
        }
        public MatchContext()
        {
            
        }
        //MatchSet
        public virtual DbSet<Fixtures> Fixtures { get; set; }
        public virtual DbSet<Leagues> Leagues { get; set; }
        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<FixtureStat> FixtureStat { get; set; }
        //UserSet
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<AppRole> AppRoles { get; set; }
        public virtual DbSet<AppUserRoles> AppUserRoles { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        //Prediction
        public virtual DbSet<Prediction> Predictions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 

                optionsBuilder.UseSqlServer("Server=DESKTOP-D657JCR\\SQLEXPRESS;Database=GoallyticsApp;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Match
            modelBuilder.ApplyConfiguration(new FixturesConfiguration());
            modelBuilder.ApplyConfiguration(new LeaguesConfiguration());
            modelBuilder.ApplyConfiguration(new RoundConfiguration());
            modelBuilder.ApplyConfiguration(new SeasonConfiguration());
            modelBuilder.ApplyConfiguration(new TeamsConfiguration());
            modelBuilder.ApplyConfiguration(new FixtureStatConfiguration());

            //User
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new GenderConfiguration());
            base.OnModelCreating(modelBuilder);

        }
    }
    
    
    
}
