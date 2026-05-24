using System.Drawing;
using System.Drawing.Drawing2D;
using miniRPG.GameEngine.Core;

namespace miniRPG.GameEngine.Rendering.Layers;

public class PostProcessingLayer : IRenderLayer
{
    // NOT IN USE CURRENTLY JUST A PROTOTYPE
    
    private static readonly Color WarmTint = Color.FromArgb(15, 255, 200, 100);
    private static readonly Color VignetteColor = Color.FromArgb(80, 0, 0, 0);

    // Cached so we dont recreate them every frame
    private PathGradientBrush? _vignetteBrush;
    private SolidBrush _tintBrush = new SolidBrush(WarmTint);
    private int _lastWidth = 0;
    private int _lastHeight = 0;

    public void Render(World world, RenderContext context)
    {
        RebuildIfResized(context.ScreenWidth, context.ScreenHeight);
        
        context.Graphics.FillRectangle(_tintBrush, 0, 0, context.ScreenWidth, context.ScreenHeight);
        
        if (_vignetteBrush != null)
            context.Graphics.FillRectangle(_vignetteBrush, 0, 0, context.ScreenWidth, context.ScreenHeight);
    }

    private void RebuildIfResized(int w, int h)
    {
        if (w == _lastWidth && h == _lastHeight) 
            return;

        _lastWidth = w;
        _lastHeight = h;
        
        _vignetteBrush?.Dispose();

        var path = new GraphicsPath();
        path.AddEllipse(-w / 4, -h / 4, w * 1.5f, h * 1.5f);

        _vignetteBrush = new PathGradientBrush(path);
        _vignetteBrush.CenterColor = Color.Transparent;
        _vignetteBrush.SurroundColors = new Color[] { VignetteColor };
        
        // We can dispose path right after building the brush
        path.Dispose();
    }
}