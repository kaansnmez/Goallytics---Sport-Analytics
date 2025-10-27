using GoallyticsApp.Domain.Entities.MatchEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Configurations
{
    public class LeaguesConfiguration : IEntityTypeConfiguration<Leagues>
    {
        public void Configure(EntityTypeBuilder<Leagues> builder)
        {
            builder.HasKey(l => l.Id);
            builder.HasAlternateKey(l => l.LeagueApiId);
        }
    }
}
