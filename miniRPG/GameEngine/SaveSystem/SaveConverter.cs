// GameEngine/SaveSystem/SaveConverter.cs
using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.DataObjects;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.InventoryEssentials;

namespace miniRPG.GameEngine.SaveSystem;

public class SaveConverter
{
    // --- Collecting save data from live entities ---

    public SaveData CollectSaveData(World world, int seed)
    {
        var player = world.PlayerEntity;

        return new SaveData
        {
            WorldSeed = seed,
            Player = CollectPlayerData(player),
            WorldEntities = CollectWorldEntities(world)
        };
    }
    private PlayerSaveData CollectPlayerData(Entity player)
    {
        var data = new PlayerSaveData();

        // Position
        var transform = player.GetComponent<TransformComponent>();
        if (transform != null)
        {
            data.X = transform.X;
            data.Y = transform.Y;
        }

        // Health
        var health = player.GetComponent<HealthBarComponent>();
        if (health != null)
            data.CurrentHealth = health.CurrentHealth;

        // Inventory slots
        var inventoryComp = player.GetComponent<InventoryComponent>();
        if (inventoryComp != null)
        {
            for (int i = 0; i < inventoryComp.Inventory.Slots.Length; i++)
            {
                var slot = inventoryComp.Inventory.Slots[i];
                if (slot?.Item == null) continue; // skip empty slots

                data.InventorySlots.Add(new InventorySlotSaveData
                {
                    SlotIndex = i,
                    ItemDatabaseName = slot.Item.DatabaseName,
                    Amount = slot.Amount
                });
            }
        }

        // Hotbar slots
        var hotbar = player.GetComponent<HotbarComponent>();
        if (hotbar != null)
        {
            for (int i = 0; i < hotbar.Slots.Length; i++)
            {
                var slot = hotbar.Slots[i];
                if (slot?.Item == null) continue;

                data.HotbarSlots.Add(new InventorySlotSaveData
                {
                    SlotIndex = i,
                    ItemDatabaseName = slot.Item.DatabaseName,
                    Amount = slot.Amount
                });
            }
        }

        // Skills
        var skills = player.GetComponent<SkillsComponent>();
        if (skills != null)
        {
            foreach (var kvp in skills.Skills)
            {
                data.Skills[kvp.Key.ToString()] = new SkillSaveData
                {
                    Level = kvp.Value.Level,
                    CurrentExperience = kvp.Value.CurrentExperience
                };
            }
        }

        return data;
    }
    private List<EntitySaveData> CollectWorldEntities(World world)
    {
        var entities = new List<EntitySaveData>();

        foreach (var e in world.Entities)
        {
            if (e.HasComponent<PlayerComponent>() || e.HasComponent<Camera>())
                continue;

            var transform = e.GetComponent<TransformComponent>();
            if (transform == null) continue;

            // Convert pixel coords back to tile coords for saving
            int tileX = (int)(transform.X / TileDatabase.TileSize);
            int tileY = (int)(transform.Y / TileDatabase.TileSize);

            if (e.HasComponent<OreComponent>())
            {
                var ore = e.GetComponent<OreComponent>();
                entities.Add(new EntitySaveData
                {
                    EntityType = "ore",
                    TileX = tileX,
                    TileY = tileY,
                    OreType = ore!.Type.ToString(),
                    OreCurrentHealth = ore.CurrentHealth
                });
            }
            else if (e.HasComponent<ChestComponent>())
            {
                entities.Add(new EntitySaveData
                {
                    EntityType = "chest",
                    TileX = tileX,
                    TileY = tileY
                });
            }
            else if (e.HasComponent<ItemPickupComponent>())
            {
                var item = e.GetComponent<ItemPickupComponent>();
                entities.Add(new EntitySaveData
                {
                    EntityType = "item",
                    TileX = tileX,
                    TileY = tileY,
                    ItemDatabaseName = item!.Item.DatabaseName
                });
            }
        }

        return entities;
    }

    // --- Applying loaded data back to live entities ---

