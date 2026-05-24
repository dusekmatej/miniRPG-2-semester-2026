using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.DataObjects;
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
        e.AddComponent(new PlayerComponent());
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 150, Height = 150 });
        e.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        e.AddComponent(new SkillsComponent
        {
            Skills = new Dictionary<SkillType, SkillObject>
            {
                { SkillType.Mining, new SkillObject() },
                { SkillType.Woodcutting, new SkillObject() },
                { SkillType.Combat, new SkillObject() },
                { SkillType.Fishing, new SkillObject() }
            },
        });


        e.AddComponent(new InventoryComponent
        {
            Inventory = MainInventory,
            X = 20, Y = 20, Width = 200, Height = 200,
            InventorySprite = TextureDatabase.Get("inventory_open"),
        });

        e.AddComponent(new HotbarComponent
        {
            Width = 250, Height = 50,
            SlotOffsetX = 40,
            SlotOffsetY = 50,
            HotbarSprite = TextureDatabase.Get("inventory_hotbar"),
        });
        e.AddComponent(
            new HealthBarComponent
            {
                CurrentHealth = 100,
                MaxHealth = 100,
                MinHealth = 1,
                X = windowWidth ,
                Y = windowHeight -(windowHeight/2) -150,
                Width = 100,
                Height = 30,
                
                FillColor = Color.LimeGreen,
                BackgroundColor = Color.DarkRed,
                BorderColor = Color.Black,
                ShowText = true,
                FontSize = 10f
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
    e.AddComponent(new NamePlateComponent { Name = "Player", Color = Color.White });
        
        return e;
    }
    
    public static Entity CreateCamera(float playerPosX = 0f, float playerPosY = 0f)
    {
        var e = new Entity();
        e.AddComponent(new Camera { X = playerPosX, Y = playerPosY } );

        return e;
    }

    public static Entity CreateGhost(float posX = 0, float posY = 0, int windowWidth = 0, int windowHeight = 0)
    {
        var e = new Entity();

        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 150, Height = 150 });
        e.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        e.AddComponent(new Interactable(100));
        e.AddComponent(new EnemyComponent
        {
            DetectRange = 100f * 15,
            AttackRange = 100f * 5f,
            ChaseSpeed = 120f,
            StopDistance = 60f,
            CurrentHealth = 200,
            MaxHealth = 200,
            Damage = 50,
            ExperienceReward = 15,
            HealthBarHeight = 10f,
        });
        e.AddComponent(new AnimationComponent
        {
            IsIdle = true,
            AnimationFramesIdle = TextureDatabase.GetAnimation("idle_enemy") ?? [],
            AnimationFramesUp = TextureDatabase.GetAnimation("run_up_enemy") ?? [],
            AnimationFramesDown = TextureDatabase.GetAnimation("run_down_enemy") ?? [],
            AnimationFramesRight = TextureDatabase.GetAnimation("run_right_enemy") ?? [],
            AnimationFramesLeft = TextureDatabase.GetAnimation("run_left_enemy") ?? [],
        });
        
        return e;
    }
    public static Entity CreateChest(float posX, float posY)
    {
        var e = new Entity();

        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 50, Height = 50 });
        e.AddComponent(new TextureMultipleComponent { Textures = new[] { TextureDatabase.Get("small_iron_chest") }, CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable { Radius = 100 });
        e.AddComponent(new ChestComponent());
        
        return e;
    }
    public static Entity CreateHealingItem(float posX, float posY, string databaseName)
    { 
        var e = new Entity();
        
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 32, Height = 32 });
        e.AddComponent(new TextureMultipleComponent{ Textures = new[] { TextureDatabase.Get("small_health_potion") }, CurrentTextureIndex = 0 });
        e.AddComponent(new Interactable { Radius = 100 });
        e.AddComponent(new ItemPickupComponent { Item = ItemDatabase.Get("small_health_potion") });

        return e;
    }

    public static Entity CreateTrader(float posX = 0, float posY = 0)
    {
        var e = new Entity();
        
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 64 * 2, Height = 64 * 2 });
        e.AddComponent(new VelocityComponent { X = 0, Y = 0 });
        e.AddComponent(new Interactable(150));
        e.AddComponent(new TraderWanderComponent(40, 150));
        e.AddComponent(new AnimationComponent
        {
            IsIdle = true,
            AnimationFramesIdle = TextureDatabase.GetAnimation("idle_trader1") ?? [],
            AnimationFramesUp = TextureDatabase.GetAnimation("run_up_trader1") ?? [],
            AnimationFramesDown = TextureDatabase.GetAnimation("run_down_trader1") ?? [],
            AnimationFramesRight = TextureDatabase.GetAnimation("run_right_trader1") ?? [],
            AnimationFramesLeft = TextureDatabase.GetAnimation("run_left_trader1") ?? [],
        });
        
        e.AddComponent(new TraderComponent
        {
            Type = TraderType.Selling, // Set default type to selling
            Items = new Dictionary<Item, int>
            {
                { ItemDatabase.Get("bronze_ore"), 15 },  
                { ItemDatabase.Get("coal_ore"), 30 }, 
                { ItemDatabase.Get("iron_ore"), 40 }, 
            },
        });
        
        return e;
    }

    public static Entity CreateCoin(float posX, float posY)
    {
        var e = new Entity();
        e.AddComponent(new TransformComponent { X = posX, Y = posY, Width = 25, Height = 25 });
        e.AddComponent(new ItemPickupComponent { Item = ItemDatabase.Get("coin") });
        e.AddComponent(new Interactable(150));
        e.AddComponent(TextureDatabase.Get("coin"));

        return e;
    }
}