using miniRPG.Environment;

namespace miniRPG.GameEngine.Rendering;

using Components;
using Core;

public interface IRenderLayer
{
    public void Render(World world, Terrain? terrain, RenderContext context);
}