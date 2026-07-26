using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.SMSHistories.Queries;

namespace SchoolManagementSystem.Application.School.SMSHistories.Handlers.QueryHandlers;

public class GetSMSHistoryByStudentIdQueryHandler : IRequestHandler<GetSMSHistoryByStudentIdQuery, IResult<List<SMSHistoryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSMSHistoryByStudentIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult<List<SMSHistoryResponse>>> Handle(GetSMSHistoryByStudentIdQuery request, CancellationToken cancellationToken)
    {
        var history = await _unitOfWork.SMSHistoryRepository.GetAll()
            .Where(x => x.StudentId == request.StudentId)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new SMSHistoryResponse
            {
                Id = x.Id,
                StudentId = x.StudentId,
                Message = x.Message,
                Phone = x.Phone,
                CreatedDate = x.CreatedDate
            })
            .ToListAsync(cancellationToken);

        return Result.Success(history);
    }
}
