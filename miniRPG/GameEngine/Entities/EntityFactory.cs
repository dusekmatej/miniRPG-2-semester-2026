using System.Drawing;
using System.Linq;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Models;
using miniRPG.Services;

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    private static readonly Lazy<List<Image>> PlayerAnimationIdle = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/Idle");
        return imgs ?? new List<Image>();
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsUp = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_up");
        return imgs ?? new List<Image>();
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsDown = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_down");
        return imgs ?? new List<Image>();
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsRight = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_right");
        return imgs ?? new List<Image>();
    });

    private static readonly Lazy<List<Image>> PlayerAnimationLeft = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_left");
        return imgs ?? new List<Image>();
    });
        
    public static Entity CreatePlayer()
    {
        var player = new Entity();
        // Add components to the player entity
        player.AddComponent(new PlayerComponent { Stone = 100 });
        player.AddComponent(new PositionComponent { X = 0, Y = 0 });
        player.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        player.AddComponent(new HealthComponent { Health = 100 });

        // ensure the player has a camera component so rendering systems can find it
        player.AddComponent(new Camera { X = 0, Y = 0 });

        // Set AnimationFrames and initialize CurrentFrame to first frame (or null if none)
        var framesIdle = PlayerAnimationIdle.Value ?? [];
        var framesUp = PlayerAnimationsUp.Value ?? [];
        var framesDown = PlayerAnimationsDown.Value ?? [];
        var framesRight = PlayerAnimationsRight.Value ?? [];
        var framesLeft = PlayerAnimationLeft.Value ?? [];
        
        var current = framesIdle.FirstOrDefault();
        // player.AddComponent(new AnimationComponent { CurrentFrameIndex = 0, CurrentFrame = current, AnimationFramesIdle = framesIdle, IsRunningUp = false});
        player.AddComponent(
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
        
        return player;
    }
    
    public static Entity CreateCamera()
    {
        var camera = new Entity();
        camera.AddComponent(new Camera());
        return camera;
    }
}