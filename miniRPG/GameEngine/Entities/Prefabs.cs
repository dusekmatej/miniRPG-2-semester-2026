using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.Entities;

public static class Prefabs
{
    public static Entity CreateBronzeRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent(100) { MaxHealth = 100, Type = OreType.Bronze, XpAward = 15});
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 64, Height = 64 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("bronze_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable(100));
        
        return e;
    }
    
    public static Entity CreateCoalRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100) { MaxHealth = 100,Type = OreType.Coal, XpAward = 10 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 64, Height = 64 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("coal_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable(100));
        
        return e;
    }
    
    public static Entity CreateIronRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Iron, XpAward = 35 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 64, Height = 64 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("iron_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable(100));

        
        return e;
    }
    
    public static Entity CreateGoldRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Gold, XpAward = 100 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 64, Height = 64 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("gold_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable(100));

        
        return e;
    }
    public static Entity CreateHealingItem(float posX, float posY, string itemDatabaseName)
    { 
        var e = new Entity();

        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 32, Height = 32 });
        e.AddComponent(new TextureMultipleComponent{ Textures = new[] { TextureDatabase.Get("small_health_potion") }, CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable(100));

        e.AddComponent(new ItemPickupComponent { Item = ItemDatabase.Get("small_health_potion") });

        return e;
    }
    public static Entity CreateChest(float posX, float posY)
    {
        var e = new Entity();

        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 50, Height = 50 });
        e.AddComponent(new TextureMultipleComponent { Textures = new[] { TextureDatabase.Get("small_iron_chest") }, CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable(100));
        e.AddComponent(new ChestComponent());

        return e;
    }

    public static Entity CreateTree(float x, float y, int variation)
    {
        var e = new Entity();
        
        e.AddComponent(new TransformComponent { X = x, Y = y, Width = 64 * 5, Height = 64 * 5 });
        e.AddComponent(new TextureMultipleComponent{ Textures = new[] { TextureDatabase.Get($"tree{variation}")}});
        e.AddComponent(new Interactable(200));
        e.AddComponent(new TreeComponent(variation, 100));
        
        return e;
    }
    
    
    
}