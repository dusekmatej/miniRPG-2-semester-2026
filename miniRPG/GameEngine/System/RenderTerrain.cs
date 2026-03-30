using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Core;
using miniRPG.GameEngine.Other;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.System;

public class RenderTerrain
{
    public void Render(Terrain t, World world, RenderContext context)
    {
        var cameraEntity = world.Entities.FirstOrDefault(e => e.HasComponent<Camera>());
        
        if (cameraEntity == null)
            throw new Exception("Camera entity was not found! In TerrainRender!");
        
        var camera = cameraEntity.GetComponent<Camera>();
        
        if (camera == null)
            throw new Exception("Camera component was not found! In TerrainRender!");
            
        for (int x = 0; x < t.Width; x++)
        {
            for (int y = 0; y < t.Height; y++)
            {
                Tile tile = t.Map[x, y];

                var texture = TerrainHelper.GetTexture(tile);

                if (texture == null)
                    Console.WriteLine("Texture not found!");

                var posX = x * t.TileSize;
                var posY = y * t.TileSize;

                var screenX = posX - camera.X + context.X;
                var screenY = posY - camera.Y + context.Y;

                if (texture != null)
                    context.Graphics.DrawImage(texture.Image, screenX, screenY, t.TileSize + t.TILE_MIXING, t.TileSize + t.TILE_MIXING);
            }
        }
    }
}