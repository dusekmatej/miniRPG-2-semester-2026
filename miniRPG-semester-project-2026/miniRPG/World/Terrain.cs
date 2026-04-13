using miniRPG.GameEngine.Other;

namespace miniRPG.GameEngine.Components;

public class Terrain
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public int TileSize { get; private set; } = 100;
    public float TILE_MIXING { get; private set;} = 2f;
    
    public Tile[,] Map { get; private set; }

    public Terrain(int width, int height)
    {
        Width = width;
        Height = height;

        Map = new Tile[width, height];
    }

    public void GeneratePerlinMap(int seed)
    {
        PerlinNoise noise = new PerlinNoise(seed);

        float biomeScale = 0.02f;
        float detailScale = 0.2f;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // --- BIOME NOISE ---
                float biomeValue = noise.Sample(x * biomeScale, y * biomeScale);

                TileType type;

                if (biomeValue < 0.3f) type = TileType.Water;
                else if (biomeValue < 0.6f) type = TileType.Grass;
                else if (biomeValue < 0.9f) type = TileType.Mountain;
                else type = TileType.Mountain;

                // --- DETAIL NOISE (variation) ---
                float detailValue = noise.Sample(x * detailScale, y * detailScale);

                int variant = 0;

                if (type == TileType.Grass)
                {
                    if (detailValue < 0.33f) variant = 0;
                    else if (detailValue < 0.66f) variant = 1;
                    else variant = 2;
                }

                // --- ASSIGN TILE ---
                Map[x, y] = new Tile
                {
                    Type = type,
                    Variation = variant
                };
            }
        }
    }
    

    public void GenerateTestMap()
    {
        Random rand = new Random();

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileType type;

                // simple biome bands
                if (y < Height / 5) type = TileType.Water;
                else if (y < 2 * Height / 5) type = TileType.Grass;
                else if (y < 3 * Height / 5) type = TileType.Grass;
                else if (y < 4 * Height / 5) type = TileType.Mountain;
                else type = TileType.Mountain;

                int variant = 0;

                if (type == TileType.Grass)
                    variant = rand.Next(0, 3);
                
                Map[x, y] = new Tile
                {
                    Type = type,
                    Variation = variant,
                };
            }
        }
    }
}