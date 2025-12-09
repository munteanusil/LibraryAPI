using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace Library.Infrastructure.Extensions
{
    public static class SystemCollectionExtensions
    {

        public static IServiceCollection ConfigureEFCore(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<LibraryContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("lib_db"));
            });
            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository,BookRepository>();
            services.AddScoped<IAuthorRepository,AuthorRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            services.AddScoped<IGenreRepository,GenreRepository>();

            return services;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthorMappingProfile).Assembly);
            return services;
        }

        public static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateAuthorValidator>();
            return services;
        }
    }
}
