using WebWork.Infrastructure.Middleware;
using WebWork.Infrastructure.Convertions;
using WebWork.Services.Interfaces;
using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore; //for db context
using Microsoft.AspNetCore.Identity; //for base identity
using WebWork.Data;
using WebWork.Services.InMemory;
using WebWork.Services.InSQL;
using WebWork.Services.InCookies;
using WebWork.Domain.Entities.Identity;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
//добавление Identity в сервисы и конфигурирование
//services.AddIdentity<IdentityUser, IdentityRole>();//если не расширять возможности базовых классов
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

//настройка cookies
services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "GB.WebWork";
    opt.Cookie.HttpOnly = true;         //настройка передачи

    opt.ExpireTimeSpan = TimeSpan.FromDays(10); //заново получить cook
    //система перенаправления при регистрации и выходе (контроллер/экшн)
    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Account/Logout";
    opt.AccessDeniedPath = "/Account/AccessDenide";//отказ в доступе!!!!!!!!!

    opt.SlidingExpiration = true;//новый идентификатор сеанса при каждом заходе
});

//регистрация сервиса
//универсальный способ добавления в контейнер сервисов сервисы (как синглтон, но может меняться)
//services.AddScoped<IEmployeesData, InMemoryEmployeeData>(); // добавление сервиса в виде <интерфейс, реализация> тестовые данные

//services.AddScoped<IProductData, InMemoryProductData>();//тестовые данные

services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<IEmployeeData, SqlEmployeeData>();
services.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IOrderService, SqlOrderService>();

services.AddDbContext<WebWorkDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));//добавление контекста БД, указывается строка подключения в аргументе (см. appsettings.json)

services.AddScoped<DbInitializer>();//инициализатор БД

//объект создается единожды (в области будет только данный объект)
//builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
//объект сервиса создается заново
//builder.Services.AddTransient<IEmployeesData, InMemoryEmployeesData>();



services.AddControllersWithViews(opt => //настройка сервисов путем добавления контроллеров и представлений 
{
    opt.Conventions.Add(new TestConvertion()); //реализация добавления и/или удаления соглашений
    opt.Conventions.Add(new AddAreaToControllerConvertion());
}
);

services.AddAutoMapper(typeof(Program));//добавление автомаппера

var app = builder.Build();

using (var scope = app.Services.CreateScope())//после построения инициализация БД
{
    var db_init = scope.ServiceProvider.GetService<DbInitializer>();
    await db_init.InitializeAsync(
        RemoveBefore: app.Configuration.GetValue("DbRecreated", false),
        AddTestData: app.Configuration.GetValue("DbRecreated", false));
}

//подключим страничку отладчика в режиме разработчика, на хостинге работать не будет
//см. поле "ASPNETCORE_ENVIRONMENT" в Properties/launchSettings.json раздела profiles

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();//добавление возможности серверу выдавать статическое содержимое (js, css, png, fonts ...) хранится в wwwroot (по умолчанию)

app.UseRouting();//подключение маршрутизации (возможность функциям приложения использовать данные внутри приложения - извлечения информации из маршрута (пути))

app.UseAuthentication(); //после роутинга обязательно
app.UseAuthorization();

app.UseMiddleware<TestMiddleware>();//добавление промежуточного ПО

app.MapGet("/greatings", () => app.Configuration["ServerGreatings"]);

app.UseWelcomePage("/welcome");

//app.MapDefaultControllerRoute();//настройка маршрутизации

//конфигурация машрутизации, на основе tag-helperы строят адреса на страничках

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
