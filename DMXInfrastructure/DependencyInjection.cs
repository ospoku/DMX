using DMXInfrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DMXApplication.Interfaces;
using DMXInfrastructure.Repositories;

namespace DMXInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<XContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(
                        "DMX")));
            services.AddScoped<IFeeRepository, FeeRepository>();
             services.AddScoped<IEntityPersistence,EntityPersistenceRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAllowanceRepository, AllowanceRepository>();

            return services;
        }
    }
}