    public void ApplySaveData(SaveData save, World world)
    {
        ApplyPlayerData(save.Player, world.PlayerEntity);
        ApplyWorldEntities(save.WorldEntities, world);
    }


    private void ApplyPlayerData(PlayerSaveData data, Entity player)
    {
        // Position
        var transform = player.GetComponent<TransformComponent>();
        if (transform != null)
        {
            transform.X = data.X;
            transform.Y = data.Y;
        }

        // Health
        var health = player.GetComponent<HealthBarComponent>();
        if (health != null)
            health.CurrentHealth = data.CurrentHealth;

        // Inventory — clear first, then refill
        var inventoryComp = player.GetComponent<InventoryComponent>();
        if (inventoryComp != null)
        {
            Array.Clear(inventoryComp.Inventory.Slots, 0, inventoryComp.Inventory.Slots.Length);

            foreach (var slotData in data.InventorySlots)
            {
                var item = ItemDatabase.Get(slotData.ItemDatabaseName);
                inventoryComp.Inventory.Slots[slotData.SlotIndex] = new InventorySlot(item, slotData.Amount);
            }
        }

        // Hotbar
        var hotbar = player.GetComponent<HotbarComponent>();
        if (hotbar != null)
        {
            Array.Clear(hotbar.Slots, 0, hotbar.Slots.Length);

            foreach (var slotData in data.HotbarSlots)
            {
                var item = ItemDatabase.Get(slotData.ItemDatabaseName);
                hotbar.Slots[slotData.SlotIndex] = new InventorySlot(item, slotData.Amount);
            }
        }

        // Skills
        var skills = player.GetComponent<SkillsComponent>();
        if (skills != null)
        {
            foreach (var kvp in data.Skills)
            {
                // Convert the string key back to the enum
                if (Enum.TryParse<SkillType>(kvp.Key, out var skillType) && skills.Skills.ContainsKey(skillType))
                {
                    skills.Skills[skillType].Level = kvp.Value.Level;
                    skills.Skills[skillType].CurrentExperience = kvp.Value.CurrentExperience;
                }
            }
        }
    }
    private void ApplyWorldEntities(List<EntitySaveData> entities, World world)
    {
        // Collect ore positions to block chunk spawner from re-spawning them
        var savedOrePositions = entities
            .Where(e => e.EntityType == "ore")
            .Select(e => (e.TileX, e.TileY))
            .ToList();

        world.ChunkManager.RegisterSavedOrePositions(savedOrePositions);

        foreach (var data in entities)
        {
            // Convert tile coords back to pixel coords
            float pixelX = data.TileX * TileDatabase.TileSize;
            float pixelY = data.TileY * TileDatabase.TileSize;

            Entity? e = data.EntityType switch
            {
                "ore"  => CreateOreFromSave(data, pixelX, pixelY),
                "chest" => EntityFactory.CreateChest(pixelX, pixelY),
                "item" when data.ItemDatabaseName != null => EntityFactory.CreateHealingItem(pixelX, pixelY, data.ItemDatabaseName),
                _ => null
            };

            if (e != null)
                world.Entities.Add(e);
        }
    }

    private Entity? CreateOreFromSave(EntitySaveData data, float pixelX, float pixelY)
    {
        if (data.OreType == null) return null;

        if (!Enum.TryParse<OreType>(data.OreType, out var oreType))
            return null;

        var e = oreType switch
        {
            OreType.Coal   => Prefabs.CreateCoalRock(pixelX, pixelY),
            OreType.Iron   => Prefabs.CreateIronRock(pixelX, pixelY),
            OreType.Bronze => Prefabs.CreateBronzeRock(pixelX, pixelY),
            OreType.Gold   => Prefabs.CreateGoldRock(pixelX, pixelY),
            _ => null
        };

        if (e == null) return null;

        var ore = e.GetComponent<OreComponent>();
        if (ore != null)
            ore.CurrentHealth = data.OreCurrentHealth;

        return e;
    }
    
}