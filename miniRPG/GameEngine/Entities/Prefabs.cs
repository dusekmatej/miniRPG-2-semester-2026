using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.DataObjects;

namespace miniRPG.GameEngine.Entities;

public static class Prefabs
{
    public static Entity CreateBronzeRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent(100) { MaxHealth = 100, Type = OreType.Bronze, XpAward = 15});
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("bronze_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    public static Entity CreateCoalRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100) { MaxHealth = 100,Type = OreType.Coal, XpAward = 10 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("coal_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    public static Entity CreateIronRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Iron, XpAward = 35 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("iron_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    public static Entity CreateGoldRock(float posX, float posY)
    {
        var e = new Entity();
        
        e.AddComponent(new OreComponent (100){ MaxHealth = 100,Type = OreType.Gold, XpAward = 100 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(new TextureMultipleComponent { Textures = TextureDatabase.GetAnimation("gold_rock"), CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable { Radius = 100 });
        
        return e;
    }
    
    
    
}