//using MediatR;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using SchoolManagementSystem.Application.GS.Divisions.Commands;
//using SchoolManagementSystem.Infrastructure.Common;
//using SchoolManagementSystem.Infrastructure.DependencyContainers;
//using SchoolManagementSystem.Infrastructure.Persistence;
//using SchoolManagementSystem.Infrastructure.Persistence.Services;
//using System.Reflection;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//#region Configuration

//var configuration = builder.Configuration;

//#endregion

//#region Add Controllers

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();

//#endregion

//#region Database

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<AttendanceDbContext>(options =>
//{
//    options.UseSqlServer(
//        configuration.GetConnectionString("AttendanceConnection"));
//});

//#endregion

//#region Background Services

//builder.Services.AddHostedService<AttendanceSmsBackgroundService>();

//#endregion

//#region HTTP Client

//builder.Services.AddHttpClient<ISmsService, SmsService>();

//#endregion

//#region CORS

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AppCORSPolicy", policy =>
//    {
//        policy
//            .WithOrigins(
//                "https://sms.edugateserp.com", "http://www.smsapi.edugateserp.com", "http://localhost:4200"
//            )
//            .AllowAnyMethod()
//            .AllowAnyHeader();
//    });
//});

//#endregion

//#region HttpContext

//builder.Services.AddHttpContextAccessor();

//#endregion

//#region Swagger

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "School Management System API",
//        Version = "v1",
//        Description = "School Management System API"
//    });

//    c.ResolveConflictingActions(apiDescriptions =>
//        apiDescriptions.First());

//    // JWT Bearer Authentication
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description =
//            "JWT Authorization header using the Bearer scheme. " +
//            "Enter only your JWT token below. " +
//            "Example: eyJhbGciOiJIUzI1NiIs...",

//        Name = "Authorization",

//        In = ParameterLocation.Header,

//        Type = SecuritySchemeType.Http,

//        Scheme = "bearer",

//        BearerFormat = "JWT"
//    });

//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

//#endregion

//#region JWT Authentication

//var jwtKey =
//    configuration["Jwt:Key"]
//    ?? "CHANGE_THIS_TO_A_LONG_PRODUCTION_SECRET_KEY";

//var jwtIssuer =
//    configuration["Jwt:Issuer"]
//    ?? "https://smsapi.edugateserp.com/";

//var jwtAudience =
//    configuration["Jwt:Audience"]
//    ?? "https://smsapi.edugateserp.com/";

//builder.Services
//    .AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme =
//            JwtBearerDefaults.AuthenticationScheme;

//        options.DefaultChallengeScheme =
//            JwtBearerDefaults.AuthenticationScheme;
//    })
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters =
//            new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,

//                ValidIssuer = jwtIssuer,
//                ValidAudience = jwtAudience,

//                IssuerSigningKey =
//                    new SymmetricSecurityKey(
//                        Encoding.UTF8.GetBytes(jwtKey)
//                    ),

//                ClockSkew = TimeSpan.Zero
//            };
//    });

//#endregion

//#region MediatR

//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssembly(
//        Assembly.GetExecutingAssembly()));

//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssembly(
//        typeof(InsertDivisionCommand).Assembly));

//#endregion

//#region Dependency Injection

//ContextDependencyContainer.RegisterServices(
//    builder.Services);

//RepositoryDependencyContainer.RegisterServices(
//    builder.Services);

//ServiceDependencyContainer.RegisterServices(
//    builder.Services);

//#endregion

//var app = builder.Build();

//#region Swagger

//// IMPORTANT:
//// Swagger is enabled for Production also.
//// Do NOT put this inside IsDevelopment().
//app.UseSwagger();

//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint(
//        "/swagger/v1/swagger.json",
//        "School Management System API V1");

//    c.RoutePrefix = "swagger";
//});

//#endregion

//#region HTTPS

//app.UseHttpsRedirection();

//#endregion

//#region Routing

//app.UseRouting();

//#endregion

//#region CORS

//app.UseCors("AppCORSPolicy");

//#endregion

//#region Authentication & Authorization

//app.UseAuthentication();

//app.UseAuthorization();

//#endregion

//#region Static Files

//app.UseStaticFiles();

//var uploadsPath = Path.Combine(
//    builder.Environment.WebRootPath ?? "wwwroot",
//    "uploads");

//if (!Directory.Exists(uploadsPath))
//{
//    Directory.CreateDirectory(uploadsPath);
//}

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        uploadsPath),

//    RequestPath = "/uploads"
//});

//#endregion

//#region Controllers

//app.MapControllers();

//#endregion

//app.Run();