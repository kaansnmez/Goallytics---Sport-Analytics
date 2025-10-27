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
    public class FixtureStatConfiguration : IEntityTypeConfiguration<FixtureStat>
    {
        public void Configure(EntityTypeBuilder<FixtureStat> builder)
        {
            builder.HasKey(f => f.Id);
            builder.HasIndex(f => new { f.FixtureId, f.TeamId }).IsUnique();
            builder.Property(f => f.AwayScore).IsRequired(false).HasDefaultValue(null);
            builder.Property(f => f.HomeScore).IsRequired(false).HasDefaultValue(null);
            builder.HasOne(f => f.Fixtures).WithMany(t =>t.FStats ).HasForeignKey(f => new { f.FixtureId, f.HomeTeamId }).HasPrincipalKey(f => new { f.FixturesId, f.HomeTeamId });





        }
    }
}
