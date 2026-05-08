using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Branches.Models;
using SchoolManagementSystem.Application.School.Branches.Queries;

namespace SchoolManagementSystem.Application.School.Branches.Handlers.QueryHandlers;

public class GetBranchByIdQueryHandler : IHttpRequestHandler<GetBranchByIdQuery>
{
    private IUnitOfWork _unitOfWork;
    public GetBranchByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(GetBranchByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                return Result.Fail<BranchResponse>(StatusCodes.Status406NotAcceptable);
            }

            var response = await _unitOfWork.BranchRepository.GetAllNoneDeleted(true)
                .Where(x => x.Id == request.Id)
                .Select(x => new BranchResponse
                {
                    Id = x.Id,
                    BranchName = x.BranchName,
                    BranchAddress = x.BranchAddress,
                    ContactPerson = x.ContactPerson,
                    ContactNumber = x.ContactNumber,
                    HomeThemeImagePath = x.HomeThemeImagePath,
                    InstituteId = x.InstituteId!.Value,
                    InstituteName = x.Institute.InstituteName,
                    IsActive = x.IsActive
                }).FirstOrDefaultAsync(cancellationToken);
            if (response is null)
            {
                return Result.Fail<BranchResponse>(StatusCodes.Status404NotFound);
            }

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<BranchResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
