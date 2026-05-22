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
            if (!e.HasComponent<HealthBarComponent>())
                continue;

            var healthBar = e.GetComponent<HealthBarComponent>();

            if (healthBar == null)
                continue;

            if (Keyboard.GetPressedKey() == Keys.K)
            {
                healthBar.CurrentHealth += 10;
                if (healthBar.CurrentHealth > healthBar.MaxHealth)
                {
                    healthBar.CurrentHealth = healthBar.MaxHealth;
                }
            }

            if (Keyboard.GetPressedKey() == Keys.O)
            {
                healthBar.CurrentHealth -= 10;
                if (healthBar.CurrentHealth < 0)
                {
                    healthBar.CurrentHealth = 0;
                }
            }
        }
    }
}