using miniRPG.GameEngine.Core.WorldTerrain;

namespace miniRPG.GameEngine.Rendering;

using Components;
using Core;

public interface IRenderLayer
{
    public void Render(World world, RenderContext context);
}