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
    public class RoundConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasAlternateKey(r => r.RoundApiId);
           
            builder.HasOne(r => r.League).WithMany(l => l.Rounds).HasForeignKey(r => r.LeagueApiId).HasPrincipalKey(f => f.LeagueApiId);
            builder.HasOne(r => r.Season).WithMany(s => s.Rounds).HasForeignKey(r => r.SeasonApiId).HasPrincipalKey(f => f.SeasonApiId);

        }
    
    }
}
