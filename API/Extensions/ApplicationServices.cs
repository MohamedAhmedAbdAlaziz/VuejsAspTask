using API.Helpers;
using API.Services;
using Core.Interfaces;
using Infrastructure.Data;

namespace API.Extensions;
   
        public static class ApplicationServices
        {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
            

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<JWTService>();
        services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddAutoMapper(typeof(MappingProfiles));
      
        return services;
            }
        }
    

