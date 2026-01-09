using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Infrastructure.Persistence.Services;

namespace SchoolManagementSystem.Infrastructure.DependencyContainers;
public class ServiceDependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        //services.AddMediatR(typeof(Program).Assembly); // Adjust the assembly as needed
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IPagedService, PagedService>();
    }
}
