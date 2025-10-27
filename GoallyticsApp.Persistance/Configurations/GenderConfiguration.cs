using GoallyticsApp.Domain.Entities.AuthEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoallyticsApp.Persistance.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.Property(g => g.Definition).IsRequired().HasMaxLength(50);
            builder.HasData(new Gender[]
                {
                new()
                {
                    Definition="Male",
                    Id=1
                },
                new() {
                    Definition="Female",
                    Id=2
                },
                new() {
                    Definition="Prefer not to say",
                    Id=3
                }
            });
        }
    }
}
