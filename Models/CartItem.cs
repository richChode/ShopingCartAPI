using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shoppingcart.Models;

public class CartItem
{
    public int Id { get; set; }
    [JsonIgnore]
    public Cart Cart { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    [Range(1, 10000)]
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public User User { get; set; } = null!;
}