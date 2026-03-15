using System.Linq;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Services;

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    private static readonly Lazy<List<Image>> PlayerAnimationIdle = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/Idle");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsUp = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_up");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsDown = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_down");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationsRight = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_right");
        return imgs ?? [];
    });

    private static readonly Lazy<List<Image>> PlayerAnimationLeft = new(() =>
    {
        var imgs = AnimationService.LoadAllDirImages("Resources/run_left");
        return imgs ?? [];
    });
        
    public static Entity CreatePlayer()
    {
        var player = new Entity();
        player.AddComponent(new PlayerComponent { Stone = 100 });
        player.AddComponent(new PositionComponent { X = 0, Y = 0 });
        player.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        player.AddComponent(new HealthComponent { Health = 100 });

        // Set AnimationFrames and initialize CurrentFrame to first frame (or null if none)
        var framesIdle = PlayerAnimationIdle.Value ?? [];
        var framesUp = PlayerAnimationsRight.Value ?? [];
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
}