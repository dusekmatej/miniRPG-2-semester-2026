using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Core.WorldTerrain;
using miniRPG.GameEngine.Databases;

namespace miniRPG.GameEngine.Rendering.Layers;

public class TerrainLayer : IRenderLayer
{
    private readonly ChunkBaker _baker = new();

    public void Render(World world, RenderContext context)
    {
        var camera = world.CameraEntity.GetComponent<Camera>() ?? throw new Exception("Camera component not found!");

        var visibleChunks = world.ChunkManager.GetVisibleChunks(
            (int)(camera.X - context.ScreenWidth / 2f),
            (int)(camera.Y - context.ScreenHeight / 2f),
            context.ScreenWidth,
            context.ScreenHeight,
            Chunk.TileSize
            );

        int baked = 0, cached = 0;
        
        foreach (var chunk in visibleChunks)
        {
            // Bake once, reuse until dirty
            if (chunk.IsDirty || chunk.Bitmap == null)
            {
                _baker.Bake(chunk);
                baked++;
            }
            else cached++;

            
            var chunkPixelX = chunk.ChunkX * Chunk.Size * Chunk.TileSize;
            var chunkPixelY = chunk.ChunkY * Chunk.Size * Chunk.TileSize;

            // Important: draw at integer pixels to avoid seams between adjacent chunk bitmaps
            var screenX = (int)MathF.Round(chunkPixelX - camera.X + context.ScreenWidth / 2f);
            var screenY = (int)MathF.Round(chunkPixelY - camera.Y + context.ScreenHeight / 2f);

            context.Graphics.DrawImage(chunk.Bitmap!, screenX, screenY);

            if (Flags.IsGraphicDebug)
            {
                context.Graphics.DrawRectangle(Pens.Red, screenX, screenY,
                    Chunk.Size * TileDatabase.TileSize,
                    Chunk.Size * TileDatabase.TileSize);
                context.Graphics.DrawString($"{chunk.ChunkX},{chunk.ChunkY}",
                    SystemFonts.DefaultFont, Brushes.Red, screenX + 4, screenY + 4);
            }
        }
    }
}