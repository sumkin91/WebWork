using WebWork.DAL.Context;
using Microsoft.EntityFrameworkCore; //для миграции

namespace WebWork.Data;

public class DbInitializer
{
    private readonly WebWorkDB _db;

    private readonly ILogger<DbInitializer> _Logger;
    public DbInitializer(WebWorkDB db, ILogger<DbInitializer> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public async Task<bool> RemoveAsync(CancellationToken Cansel = default)
    {
        _Logger.LogInformation("Удаление БД ...");

        var result = await _db.Database.EnsureDeletedAsync(Cansel).ConfigureAwait(false);
        if (result)
            _Logger.LogInformation("Удаление БД выполнено успешно!");
        else
            _Logger.LogInformation("Удаление БД не выполнено - БД отсутствует на сервере!");

        return result;
    }

    public async Task InitializeAsync(
        bool RemoveBefore,
        bool AddTestData,
        CancellationToken Cancel = default)
    {
        _Logger.LogInformation("Инициализация БД ...");

        if(RemoveBefore)
            await RemoveAsync(Cancel).ConfigureAwait(false);

        //await _db.Database.EnsureCreatedAsync(Cancel).ConfigureAwait(false);

        _Logger.LogInformation("Применение миграций БД ...");
        await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);
        _Logger.LogInformation("Применение миграций БД выполнено!");


        if (AddTestData)
        {
            await InitializeEmployeesAsync(Cancel);
            await InitializeProductsAsync(Cancel);
        }

        _Logger.LogInformation("Инициализация БД выполнена!");


    }

    private async Task InitializeEmployeesAsync(CancellationToken Cancel)
    {
        _Logger.LogInformation("Инициализация БД тестовыми данными Работников...");

        if (await _db.Employees.AnyAsync(Cancel).ConfigureAwait(false))
        {
            _Logger.LogInformation("Инициализация БД тестовыми данными не требуется");
            return;
        }

        await using var transaction = await _db.Database.BeginTransactionAsync(Cancel);

        _Logger.LogInformation("Добавление в БД секций...");
        await _db.Employees.AddRangeAsync(TestData.Employees, Cancel);

        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] ON", Cancel);
        await _db.SaveChangesAsync(Cancel);//сохранение изменений
        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] OFF", Cancel);
        _Logger.LogInformation("Добавление в БД секций выполнено успешно!");

        await transaction.CommitAsync(Cancel);
    }

    private async Task InitializeProductsAsync(CancellationToken Cancel)
    {
        _Logger.LogInformation("Инициализация БД тестовыми данными Продуктов...");

        if (await _db.Products.AnyAsync(Cancel).ConfigureAwait(false))//если что-то есть в БД
        {
            _Logger.LogInformation("Инициализация БД тестовыми данными не требуется");
            return;
        }

        await using var transaction = await _db.Database.BeginTransactionAsync(Cancel); //начало транзакции, если до коммита транзакции не отработают, то данные внесены в БД не будут

        _Logger.LogInformation("Добавление в БД секций...");
        await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);// добавление в БД данные секций

        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", Cancel);// сырой sql - переключение таблицы в спец режим для работы с ключами
        await _db.SaveChangesAsync(Cancel);//сохранение изменений
        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", Cancel);
        _Logger.LogInformation("Добавление в БД секций выполнено успешно!");


        _Logger.LogInformation("Добавление в БД брендов...");
        await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);

        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", Cancel);
        await _db.SaveChangesAsync(Cancel);
        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", Cancel);
        _Logger.LogInformation("Добавление в БД брендов выполнено успешно!");

        _Logger.LogInformation("Добавление в БД товаров...");
        await _db.Products.AddRangeAsync(TestData.Products, Cancel);

        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", Cancel);
        await _db.SaveChangesAsync(Cancel);
        await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", Cancel);
        _Logger.LogInformation("Добавление в БД товаров выполнено успешно!");

        await transaction.CommitAsync(Cancel);//принимаем транзакцию
    }
}
