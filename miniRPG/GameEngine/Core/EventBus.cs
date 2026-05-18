namespace miniRPG.GameEngine.Core;

// PROMPT: Based on my current architecture, explain to me how can I create
// a good system for rock breaking and leveling systems. I would want to use Events or Delegates

// PROMPT 2: Explain to me in depth what are delegates I don't
// get them and everything I need to understand to implement it

public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    public void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (!_subscribers.TryGetValue(eventType, out var listeners))
        {
            listeners = new List<Delegate>();
            _subscribers[eventType] = listeners;
        }
        
        listeners.Add(listener);
    }

    public void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (_subscribers.TryGetValue(eventType, out var listeners))
            listeners.Remove(listener);
    }

    public void Post<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (_subscribers.TryGetValue(eventType, out var listeners))
            foreach (var listener in listeners)
                ((Action<T>)listener).Invoke(eventData);
    }
}