using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Infrastructure.DependencyContainers;
public class ContextDependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        //services.AddScoped<IQmsDbContext, QmsDbContext>();
        //services.AddScoped<QmsDbContext, QmsDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}