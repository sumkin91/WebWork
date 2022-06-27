using WebWork.Infrastructure.Middleware;
using WebWork.Infrastructure.Convertions;
using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore; //for db context
using Microsoft.AspNetCore.Identity; //for base identity
using WebWork.Domain.Entities.Identity;
using WebWork.Intefaces.Services;
using WebWork.Services.Services.InSQL;
using WebWork.Services.Services.InCookies;
using WebWork.Services.Data;
using WebWork.Intefaces.TestApi;
using WebWork.WebAPI.Clients.Values;
using WebWork.WebApi.Clients.Employees;
using WebWork.WebApi.Clients.Orders;
using WebWork.WebApi.Clients.Products;
using static WebWork.WebApi.Clients.Identity.UsersClients;
using static WebWork.WebApi.Clients.Identity.RolesClients;
using WebWork.Intefaces.Services.Identity;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var services = builder.Services;

services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders();//генерация токена после сброма пароля


services.AddHttpClient("WebWorkApiIdentity",
    client => {
        //client.DefaultRequestHeaders.Add("accept", "application/json");
        client.BaseAddress = new(config["WebApi"]);
    })
    .AddTypedClient<IUsersClient, UsersClient>()
    .AddTypedClient<IUserStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserRoleStore<User>, UsersClient>()
    .AddTypedClient<IUserPasswordStore<User>, UsersClient>()
    .AddTypedClient<IUserEmailStore<User>, UsersClient>()
    .AddTypedClient<IUserPhoneNumberStore<User>, UsersClient>()
    .AddTypedClient<IUserTwoFactorStore<User>, UsersClient>()
    .AddTypedClient<IUserClaimStore<User>, UsersClient>()
    .AddTypedClient<IUserLoginStore<User>, UsersClient>()
    .AddTypedClient<IRolesClient, RolesClient>()
    .AddTypedClient<IRoleStore<Role>, RolesClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy())
    ;
;
//конфигурирование системы индентификации
services.Configure<IdentityOptions>(opt =>
{
#if DEBUG
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 3;
    opt.Password.RequiredUniqueChars = 3;
#endif
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



services.AddHttpClient("WebWorkApi", client => client.BaseAddress = new(config["WebApi"]))
    .AddTypedClient<IValuesService, ValuesClient>()
    .AddTypedClient<IEmployeeData, EmployeesClient>()
    .AddTypedClient<IProductData, ProductsClient>()
    .AddTypedClient<IOrderService, OrdersClient>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy())
    ;

//политика повторных запросов
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int MaxRetryCount = 5, int MaxJitterTime = 1000)
{
    var jitter = new Random();
    return HttpPolicyExtensions
       .HandleTransientHttpError()
       .WaitAndRetryAsync(MaxRetryCount, RetryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, RetryAttempt)) +
            TimeSpan.FromMilliseconds(jitter.Next(0, MaxJitterTime)));
}

//политика разрушения цепочек (при замыкании)
static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
    HttpPolicyExtensions
       .HandleTransientHttpError()
       .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 5, TimeSpan.FromSeconds(30));

services.AddScoped<ICartService, InCookiesCartService>();

services.AddControllersWithViews(opt => //настройка сервисов путем добавления контроллеров и представлений 
{
    opt.Conventions.Add(new TestConvertion()); //реализация добавления и/или удаления соглашений
    opt.Conventions.Add(new AddAreaToControllerConvertion());
}
);

services.AddAutoMapper(typeof(Program));//добавление автомаппера

var app = builder.Build();

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
