using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Branches.Commands;
using SchoolManagementSystem.Application.School.Branches.Models;

namespace SchoolManagementSystem.Application.School.Branches.Handlers.CommandHandlers;

public class UpdateBranchCommandHandler : IHttpRequestHandler<UpdateBranchCommand>
{
    private IUnitOfWork _unitOfWork;
    public UpdateBranchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Branch.Id == Guid.Empty)
            {
                return Result.Fail<BranchResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.Branch.BranchName = request.Branch.BranchName.Trim();
            var entity = await _unitOfWork.BranchRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Branch.Id);
            if (entity is null)
            {
                return Result.Fail<BranchResponse>(StatusCodes.Status404NotFound);
            }

            var duplicate = await _unitOfWork.BranchRepository.GetAllNoneDeleted()
                .Where(x => x.Id != request.Branch.Id && x.InstituteId == request.Branch.InstituteId && x.BranchName.ToLower() == request.Branch.BranchName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (duplicate is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Branch name already exists!");
            }

            entity.BranchName = request.Branch.BranchName;
            entity.BranchAddress = request.Branch.BranchAddress;
            entity.ContactPerson = request.Branch.ContactPerson;
            entity.ContactNumber = request.Branch.ContactNumber;
            entity.HomeThemeImagePath = request.Branch.HomeThemeImagePath;
            entity.InstituteId = request.Branch.InstituteId;
            entity.IsActive = request.Branch.IsActive;

            await _unitOfWork.BranchRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<BranchResponse>();
            return Result.Success(response, "Branch " + AlertMessage.UpdateMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<BranchResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
