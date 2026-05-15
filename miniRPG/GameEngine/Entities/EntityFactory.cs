using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

// ReSharper disable InconsistentNaming

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    private static readonly Inventory MainInventory;
    
    static EntityFactory()
    {
        MainInventory = new Inventory();
    }
    
    public static Entity CreatePlayer(float posX = 0, float posY = 0, int windowWidth = 0, int windowHeight = 0)
    {
        var e = new Entity();
        
        // Add components to the player entity
        e.AddComponent(new PlayerComponent { Stone = 100 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 150, Height = 150 });
        e.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        e.AddComponent(new UiComponent());

        e.AddComponent(new InventoryComponent
        {
            Inventory = MainInventory,
            X = 20, Y = 20, Width = 200, Height = 160, 
            InventorySprite = TextureDatabase.Get("inventory_open"),
        } );
        
        e.AddComponent(new HotbarComponent
        {
            Inventory = MainInventory,
            X = 0, Y = 0, 
            Width = 250, Height = 50,
            SlotOffsetX = 40,
            SlotOffsetY = 50,
            HotbarSprite = TextureDatabase.Get("inventory_hotbar"),
        });
        
        e.AddComponent(
            new AnimationComponent
            {
                CurrentFrameIndex = 0,
                CurrentFrame = TextureDatabase.GetAnimation("idle").FirstOrDefault(),
                
                IsIdle = true,
                IsRunningUp = false,
                IsRunningDown = false,
                IsRunningRight = false,
                IsRunningLeft = false,
                
                AnimationFramesIdle = TextureDatabase.GetAnimation("idle") ?? [],
                AnimationFramesUp = TextureDatabase.GetAnimation("run_up") ?? [],
                AnimationFramesDown = TextureDatabase.GetAnimation("run_down") ?? [],
                AnimationFramesRight = TextureDatabase.GetAnimation("run_right") ?? [],
                AnimationFramesLeft = TextureDatabase.GetAnimation("run_left") ?? [],
            });
        
        return e;
    }
    
    public static Entity CreateCamera(float playerPosX = 0f, float playerPosY = 0f)
    {
        var e = new Entity();
        e.AddComponent(new Camera { X = playerPosX, Y = playerPosY } );

        return e;
    }

    public static Entity HealthBar(int windowWidth, int windowHeight)
    {
        var e = new Entity();
     
        e.AddComponent(new StatisticBarComponent { StatBarColor = Brushes.Red, CurrentValue = 100, X = windowWidth - 150, Y = windowHeight - (windowHeight / 2) - 200, Width = 100, Height = 30  });
        
        return e;
    }
}