//зависимости entityframework sql server
//entityfremwork tools (инструмент командной строки, для работы с БД)
using Microsoft.EntityFrameworkCore;
using WebWork.Domain.Entities;
using WebWork.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebWork.Domain.Entities.Orders;

namespace WebWork.DAL.Context;
//обычно контексты данных разделяют (продукты и пользователи)
public class WebWorkDB : IdentityDbContext<User, Role, string>//класс контекста базы данных (DbContext), для совместного использования с пользователями используется шаблонный класс (последний параметр ключ)
{
    public DbSet<Product> Products { get; set; } = null!; //должно быть достаточно для создание остальных таблиц из-за указанных взаимосвязей с другими сущностями

    public DbSet<Brand> Brands { get; set; } = null!;//для запросов в таблицу брэндов или секций требуется явное определение для удобства

    public DbSet<Section> Sections { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Order> Orders { get; set; } = null!;

    //public DbSet<OrderItems> OrderItems { get; set; } = null!;//для непосредственных обращений к таблице вызов через Set<>

    public WebWorkDB(DbContextOptions<WebWorkDB> options) : base(options) // конструктор передает базовому классу объект options (в шаблоне тип контекста)
    {

    }

    //protected override void OnModelCreating(ModelBuilder db)
    //{
    //    base.OnModelCreating(db);

    //    //автоматически настроено без нашего участия
    //    //db.Entity<Section>()                    //для определения
    //    //    .HasMany(s => s.Products)           //что у секции есть много товаров
    //    //    .WithOne(p => p.Section)            //что кадому продукту соответствует одна секция
    //    //    .OnDelete(DeleteBehavior.Cascade);  //при удалении секции каскадно удаляются все товары
    //}
}
