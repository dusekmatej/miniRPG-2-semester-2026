using miniRPG.Enums;
using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.Core.WorldTerrain;

public class Terrain
{
    private int Width { get; set; }
    private int Height { get; set; }
    
    public Tile[,] Map { get; private set; }
    private readonly Dictionary<(int chunkX, int chunkY), Chunk> _chunks = new();

    private readonly PerlinNoise _noise;
    private readonly float _noiseScale = 0.02f;
    private readonly float _detailScale = 0.15f;

    public Terrain(int seed, int width = 1000, int height = 1000)
    {
        Width = width;
        Height = height;
        _noise = new PerlinNoise(seed);

        Map = new Tile[width, height];
    }

    public void FillChunkPerlin(Chunk chunk)
    {
        int offsetX = chunk.ChunkX * Chunk.Size;
        int offsetY = chunk.ChunkY * Chunk.Size;

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                var biome = _noise.Sample(
                    (offsetX + x) * _detailScale, 
                    (offsetY + y) * _detailScale);
                var detail = _noise.Sample(
                    (offsetX + x) * _detailScale,
                    (offsetY + y) * _detailScale);

                chunk.Map[x, y] = GenerateTile(biome, detail);
            }
        }
    }

    private Tile GenerateTile(float biome, float detail)
    {
        var newTile = new Tile();
        TileType type = TileType.Water;

        if (biome < 0.33f)
        {
            newTile.Type = TileType.Water;
        }
        else if (biome < 0.66f)
        {
            newTile.Type = TileType.Grass;

            newTile.Variation = detail switch
            {
                < 0.4f => 0,
                < 0.5f => 1,
                < 0.55f => 2,
                < 0.65f => 3,
                _ => 4
            };
        }
        else
            newTile.Type = TileType.Mountain;

        return newTile;
    }

    private bool IsDifferent(int x, int y, TileType type)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return false;

        if (Map[x, y].Type != type)
            return true;
        else
            return false;
    }
}