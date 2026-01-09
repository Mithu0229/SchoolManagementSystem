using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Infrastructure.Repositories;

namespace SchoolManagementSystem.Infrastructure.DependencyContainers;
public class RepositoryDependencyContainer
{

    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IDivisionRepository, DivisionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

    }
}
