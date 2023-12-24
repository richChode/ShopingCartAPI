using shoppingcart.Dto;
using shoppingcart.Models;

namespace shoppingcart.Services.UserService;

public interface IUserService
{
    Task<User?> CreateUser(RegisterRequest user);
    Task<string?> LoginUser(LoginRequest user);
    Task<User?> CheckUsername(string username);
    Task<User?> GetUserById(int id);

    CartItem? GetCartItem(int itemId, User user);

    Task<bool> RemoveCartItem(int itemId, User user);

    Task<List<CartItem>?> GetCartItems(int userId);
    Task<bool> AddCartItem(NewCartItem newCartItem, User user);

    Task Save();
}