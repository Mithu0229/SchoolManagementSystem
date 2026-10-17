using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.AdmitCards.Models;
using SchoolManagementSystem.Application.School.AdmitCards.Queries;
using System;
using System.Threading.Tasks;

namespace SchoolManagementSystem.API.Controllers;

public class AdmitCardController : PublicBaseController
{
    [HttpGet("get-student-admit-cards/{studentId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentAdmitCardListResponse))]
    public async Task<IResult> GetStudentAdmitCards(Guid studentId, [FromQuery] string? cutoffType = "exam_date")
    {
        return await Mediator.Send(new GetStudentAdmitCardsQuery(studentId, cutoffType));
    }

    [HttpGet("download/{studentId}/{examType}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<IActionResult> DownloadAdmitCard(Guid studentId, string examType)
    {
        var result = await Mediator.Send(new DownloadAdmitCardQuery(studentId, examType, IgnoreDueCheck: false));
        if (result.IsSuccess && result is IResult<AdmitCardPdfResponse> fileResult && fileResult.Data != null)
        {
            return File(fileResult.Data.Content, fileResult.Data.ContentType, fileResult.Data.FileName);
        }

        return BadRequest(new { isSuccess = false, message =  "Unable to download admit card." });
    }

    [HttpGet("preview/{studentId}/{examType}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<IActionResult> PreviewAdmitCard(Guid studentId, string examType)
    {
        var result = await Mediator.Send(new DownloadAdmitCardQuery(studentId, examType, IgnoreDueCheck: true));
        if (result.IsSuccess && result is IResult<AdmitCardPdfResponse> fileResult && fileResult.Data != null)
        {
            Response.Headers.Append("Content-Disposition", $"inline; filename=\"{fileResult.Data.FileName}\"");
            return File(fileResult.Data.Content, "application/pdf");
        }

        return BadRequest(new { isSuccess = false, message =  "Unable to preview admit card." });
    }
}
