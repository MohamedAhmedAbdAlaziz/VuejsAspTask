namespace API.Extensions
{
    public static class AuthorizationPolicyServices
    {
        public static IServiceCollection AddAuthorizationPolicyServices(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("NormalPolicy", policy => policy.RequireRole("Normal"));
            });
        return services;
        }
    }
}
