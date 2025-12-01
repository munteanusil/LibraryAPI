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
    public class AuthorGenereConfiguration : IEntityTypeConfiguration<AuthorGeneres>
    {
        public void Configure(EntityTypeBuilder<AuthorGeneres> builder)
        {
            builder.ToTable("author_genres");
            builder.HasKey(p => new { p.AuthorId,p.GenreId});

          
            builder.HasOne(b => b.Author)
                    .WithMany(c => c.AuthorGeneres)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Genre)
                    .WithMany(c => c.AuthorGeneres)
                    .HasForeignKey(b => b.GenreId)
                    .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
