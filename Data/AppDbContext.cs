using Microsoft.EntityFrameworkCore;
using shoppingcart.Models;

namespace shoppingcart.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<CartItem> CartItems { get; set; } = null!;
    public DbSet<Cart> Carts { get; set; } = null!;


}