namespace miniRPG.GameEngine.Core;

public class Entity
{
    private Dictionary<Type, object> _components = new();

    public void AddComponent<T>(T component)
    {
        _components[typeof(T)] = component!;
    }

    public bool HasComponent<T>()
    {
        return _components.ContainsKey(typeof(T));
    }

    public T? GetComponent<T>()
    {
        if (_components.TryGetValue(typeof(T), out var component))
            return (T)component;

        return default;
    }
}