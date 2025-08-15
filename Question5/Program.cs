using System;

class Program
{
    static void Main()
    {
        string filePath = "inventory.json";

    InventoryApp app = new InventoryApp(filePath);
        app.SeedSampleData();
        app.SaveData();

        Console.WriteLine("\n--- New Session ---\n");
        InventoryApp newAppSession = new InventoryApp(filePath);
        newAppSession.LoadData();
        newAppSession.PrintAllItems();
    }
}
