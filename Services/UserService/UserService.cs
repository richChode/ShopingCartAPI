using Microsoft.EntityFrameworkCore;
using shoppingcart.Data;
using shoppingcart.Dto;
using shoppingcart.Exceptions;
using shoppingcart.Models;
using shoppingcart.Services.CartItemService;
using shoppingcart.Services.ItemService;
using shoppingcart.Services.TokenGeneratorService;

namespace shoppingcart.Services.UserService;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IItemService _itemService;
    private readonly ITokenGeneratorService _tokenGenerator;
    private readonly ICartItemService _cartItemService;

    public UserService(AppDbContext context, IItemService itemService, ITokenGeneratorService tokenGenerator, ICartItemService cartItemService)
    {
        _context = context;
        _itemService = itemService;
        _tokenGenerator = tokenGenerator;
        _cartItemService = cartItemService;

    }
    public async Task<User?> CreateUser(RegisterRequest registerDetails)
    {
        var user = await CheckUsername(registerDetails.UserName);
        if (user == null)
        {
            var cart = new Cart();
            var result = await _context.Users.AddAsync(new User
            {
                UserName = registerDetails.UserName,
                FirstName = registerDetails.FirstName,
                LastName = registerDetails.LastName,
                Password = registerDetails.Password,
                PhoneNumber = registerDetails.PhoneNumber,
                Cart = cart
            });
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        return null;
    }


    public Task<User?> CheckUsername(string username)
    {
        return _context.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
    }

    public async Task<string?> LoginUser(LoginRequest loginDetails)
    {
        var user = await _context.Users.Where(u => u.UserName == loginDetails.Username && u.Password == loginDetails.Password).FirstOrDefaultAsync();
        if (user == null)
        {
            return null;
        }
        return _tokenGenerator.GenerateToken(user);
    }

    public async Task<bool> AddCartItem(NewCartItem newCartItem, User user)
    {
        var item = await _itemService.GetItemById(newCartItem.ItemId);
        if (item != null)
        {
            if (item.Quantity < newCartItem.Quantity)
            {
                throw new QuantityError("Available quantity is less than specefied quantity");
            }
            var cartItem = GetCartItemByItemId(newCartItem.ItemId, user);
            if (cartItem != null)
            {
                cartItem.Quantity += newCartItem.Quantity;
            }
            else
            {
                var addNew = new CartItem
                {
                    ItemId = newCartItem.ItemId,
                    Item = item,
                    Date = DateTime.Now.ToUniversalTime(),
                    User = user,
                    Quantity = newCartItem.Quantity
                };
                user.Cart.CartItems.Add(addNew);
            }
            item.Quantity -= newCartItem.Quantity;
            return true;
        }
        return false;
    }

    public Task<User?> GetUserById(int userId)
    {
        return _context.Users.Include(u => u.Cart.CartItems)
        .ThenInclude(ci => ci.Item).Where(u => u.Id == userId).FirstOrDefaultAsync();
    }

    public CartItem? GetCartItemByItemId(int itemId, User user)
    {
        var cartItem = user.Cart.CartItems.FirstOrDefault(ci => ci.Item.Id == itemId);
        if (cartItem == null)
        {
            return null;
        }
        return cartItem;
    }

    public Task Save()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<bool> RemoveCartItem(int cartItemId, User user)
    {
        var cartItem = await _cartItemService.GetCartItemByUser(cartItemId, user.Id);
        if (cartItem != null)
        {
            cartItem.Item.Quantity += cartItem.Quantity;
            user.Cart.CartItems.Remove(cartItem);
            return true;
        }
        return false;
    }

    public async Task<List<CartItem>?> GetCartItems(int userId)
    {
        var user = await GetUserById(userId);
        if (user != null)
        {
            return user.Cart.CartItems.ToList();
        }
        return null;
    }

}
