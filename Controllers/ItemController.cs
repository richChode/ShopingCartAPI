using Microsoft.AspNetCore.Mvc;
using shoppingcart.Data;
using shoppingcart.Dto;
using shoppingcart.Models;
using shoppingcart.Services.CartItemService;
using shoppingcart.Services.ItemService;

namespace shoppingcart.Controllers;
[ApiController]
[Route("items")]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly ICartItemService _cartItemService;

    public ItemController(AppDbContext context, IItemService itemService, ICartItemService cartItemService)
    {
        _itemService = itemService;
        _cartItemService = cartItemService;
    }


    [HttpGet("list")]
    public async Task<ActionResult> GetAllItems()
    {
        var items = await _itemService.GetAllItems();
        return Ok(items);
    }

    [HttpPost("additem")]
    public async Task<ActionResult<Item>> AddItem([FromBody] NewItem newItem)
    {

        var item = await _itemService.AddItem(newItem);
        if (item != null)
        {
            await _itemService.Save();
            return Ok(item);
        }
        return Conflict("Item already exists");

    }

    [HttpDelete("{itemId}/remove")]
    public async Task<IActionResult> RemoveItem(int itemId)
    {
        var result = await _itemService.RemoveItem(itemId);
        await _cartItemService.RemoveItemFromCartItems(itemId);
        if (result)
        {
            await _itemService.Save();
            return NoContent();
        }
        return NotFound("Item not found");

    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<Item?>> GetItem(int itemId)
    {
        var item = await _itemService.GetItemById(itemId);
        if (item != null)
        {
            return Ok(item);
        }
        return NotFound("Item not found");
    }
}