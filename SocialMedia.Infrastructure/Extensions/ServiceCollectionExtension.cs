using System;
using System.IO;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialMedia.Domain.CustomEntities;
using SocialMedia.Domain.Interfaces;
using SocialMedia.Domain.Services;
using SocialMedia.Infrastructure.Data;
using SocialMedia.Infrastructure.Filters;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Services;

namespace SocialMedia.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<RestFullApiContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("SocialMedia"));
            });
            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<PaginationConfiguration>(Configuration.GetSection("Pagination"));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWOrk, UnitOfWork>();
            services.AddSingleton<IUriService>(provider => {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUrl = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriServices(absoluteUrl);
            });
            return services;
        }

        public static IServiceCollection AddFilters(this IServiceCollection services)
        {
            services.AddMvc(options => {
                options.Filters.Add<ValidationFilters>();
            }).AddFluentValidation(options => {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

            services.AddControllers(options => {
                options.Filters.Add<GlobalExceptionFilter>();
            })
            .AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFile)
        {
             services.AddSwaggerGen(options =>{
                options.SwaggerDoc("V1", new OpenApiInfo() {Title = "Social Media", Version = "V1"});
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}