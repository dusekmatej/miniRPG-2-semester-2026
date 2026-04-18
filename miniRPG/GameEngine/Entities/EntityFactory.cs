using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.InventoryEssentials;

// ReSharper disable InconsistentNaming

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    public static Entity CreatePlayer(float posX = 0, float posY = 0)
    {
        var e = new Entity();
        
        // Add components to the player entity
        e.AddComponent(new PlayerComponent { Stone = 100 });
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 150, Height = 150 });
        e.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        e.AddComponent(new Inventory());
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

    public static Entity TestInteractable(float posX, float posY)
    {
        var e = new Entity();
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 98, Height = 98 });
        e.AddComponent(TextureDatabase.Get("bronze_rock"));
        e.AddComponent(new Interactable());
        
        return e;
    }

    public static Entity HealthBar(int windowWidth, int windowHeight)
    {

        var e = new Entity();
        e.AddComponent(new UiComponent { X = windowWidth - 150, Y = windowHeight - (windowHeight / 2) - 200, Width = 100, Height = 30 });
        e.AddComponent(new StatisticBarComponent { StatBarColor = Brushes.Red, CurrentValue = 100 });
        
        return e;
    }
}