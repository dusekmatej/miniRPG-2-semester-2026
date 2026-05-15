namespace miniRPG.Helpers;

public static class Keyboard
{
    private static HashSet<Keys> _currentKeys = new HashSet<Keys>();
    private static HashSet<Keys> _previousKeys = new HashSet<Keys>();
    private static Keys _currentlyPressedKey = Keys.None;

    private static List<Keys> _rework = new();
    private static List<Keys> _reworkPrev = new();

    // Call this at the START of every Update() frame
    public static void Update()
    {
        _previousKeys = new HashSet<Keys>(_currentKeys);
        _reworkPrev.Clear();
    }

    public static void KeyDown(Keys key)
    {
        _currentKeys.Add(key);
        _currentlyPressedKey = key;
        
        _rework.Add(key);
    }

    public static void KeyUp(Keys key)
    {
        _currentKeys.Remove(key);
        _currentlyPressedKey = Keys.None;

        _rework.Remove(key);
    }

    // Held down (good for movement)
    public static bool IsKeyDown(Keys key)
    {
        return _currentKeys.Contains(key);
    }

    public static bool IsPressed(Keys key)
    {
        return _rework.Contains(key) && !_reworkPrev.Contains(key);
    }

    // Fires exactly once when first pressed (good for interact, open menu, etc.)
    public static bool IsKeyJustPressed(Keys key)
    {
        return _currentKeys.Contains(key) && !_previousKeys.Contains(key);
    }

    public static Keys GetPressedKey()
    {
        return _currentlyPressedKey;
    }
}