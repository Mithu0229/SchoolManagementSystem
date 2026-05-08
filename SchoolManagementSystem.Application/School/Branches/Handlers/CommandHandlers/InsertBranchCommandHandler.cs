using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.School.Branches.Commands;
using SchoolManagementSystem.Application.School.Branches.Models;

namespace SchoolManagementSystem.Application.School.Branches.Handlers.CommandHandlers;

public class InsertBranchCommandHandler : IHttpRequestHandler<InsertBranchCommand>
{
    private IUnitOfWork _unitOfWork;
    public InsertBranchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(InsertBranchCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<BranchResponse>(StatusCodes.Status406NotAcceptable);
            }

            request.Branch.BranchName = request.Branch.BranchName.Trim();
            var branch = await _unitOfWork.BranchRepository.GetAllNoneDeleted()
                .Where(x => x.InstituteId == request.Branch.InstituteId && x.BranchName.ToLower() == request.Branch.BranchName.ToLower())
                .FirstOrDefaultAsync(cancellationToken);
            if (branch is not null)
            {
                return Result.Fail(StatusCodes.Status409Conflict, "Branch name already exists!");
            }

            var entity = request.Branch.Adapt<Branch>();
            await _unitOfWork.BranchRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            var response = entity.Adapt<BranchResponse>();
            return Result.Success(response, "Branch " + AlertMessage.SaveMessage);
        }
        catch (Exception ex)
        {
            return Result.Fail<BranchResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
