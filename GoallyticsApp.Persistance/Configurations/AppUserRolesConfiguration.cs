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
    public class AppUserRolesConfiguration : IEntityTypeConfiguration<AppUserRoles>
    {
        public void Configure(EntityTypeBuilder<AppUserRoles> builder)
        {
            // Configure composite primary key
            builder.HasIndex(ur => new { ur.AppUserId, ur.AppRoleId }).IsUnique();
            // Configure relationships
            builder.HasOne(ur => ur.AppUser)
                   .WithMany(u => u.AppUserRoles).HasForeignKey(u => u.AppUserId);
            builder.HasOne(ur => ur.AppRole)
                   .WithMany(u => u.AppUserRoles).HasForeignKey(u => u.AppRoleId);

        }
    }
    
}
