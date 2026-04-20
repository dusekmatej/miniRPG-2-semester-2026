
using System.Windows.Input;
using Microsoft.VisualBasic.Devices;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace miniRPG.Forms;

// using System.Windows.Forms.Integration;
using Helpers;
using GameEngine.Rendering;

public partial class MainForm : Form
{
    private Game.Game _game = new();
    
    public MainForm()
    {
        InitializeComponent();
        
        _game.Initialize(ClientSize.Width, ClientSize.Height);
    }
    
    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var context = new RenderContext
        {
            Graphics = e.Graphics,
            ScreenWidth = ClientSize.Width,
            ScreenHeight = ClientSize.Height,
        };

        _game.Render(context);
    }

    private void Update(object? sender, EventArgs e)
    {
        Task.Run(() => _game.Update());
        Invalidate();
    }
    
    #region Helpers
    // These methods provide information for my helper classes
    private void MainForm_KeyDown(object sender, KeyEventArgs e) 
    {
        Keyboard.KeyDown(e.KeyCode);
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e) 
    {
        Keyboard.KeyUp(e.KeyCode);
    }
    #endregion

    private void MainForm_MouseMove(object sender, MouseEventArgs e)
    {
        var x = e.X;
        var y = e.Y;
        
            MouseHelper.GetMouseX(x);
            MouseHelper.GetMouseY(y);
    }


    private void MainForm_MouseEnter(object sender, EventArgs e)
    {
        
    }

    private void MainForm_MouseClick(object sender, MouseEventArgs e)
    {
        
        
    }


}