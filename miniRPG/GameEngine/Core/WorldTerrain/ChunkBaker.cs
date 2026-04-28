using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.Enums;
using miniRPG.Helpers;

namespace miniRPG.GameEngine.Core.WorldTerrain;

// Environment/Terrain/ChunkBaker.cs
public class ChunkBaker
{
    public void Bake(Chunk chunk)
    {
        int px = Chunk.Size * TileDatabase.TileSize;
        var bmp = new Bitmap(px, px, PixelFormat.Format32bppArgb);

        using var g = Graphics.FromImage(bmp);
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode   = PixelOffsetMode.Half;

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                var tile    = chunk.Map[x, y];
                var texture = TileLoader.GetTexture(tile);
                if (texture == null) continue;

                int tileSize = TileDatabase.TileSize;
                int drawSize = tile.Type == TileType.Mountain
                    ? tileSize + 5   // your existing mountain offset
                    : tileSize;

                g.DrawImage(texture.Image, x * tileSize, y * tileSize, drawSize, drawSize);
            }
        }

        // Dispose old bitmap before replacing
        chunk.Bitmap?.Dispose();
        chunk.Bitmap = bmp;
        chunk.IsDirty = false;
    }
}