namespace miniRPG.GameEngine.System;

using Components;
using Core;
using Helpers;


public class StatBarSystem
{
    public void Update(World world)
    {
        foreach (var e in world.Entities)
        {
            if (!e.HasComponent<StatisticBarComponent>())
                continue;

            var comp = e.GetComponent<StatisticBarComponent>();

            if (comp == null)
                continue;

            if (Keyboard.GetPressedKey() == Keys.K)
            {
                comp.CurrentValue += comp.ModifyBy;
                if (comp.CurrentValue > comp.MaxValue)
                {
                    comp.CurrentValue = comp.MaxValue;
                }
            }

            if (Keyboard.GetPressedKey() == Keys.O)
            {
                comp.CurrentValue -= comp.ModifyBy;
                if (comp.CurrentValue < comp.MinValue)
                {
                    comp.CurrentValue = comp.MinValue;
                }
            }
            
        }
    }
}