var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();//настройка сервисов путем добавления контроллеров и представлений

var app = builder.Build();

//подключим страничку отладчика в режиме разработчика, на хостинге работать не будет
//см. поле "ASPNETCORE_ENVIRONMENT" в Properties/launchSettings.json раздела profiles
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();//добавление возможности серверу выдавать статическое содержимое (js, css, png, fonts ...) хранится в wwwroot (по умолчанию)

app.UseRouting();//подключение маршрутизации (возможность функциям приложения использовать данные внутри приложения - извлечения информации из маршрута (пути))

app.MapGet("/greatings", () => app.Configuration["ServerGreatings"]);

//app.MapDefaultControllerRoute();//настройка маршрутизации

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
