var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();//настройка сервисов путем добавления контроллеров и представлений

var app = builder.Build();

app.MapGet("/greatings", () => app.Configuration["ServerGreatings"]);

//app.MapDefaultControllerRoute();//настройка маршрутизации

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
