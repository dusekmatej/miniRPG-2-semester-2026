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
    private readonly PerlinNoise _treeNoise;
    private readonly Random     _rand;

    // Raised from 0.014 → 0.028 so mountain blobs are roughly 2x smaller in diameter.
    private const float BiomeScale  = 0.028f;

    // Forest uses its own lower scale so patches stay large and coherent.
    private const float ForestScale = 0.04f;

    private const float DetailScale = 0.12f;
    private const float OreScale    = 0.18f;
    private const float TreeScale   = 0.15f;

    // Mountain start raised from 0.65 → 0.72 so less of the noise range qualifies.
    // Combined with the higher BiomeScale this makes mountains noticeably smaller.
    private const float MountainThreshold = 0.72f;

    // Forest thresholds
    private const float ForestTileThreshold = 0.52f;
    private const float TreeSpawnThreshold  = 0.60f;

    public Terrain(int seed, int width = 1000, int height = 1000)
    {
        Width  = width;
        Height = height;
        _rand  = new Random(seed);

        _biomeNoise  = new PerlinNoise(seed);
        _detailNoise = new PerlinNoise(seed + 1);
        _oreNoise    = new PerlinNoise(seed + 2);
        _forestNoise = new PerlinNoise(seed + 3);
        _treeNoise   = new PerlinNoise(seed + 4);

        Map = new Tile[width, height];
    }

    public void FillChunkPerlin(Chunk chunk)
    {
        int offsetX = chunk.ChunkX * Chunk.Size;
        int offsetY = chunk.ChunkY * Chunk.Size;

        // --- First pass: fill tiles and determine ores ---
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
    //  Tile generation                                                     //
    // ------------------------------------------------------------------ //

    private Tile GenerateTile(float biome, float detail, float forest)
    {
        var tile = new Tile();

        if (biome < 0.30f)
        {
            tile.Type = TileType.DeepWater;
        }
        else if (biome < 0.44f)
        {
            tile.Type = TileType.Water;
        }
        else if (biome < MountainThreshold)
        {
            if (forest > ForestTileThreshold)
            {
                tile.Type = TileType.Forest;

                tile.Variation = detail switch
                {
                    < 0.30f => 0,
                    < 0.55f => 1,
                    < 0.68f => 2,
                    < 0.82f => 3,
                    _       => 4
                };
            }
            else
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
        }
        else
        {
            tile.Type = TileType.Mountain;

            tile.Variation = detail switch
            {
                < 0.50f => 0,
                _       => 1
            };
        }

        return tile;
    }

    // ------------------------------------------------------------------ //
    //  Ore generation                                                      //
    // ------------------------------------------------------------------ //

    //  Ore       | Biome              | Noise threshold | Roll
    //  --------- | ------------------ | --------------- | ----
    //  Gold      | Mountain only      | > 0.75          | 10%
    //  Bronze    | Mountain + Forest  | > 0.65 / > 0.72 | 25% / 8%
    //  Iron      | Mountain only      | > 0.70          | 20%
    //  Coal      | Mountain only      | > 0.62          | 28%

    private OreType GetOreType(Tile tile, float oreNoise)
    {
        bool isMountain = tile.Type == TileType.Mountain;
        bool isForest   = tile.Type == TileType.Forest;

        // Bronze rarely spawns in forest biomes — higher threshold, much lower roll
        if (isForest && oreNoise > 0.72f && Roll(8))
            return OreType.Bronze;

        if (!isMountain)
            return OreType.None;

        // Gold — rarest, checked first
        if (oreNoise > 0.75f && Roll(10))
            return OreType.Gold;

        // Bronze — second least rare on mountains
        if (oreNoise > 0.65f && Roll(25))
            return OreType.Bronze;

        // Iron — medium
        if (oreNoise > 0.70f && Roll(20))
            return OreType.Iron;

        // Coal — most common, threshold and roll both loosened
        if (oreNoise > 0.62f && Roll(28))
            return OreType.Coal;

        return OreType.None;
    }

    // ------------------------------------------------------------------ //
    //  Entity spawn — Ores                                                 //
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

    // ------------------------------------------------------------------ //
    //  Entity spawn — Trees                                                //
    // ------------------------------------------------------------------ //

    public void SpawnChunkTrees(World world, Chunk chunk)
    {
        int offsetX = chunk.ChunkX * Chunk.Size;
        int offsetY = chunk.ChunkY * Chunk.Size;

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                var tile = chunk.Map[x, y];

                int worldX = offsetX + x;
                int worldY = offsetY + y;

                float tree   = _treeNoise.SampleOctaves(  worldX * TreeScale,   worldY * TreeScale,   octaves: 2, persistence: 0.5f, lacunarity: 2f);
                float forest = _forestNoise.SampleOctaves(worldX * ForestScale, worldY * ForestScale, octaves: 3, persistence: 0.5f, lacunarity: 2f);

                if (tile.Type == TileType.Forest)
                {
                    if (forest > TreeSpawnThreshold && tree > 0.50f && Roll(65))
                    {
                        int variation = Roll(25) ? 1 : 0;
                        world.Entities.Add(Prefabs.CreateTree(worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize, variation));
                    }
                }
                else if (tile.Type == TileType.Grass)
                {
                    if (forest < ForestTileThreshold - 0.10f && tree > 0.78f && Roll(6))
                    {
                        int variation = Roll(15) ? 1 : 0;
                        world.Entities.Add(Prefabs.CreateTree(worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize, variation));
                    }
                }
            }
        }
    }

    // ------------------------------------------------------------------ //
    //  Helpers                                                             //
    // ------------------------------------------------------------------ //

    private bool Roll(int percent) => _rand.Next(100) < percent;
}