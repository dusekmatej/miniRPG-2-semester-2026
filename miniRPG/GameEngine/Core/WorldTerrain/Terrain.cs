using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.Entities;

namespace miniRPG.GameEngine.Core.WorldTerrain;

public class Terrain
{
    private int Width  { get; set; }
    private int Height { get; set; }

    public Tile[,] Map { get; private set; }
    private readonly Dictionary<(int chunkX, int chunkY), Chunk> _chunks = new();

    private readonly PerlinNoise _biomeNoise;
    private readonly PerlinNoise _detailNoise;
    private readonly PerlinNoise _oreNoise;
    private readonly PerlinNoise _forestNoise;
    private readonly Random     _rand;

    // Škály — čím menší číslo, tím "rozmazanější" a větší oblasti
    private const float BiomeScale  = 0.008f;  // velké biomy
    private const float DetailScale = 0.12f;   // variace textury
    private const float OreScale    = 0.18f;   // žíly rudy
    private const float ForestScale = 0.09f;   // lesy

    public Terrain(int seed, int width = 1000, int height = 1000)
    {
        Width  = width;
        Height = height;
        _rand  = new Random(seed);

        // Každý noise má jiný seed → nezávislé vzory
        _biomeNoise  = new PerlinNoise(seed);
        _detailNoise = new PerlinNoise(seed + 1);
        _oreNoise    = new PerlinNoise(seed + 2);
        _forestNoise = new PerlinNoise(seed + 3);

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
                int worldX = offsetX + x;
                int worldY = offsetY + y;

                float biome  = _biomeNoise.SampleOctaves(worldX * BiomeScale,  worldY * BiomeScale,  octaves: 4, persistence: 0.5f, lacunarity: 2f);
                float detail = _detailNoise.SampleOctaves(worldX * DetailScale, worldY * DetailScale, octaves: 2, persistence: 0.5f, lacunarity: 2f);
                float ore    = _oreNoise.SampleOctaves(   worldX * OreScale,    worldY * OreScale,    octaves: 3, persistence: 0.6f, lacunarity: 2f);
                float forest = _forestNoise.SampleOctaves(worldX * ForestScale, worldY * ForestScale, octaves: 3, persistence: 0.5f, lacunarity: 2f);

                chunk.Map[x, y] = GenerateTile(biome, detail, forest);
                chunk.Ores[(worldX, worldY)] = GetOreType(chunk.Map[x, y], ore);
            }
        }
    }

    // ------------------------------------------------------------------ //
    //  Tile generace                                                       //
    // ------------------------------------------------------------------ //

    private Tile GenerateTile(float biome, float detail, float forest)
    {
        var tile = new Tile();

        if (biome < 0.25f)
        {
            tile.Type = TileType.DeepWater;
        }
        else if (biome < 0.38f)
        {
            tile.Type = TileType.Water;
        }
        else if (biome < 0.65f)
        {
            tile.Type = TileType.Grass;

            tile.Variation = detail switch
            {
                < 0.35f => 0,
                < 0.50f => 1,
                < 0.62f => 2,
                < 0.75f => 3,
                _       => 4
            };
        }
        else if (biome < 0.80f)
        {
            tile.Type = TileType.Mountain; // Hill až budeš mít
        }
        else
        {
            tile.Type = TileType.Mountain;
        }

        return tile;
    }

    // ------------------------------------------------------------------ //
    //  Ore generace                                                        //
    // ------------------------------------------------------------------ //

    // Ore noise tvoří přirozené "žíly" — vysoká hodnota = střed žíly
    // Každý ore má svůj práh + omezení na biom kde se může spawnnout
    //
    //  Ore       | Biom          | Noise práh | Šance (po prahu)
    //  --------- | ------------- | ---------- | ----------------
    //  Coal      | Hill+Mountain | > 0.60     | 35 %
    //  Iron      | Hill+Mountain | > 0.68     | 20 %
    //  Bronze    | Mountain      | > 0.72     | 12 %
    //  Gold      | Mountain      | > 0.80     |  5 %

    private OreType GetOreType(Tile tile, float oreNoise)
    {
        // bool isHill     = tile.Type == TileType.Hill;
        bool isMountain = tile.Type == TileType.Mountain;

        if (!isMountain)
            return OreType.None;
        
        // if (!isHill && !isMountain)
        //     return OreType.None;

        // Gold — pouze hora, vzácný
        if (isMountain && oreNoise > 0.80f && Roll(5))
            return OreType.Gold;

        // Bronze — pouze hora
        if (isMountain && oreNoise > 0.72f && Roll(12))
            return OreType.Bronze;

        // Iron — hora i kopec
        if (oreNoise > 0.68f && Roll(20))
            return OreType.Iron;

        // Coal — nejběžnější, hora i kopec
        if (oreNoise > 0.60f && Roll(35))
            return OreType.Coal;

        return OreType.None;
    }

    // ------------------------------------------------------------------ //
    //  Entity spawn                                                        //
    // ------------------------------------------------------------------ //

    public void SpawnChunkOres(World world, Chunk chunk)
    {
        foreach (var (pos, oreType) in chunk.Ores)
        {
            if (oreType == OreType.None)
                continue;

            var (worldX, worldY) = pos;

            var entity = oreType switch
            {
                OreType.Coal   => Prefabs.CreateCoalRock(  worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize),
                OreType.Iron   => Prefabs.CreateIronRock(  worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize),
                OreType.Bronze => Prefabs.CreateBronzeRock(worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize),
                OreType.Gold   => Prefabs.CreateGoldRock(  worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize),
                _              => null
            };

            if (entity != null)
                world.Entities.Add(entity);
        }
    }

    /*public void SpawnChunkForest(World world, Chunk chunk)
    {
        int offsetX = chunk.ChunkX * Chunk.Size;
        int offsetY = chunk.ChunkY * Chunk.Size;

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                if (chunk.Map[x, y].Type != TileType.Forest)
                    continue;

                // Ne každý Forest tile dostane strom — 60% šance
                if (!Roll(60))
                    continue;

                int worldX = (offsetX + x) * TileDatabase.TileSize;
                int worldY = (offsetY + y) * TileDatabase.TileSize;

                world.Entities.Add(EntityFactory.CreateTree(worldX, worldY));
            }
        }
    }*/

    // ------------------------------------------------------------------ //
    //  Helpers                                                             //
    // ------------------------------------------------------------------ //

    // Vrátí true s pravděpodobností 'percent' procent (0–100)
    private bool Roll(int percent) => _rand.Next(100) < percent;

    private bool IsDifferent(int x, int y, TileType type)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return false;

        return Map[x, y].Type != type;
    }
}