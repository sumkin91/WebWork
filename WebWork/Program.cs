﻿using WebWork.Infrastructure.Middleware;
using WebWork.Infrastructure.Convertions;
using WebWork.Services.Interfaces;
using WebWork.Services;

var builder = WebApplication.CreateBuilder(args);

//регистрация сервиса
//универсальный способ добавления в контейнер сервисов сервисы (как синглтон, но может меняться)
builder.Services.AddScoped<IEmployeesData, InMemoryEmployeesData>(); // добавление сервиса в виде <интерфейс, реализация>

//объект создается единожды (в области будет только данный объект)
//builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
//объект сервиса создается заново
//builder.Services.AddTransient<IEmployeesData, InMemoryEmployeesData>();


builder.Services.AddControllersWithViews(opt => //настройка сервисов путем добавления контроллеров и представлений 
{
    opt.Conventions.Add(new TestConvertion()); //реализация добавления и/или удаления соглашений
}
);

var app = builder.Build();

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
