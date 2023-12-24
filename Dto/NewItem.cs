using System.ComponentModel.DataAnnotations;

namespace shoppingcart.Dto;

public class NewItem
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    [Range(0, 10000)]
    public int Quantity { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public double Price { get; set; }
}