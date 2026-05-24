namespace miniRPG.GameEngine.SaveSystem;

public class SaveData
{
    public int WorldSeed { get; set; }
    public PlayerSaveData Player { get; set; } = new();
    public List<EntitySaveData> WorldEntities { get; set; } = new();
}

public class PlayerSaveData
{
    public float X { get; set; }
    public float Y { get; set; }
    public int CurrentHealth { get; set; }
    public List<InventorySlotSaveData> InventorySlots { get; set; } = new();
    public List<InventorySlotSaveData> HotbarSlots { get; set; } = new();
    public Dictionary<string, SkillSaveData> Skills { get; set; } = new();
}

public class InventorySlotSaveData
{
    public int SlotIndex { get; set; }
    public string ItemDatabaseName { get; set; } = "";
    public int Amount { get; set; }
}

public class SkillSaveData
{
    public int Level { get; set; }
    public int CurrentExperience { get; set; }
}


public class EntitySaveData
{
    public string EntityType { get; set; } = "";
    public int TileX { get; set; }
    public int TileY { get; set; }
    public string? OreType { get; set; }
    public int OreCurrentHealth { get; set; }
    public string? ItemDatabaseName { get; set; }
}