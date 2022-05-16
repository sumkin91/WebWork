var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();//��������� �������� ����� ���������� ������������ � �������������

var app = builder.Build();

app.MapGet("/greatings", () => app.Configuration["ServerGreatings"]);

//app.MapDefaultControllerRoute();//��������� �������������

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
