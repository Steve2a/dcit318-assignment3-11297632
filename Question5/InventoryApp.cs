using System;

public class InventoryApp
{
    private Logger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new Logger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 5, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Keyboard", 15, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Mouse", 25, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Monitor", 10, DateTime.Now));
        _logger.Add(new InventoryItem(5, "USB Cable", 50, DateTime.Now));
    }

    public void SaveData() => _logger.SaveToFile();

    public void LoadData() => _logger.LoadFromFile();

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Qty: {item.Quantity}, Added: {item.DateAdded}");
        }
    }
}
