using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Enums;

namespace miniRPG.GameEngine.Core.WorldTerrain;

public class Terrain
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TileSize { get; private set; } = 100;
    public float TILE_MIXING { get; private set;} = 2f;
    private Random _random;
    
    public Tile[,] Map { get; private set; }
    private readonly Dictionary<(int chunkX, int chunkY), Chunk> _chunks = new();

    public Terrain(int width, int height)
    {
        Width = width;
        Height = height;
        _random = new Random();

        Map = new Tile[width, height];
    }

    public Chunk FillChunkPerlin(Chunk chunk, int seed, int offsetX, int offsetY)
    {
        PerlinNoise noise = new PerlinNoise(seed);

        float biomeSize = 0.02f;
        float detailSize = 0.2f;

        int variant = 0;
        
        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                var calculatedSizeX = (offsetX + x) * biomeSize;
                var calculatedSizeY = (offsetY + y) * detailSize;
                Console.WriteLine(calculatedSizeX);
                Console.WriteLine(calculatedSizeY);
                
                float biomeValue = noise.Sample(calculatedSizeX, calculatedSizeY);
            }
        }

        return chunk;
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