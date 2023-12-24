using Microsoft.EntityFrameworkCore;
using shoppingcart.Data;
using shoppingcart.Dto;
using shoppingcart.Models;

namespace shoppingcart.Services.ItemService;

public class ItemService : IItemService
{
    private readonly AppDbContext _context;

    public ItemService(AppDbContext context)
    {
        _context = context;
    }


    public Task<List<Item>> GetAllItems()
    {
        return _context.Items.ToListAsync();
    }

    public async Task<Item?> GetItemById(int itemId)
    {
        return await _context.Items.FindAsync(itemId);
    }

    public Task<Item?> GetItemByName(string itemName)
    {
        return _context.Items.Where(i => i.Name == itemName).FirstOrDefaultAsync(); ;
    }

    public Task Save()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<Item?> AddItem(NewItem newItem)
    {
        var item = await GetItemByName(newItem.Name);
        if (item != null)
        {
            return null;
        }
        var result = await _context.Items.AddAsync(new Item
        {
            Name = newItem.Name,
            Price = newItem.Price,
            Quantity = newItem.Quantity
        });

        return result.Entity;
    }
    public async Task<bool> RemoveItem(int itemId)
    {
        var item = await GetItemById(itemId);
        if (item != null)
        {
            _context.Items.Remove(item);
            return true;
        }
        return false;
    }
}