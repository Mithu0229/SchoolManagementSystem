namespace SchoolManagementSystem.Application.Common;
public interface ILoginService
{
    Task<object> LoginSuccessAsync(User user, bool isRemember, CancellationToken cancellationToken);
}

