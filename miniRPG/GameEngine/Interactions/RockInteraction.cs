using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Interactions;

public static class RockInteraction
{
    public static void BreakRock(World world, Entity entity)
    {
        if (entity.HasComponent<OreComponent>())
        {
            var ore = entity.GetComponent<OreComponent>();
            var inventory = world.PlayerEntity.GetComponent<InventoryComponent>();
            var levelComponent = world.PlayerEntity.GetComponent<LevelComponent>();

            if (inventory == null) throw new Exception("Inventory component is null!");
            if (ore == null) throw new Exception("No ore component found!");

            switch (ore.Type)
            {
                case OreType.Bronze:
                    inventory.Inventory.Add("bronze_ore");
                    break;
                case OreType.Coal:
                    inventory.Inventory.Add("coal_ore");
                    break;
                default:
                    throw new Exception("Unknown ore type!");
            }
            world.RemoveEntity(entity);
        }
    }
}