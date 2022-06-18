using WebWork.Domain.Entities.Base;
using WebWork.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebWork.Domain.Entities.Orders;

public class Order : Entity
{
    [Required]
    public User? User { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Phone { get; set; }

    [Required]
    [MaxLength(200)]
    public string? Address { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;

    public ICollection<OrderItems> Items { get; set; } = new HashSet<OrderItems>();

    [NotMapped]
    public decimal TotalPrice => Items.Sum(item => item.TotalItemPrice);
}

public class OrderItems : Entity
{
    [Required]
    public Product? Product { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public Order? Order { get; set; }

    [NotMapped]
    public decimal TotalItemPrice => Price * Quantity;
}
