using System.ComponentModel.DataAnnotations;

namespace shoppingcart.Dto;

public class NewCartItem
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [Required]
    [Range(1, 10000)]
    public int Quantity { get; set; }
}