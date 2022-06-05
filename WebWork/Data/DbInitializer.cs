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

    public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
    {
        _Logger.LogInformation("Удаление БД ...");

        var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);

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
            await InitializeProductsAsync(Cancel);
            await InitializeEmployeesAsync(Cancel);
            
        }

        _Logger.LogInformation("Инициализация БД выполнена!");

    }

    private async Task InitializeProductsAsync(CancellationToken Cancel)
    {
        _Logger.LogInformation("Инициализация БД тестовыми данными Продуктов...");

        if (await _db.Products.AnyAsync(Cancel).ConfigureAwait(false))//если что-то есть в БД
        {
            _Logger.LogInformation("Инициализация БД тестовыми данными не требуется");
            return;
        }

        //удаление ID у сущностей
        var sections_pool = TestData.Sections.ToDictionary(s => s.Id);
        var brands_pool = TestData.Brands.ToDictionary(b => b.Id);

        foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
            child_section.Parent = sections_pool[(int)child_section.ParentId!];//опасное место!!! делали циклическую запись через Id той же секции(так делать нельзя! не собиралось!)

        foreach (var product in TestData.Products)
        {
            product.Section = sections_pool[product.SectionId];
            if (product.BrandId is { } brand_id)
                product.Brand = brands_pool[brand_id];

            product.Id = 0;
            product.SectionId = 0;
            product.BrandId = null;
        }

        foreach (var brand in TestData.Brands)
            brand.Id = 0;

        foreach (var section in TestData.Sections)
        {
            section.Id = 0;
            section.ParentId = null;
            _Logger.LogInformation(section.ToString());
        }

        await using var transaction = await _db.Database.BeginTransactionAsync(Cancel);

        _Logger.LogInformation("Добавление данных в БД...");
        
        await _db.Products.AddRangeAsync(TestData.Products, Cancel);

        await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);

        await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);

        await _db.SaveChangesAsync(Cancel);
        
        _Logger.LogInformation("Добавление данных в БД выполнено успешно");

        await transaction.CommitAsync(Cancel);
        _Logger.LogInformation("Транзакция в БД завершена");
    }

    private async Task InitializeEmployeesAsync(CancellationToken Cancel)
    {
        _Logger.LogInformation("Инициализация БД тестовыми данными Работников...");

        if (await _db.Employees.AnyAsync(Cancel).ConfigureAwait(false))
        {
            _Logger.LogInformation("Инициализация БД тестовыми данными не требуется");
            return;
        }

        foreach (var employee in TestData.Employees) employee.Id = 0;

        _Logger.LogInformation("Добавление в БД секций...");
        await _db.AddRangeAsync(TestData.Employees, Cancel);
        await _db.SaveChangesAsync(Cancel);//сохранение изменений

        _Logger.LogInformation("Добавление в БД секций выполнено успешно!");

    }
}
