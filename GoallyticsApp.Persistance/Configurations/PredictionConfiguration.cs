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
    public class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
    {
        public void Configure(EntityTypeBuilder<Prediction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Market)
            .IsRequired()
            .HasMaxLength(20);
            builder.HasAlternateKey(p=>p.FixtureId);
            builder.Property(p => p.Forecast)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Confidence)
                .HasPrecision(5, 2); // 99.99

            builder.Property(p => p.ExpectedGoals)
                .HasPrecision(5, 2);

            builder.Property(p => p.HomeExpected)
                .HasPrecision(5, 2);

            builder.Property(p => p.AwayExpected)
                .HasPrecision(5, 2);
            builder.HasIndex(p => new { p.FixtureId, p.Market })
            .IsUnique();
            builder.HasOne(f => f.Fixtures).WithMany(f=>f.Predictions).HasForeignKey(f => f.FixtureId).HasPrincipalKey(f => f.FixturesId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
