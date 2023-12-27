using System.ComponentModel.DataAnnotations;

namespace shoppingcart.Models;

public class Item
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Range(1, 10000)]
    public int Quantity { get; set; }
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
}