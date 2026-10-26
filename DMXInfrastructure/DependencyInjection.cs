using DMXInfrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DMXInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AuditableIdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(
                        "DefaultConnection")));

            return services;
        }
    }
}