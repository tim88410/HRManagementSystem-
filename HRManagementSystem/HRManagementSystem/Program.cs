using DBUtility;
using HRManagementSystem.Common.Helper;
using HRManagementSystem;
using Serilog;
using System.Reflection;
using HRManagementSystem.Application.Commands;
using HRManagementSystem.Application.Queries;
using HRManagementSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
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
// 套用 CORS middleware（要在 UseAuthorization 之前）
app.UseCors("AllowFrontend");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

