using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Core.Events;

public class PlayerDeathEvent
{
    public Entity Player { get; }

    public PlayerDeathEvent(Entity player)
    {
        Player = player;
    }
}

