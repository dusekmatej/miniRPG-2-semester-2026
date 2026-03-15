namespace miniRPG.Helpers;

public static class Keyboard
{
    private static HashSet<Keys> keyPress = new HashSet<Keys>();
    private static Keys currentlyPressedKey = Keys.None;
    
    public static void KeyDown(Keys key)
    {
        keyPress.Add(key);
        currentlyPressedKey = key;
    }

    public static void KeyUp(Keys key)
    {
        keyPress.Remove(key);
        currentlyPressedKey = Keys.None;
    }

    public static Keys GetPressedKey()
    {
        return currentlyPressedKey;
    }

    public static bool IsKeyDown(Keys key)
    {
        return keyPress.Contains(key);
    }
}