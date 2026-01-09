using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentInfoController : ProtectedBaseController
    {
        private readonly IWebHostEnvironment _env;
        public StudentInfoController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpPost("save-student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentInfoResponse))]
        public  Task<IResult> Post([FromForm] StudentInfoRequest request)
        {
            string? imagePath = null;

            if (request.Image != null)
            {
                // ✅ Validate file
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(request.Image.FileName).ToLower();

                //if (!allowedExtensions.Contains(extension))
                //    return BadRequest("Only JPG and PNG images are allowed.");

                //if (request.Image.Length > 2 * 1024 * 1024)
                //    return BadRequest("Image size must be less than 2MB.");

                // ✅ Create directory if not exists
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads/students");
                Directory.CreateDirectory(uploadFolder);

                // ✅ Generate safe filename
                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadFolder, fileName);

                // ✅ Save image
                using var stream = new FileStream(filePath, FileMode.Create);
                request.Image.CopyToAsync(stream);

                imagePath = $"/uploads/students/{fileName}";
                request.ImagePath = imagePath;
            }
            InsertStudentInfoCommand cmd = new InsertStudentInfoCommand() { StudentInfo = request };

            return Mediator.Send(cmd);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentInfoResponse))]
        public Task<IResult> Get(Guid id)
        {
            return Mediator.Send(new GetStudentInfoByIdQuery(id));
        }


        [HttpPost(("get-student-list"))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentInfoResponse))]
        public async Task<IResult> GetStudentList([FromBody] PagedRequest request)
        {
            return await Mediator.Send(new GetStudentListQuery() { PagedRequest = request });//
        }
    }
}
