using miniRPG.Game;
using miniRPG.Helpers;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.Other;
using Timer = System.Windows.Forms.Timer;

namespace miniRPG.Forms;

// TODO: Game\
// TODO:    \GAME
// TODO:    \GameBootstrapper.cs
// TODO: Components\
// TODO:    \RenderLayerComponent.cs

// TODO: Rendering\
// TODO:    /RenderContext.cs
// TODO:    /RenderPipeline.cs
// TODO:    /IRenderPass.cs
// TODO:        /Passes
// TODO:            /RenderTerrain.cs
// TODO:            /RenderSystem.cs


// TODO: Root\
// TODO:    \World
// TODO:        \Terrain.cs
// TODO:        \PerlinNoise.cs


public partial class MainForm : Form
{
    private Game.Game _game = new();
    
    private readonly Terrain _terrain;
    private long _lastElapsedMs;
    
    public MainForm()
    {
        InitializeComponent();
        _game.Initialize();
        
        // start stopwatch to calculate delta time
        _lastElapsedMs = 0;
    }

    private void GameTimerLoop(object sender, EventArgs e)
    {
        Invalidate();
    }

    // Run render method of the engine, passing with all the context
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

    private void Update(object? sender, EventArgs e)
    {
        _game.Update();

        Invalidate();
    }
}