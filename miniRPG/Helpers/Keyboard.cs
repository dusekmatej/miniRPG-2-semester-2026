namespace miniRPG.Helpers;

public static class Keyboard
{
    private static readonly List<Keys> _heldKeys = new List<Keys>();
    private static readonly List<Keys> _justPressedKeys = new List<Keys>();
    private static readonly List<Keys> _usedKeys = new List<Keys>();

    public static void KeyDown(Keys key)
    {
        if (!_heldKeys.Contains(key))
        {
            _heldKeys.Add(key);
            _justPressedKeys.Add(key);
        }
    }

    public static void KeyUp(Keys key)
    {
        _heldKeys.Remove(key);
        _justPressedKeys.Remove(key);
        _usedKeys.Remove(key);
    }

    public static bool WasKeyPressed(Keys key)
    {
        if (_usedKeys.Contains(key))
            return false;

        if (_justPressedKeys.Contains(key))
        {
            _usedKeys.Add(key);
            return true;
        }

        return false;
    }

    public static bool IsKeyDown(Keys key)
    {
        return _heldKeys.Contains(key);
    }

    public static Keys GetPressedKey()
    {
        if (_heldKeys.Count == 0)
            return Keys.None;

        return _heldKeys[_heldKeys.Count - 1];
    }
}