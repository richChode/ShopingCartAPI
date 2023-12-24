using shoppingcart.Models;

namespace shoppingcart.Services.CartItemService;

public interface ICartItemService
{
    Task RemoveItemFromCartItems(int itemId);
    Task<List<CartItem>> GetAllCartItems();
    IQueryable<CartItem> GetAllCartItemsAsQuery();

    Task<CartItem?> GetCartItem(int id);
}