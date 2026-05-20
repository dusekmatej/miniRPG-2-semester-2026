namespace miniRPG.GameEngine.Core.Events;

// Here you can define empty events that can be used as triggers
// For systems that don't need to pass any data, for example - keyboard input detection, toggling inventory

public interface IEmptyEvents { }
public class InventoryToggleEvent : IEmptyEvents { }
public class RandomInventoryEvent : IEmptyEvents { }