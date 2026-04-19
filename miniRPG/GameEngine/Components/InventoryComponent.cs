using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.Components;

public class InventoryComponent
{
    public Texture Sprite;
    public Inventory Inventory;
    public bool IsOpen = false;
    public int row = 1;
    public int collum = 1;

    public InventoryComponent()
    {
        if (TextureDatabase.Contains("inventory_open"))
        {
            Sprite = TextureDatabase.Get("inventory_open");
        }

        Inventory = new Inventory();
    }
}