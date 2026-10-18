using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Subjects.Commands;
using SchoolManagementSystem.Application.School.Subjects.Models;
using SchoolManagementSystem.Application.School.Subjects.Queries;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Application.School.Subjects.Handlers;

public class SubjectHandlers :
    IHttpRequestHandler<InsertSubjectCommand>,
    IHttpRequestHandler<UpdateSubjectCommand>,
    IHttpRequestHandler<DeleteSubjectCommand>,
    IHttpRequestHandler<AssignClassSubjectsCommand>,
    IHttpRequestHandler<GetSubjectListQuery>,
    IHttpRequestHandler<GetSubjectByIdQuery>,
    IHttpRequestHandler<GetSubjectDropdownQuery>,
    IHttpRequestHandler<GetSubjectsByClassQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public SubjectHandlers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(InsertSubjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.Subject;
            req.SubjectName = req.SubjectName.Trim();
            var exists = await _unitOfWork.SubjectRepository.GetAllNoneDeleted()
                .AnyAsync(x => x.SubjectName.ToLower() == req.SubjectName.ToLower(), cancellationToken);
            if (exists)
                return Result.Fail(StatusCodes.Status409Conflict, "Subject with this name already exists.");

            var entity = req.Adapt<Subject>();
            await _unitOfWork.SubjectRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<SubjectResponse>(), "Subject created successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.Subject;
            if (!req.Id.HasValue)
                return Result.Fail(StatusCodes.Status400BadRequest, "Subject ID is required.");

            var entity = await _unitOfWork.SubjectRepository.GetSingleNoneDeletedAsync(x => x.Id == req.Id.Value);
            if (entity == null)
                return Result.Fail(StatusCodes.Status404NotFound, "Subject not found.");

            req.SubjectName = req.SubjectName.Trim();
            var duplicate = await _unitOfWork.SubjectRepository.GetAllNoneDeleted()
                .AnyAsync(x => x.SubjectName.ToLower() == req.SubjectName.ToLower() && x.Id != req.Id.Value, cancellationToken);
            if (duplicate)
                return Result.Fail(StatusCodes.Status409Conflict, "Another subject with this name already exists.");

            entity.SubjectName = req.SubjectName;
            entity.SubjectCode = req.SubjectCode;
            entity.Description = req.Description;
            entity.IsActive = req.IsActive;

            await _unitOfWork.SubjectRepository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(entity.Adapt<SubjectResponse>(), "Subject updated successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _unitOfWork.SubjectRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (entity == null)
                return Result.Fail(StatusCodes.Status404NotFound, "Subject not found.");

            _unitOfWork.SubjectRepository.Delete(entity);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(request.Id, "Subject deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(AssignClassSubjectsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var classId = request.ClassSubject.ClassId;
            var subjectIds = request.ClassSubject.SubjectIds.Distinct().ToList();

            var existing = await _unitOfWork.ClassSubjectRepository.GetAllNoneDeleted()
                .Where(x => x.ClassId == classId)
                .ToListAsync(cancellationToken);

            foreach (var item in existing)
            {
                if (!subjectIds.Contains(item.SubjectId))
                {
                    _unitOfWork.ClassSubjectRepository.Delete(item);
                }
            }

            for (int i = 0; i < subjectIds.Count; i++)
            {
                var sId = subjectIds[i];
                var match = existing.FirstOrDefault(x => x.SubjectId == sId);
                if (match != null)
                {
                    match.OrderIndex = i + 1;
                    await _unitOfWork.ClassSubjectRepository.UpdateAsync(match);
                }
                else
                {
                    await _unitOfWork.ClassSubjectRepository.AddAsync(new ClassSubject
                    {
                        ClassId = classId,
                        SubjectId = sId,
                        OrderIndex = i + 1,
                        IsActive = true
                    });
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(true, "Subjects assigned to class successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetSubjectListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var pagedRequest = request.PagedRequest ?? new PagedRequest();
            var query = _unitOfWork.SubjectRepository.GetAllNoneDeleted(true);

            if (!string.IsNullOrWhiteSpace(pagedRequest.Search))
            {
                var search = pagedRequest.Search.Trim().ToLower();
                query = query.Where(x => x.SubjectName.ToLower().Contains(search) || (x.SubjectCode != null && x.SubjectCode.ToLower().Contains(search)));
            }

            var totalRecord = await query.CountAsync(cancellationToken);
            if (pagedRequest.Page > 0 && pagedRequest.PageSize > 0)
            {
                query = query.Skip((pagedRequest.Page - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            }

            var items = await query.OrderBy(x => x.SubjectName)
                .Select(x => new SubjectResponse
                {
                    Id = x.Id,
                    SubjectName = x.SubjectName,
                    SubjectCode = x.SubjectCode,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return Result.Success(new PagedResult<SubjectResponse>
            {
                Items = items,
                TotalRecord = totalRecord,
                Page = pagedRequest.Page,
                PageSize = pagedRequest.PageSize
            });
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _unitOfWork.SubjectRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (entity == null)
                return Result.Fail(StatusCodes.Status404NotFound, "Subject not found.");

            return Result.Success(entity.Adapt<SubjectResponse>());
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetSubjectDropdownQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await _unitOfWork.SubjectRepository.GetAllNoneDeleted()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SubjectName)
                .Select(x => new { value = x.Id, text = x.SubjectName, code = x.SubjectCode })
                .ToListAsync(cancellationToken);

            return Result.Success(list);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetSubjectsByClassQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await _unitOfWork.ClassSubjectRepository.GetAllNoneDeleted()
                .Where(x => x.ClassId == request.ClassId && x.IsActive)
                .Include(x => x.Subject)
                .OrderBy(x => x.OrderIndex)
                .Select(x => new AssignedSubjectDto
                {
                    SubjectId = x.SubjectId,
                    SubjectName = x.Subject != null ? x.Subject.SubjectName : string.Empty,
                    SubjectCode = x.Subject != null ? x.Subject.SubjectCode : null,
                    OrderIndex = x.OrderIndex
                })
                .ToListAsync(cancellationToken);

            return Result.Success(list);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
