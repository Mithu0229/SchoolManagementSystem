using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.GS.Tenants.Models;
using SchoolManagementSystem.Application.GS.Tenants.Queries;
using SchoolManagementSystem.Application.GS.Users.Models;

namespace SchoolManagementSystem.Application.GS.Tenants.Handlers.QueryHandlers;

public class GetTenantByIdQueryHandler : IHttpRequestHandler<GetTenantByIdQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTenantByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {


            // Step 1: Fetch users
            var users = await _unitOfWork.UserRepository.GetAllNoneDeleted(false, true)
                .Where(x => x.TenantId == request.id)
                .ToListAsync(cancellationToken);

            var userList = new List<UserResponse>();

            // Step 2: Fetch profile for each user
            foreach (var user in users)
            {
                userList.Add(new UserResponse
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address!,
                    IsActive = user.IsActive,
                    TenantId = (Guid)user.TenantId!,
                    UserType = user.UserType,
                    Password = user.Password,

                });
            }





            var response = await _unitOfWork.TenantRepository.GetAllNoneDeleted(false, true)
               .Where(x => x.Id == request.id)
               .Select(s => new TenantResponse()
               {
                   Id = s.Id,
                   TenantName = s.TenantName,
                   BinNo = s.BinNo,
                   TenantEmail = s.TenantEmail,
                   PhoneNumber = s.PhoneNumber,
                   Domain = s.Domain,
                   Street = s.Street,
                   City = s.City,
                   Province = s.Province,
                   PostCode = s.PostCode,
                   IsActive = s.IsActive,
                   TenantUserList = userList,

               })
               .FirstOrDefaultAsync();
            return Result.Success(response, StatusCodes.Status200OK);
        }
        catch (Exception ex)
        {
            return Result.Fail<IList<TenantResponse>>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
