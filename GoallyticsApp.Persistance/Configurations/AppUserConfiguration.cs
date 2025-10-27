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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.DateOfBirth).IsRequired();
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasOne(u => u.Gender).WithMany(g => g.AppUsers).HasForeignKey(u => u.GenderId);
        }
    }
}
