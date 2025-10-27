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
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.Property(r => r.Definition).IsRequired().HasMaxLength(100);
            builder.HasData(new AppRole[]
                {
                new AppRole
                {
                    Id=1,
                    Definition="Admin"
                },
                new AppRole
                {
                    Id=2,
                    Definition="Member"
                }
            });
        }
    }
}
