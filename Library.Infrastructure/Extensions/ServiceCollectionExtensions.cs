using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Library.Application.DTOs.Books;
using Library.Application.DTOs.Categories;
using Library.Application.DTOs.Genres;
using Library.Infrastructure.ExternalServices;
using Library.Infrastructure.Configurations;
using Library.Infrastructure.Implementations;

namespace Library.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
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
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IChatRepository,ChatRepository>();
            return services;
        }

        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthorMappingProfile).Assembly);
            services.AddAutoMapper(typeof(BookMappingProfile).Assembly);
            services.AddAutoMapper(typeof(CategoryMappingProfile).Assembly);
            services.AddAutoMapper(typeof(GenreMappingProfile).Assembly);
            return services;
        }

        public static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateAuthorValidator>();
            return services;
        }

        public static IServiceCollection ConfigureAuthService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();
            services.Configure<GoogleConfigurations>(configuration.GetSection(GoogleConfigurations.SectionName));
            services.AddHttpClient<IGoogleService,GoogleService>(client =>
            {
                client.BaseAddress = new Uri(configuration[$"{GoogleConfigurations.SectionName}:TokenUrl"]);
            });
            return services;
        }
        
    }
}
