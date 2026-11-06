using DMXApplication.Interfaces;
using DMXApplication.Services;
using DMXInfrastructure.Data;
using DMXInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
             services.AddScoped<IEntityRepository,EntityRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAllowanceRepository, AllowanceRepository>();
            services.AddScoped<IMemoService, MemoService>();
            services.AddScoped<IMemoRepository,
     MemoRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            var serviceCollection = services.AddScoped<IEntityService, EntityService>();
            return services;
        }
    }
}