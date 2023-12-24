using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace shoppingcart.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public string UserName { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
    [Required]
    [JsonIgnore]
    public string Password { get; set; } = null!;
    [Phone]
    public string PhoneNumber { get; set; } = null!;
}