using miniRPG.Helpers;
using miniRPG.Services;
using miniRPG.GameEngine;
using miniRPG.GameEngine.Entities;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using System.Diagnostics;

namespace miniRPG.Forms;

public partial class MainForm : Form
{
    // Game engine loop initialization
    private Engine _engine;
    private Timer _gameTimer;
    private Stopwatch _stopwatch;
    private long _lastElapsedMs;

    private float translateX = 0f;
    private float translateY = 0f;
    private Image? playerImage;
    private int currentFrame = 0;
    private static string basePath = AppDomain.CurrentDomain.BaseDirectory;
    private string _resourcesPath = Path.Combine(basePath, "Resources");

    private readonly List<Image>? _playerImages;
    
    public MainForm()
    {
        InitializeComponent();
        KeyPreview = true;

        // Reduce flicker: enable double buffering and paint optimizations
        this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        this.UpdateStyles();
        this.DoubleBuffered = true;

        var player = EntityFactory.CreatePlayer();

        _engine = new Engine();
        _engine.World.Entities.Add(player);
        _gameTimer = new Timer();
        _gameTimer.Interval = 16;
        _gameTimer.Tick += GameLoop;
        _gameTimer.Start();

        // start stopwatch to calculate delta time
        _stopwatch = Stopwatch.StartNew();
        _lastElapsedMs = 0;


        _tmrAnimation.Interval = 200;
        // don't use the designer timer for animation; it conflicts with the engine-driven animation
        _tmrAnimation.Stop();
        // _tmrAnimation.Tick += AnimationTick;
        // _tmrAnimation.Start();
    }

    // Prevent background from being erased (avoids flicker when using double buffering)
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Intentionally empty - full redraw performed in OnPaint
    }

    private void GameLoop(object sender, EventArgs e)
    {
        var now = _stopwatch.ElapsedMilliseconds;
        var delta = (int)(now - _lastElapsedMs);
        if (delta <= 0) delta = 1;
        _lastElapsedMs = now;

        _engine.Update(delta);

        Invalidate();
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        // Render the game with engine's system
        _engine.GeneralRender.Render(_engine.World, e.Graphics);
        
        g.ResetTransform();
    }

    
    // Methods providing information for my helper class so the keyboard
    // is "global" to the whole project
    private void MainForm_KeyDown(object sender, KeyEventArgs e) {
        Keyboard.KeyDown(e.KeyCode);
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e) {
        Keyboard.KeyUp(e.KeyCode);
    }
}