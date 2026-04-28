using System.Diagnostics;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class DebugLayer : IRenderLayer
{
    private int _frameCount = 0;
    private float _fpsTimer = 0f;
    private float _lastFps = 0f;
    private Stopwatch _stopwatch = new Stopwatch();
    private DateTime _lastUpdate = DateTime.Now;
    
    public void Render(World world, RenderContext context)
    {
        // Delta time built in here
        var now = DateTime.Now;
        float delta = (float)(now - _lastUpdate).TotalSeconds;
        _lastUpdate = now;
        _fpsTimer += delta;
        _frameCount++;

        if (_fpsTimer >= 1f)
        {
            _lastFps = _frameCount / _fpsTimer;
            _frameCount = 0;
            _fpsTimer = 0f;
        }
        
        if (Flags.IsGraphicDebug)
            context.Graphics.DrawString($"FPS: {_lastFps:F0}", SystemFonts.DefaultFont, Brushes.Yellow, 200, 200);
    }
}