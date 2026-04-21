using System.Windows.Forms;

namespace miniRPG.Helpers;

public static class MouseHelper
{
    private static HashSet<MouseButtons> pressedButtons = new HashSet<MouseButtons>();
    private static int windowX, windowY, windowWidth, windowHeight;
    private static bool leftClicked = false;
    private static int clickX, clickY;

    public static void SetWindow(int x, int y, int w, int h)
    {
        windowX = x;
        windowY = y;
        windowWidth = w;
        windowHeight = h;
    }

    public static void MouseDown(MouseButtons button)
    {
        pressedButtons.Add(button);
        if (button == MouseButtons.Left)
        {
            leftClicked = true;
            clickX = GetMouseX();
            clickY = GetMouseY();
        }
    }

    public static void MouseUp(MouseButtons button)
    {
        pressedButtons.Remove(button);
    }

    public static bool IsMouseDown(MouseButtons button)
    {
        return pressedButtons.Contains(button);
    }

    public static int GetMouseX()
    {
        return Cursor.Position.X - windowX;
    }

    public static int GetMouseY()
    {
        return Cursor.Position.Y - windowY;
    }

    public static bool WasLeftClicked(out int x, out int y)
    {
        if (leftClicked)
        {
            x = clickX;
            y = clickY;
            leftClicked = false;
            return true;
        }
        x = 0;
        y = 0;
        return false;
    }

    public static void ProcessClick(MouseButtons button)
    {
        MouseDown(button);
    }
}