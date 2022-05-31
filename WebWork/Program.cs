﻿using WebWork.Infrastructure.Middleware;
using WebWork.Infrastructure.Convertions;
using WebWork.Services.Interfaces;
using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore; //for db context
using WebWork.Data;
using WebWork.Services.InMemory;
using WebWork.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//регистрация сервиса
//универсальный способ добавления в контейнер сервисов сервисы (как синглтон, но может меняться)
//services.AddScoped<IEmployeesData, InMemoryEmployeeData>(); // добавление сервиса в виде <интерфейс, реализация> тестовые данные

//services.AddScoped<IProductData, InMemoryProductData>();//тестовые данные

services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<IEmployeesData, SqlEmployeeData>();

services.AddDbContext<WebWorkDB>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));//добавление контекста БД, указывается строка подключения в аргументе (см. appsettings.json)

services.AddScoped<DbInitializer>();//инициализатор БД

//объект создается единожды (в области будет только данный объект)
//builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
//объект сервиса создается заново
//builder.Services.AddTransient<IEmployeesData, InMemoryEmployeesData>();



services.AddControllersWithViews(opt => //настройка сервисов путем добавления контроллеров и представлений 
{
    opt.Conventions.Add(new TestConvertion()); //реализация добавления и/или удаления соглашений
}
);

services.AddAutoMapper(typeof(Program));//добавление автомаппера

var app = builder.Build();

using(var scope = app.Services.CreateScope())//после построения инициализация БД
{
    var db_init = scope.ServiceProvider.GetService<DbInitializer>();
    await db_init.InitializeAsync(
        RemoveBefore: app.Configuration.GetValue("DbRecreated",false),
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

app.UseMiddleware<TestMiddleware>();//добавление промежуточного ПО

app.MapGet("/greatings", () => app.Configuration["ServerGreatings"]);

app.UseWelcomePage("/welcome");

//app.MapDefaultControllerRoute();//настройка маршрутизации

//конфигурация машрутизации, на основе tag-helperы строят адреса на страничках
app.MapControllerRoute( 
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
