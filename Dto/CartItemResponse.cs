using System.ComponentModel.DataAnnotations;

namespace shoppingcart.Dto;

public class CartItemResponse
{
    [Required]
    [Range(1, int.MaxValue)]
    public int ItemId { get; set; }
    [Range(1, 10000)]
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string ItemName { get; set; } = null!;
    public string Username { get; set; } = null!;


}