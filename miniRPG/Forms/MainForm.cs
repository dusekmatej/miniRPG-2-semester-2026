using miniRPG.Helpers;
using miniRPG.GameEngine;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.Other;

namespace miniRPG.Forms;
    
public partial class MainForm : Form
{
    // Game engine loop initialization
    private readonly Engine _engine;
    private long _lastElapsedMs;
    
    public MainForm()
    {
        InitializeComponent();
        
        // Performance optimizations for smoother rendering

        var player = EntityFactory.CreatePlayer();
        var camera = EntityFactory.CreateCamera();

        _engine = new Engine();
        
        _engine.World.Entities.Add(camera);
        _engine.World.Entities.Add(player);

        // start stopwatch to calculate delta time
        _lastElapsedMs = 0;
    }

    private void GameTimerLoop(object sender, EventArgs e)
    {
        var now = _stopwatch.ElapsedMilliseconds;
        var delta = (int)(now - _lastElapsedMs);
        if (delta <= 0) delta = 1;
        _lastElapsedMs = now;

        _engine.Update(delta);

        Invalidate();
    }

    // Run render method of the engine, passing with all the context
    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var context = new RenderContext
        {
            Graphics = e.Graphics,
            X = ClientSize.Width / 2f,
            Y = ClientSize.Height / 2f,
        };
        
        _engine.GeneralRender.Render(_engine.World, context);
        g.ResetTransform();
    }
    
    // Prevent background from being erased (avoids flicker when using double buffering)
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Intentionally empty - full redraw performed in OnPaint
    }
    
    // Methods providing information for my helper class so the keyboard
    // is "global" to the whole project
    private void MainForm_KeyDown(object sender, KeyEventArgs e) 
    {
        Keyboard.KeyDown(e.KeyCode);
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e) 
    {
        Keyboard.KeyUp(e.KeyCode);
    }
}