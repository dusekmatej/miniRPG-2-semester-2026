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
        
        e.AddComponent(new OreComponent(100) { MaxHealth = 100, Type = OreType.Bronze });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(TextureDatabase.Get("bronze_rock"));
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    public static Entity CreateCoalRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Coal });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(TextureDatabase.Get("coal_rock"));
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    public static Entity CreateIronRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Iron });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(TextureDatabase.Get("iron_rock"));
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    public static Entity CreateGoldRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Gold });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(TextureDatabase.Get("gold_rock"));
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
}