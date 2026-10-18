using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.GradingScales.Commands;
using SchoolManagementSystem.Application.School.GradingScales.Models;
using SchoolManagementSystem.Application.School.GradingScales.Queries;
using SchoolManagementSystem.Domain.Entities.Schools;

namespace SchoolManagementSystem.Application.School.GradingScales.Handlers;

public class GradingScaleHandlers :
    IHttpRequestHandler<SaveGradingScaleCommand>,
    IHttpRequestHandler<DeleteGradingScaleCommand>,
    IHttpRequestHandler<GetGradingScaleListQuery>,
    IHttpRequestHandler<GetGradingScaleByIdQuery>,
    IHttpRequestHandler<GetDefaultGradingScaleQuery>
{
    private readonly IUnitOfWork _unitOfWork;

    public GradingScaleHandlers(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> Handle(SaveGradingScaleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var req = request.GradingScale;
            GradingScale? scale = null;

            if (req.Id.HasValue && req.Id.Value != Guid.Empty)
            {
                scale = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                    .Include(x => x.Details)
                    .FirstOrDefaultAsync(x => x.Id == req.Id.Value, cancellationToken);
            }

            if (scale == null)
            {
                scale = new GradingScale
                {
                    ScaleName = req.ScaleName,
                    Description = req.Description,
                    IsDefault = req.IsDefault,
                    IsActive = true
                };
                await _unitOfWork.GradingScaleRepository.AddAsync(scale);
            }
            else
            {
                scale.ScaleName = req.ScaleName;
                scale.Description = req.Description;
                scale.IsDefault = req.IsDefault;
                await _unitOfWork.GradingScaleRepository.UpdateAsync(scale);

                // Clear old details
                scale.Details.Clear();
            }

            int index = 1;
            foreach (var d in req.Details.OrderByDescending(x => x.MinMark))
            {
                scale.Details.Add(new GradingScaleDetail
                {
                    MinMark = d.MinMark,
                    MaxMark = d.MaxMark,
                    LetterGrade = d.LetterGrade,
                    GradePoint = d.GradePoint,
                    Remarks = d.Remarks,
                    OrderIndex = index++
                });
            }

            // If this is default, make others not default
            if (scale.IsDefault)
            {
                var others = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                    .Where(x => x.Id != scale.Id && x.IsDefault)
                    .ToListAsync(cancellationToken);
                foreach (var o in others)
                {
                    o.IsDefault = false;
                    await _unitOfWork.GradingScaleRepository.UpdateAsync(o);
                }
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            var resp = new GradingScaleResponse
            {
                Id = scale.Id,
                ScaleName = scale.ScaleName,
                Description = scale.Description,
                IsDefault = scale.IsDefault,
                Details = scale.Details.OrderByDescending(x => x.MinMark).Select(x => new GradingScaleDetailDto
                {
                    Id = x.Id,
                    MinMark = x.MinMark,
                    MaxMark = x.MaxMark,
                    LetterGrade = x.LetterGrade,
                    GradePoint = x.GradePoint,
                    Remarks = x.Remarks,
                    OrderIndex = x.OrderIndex
                }).ToList()
            };

            return Result.Success(resp, "Grading Scale saved successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(DeleteGradingScaleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var scale = await _unitOfWork.GradingScaleRepository.GetSingleNoneDeletedAsync(x => x.Id == request.Id);
            if (scale == null)
                return Result.Fail(StatusCodes.Status404NotFound, "Grading scale not found.");

            _unitOfWork.GradingScaleRepository.Delete(scale);
            await _unitOfWork.CommitAsync(cancellationToken);
            return Result.Success(request.Id, "Grading scale deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetGradingScaleListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await EnsureDefaultScaleSeededAsync(cancellationToken);

            var list = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                .Include(x => x.Details)
                .OrderByDescending(x => x.IsDefault)
                .ThenBy(x => x.ScaleName)
                .Select(x => new GradingScaleResponse
                {
                    Id = x.Id,
                    ScaleName = x.ScaleName,
                    Description = x.Description,
                    IsDefault = x.IsDefault,
                    Details = x.Details.OrderByDescending(d => d.MinMark).Select(d => new GradingScaleDetailDto
                    {
                        Id = d.Id,
                        MinMark = d.MinMark,
                        MaxMark = d.MaxMark,
                        LetterGrade = d.LetterGrade,
                        GradePoint = d.GradePoint,
                        Remarks = d.Remarks,
                        OrderIndex = d.OrderIndex
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return Result.Success(list);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetGradingScaleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var scale = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (scale == null)
                return Result.Fail(StatusCodes.Status404NotFound, "Grading scale not found.");

            var resp = new GradingScaleResponse
            {
                Id = scale.Id,
                ScaleName = scale.ScaleName,
                Description = scale.Description,
                IsDefault = scale.IsDefault,
                Details = scale.Details.OrderByDescending(d => d.MinMark).Select(d => new GradingScaleDetailDto
                {
                    Id = d.Id,
                    MinMark = d.MinMark,
                    MaxMark = d.MaxMark,
                    LetterGrade = d.LetterGrade,
                    GradePoint = d.GradePoint,
                    Remarks = d.Remarks,
                    OrderIndex = d.OrderIndex
                }).ToList()
            };

            return Result.Success(resp);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    public async Task<IResult> Handle(GetDefaultGradingScaleQuery request, CancellationToken cancellationToken)
    {
        try
        {
            await EnsureDefaultScaleSeededAsync(cancellationToken);

            var scale = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.IsDefault, cancellationToken);

            if (scale == null)
            {
                scale = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted()
                    .Include(x => x.Details)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            if (scale == null)
                return Result.Fail(StatusCodes.Status404NotFound, "No grading scale found.");

            var resp = new GradingScaleResponse
            {
                Id = scale.Id,
                ScaleName = scale.ScaleName,
                Description = scale.Description,
                IsDefault = scale.IsDefault,
                Details = scale.Details.OrderByDescending(d => d.MinMark).Select(d => new GradingScaleDetailDto
                {
                    Id = d.Id,
                    MinMark = d.MinMark,
                    MaxMark = d.MaxMark,
                    LetterGrade = d.LetterGrade,
                    GradePoint = d.GradePoint,
                    Remarks = d.Remarks,
                    OrderIndex = d.OrderIndex
                }).ToList()
            };

            return Result.Success(resp);
        }
        catch (Exception ex)
        {
            return Result.Fail(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private async Task EnsureDefaultScaleSeededAsync(CancellationToken cancellationToken)
    {
        var anyScale = await _unitOfWork.GradingScaleRepository.GetAllNoneDeleted().AnyAsync(cancellationToken);
        if (!anyScale)
        {
            var defaultScale = new GradingScale
            {
                ScaleName = "Standard GPA / Grading Scale",
                Description = "Default standard academic grading system",
                IsDefault = true,
                IsActive = true,
                Details = new List<GradingScaleDetail>
                {
                    new() { MinMark = 80, MaxMark = 100, LetterGrade = "A+", GradePoint = 5.00m, OrderIndex = 1 },
                    new() { MinMark = 70, MaxMark = 79, LetterGrade = "A", GradePoint = 4.00m, OrderIndex = 2 },
                    new() { MinMark = 60, MaxMark = 69, LetterGrade = "A-", GradePoint = 3.50m, OrderIndex = 3 },
                    new() { MinMark = 50, MaxMark = 59, LetterGrade = "B", GradePoint = 3.00m, OrderIndex = 4 },
                    new() { MinMark = 40, MaxMark = 49, LetterGrade = "C", GradePoint = 2.00m, OrderIndex = 5 },
                    new() { MinMark = 33, MaxMark = 39, LetterGrade = "D", GradePoint = 1.00m, OrderIndex = 6 },
                    new() { MinMark = 0, MaxMark = 32, LetterGrade = "F", GradePoint = 0.00m, OrderIndex = 7 },
                }
            };
            await _unitOfWork.GradingScaleRepository.AddAsync(defaultScale);
            await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
