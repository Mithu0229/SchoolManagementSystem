using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SchoolManagementSystem.Application.Common;
using SchoolManagementSystem.Application.GS.Users.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UAParser;

namespace SchoolManagementSystem.Infrastructure.Common;

public class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _accessor;

    public LoginService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }

    public async Task<object> LoginSuccessAsync(User user, bool isRemember, CancellationToken cancellationToken)
    {
        try
        {
            // --- Record login history ---
            var ua = _accessor.HttpContext.Request.Headers[HeaderNames.UserAgent];
            var uaParser = Parser.GetDefault();
            ClientInfo c = uaParser.Parse(ua);

            var loginHistory = new UsersLoginHistory
            {
                Id = Guid.NewGuid(),
                CreatedById = user.Id,
                CreatedDate = DateTime.UtcNow,
                IP = _accessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
                MAC = GetMacAddress(),
                NetworkType = GetNetworkType(),
                OperatingSystem = c.OS.ToString(),
                TenantId = user.TenantId,
            };

            await _unitOfWork.UserLoginHistoryRepository.AddAsync(loginHistory);

            user.IsActive = true;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            //await _unitOfWork.CommitAsync();

            // --- JWT creation ---
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            var userRoles = await (from ur in _unitOfWork.UserRoleRepository.GetAllNoneDeleted(false, true)
                                   join r in _unitOfWork.RoleRepository.GetAllNoneDeleted(false, true)
                                       on ur.RoleId equals r.Id
                                   where ur.UserId == user.Id && ur.IsActive && r.IsActive
                                   select new { ur.RoleId, r.RoleName })
                .Distinct()
                .ToListAsync(cancellationToken);

            var rolesString = JsonSerializer.Serialize(userRoles);
            var tenantIdClaimValue = user.TenantId?.ToString() ?? string.Empty;

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5015/",//_appSettings.Domain,
                audience: "http://localhost:5015/",// _appSettings.Domain,
                claims: new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim("tenantId", tenantIdClaimValue),
                    new Claim("email", user.Email),
                    new Claim("roles", rolesString),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: isRemember ? DateTime.Now.AddMonths(1) : DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenOptions);

            // Replace this line:
            // var passwordAge = (DateTime.UtcNow - user.PasswordResetDate).Days;

            // With the following code to handle nullable DateTime:

            // --- Logo & profile ---


            var userLoginInfo = new LoginUserResponse
            {
                TenantId = user.TenantId,
                TenantName = user.Tenant?.TenantName,
                UserName = user != null ? $"{user.FirstName} {user.LastName}" : null,
                UserEmail = user?.Email,
            };
           
            return new
            {
                ok = true,
                token,
                user!.Id,
                index = "/home/index",
                userLoginInfo,
                roles = userRoles,
                userType = user.UserType
            };
        }
        catch (Exception ex)
        {
            // LogHelpers.Error(ex);
            return Result.Fail<UserResponse>(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private static string GetNetworkType() =>
        (from nic in NetworkInterface.GetAllNetworkInterfaces()
         where nic.OperationalStatus == OperationalStatus.Up
         select nic.NetworkInterfaceType.ToString()).FirstOrDefault();

    private static string GetMacAddress() =>
        (from nic in NetworkInterface.GetAllNetworkInterfaces()
         where nic.OperationalStatus == OperationalStatus.Up
         select nic.GetPhysicalAddress().ToString()).FirstOrDefault();

}
