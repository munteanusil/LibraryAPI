using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Library.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tests
{
    public class GenreRepositoryTests
    {
        [Fact]
        public async Task CreateGenShouldSaveANewGenInDb()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var libContext = new LibraryContext(options);
            IGenreRepository repo = new GenreRepository(libContext);

            var genToAdd = new Genre
            {
                Name = "Test Genre"
            };

            await repo.CreateGenre(genToAdd);

            Assert.True(genToAdd.Id > 0);
            var genInDb = await libContext.Set<Genre>().FindAsync(genToAdd.Id);
            Assert.NotNull(genInDb);
            Assert.Equal("Test Genre", genInDb.Name);
        }

        [Fact]
        public async Task GetGenByIdShouldReturnGenWithAuthorGenres()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var libContext = new LibraryContext(options);

            var gen = new Genre { Name = "Mystery" };
            await libContext.Set<Genre>().AddAsync(gen);

            var author = new Author
            {
                FirstName = "Marin",
                LastName = "Preda",
                Site = "marinPreda.com"
            };
            await libContext.Authors.AddAsync(author);
            await libContext.SaveChangesAsync();

            await libContext.AuthorGenres.AddAsync(new AuthorGenres
            {
                AuthorId = author.Id,
                GenreId = gen.Id
            });
            await libContext.SaveChangesAsync();

            IGenreRepository repo = new GenreRepository(libContext);

            var result = await repo.GetGenreById(gen.Id);

            Assert.NotNull(result);
            Assert.Equal(gen.Id, result.Id);
            Assert.Equal("Mystery", result.Name);
            Assert.NotNull(result.AuthorGeneres);
            Assert.Single(result.AuthorGeneres);
            Assert.Equal(author.Id, result.AuthorGeneres.First().AuthorId);
        }

        [Fact]
        public async Task GetGensShouldReturnPaginatedList()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var libContext = new LibraryContext(options);

            for (int i = 1; i <= 8; i++)
            {
                await libContext.Set<Genre>().AddAsync(new Genre
                {
                    Name = $"Genre {i}"
                });
            }
            await libContext.SaveChangesAsync();

            IGenreRepository repo = new GenreRepository(libContext);

            var page1 = await repo.GetGenres(1, 3);
            var page2 = await repo.GetGenres(2, 3);
            var page3 = await repo.GetGenres(3, 3);

            Assert.Equal(3, page1.Items.Count);
            Assert.Equal(3, page2.Items.Count);
            Assert.Equal(2, page3.Items.Count);
            Assert.Equal(3, page1.TotalPages);
        }

        [Fact]
        public async Task UpdateGenShouldUpdateExistingGen()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var libContext = new LibraryContext(options);

            var gen = new Genre { Name = "Original Name" };
            await libContext.Set<Genre>().AddAsync(gen);
            await libContext.SaveChangesAsync();

            IGenreRepository repo = new GenreRepository(libContext);

            gen.Name = "Updated Name";

            await repo.UpdateGenre(gen);

            var updatedGen = await libContext.Set<Genre>().FindAsync(gen.Id);
            Assert.NotNull(updatedGen);
            Assert.Equal("Updated Name", updatedGen.Name);
        }

        [Fact]
        public async Task DeleteGenShouldRemoveGenFromDb()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var libContext = new LibraryContext(options);

            var gen = new Genre { Name = "Genre to Delete" };
            await libContext.Set<Genre>().AddAsync(gen);
            await libContext.SaveChangesAsync();

            IGenreRepository repo = new GenreRepository(libContext);

            await repo.DeleteGenre(gen.Id);

            var deletedGen = await libContext.Set<Genre>().FindAsync(gen.Id);
            Assert.Null(deletedGen);
        }

        [Fact]
        public async Task DeleteGenShouldThrowIfGenDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var libContext = new LibraryContext(options);
            IGenreRepository repo = new GenreRepository(libContext);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteGenre(999));
        }

    }
}

