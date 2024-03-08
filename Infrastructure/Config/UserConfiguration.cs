using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
       public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName).IsRequired();
            builder.Property(p => p.LastName).IsRequired();
            builder.HasMany(e => e.Employees)
           .WithOne(e => e.User)
          .HasForeignKey(e => e.CreatedBy);
        }
    }
}
