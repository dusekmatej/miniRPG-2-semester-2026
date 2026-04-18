namespace miniRPG.GameEngine.Rendering.Layers;

using Components;
using Core;
using Other;
using Rendering;
using Helpers;


public class TerrainLayer : IRenderLayer
{
    public void Render(World world, Terrain? t, RenderContext context)
    {
        var cameraEntity = world.Entities.FirstOrDefault(e => e.HasComponent<Camera>());
        
        if (cameraEntity == null)
            throw new Exception("Camera entity was not found! In TerrainRender!");
        
        var camera = cameraEntity.GetComponent<Camera>();
        
        if (camera == null)
            throw new Exception("Camera component was not found! In TerrainRender!");
        
        if (t == null)
            throw new Exception("Terrain is null in TerrainLayer!");

        var halfW = context.ScreenWidth / 2f;
        var halfH = context.ScreenHeight / 2f;

        var worldLeft = camera.X - halfW;
        var worldTop = camera.Y - halfH;
        var worldRight = camera.X + halfW;
        var worldBottom = camera.Y + halfH;

        var startX = (int)MathF.Floor(worldLeft / t.TileSize);
        var startY = (int)MathF.Floor(worldTop / t.TileSize);
        var endX = (int)MathF.Ceiling(worldRight / t.TileSize);
        var endY = (int)MathF.Ceiling(worldBottom / t.TileSize);

        startX = Math.Clamp(startX, 0, t.Width);
        startY = Math.Clamp(startY, 0, t.Height);
        endX = Math.Clamp(endX, 0, t.Width);
        endY = Math.Clamp(endY, 0, t.Height);
                        
        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                Tile tile = t.Map[x, y];

                var texture = TileLoader.GetTexture(tile);

                if (texture == null)
                    Console.WriteLine(@"TerrainLayer: Texture not found!");

                var posX = x * t.TileSize;
                var posY = y * t.TileSize;

                var screenX = posX - camera.X + (context.ScreenWidth / 2f);
                var screenY = posY - camera.Y + (context.ScreenHeight / 2f);

                if (texture != null)
                    context.Graphics.DrawImage(texture.Image, screenX, screenY, t.TileSize + t.TILE_MIXING, t.TileSize + t.TILE_MIXING);
            }
        }
    }
}