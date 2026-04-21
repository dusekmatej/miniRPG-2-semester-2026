using miniRPG.GameEngine.Components;

namespace miniRPG.Environment;

public class Chunk
{
    public const int Size = 16;
    public const int TileSize = 100;
    
    public int ChunkX { get; }
    public int ChunkY { get; }

    public Tile[,] Tiles { get; } = new Tile[Size, Size];

    public bool IsDirty = true; // Means that something has been modified and needs a re-render
    public Bitmap? Bitmap { get; set; }

    public Rectangle WorldBorder => new Rectangle(
        ChunkX * Size * TileSize, ChunkY * Size * TileSize,
        Size * TileSize, Size * TileSize);
}