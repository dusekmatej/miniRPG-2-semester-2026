using System.Windows.Input;

namespace miniRPG.Helpers;

public static class MouseHelper
{
    public static event Action OnLeftClick;
    public static event Action OnRightClick;
    
    public static int GetMouseX(int x) => x;

    public static int GetMouseY(int y) => y;

    public static void ProcessClick(MouseButtons m)
    {
        if (m == MouseButtons.Left)
        {
            Console.WriteLine("LeftClick");
            OnLeftClick?.Invoke();
        }
            
        else if (m == MouseButtons.Right)
        {
            Console.WriteLine("RightClick");
            OnRightClick?.Invoke(); 
        }
    }
}