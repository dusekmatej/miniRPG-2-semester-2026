using System.IO;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

// ReSharper disable InconsistentNaming

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    private static readonly string APP_BASE = AppDomain.CurrentDomain.BaseDirectory;
    private static readonly string BASE_CHARACTER = Path.Join(APP_BASE, "Art/Character");
    private static readonly string BASE_TERRAIN = Path.Join(APP_BASE, "Art/Terrain");
    
    // Load character frames
    private static readonly Lazy<List<Image>> PlayerAnimationIdle = new(() =>
    {
        var imgs = ImageLoader.LoadAnimation($"{BASE_CHARACTER}/Idle");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsUp = new(() =>
    {
        var imgs = ImageLoader.LoadAnimation($"{BASE_CHARACTER}/run_up");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsDown = new(() =>
    {
        var imgs = ImageLoader.LoadAnimation($"{BASE_CHARACTER}/run_down");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsRight = new(() =>
    {
        var imgs = ImageLoader.LoadAnimation($"{BASE_CHARACTER}/run_right");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationLeft = new(() =>
    {
        var imgs = ImageLoader.LoadAnimation($"{BASE_CHARACTER}/run_left");
        return imgs ?? [];
    });

    
    // Load object frames
    private static readonly Image RockImage;
    
    static EntityFactory()
    {
        // RockImage = ImageLoader.Image($"{BASE_TERRAIN}/rock/frame_001.png");
        RockImage = ImageLoader.Image("Art/Terrain/rock/frame_001.png");
    }
    
    public static Entity CreatePlayer(float posX = 0, float posY = 0)
    {
        var e = new Entity();
        // Add components to the player entity
        e.AddComponent(new PlayerComponent { Stone = 100 });
        e.AddComponent(new TrensformComponent { X = posX, Y = posY, Width = 150, Height = 150 });
        e.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        
        // Set AnimationFrames and initialize CurrentFrame to first frame (or null if none)
        var framesIdle = PlayerAnimationIdle.Value ?? [];
        var framesUp = PlayerAnimationsUp.Value ?? [];
        var framesDown = PlayerAnimationsDown.Value ?? [];
        var framesRight = PlayerAnimationsRight.Value ?? [];
        var framesLeft = PlayerAnimationLeft.Value ?? [];
        
        var current = framesIdle.FirstOrDefault();
        e.AddComponent(
            new AnimationComponent
            {
                CurrentFrameIndex = 0,
                CurrentFrame = current,
                
                IsIdle = true,
                IsRunningUp = false,
                IsRunningDown = false,
                IsRunningRight = false,
                IsRunningLeft = false,
                
                AnimationFramesIdle = framesIdle,
                AnimationFramesUp = framesUp,
                AnimationFramesDown = framesDown,
                AnimationFramesRight = framesRight,
                AnimationFramesLeft = framesLeft,
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
        e.AddComponent(new TrensformComponent { X = posX, Y = posY, Width = 150, Height = 150 });
        e.AddComponent(new TextureComponent { Image = RockImage });
    return e;
    }
}