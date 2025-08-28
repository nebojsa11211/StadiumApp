using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Customer.Services;

public interface ICartService
{
    List<CartItem> Items { get; }
    event Action? OnCartChanged;
    void AddItem(DrinkDto drink, int quantity = 1, string? specialInstructions = null);
    void UpdateQuantity(int drinkId, int quantity);
    void RemoveItem(int drinkId);
    void ClearCart();
    decimal GetTotalPrice();
    int GetTotalItems();
}

public class CartItem
{
    public DrinkDto Drink { get; set; } = null!;
    public int Quantity { get; set; }
    public string? SpecialInstructions { get; set; }
    public decimal TotalPrice => Drink.Price * Quantity;
}

public class CartService : ICartService
{
    private readonly List<CartItem> _items = new();

    public List<CartItem> Items => _items;

    public event Action? OnCartChanged;

    public void AddItem(DrinkDto drink, int quantity = 1, string? specialInstructions = null)
    {
        var existingItem = _items.FirstOrDefault(i => i.Drink.Id == drink.Id && i.SpecialInstructions == specialInstructions);
        
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem
            {
                Drink = drink,
                Quantity = quantity,
                SpecialInstructions = specialInstructions
            });
        }

        OnCartChanged?.Invoke();
    }

    public void UpdateQuantity(int drinkId, int quantity)
    {
        var item = _items.FirstOrDefault(i => i.Drink.Id == drinkId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                _items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            OnCartChanged?.Invoke();
        }
    }

    public void RemoveItem(int drinkId)
    {
        var item = _items.FirstOrDefault(i => i.Drink.Id == drinkId);
        if (item != null)
        {
            _items.Remove(item);
            OnCartChanged?.Invoke();
        }
    }

    public void ClearCart()
    {
        _items.Clear();
        OnCartChanged?.Invoke();
    }

    public decimal GetTotalPrice()
    {
        return _items.Sum(i => i.TotalPrice);
    }

    public int GetTotalItems()
    {
        return _items.Sum(i => i.Quantity);
    }
}

