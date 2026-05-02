using System.Diagnostics;
using System.Drawing.Drawing2D;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace miniRPG.Forms;

// using System.Windows.Forms.Integration;
using Helpers;
using GameEngine.Rendering;

public partial class MainForm : Form
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    private float _lastTime = 0f;
    private Game.Game _game = new();

    private const float TargetFps = 600f;
    private readonly float _targetFrameTime = 1f / TargetFps;

    public MainForm()
    {
        InitializeComponent();
        _game.Initialize(ClientSize.Width, ClientSize.Height);
        MouseHelper.SetWindow(Location.X, Location.Y, ClientSize.Width, ClientSize.Height);
        this.LocationChanged += MainForm_LocationChanged;
        
        Application.Idle += OnIdle;
    }

    private int _frameCount = 0;
    private float _fpsTimer = 0f;
    private float _lastFps = 0f;
    
    private void OnIdle(object? sender, EventArgs e)
    {
        if (!IsIdle()) return;

        float now = (float)_stopwatch.Elapsed.TotalSeconds;
        float rawDelta = now - _lastTime;
        _lastTime = now;

        float deltaTime = rawDelta;
        if (deltaTime < 0f) deltaTime = 0f;
        if (deltaTime > 0.1f) deltaTime = 0.1f;

        if (rawDelta < _targetFrameTime) return;
        
        _fpsTimer += rawDelta;
        _frameCount++;

        if (_fpsTimer >= 1f)
        {
            _lastFps = _frameCount / _fpsTimer;
            _frameCount = 0;
            _fpsTimer = 0f;
            Console.WriteLine($"FPS: {_lastFps}, delta: {deltaTime}");
        }
        

        _game.Update(deltaTime);
        Invalidate();
        Update();
    }

    private bool IsIdle()
    {
        return !NativeMethods.PeekMessage(out _, IntPtr.Zero, 0, 0, 0);
    }

    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.None;
        e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
        e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

        var context = new RenderContext
        {
            Graphics = e.Graphics,
            ScreenWidth = ClientSize.Width,
            ScreenHeight = ClientSize.Height,
        };

        _game.Render(context);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        Application.Idle -= OnIdle;
        base.OnFormClosed(e);
    }

    #region Input
    private void MainForm_KeyDown(object sender, KeyEventArgs e) => Keyboard.KeyDown(e.KeyCode);
    private void MainForm_KeyUp(object sender, KeyEventArgs e) => Keyboard.KeyUp(e.KeyCode);

    private void MainForm_Resize(object sender, EventArgs e)
    {
        MouseHelper.SetWindow(Location.X, Location.Y, ClientSize.Width, ClientSize.Height);
    }

    private void MainForm_MouseUp(object sender, MouseEventArgs e)
    {
        MouseHelper.MouseUp(e.Button);
    }

    private void MainForm_LocationChanged(object sender, EventArgs e)
    {
        MouseHelper.SetWindow(Location.X, Location.Y, ClientSize.Width, ClientSize.Height);
    }
    
    #endregion
}