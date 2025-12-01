using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data.Configuration
{
    public class GenereConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("genres");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Name)
                   .HasMaxLength(50)
                   .IsRequired();
   

            builder.HasMany(b => b.AuthorGeneres)
                    .WithOne(c => c.Genre)
                    .HasForeignKey( b=> b.GenreId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
