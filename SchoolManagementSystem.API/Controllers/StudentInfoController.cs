using SchoolManagementSystem.Application.School.Students.Commands;
using SchoolManagementSystem.Application.School.Students.Models;
using SchoolManagementSystem.Application.School.Students.Queries;

namespace SchoolManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentInfoController : PublicBaseController
    {
        private readonly IWebHostEnvironment _env;
        public StudentInfoController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [HttpPost("save-student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentInfoResponse))]
        public async Task<IResult> Post([FromForm] StudentInfoRequest request)
        {
            string? imagePath = null;

            if (request.Image != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(request.Image.FileName).ToLower();
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads/students");
                Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadFolder, fileName);

                // ✅ Save image
                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Image.CopyToAsync(stream);

                imagePath = $"/uploads/students/{fileName}";
                request.ImagePath = imagePath;
            }
            InsertStudentInfoCommand cmd = new InsertStudentInfoCommand() { StudentInfo = request };

            return await Mediator.Send(cmd);
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

        [HttpPut("update-student")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentInfoResponse))]
        public async Task<IResult> Put([FromForm] StudentInfoRequest request)
        {
            string? imagePath = null;

            if (request.Image != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(request.Image.FileName).ToLower();
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads/students");
                Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadFolder, fileName);

                // ✅ Save image
                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Image.CopyToAsync(stream);

                imagePath = $"/uploads/students/{fileName}";
                request.ImagePath = imagePath;
            }

            UpdateStudentInfoCommand cmd = new UpdateStudentInfoCommand() { StudentInfo = request };
            return await Mediator.Send(cmd);
        }

        [HttpPut("update-student-info")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentInfoResponse))]
        public async Task<IResult> UpdateStudentInfoOnly([FromForm] StudentInfoUpdateRequest request)
        {
            string? imagePath = null;

            if (request.Image != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(request.Image.FileName).ToLower();
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads/students");
                Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Image.CopyToAsync(stream);

                imagePath = $"/uploads/students/{fileName}";
                request.ImagePath = imagePath;
            }

            UpdateStudentInfoOnlyCommand cmd = new UpdateStudentInfoOnlyCommand() { StudentInfo = request };
            return await Mediator.Send(cmd);
        }

        [HttpDelete("delete-student/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IResult> Delete(Guid id)
        {
            return await Mediator.Send(new DeleteStudentInfoCommand(id));
        }
    }
}
