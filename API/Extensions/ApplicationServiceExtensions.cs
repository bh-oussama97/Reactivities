using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(typeof(List.Handler).Assembly);


            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            //configuration for user accessor 
            //you can access the current user from any place in application and get informations
            services.AddScoped<IUserAccessor, UserAccessor>();

            //adding scoped for service photo accessor
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();


            //configuration for cloudinary api 
            // getting the configuration section name from the AppSettings.json
            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));


            return services;
        }
    }
}
