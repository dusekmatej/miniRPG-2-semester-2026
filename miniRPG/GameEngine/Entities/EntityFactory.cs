using System.Linq;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Services;

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    private static readonly Lazy<List<Image>> PlayerAnimationIdle = new(() =>
    {
        var imags = AnimationService.LoadAllDirImages("Resources/Idle");
        return imags ?? new List<Image>();
    });
        
    public static Entity CreatePlayer()
    {
        var player = new Entity();
        player.AddComponent(new PlayerComponent { Stone = 100 });
        player.AddComponent(new PositionComponent { X = 0, Y = 0 });
        player.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        player.AddComponent(new HealthComponent { Health = 100 });

        // Set AnimationFrames and initialize CurrentFrame to first frame (or null if none)
        var frames = PlayerAnimationIdle.Value ?? new List<Image>();
        var current = frames.FirstOrDefault();
        player.AddComponent(new AnimationComponent { CurrentFrameIndex = 0, CurrentFrame = current, AnimationFrames = frames});

        return player;
    }
}