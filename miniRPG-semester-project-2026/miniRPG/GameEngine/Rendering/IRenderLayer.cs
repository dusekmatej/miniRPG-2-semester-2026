namespace miniRPG.GameEngine.Rendering;

using Components;
using Core;
using Other;

public interface IRenderLayer
{
    public void Render(World world, Terrain? terrain, RenderContext context);
}