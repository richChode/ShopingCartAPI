using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoppingcart.Dto;
using shoppingcart.Models;
using shoppingcart.Services.CartItemService;

namespace shoppingcart.Controllers;
[ApiController]
[Route("carts")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet("allcartitems")]
    public async Task<ActionResult<List<CartItem>>> GetAllCartItems([FromQuery] int? userId, [FromQuery] string? phoneNumber, [FromQuery] int? quantity, [FromQuery] int? itemId, [FromQuery] string? date)
    {

        var query = _cartItemService.GetAllCartItemsAsQuery();
        if (!string.IsNullOrEmpty(phoneNumber))
        {
            query = query.Where(ci => ci.User.PhoneNumber == phoneNumber);
        }
        if (userId != null)
        {
            query = query.Where(ci => ci.User.Id == userId);
        }
        if (quantity != null)
        {
            query = query.Where(ci => ci.Quantity == quantity);
        }
        if (itemId != null)
        {
            query = query.Where(ci => ci.ItemId == itemId);
        }
        if (!string.IsNullOrEmpty(date))
        {
            var targetDate = DateTime.Parse(date);
            query = query.Where(ci => ci.Date == targetDate.ToUniversalTime());
        }

        var cartIterms = await query.ToListAsync();
        var newiterms = cartIterms.Select(ci => new CartItemResponse
        {
            CartItemId = ci.Id,
            ItemId = ci.ItemId,
            Quantity = ci.Quantity,
            ItemName = ci.Item.Name,
            Username = ci.User.UserName,
            PhoneNumber = ci.User.PhoneNumber,
            Date = ci.Date
        });
        return Ok(newiterms);
    }
    [HttpGet("getcartitem/{id}")]
    public async Task<ActionResult<CartItem?>> GetCartItem(int id)
    {
        var result = await _cartItemService.GetCartItem(id);
        return Ok(result);
    }
}