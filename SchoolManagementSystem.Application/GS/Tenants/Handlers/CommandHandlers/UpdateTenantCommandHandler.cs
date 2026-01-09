using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Tenants.Commands;
using SchoolManagementSystem.Application.GS.Tenants.FluentValidations;
using SchoolManagementSystem.Application.GS.Tenants.Models;
using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Tenants.Handlers.CommandHandlers;
public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, IResult>
{
    private readonly IUnitOfWork _unitOfWork;
    ICurrentUserService _currentUserService;
    public UpdateTenantCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService
        )
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<IResult> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var validation = await new UpdateTenantCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return Result.Fail<string>(StatusCodes.Status400BadRequest, validation.Errors);
            var tenantReq = request.Tenant;
            if (tenantReq == null)
                return Result.Fail<TenantResponse>(StatusCodes.Status400BadRequest, "Tenant data is required.");
            var tenantEntity = await _unitOfWork.TenantRepository.GetSingleNoneDeletedAsync(x => x.Id == tenantReq.Id);
            if (tenantEntity == null)
                return Result.Fail<TenantResponse>(StatusCodes.Status404NotFound, "Tenant not found.");
            var userRequest = tenantReq.TenantUserList?.FirstOrDefault();
            if (userRequest == null)
                return Result.Fail<TenantResponse>(StatusCodes.Status400BadRequest, "Tenant must have at least one user.");
            // Role-specific branching
            if (_currentUserService.Roles!.Contains(Guid.Empty))//_fieldValue.SuperAdmin_Role_ID
                await UpdateTenantAsSuperAdminAsync(tenantReq, tenantEntity, userRequest);
            else if (_currentUserService.Roles.Contains(Guid.Empty))//_fieldValue.TenantAdmin_Role_ID
                await UpdateTenantAsTenantAdminAsync(tenantReq, tenantEntity, userRequest);
            await _unitOfWork.TenantRepository.UpdateAsync(tenantEntity);
            await _unitOfWork.CommitAsync();
            var response = tenantEntity.Adapt<TenantResponse>();
            return Result.Success(response, "Tenant " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            // LogHelpers.Error(ex.InnerException.Message);
            return Result.Fail<TenantResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private async Task UpdateTenantAsSuperAdminAsync(TenantRequest request, Tenant tenant, UserRequest userRequest)
    {
        if (await EmailExistsAsync(request.TenantEmail, request.Id))
            throw new ApplicationException("Email already exists");

        tenant.TenantEmail = request.TenantEmail;

        await UpdateUserAsync(userRequest);
        UpdateTenantBasicInfo(request, tenant);
        
    }

    private async Task UpdateTenantAsTenantAdminAsync(TenantRequest request, Tenant tenant, UserRequest userRequest)
    {
        await UpdateUserAsync(userRequest);
        UpdateTenantBasicInfo(request, tenant);
       
    }

    private void UpdateTenantBasicInfo(TenantRequest request, Tenant tenant)
    {
        tenant.PhoneNumber = request.PhoneNumber;
        tenant.BinNo = request.BinNo;
       // tenant.TenantPath = request.TenantPath;
        tenant.PostCode = request.PostCode;
        tenant.Street = request.Street;
        tenant.City = request.City;
        tenant.Province = request.Province;
        //tenant.CountryId = request.CountryId;
    }

    private async Task UpdateUserAsync(UserRequest userRequest)
    {
        // Get all users under the tenant
        var existingUsersList = await _unitOfWork.UserRepository.GetAllNoneDeleted(false, true)
            .Where(x => x.TenantId == userRequest.TenantId)
            .ToListAsync();

        if (existingUsersList == null || existingUsersList.Count == 0)
            throw new ApplicationException("No users found for this tenant");

       
        // Update all users under tenant (2FA settings + Authentication Mode)
        foreach (var user in existingUsersList)
        {
            if (user.Id == userRequest.Id)
            {
                // Update this specific user's phone number
                user.PhoneNumber = userRequest.PhoneNumber;
            }
            
            await _unitOfWork.UserRepository.UpdateAsync(user);
        }
        
    }


    private async Task<bool> EmailExistsAsync(string email, Guid tenantId)
    {
        return await _unitOfWork.TenantRepository.GetAll()
        .AnyAsync(x => x.TenantEmail.ToLower() == email.ToLower() && !x.IsDeleted && x.Id != tenantId);
    }

}

