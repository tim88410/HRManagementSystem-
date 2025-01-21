using HRManagementSystem.Infrastructure.Repositories.Leaves;
using Microsoft.Extensions.DependencyInjection;

namespace HRManagementSystem.Infrastructure
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddScoped<ILeaveActionLogRepository, LeaveActionLogRepository>();
            services.AddScoped<ILeaveAggregateRepository, LeaveAggregateRepository>();
            services.AddScoped<ILeavesQueryRepository, LeavesQueryRepository>();
            return services;
        }
    }
}
