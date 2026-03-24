using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Entities;

public static class EntityFactory
{
    // ReSharper disable once InconsistentNaming
    private const string BASE_CHARACTER = "Resources/Character";
    private const string BASE_TERRAIN = "Resources/Terrain";
    
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

    private static readonly string BASE_BIOME_TILES = $"{BASE_TERRAIN}/naturalMaterials/biomeBlocks";
    
    private static readonly Image RockImage;
    private static readonly Image GrassImage;
    private static readonly Image StoneImage;
    private static readonly Image WaterImage;
    static EntityFactory()
    {
        RockImage = ImageLoader.LoadImage($"{BASE_TERRAIN}/naturalMaterials/rock/frame_001.png");
        GrassImage = ImageLoader.LoadImage($"{BASE_BIOME_TILES}/grass1.png");
        StoneImage = ImageLoader.LoadImage($"{BASE_BIOME_TILES}/stone!.png");
        WaterImage = ImageLoader.LoadImage($"{BASE_BIOME_TILES}/Water.png");
    }
    
    public static Entity CreatePlayer()
    {
        var player = new Entity();
        // Add components to the player entity
        player.AddComponent(new PlayerComponent { Stone = 100 });
        player.AddComponent(new PositionComponent { X = 0, Y = 0 });
        player.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        player.AddComponent(new HealthComponent { Health = 100 });
        
        // Set AnimationFrames and initialize CurrentFrame to first frame (or null if none)
        var framesIdle = PlayerAnimationIdle.Value ?? [];
        var framesUp = PlayerAnimationsUp.Value ?? [];
        var framesDown = PlayerAnimationsDown.Value ?? [];
        var framesRight = PlayerAnimationsRight.Value ?? [];
        var framesLeft = PlayerAnimationLeft.Value ?? [];
        
        var current = framesIdle.FirstOrDefault();
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

    public static Entity CreateGrassEntity(float X, float Y)
    {
        var entity = new Entity();
        entity.AddComponent(new PositionComponent { X = X, Y = Y});
        entity.AddComponent(new TextureComponent { Image = GrassImage});

        return entity;
    }    
    
    public static Entity CreateStoneEntity(float X, float Y)
    {
        var entity = new Entity();
        entity.AddComponent(new PositionComponent { X = X, Y = Y});
        entity.AddComponent(new TextureComponent { Image = StoneImage});

        return entity;
    }    
    
    public static Entity CreateWaterEntity(float X, float Y)
    {
        var entity = new Entity();
        entity.AddComponent(new PositionComponent { X = X, Y = Y});
        entity.AddComponent(new TextureComponent { Image = WaterImage});

        return entity;
    }

    public static Entity TestEntity()
    {
        var entity = new Entity();
        entity.AddComponent(new PositionComponent { X = 100, Y = 100 });
        entity.AddComponent(new TextureComponent { Image = RockImage });

        return entity;
    }
}