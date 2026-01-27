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
    public class AuthorGenreConfiguration : IEntityTypeConfiguration<AuthorGenres>
    {
        public void Configure(EntityTypeBuilder<AuthorGenres> builder)
        {
            builder.ToTable("author_genres");
            builder.HasKey(p => new { p.AuthorId, p.GenreId });

            builder.HasOne(p => p.Author)
                .WithMany(b => b.AuthorGenres)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Genre)
                .WithMany(b => b.AuthorGeneres)
                .HasForeignKey(b => b.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
