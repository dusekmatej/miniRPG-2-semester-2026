using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Enums;

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
                < 0.17f => 0,
                < 0.33f => 1,
                < 0.50f => 2,
                < 0.67f => 3,
                < 0.83f => 4,
                _ => 5
            };
        } 
        else
            newTile.Type = TileType.Mountain;
        
        return newTile;
    }

    public void GeneratePerlinMap(int seed)
    {
        PerlinNoise noise = new PerlinNoise(seed);

        float biomeScale = 0.02f;
        float detailScale = 0.2f;
        
        int variant = 0;
        
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Generate noise layers
                float biomeValue = noise.Sample(x * biomeScale, y * biomeScale);
                float detailValue = noise.Sample(x * detailScale, y * detailScale);

                TileType type;
                
                // Set the biomes
                if (biomeValue < 0.3)
                    type = TileType.Water;
                else if (biomeValue < 0.6f)
                    type = TileType.Grass;
                else if (biomeValue < 0.9f)
                    type = TileType.Mountain;
                else
                    type = TileType.Mountain;
                
                // Set the variation by detail noise
                if (type == TileType.Grass)
                {
                    if (detailValue < 0.18) variant = 0;
                    else if (detailValue < 0.36) variant = 1;
                    else if (detailValue < 0.54) variant = 2;
                    else if (detailValue < 0.71) variant = 3;
                    else if (detailValue < 0.90) variant = 4;
                    else variant = 5;
                }

                if (type == TileType.Mountain)
                {
                    if (detailValue < 0.75) variant = 0;
                    else variant = 1;
                }

                Map[x, y] = 
                    new Tile { Type = type, Variation = variant };
            }
        }
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