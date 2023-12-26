using Microsoft.EntityFrameworkCore;
using shoppingcart.Data;
using shoppingcart.Models;

namespace shoppingcart.Services.CartItemService;

public class CartItemService : ICartItemService
{
    private readonly AppDbContext _context;

    public CartItemService(AppDbContext context)
    {
        _context = context;
    }


    public async Task RemoveItemFromCartItems(int itemId)

    {
        var items = await _context.CartItems.Where(c => c.ItemId == itemId).ToListAsync();
        foreach (var c in items)
        {
            _context.CartItems.Remove(c);
        }
    }

    public Task<List<CartItem>> GetAllCartItems()
    {
        return _context.CartItems.Include(ci => ci.Item).Include(ci => ci.User).ToListAsync();
    }

    public IQueryable<CartItem> GetAllCartItemsAsQuery()
    {
        return _context.CartItems.Include(ci => ci.Item).Include(ci => ci.User).AsQueryable();
    }

    public async Task<CartItem?> GetCartItem(int id)
    {
        return await _context.CartItems.Include(ci => ci.Item).Include(ci => ci.User).Where(ci => ci.Id == id).FirstOrDefaultAsync();
    }

    public async Task<CartItem?> GetCartItemByUser(int cartItemId, int userId)
    {
        var cartItem = await _context.CartItems.Include(ci => ci.Item).Include(ci => ci.User).Where(ci => ci.Id == cartItemId && ci.User.Id == userId).FirstOrDefaultAsync();
        return cartItem;
    }
}