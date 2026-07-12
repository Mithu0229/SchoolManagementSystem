using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Application.GS.Divisions.Repositories;
using SchoolManagementSystem.Application.GS.Users.Repositories;
using SchoolManagementSystem.Application.School.AcademicClasses.Repositories;
using SchoolManagementSystem.Application.School.AcademicSessions.Repositories;
using SchoolManagementSystem.Application.School.Admissions.Repositories;
using SchoolManagementSystem.Application.School.Branches.Repositories;
using SchoolManagementSystem.Application.School.BillMasters.Repositories;
using SchoolManagementSystem.Application.School.FeeCollections.Repositories;
using SchoolManagementSystem.Application.School.FeeHeads.Repositories;
using SchoolManagementSystem.Application.School.FeeTemplates.Repositories;
using SchoolManagementSystem.Application.School.FinancialYears.Repositories;
using SchoolManagementSystem.Application.School.Institutes.Repositories;
using SchoolManagementSystem.Application.School.SchoolStudents.Repositories;
using SchoolManagementSystem.Application.School.Sections.Repositories;
using SchoolManagementSystem.Application.School.Shifts.Repositories;
using SchoolManagementSystem.Application.School.StudentFeeLedgers.Repositories;
using SchoolManagementSystem.Application.School.StudentGroups.Repositories;
using SchoolManagementSystem.Application.School.AttendanceDevices.Repositories;
using SchoolManagementSystem.Infrastructure.Repositories;
using SchoolManagementSystem.Infrastructure.Repositories.Schools;

namespace SchoolManagementSystem.Infrastructure.DependencyContainers;
public class RepositoryDependencyContainer
{

    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IDivisionRepository, DivisionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInstituteRepository, InstituteRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IFinancialYearRepository, FinancialYearRepository>();
        services.AddScoped<IAcademicSessionRepository, AcademicSessionRepository>();
        services.AddScoped<IAcademicClassRepository, AcademicClassRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
        services.AddScoped<IFeeHeadRepository, FeeHeadRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IAdmissionRepository, AdmissionRepository>();
        services.AddScoped<IFeeTemplateRepository, FeeTemplateRepository>();
        services.AddScoped<IStudentFeeLedgerRepository, StudentFeeLedgerRepository>();
        services.AddScoped<IFeeCollectionRepository, FeeCollectionRepository>();
        services.AddScoped<IBillMasterRepository, BillMasterRepository>();
        services.AddScoped<IAttendanceDeviceRepository, AttendanceDeviceRepository>();

    }
}
