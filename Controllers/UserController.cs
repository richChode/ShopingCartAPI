using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shoppingcart.Dto;
using shoppingcart.Models;
using shoppingcart.Services.ItemService;
using shoppingcart.Services.UserService;

namespace shoppingcart.Controllers;
[ApiController]
[Route("/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IItemService _itemService;

    public UserController(IItemService itemService, IUserService userService)
    {
        _itemService = itemService;
        _userService = userService;
    }
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest registerDetails)
    {
        var user = await _userService.CreateUser(registerDetails);
        if (user != null)
        {
            return Created("", user);
        }
        return Conflict("Username already exists");
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginDetails)
    {
        var userToken = await _userService.LoginUser(loginDetails);
        if (userToken != null)
        {
            return Ok(userToken);
        }
        return Unauthorized("Invalid Credentials.");
    }

    [HttpPost("additem")]
    public async Task<ActionResult> AddItemToCart([FromBody] NewCartItem
    newCartItem)
    {
        var user = await GetCurrentUser();
        if (user == null)
        {
            return NotFound("user not found");
        }
        var result = await _userService.AddCartItem(newCartItem, user);
        if (result)
        {
            await _userService.Save();
            return Ok("Item added to cart");
        }
        return NotFound("Can't add item to cart");
    }

    [HttpGet("allitems")]
    public async Task<ActionResult> GetAllItems()
    {
        var user = await GetCurrentUser();
        if (user == null)
        {
            return NotFound("user not found");
        }
        var CartItems = await _userService.GetCartItems(user.Id);
        if (CartItems == null)
        {
            return NotFound("user does not exist");
        }
        var newCartItems = CartItems.Select(ci => new
        {
            ci.Id,
            ci.Item.Name,
            ci.Quantity
        }).ToList();
        return Ok(newCartItems);
    }
    [HttpDelete("removeitem/{cartItemId}")]
    public async Task<ActionResult> RemoveItem(int cartItemId)
    {
        var user = await GetCurrentUser();
        if (user != null)
        {
            var result = await _userService.RemoveCartItem(cartItemId, user);
            if (result)
            {
                await _userService.Save();
            }
            return NoContent();
        }
        return NotFound("user not found");
    }

    private async Task<User?> GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            var userClaims = identity.Claims;
            var userIdStr = userClaims.FirstOrDefault(o => o.Type == "userId")?.Value;
            if (userIdStr != null)
            {
                var userId = int.Parse(userIdStr);
                return await _userService.GetUserById(userId);
            }
        }
        return null;
    }
}