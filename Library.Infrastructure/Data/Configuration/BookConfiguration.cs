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
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("books");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
            builder.Property(p => p.Title)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(p => p.ISBN)
                 .HasMaxLength(50)
                 .IsRequired();

            builder.Property(p => p.Stock)
                  .IsRequired();

            builder.HasOne(p => p.Author)
                  .WithMany(b => b.Books)
                  .HasForeignKey(b => b.AuthorId)
                  .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Category)
                    .WithMany(c => c.Books)
                    .OnDelete(DeleteBehavior.Restrict);
  
        }
         
    }
}
