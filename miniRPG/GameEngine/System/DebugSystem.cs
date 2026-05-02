using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class DebugSystem
{
    public void Update()
    {
        if (Keyboard.GetPressedKey() == Keys.F3)
        {
            Console.WriteLine("F3");
            Flags.IsGraphicDebug = !Flags.IsGraphicDebug;
        }
    }
}