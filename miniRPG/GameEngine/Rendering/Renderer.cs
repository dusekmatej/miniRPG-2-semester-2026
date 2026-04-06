namespace miniRPG.GameEngine.Rendering;

using Other;
using Components;
using Core;

public class Renderer
{
    private readonly List<IRenderLayer> _renderLayers = new();

    public void Render(World world, Terrain? terrain, RenderContext context)
    {
        foreach (var layer in _renderLayers)
            layer.Render(world, terrain, context);
    }

    public void Add(IRenderLayer layer)
    {
        _renderLayers.Add(layer);
    }
}