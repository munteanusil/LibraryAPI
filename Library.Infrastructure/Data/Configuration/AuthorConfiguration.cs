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
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("authors");
            builder.HasKey(p =>p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.FirstName)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(p => p.LastName)
                 .HasMaxLength(50)
                 .IsRequired();
            builder.Property(p => p.Site)
                  .HasMaxLength(256)
                  .IsRequired();

            builder.HasMany(p => p.Books)
                  .WithOne(b =>b.Author)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.AuthorGeneres )
                  .WithOne(b => b.Author)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
