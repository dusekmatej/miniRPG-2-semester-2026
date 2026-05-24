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
    private readonly Random _rand;

    private const float BiomeScale  = 0.008f;
    private const float DetailScale = 0.12f;
    private const float OreScale    = 0.18f;
    private const float ForestScale = 0.09f;

    public Terrain(int seed, int width = 1000, int height = 1000)
    {
        Width  = width;
        Height = height;
        _rand  = new Random(seed);

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
                chunk.Trees[(worldX, worldY)] = GetTreeVariation(chunk.Map[x, y], forest);
            }
        }
    }

    // ------------------------------------------------------------------ //
    //  Tile generation                                                     //
    // ------------------------------------------------------------------ //

    private Tile GenerateTile(float biome, float detail, float forest)
    {
        var tile = new Tile();

        if (biome < 0.25f)
            tile.Type = TileType.DeepWater;
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
            tile.Type = TileType.Mountain;
        }
        else
        {
            tile.Type = TileType.Mountain;
        }

        return tile;
    }

    // ------------------------------------------------------------------ //
    //  Forest generation                                                   //
    // ------------------------------------------------------------------ //

    // Returns tree variation (0,1,2) if a tree should spawn here, -1 if not.
    // Trees only grow on grass, and forest noise drives where they cluster.
    private int GetTreeVariation(Tile tile, float forestNoise)
    {
        if (tile.Type != TileType.Grass)
            return -1;

        // Only dense forest areas get trees, and not every tile — 40% chance inside those areas
        if (forestNoise > 0.65f && Roll(40))
            return _rand.Next(0, 2); // 3 tree textures

        return -1;
    }

    // ------------------------------------------------------------------ //
    //  Ore generation                                                      //
    // ------------------------------------------------------------------ //

    // The key fix here: we check from rarest to most common, and use
    // early returns so only ONE ore type can spawn per tile.
    // Before, coal was winning because all checks ran independently.
    //
    //  Ore    | Noise threshold | Spawn chance
    //  ------ | --------------- | ------------
    //  Gold   | > 0.82          | 8%
    //  Bronze | > 0.75          | 15%
    //  Iron   | > 0.68          | 22%
    //  Coal   | > 0.58          | 30%

    private OreType GetOreType(Tile tile, float oreNoise)
    {
        if (tile.Type != TileType.Mountain)
            return OreType.None;

        // Rarest first — if gold passes, we never even check coal
        if (oreNoise > 0.82f && Roll(8))
            return OreType.Gold;

        if (oreNoise > 0.75f && Roll(15))
            return OreType.Bronze;

        if (oreNoise > 0.68f && Roll(22))
            return OreType.Iron;

        if (oreNoise > 0.58f && Roll(30))
            return OreType.Coal;

        return OreType.None;
    }

    // ------------------------------------------------------------------ //
    //  Entity spawning                                                     //
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

    public void SpawnChunkTrees(World world, Chunk chunk)
    {
        foreach (var (pos, treeVariation) in chunk.Trees)
        {
            if (treeVariation == -1)
                continue;

            var (worldX, worldY) = pos;

            var entity = Prefabs.CreateTree(
                worldX * TileDatabase.TileSize,
                worldY * TileDatabase.TileSize,
                treeVariation
            );

            world.Entities.Add(entity);
        }
    }

    // ------------------------------------------------------------------ //
    //  Helpers                                                             //
    // ------------------------------------------------------------------ //

    private bool Roll(int percent) => _rand.Next(100) < percent;

    private bool IsDifferent(int x, int y, TileType type)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return false;

        return Map[x, y].Type != type;
    }
}