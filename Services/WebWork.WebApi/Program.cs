using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebWork.DAL.Context;
using WebWork.Domain.Entities.Identity;
using WebWork.Intefaces.Services;
using WebWork.Services.Data;
using WebWork.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;
var db_type = config["DB:Type"];
var db_connection_string = config.GetConnectionString(db_type);

switch (db_type)
{
    case "DockerDB"://как SqlServer
    case "SqlServer":
        services.AddDbContext<WebWorkDB>(opt => opt.UseSqlServer(db_connection_string));
        break;
    case "Sqlite":
        services.AddDbContext<WebWorkDB>(opt => opt.UseSqlite(db_connection_string, o => o.MigrationsAssembly("WebWork.DAL.Sqlite")));
        break;
}

services.AddScoped<DbInitializer>();//инициализатор БД

services.AddIdentity<User, Role>(/*opt => { opt...}*/)
    .AddEntityFrameworkStores<WebWorkDB>() //указание в каком контекте БД хранить
    .AddDefaultTokenProviders();//генерация токена после сброма пароля

//конфигурирование системы индентификации
services.Configure<IdentityOptions>(opt =>
{

    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;

    opt.User.RequireUniqueEmail = false;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";
    //настройки блокировки
    opt.Lockout.AllowedForNewUsers = false;
    opt.Lockout.MaxFailedAccessAttempts = 10;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

});
//services.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new(config["WebApi"]));//добавление сервиса как http клиента
services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<IEmployeeData, SqlEmployeeData>();
services.AddScoped<IOrderService, SqlOrderService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
