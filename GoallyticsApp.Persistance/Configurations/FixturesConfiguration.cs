using GoallyticsApp.Domain.Entities.MatchEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Configurations
{
    public class FixturesConfiguration : IEntityTypeConfiguration<Fixtures>
    {
        public void Configure(EntityTypeBuilder<Fixtures> builder)
        {
            builder.HasKey(f => f.Id);
            builder.HasAlternateKey(l => l.FixturesId);
            builder.Property(f => f.AwayScore).IsRequired(false).HasDefaultValue(null);
            builder.Property(f => f.HomeScore).IsRequired(false).HasDefaultValue(null);
            builder.HasOne(f => f.League).WithMany(l => l.Fixtures).HasForeignKey(f => f.LeagueApiId).HasPrincipalKey(f => f.LeagueApiId);
            builder.HasOne(f => f.Season).WithMany(s => s.Fixtures).HasForeignKey(f => f.SeasonApiId).HasPrincipalKey(f => f.SeasonApiId);
            builder.HasOne(f => f.Round).WithMany(r => r.Fixtures).HasForeignKey(f => f.RoundApiId).HasPrincipalKey(f => f.RoundApiId);
            builder.HasOne(f=>f.HomeTeam).WithMany(t=>t.HomeFixtures).HasForeignKey(f=>f.HomeTeamId).HasPrincipalKey(f => f.TeamApiId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f=>f.AwayTeam).WithMany(t=>t.AwayFixtures).HasForeignKey(f=>f.AwayTeamId).HasPrincipalKey(f => f.TeamApiId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(f => f.Predictions)
            .WithOne(p => p.Fixtures)
            .HasForeignKey(p => p.FixtureId)
            .HasPrincipalKey(f => f.FixturesId)
            .OnDelete(DeleteBehavior.Cascade);
                }
    }
}
