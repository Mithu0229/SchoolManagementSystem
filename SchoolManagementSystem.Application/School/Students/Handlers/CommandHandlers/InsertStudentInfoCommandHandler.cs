using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Helpers;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Domain.Entities.Students;

namespace SchoolManagementSystem.Application.School.Students.Handlers.CommandHandlers;

public class InsertStudentInfoCommandHandler : IHttpRequestHandler<InsertStudentInfoCommand>
{
    private IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailService _emailService;

    public InsertStudentInfoCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _emailService = emailService;
    }

    public async Task<IResult> Handle(InsertStudentInfoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
            {
                return Result.Fail<StudentInfoResponse>(StatusCodes.Status406NotAcceptable);
            }
            var id = Guid.NewGuid();
            if (request.StudentInfo != null)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.StudentInfo.FullName!,
                    LastName = request.StudentInfo.FullName!,
                    Email = StudentUserHelper.BuildLoginEmail(request.StudentInfo.FullName!),
                    Password = request.StudentInfo.FullName,
                    UserType = Domain.Enums.UserTypes.Student,
                    PhoneNumber = request.StudentInfo.StudentPhone!,
                    IsActive = false,
                    StudentId = id

                };
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Email);
                await _unitOfWork.UserRepository.AddAsync(user);
            }

            var studentInfo = request.StudentInfo.Adapt<StudentInfo>();
            studentInfo.Id = id;

            // Generate unique Student Custom ID (STD-0000001, STD-0000002, ...)
            //var lastStudent = await _unitOfWork.StudentInfoRepository.GetAllNoneDeleted()
            //    .Where(x => x.StdCID != null && x.StdCID != "")
            //    .OrderByDescending(x => x.StdCID)
            //    .FirstOrDefaultAsync(cancellationToken);

            //int nextNumber = 1;
            //if (lastStudent != null && lastStudent.StdCID.StartsWith("STD-"))
            //{
            //    if (int.TryParse(lastStudent.StdCID.Substring(4), out int lastNumber))
            //    {
            //        nextNumber = lastNumber + 1;
            //    }
            //}
            studentInfo.StdCID = DateTime.Now.ToString("yyyyMMddHHmmss");
            // Keep incrementing until the code is unique (several students can be saved within
            // the same second, e.g. during Excel bulk import).
            while (await _unitOfWork.StudentInfoRepository.GetSingleAsync(x => x.StdCID == studentInfo.StdCID) != null)
            {
                studentInfo.StdCID = (Convert.ToInt64(studentInfo.StdCID) + 1).ToString();
            }
            studentInfo.IsActive = false;
            await _unitOfWork.StudentInfoRepository.AddAsync(studentInfo);
            await _unitOfWork.CommitAsync();
            var response = studentInfo.Adapt<StudentInfoResponse>();

            // Generate PDF and send email
            try
            {
                var classFor =await _unitOfWork.AcademicClassRepository.GetSingleAsync(x => x.Id == Guid.Parse(studentInfo.ApplicationForClass));
                if(classFor != null) { studentInfo.ApplicationForClass = classFor.ClassName; }

                var pdfBytes = StudentApplicationPdfBuilder.GeneratePdf(studentInfo);
                
                string targetEmail = !string.IsNullOrEmpty(request.StudentInfo.StudentEmail) 
                    ? request.StudentInfo.StudentEmail 
                    : (!string.IsNullOrEmpty(request.StudentInfo.GuardianInfo?.FatherEmail) 
                        ? request.StudentInfo.GuardianInfo.FatherEmail 
                        : null);

                if (!string.IsNullOrEmpty(targetEmail))
                {
                    await _emailService.SendEmailWithAttachmentAsync(
                        targetEmail,
                        "Admission Application - Edugates Integrated School",
                        "<h1>Welcome to Edugates Integrated School!</h1><p>Please find your admission application attached.</p>",
                        pdfBytes,
                        "AdmissionApplication.pdf"
                    );
                }
            }
            catch (Exception ex)
            {
                // Log exception if needed, but don't fail the request since student was created
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }

            return Result.Success(response, StatusCodes.Status201Created);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
