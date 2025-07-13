using DBUtility;
using HRManagementSystem;
using HRManagementSystem.Application.Commands;
using HRManagementSystem.Application.Queries;
using HRManagementSystem.Common.Auth;
using HRManagementSystem.Common.Data;
using HRManagementSystem.Common.Helper;
using HRManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HRManagementSystem API",
        Version = "v1"
    });

    // 加上 JWT Bearer 設定
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "輸入格式為：Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new string[] {}
        }
    });
});
// 設定 Serilog 日誌
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt")         // 輸出到檔案
    .CreateLogger();

builder.Services.AddSingleton(new AESHelper());

var aesHelper = builder.Services.BuildServiceProvider().GetService<AESHelper>();
string connAESString = builder.Configuration.GetConnectionString("DefaultConnection");
string connectionString = aesHelper.Decrypt(connAESString);
builder.Services.AddScoped<IDataBaseUtility, DataBaseUtility>(provider =>
    new DataBaseUtility(connectionString));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT 設定
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
})
.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization();

// 註冊 CORS（加在最前面）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Ionic 本地開發環境網址
              .AllowAnyHeader()
              .AllowAnyMethod()
              //.AllowCredentials()
              ; // 如果前端有帶 cookie 或 token，才需要這行
    });
});

// 添加服务到 DI 容器
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureLayer();

// 註冊 MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
// 設定序列化選項
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // 使用原始屬性名稱
    });

//AutoMapper
builder.Services.AddAutoMapper(
    (serviceProvider, mapperConfiguration) => mapperConfiguration.AddProfiles(new AutoMapper.Profile[]
    {
        new CommandProfile(),
        new QueriesProfile()
    }),
    AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
app.Use(async (context, next) =>
{
    Console.WriteLine($"[DEBUG] Request Path: {context.Request.Path}");
    await next.Invoke();
});

// 套用 CORS middleware（要在 UseAuthorization 之前）
app.UseCors("AllowFrontend");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRManagementSystem API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

