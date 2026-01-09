using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Tenants.Commands;
using SchoolManagementSystem.Application.GS.Tenants.FluentValidations;
using SchoolManagementSystem.Application.GS.Tenants.Models;
using SchoolManagementSystem.Application.GS.Users.Models;
using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.GS.Tenants.Handlers.CommandHandlers;

public class InsertTenantCommandHandler : IHttpRequestHandler<InsertTenantCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public InsertTenantCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService
        )
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<IResult> Handle(InsertTenantCommand request, CancellationToken cancellationToken)
    {
        var validator = await new InsertTenantCommandValidator().ValidateAsync(request, cancellationToken);
        if (validator.IsValid is false)
        {
            return Result.Fail<string>(StatusCodes.Status400BadRequest, validator.Errors);
        }
        try
        {
            if (request is null)
            {
                return Result.Fail<TenantResponse>(StatusCodes.Status406NotAcceptable);
            }

            #region Check User exists or not

            var existingTenant = await _unitOfWork.TenantRepository.GetAllNoneDeleted()
                .Where(x => x.TenantEmail == request.Tenant!.TenantEmail)
                .FirstOrDefaultAsync();

            if (existingTenant != null)
            {
                return Result.Fail<TenantResponse>(StatusCodes.Status409Conflict, "Tenant with this email already exists.");
            }


            #endregion

            request.Tenant!.Id = Guid.NewGuid();

            var userRequest = request.Tenant.TenantUserList.FirstOrDefault();

            var existingUser = await _unitOfWork.UserRepository.GetAllNoneDeleted()
             .Where(x => x.Email == userRequest!.Email)
             .FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return Result.Fail<TenantResponse>(StatusCodes.Status409Conflict, "User with this email already exists.");
            }
            var subscribername = "";
            var subscribermail = "";
            Guid tenantRoleId = new Guid("990FDD19-959F-4986-A889-70FAAC5D7CD4");
            var userRoleList = new List<UserRoleRequest>{
                new UserRoleRequest()
                {
                    Id=Guid.NewGuid(),
                    RoleId = tenantRoleId,//_fieldValue.TenantAdmin_Role_ID,
                    UserId = Guid.Empty,
                    TenantId = request.Tenant.Id,
                    IsActive = true
                }
            };

            var users = new List<UserRequest>
            {
                   new UserRequest() {
                    Id=Guid.NewGuid(),
                    FirstName = userRequest!.FirstName,
                    LastName = userRequest!.LastName,
                    Email=userRequest!.Email,
                    PhoneNumber = userRequest!.PhoneNumber,
                    Password = BCrypt.Net.BCrypt.HashPassword("TenantAdmin123"),//BCrypt.Net.BCrypt.HashPassword(GenerateRandomNumbers.RandomNumbers(6)),
                    TenantId=userRequest.TenantId,
                    UserType= UserTypes.Admin,
                    IsActive = true,
                    UserRoleList=userRoleList,
                },
            };


            request.Tenant.TenantUserList.Clear();
            request.Tenant.TenantUserList = users;

            var tenant = request.Tenant.Adapt<Tenant>();
            subscribermail = tenant.TenantEmail;
            subscribername = tenant.TenantName;
            tenant.IsActive = true;
            await _unitOfWork.TenantRepository.AddAsync(tenant);

            await _unitOfWork.CommitAsync();


            var response = tenant.Adapt<TenantResponse>();
            return Result.Success(response, "Tenant " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            //LogHelpers.Error(ex.Message);
            return Result.Fail<TenantResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
