using shoppingcart.Dto;
using shoppingcart.Models;

namespace shoppingcart.Services.ItemService;

public interface IItemService
{
    Task<Item?> GetItemById(int itemId);
    Task<Item?> GetItemByName(string itemName);
    Task<List<Item>> GetAllItems();
    Task<Item?> AddItem(NewItem newItem);
    Task Save();
    Task<bool> RemoveItem(int itemId);
}