using miniRPG.Enums;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Databases;
using miniRPG.GameEngine.Entities;

namespace miniRPG.GameEngine.Core.WorldTerrain;

public class Terrain
{
    private int Width { get; set; }
    private int Height { get; set; }
    
    public Tile[,] Map { get; private set; }
    private readonly Dictionary<(int chunkX, int chunkY), Chunk> _chunks = new();

    private Random _rand = new Random();

    private readonly PerlinNoise _noise;
    private readonly float _biomeScale = 0.02f;
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
        var offsetX = chunk.ChunkX * Chunk.Size;
        var offsetY = chunk.ChunkY * Chunk.Size;
        Console.WriteLine($"OffsetX: {offsetX} Y: {offsetY}");

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Size; y++)
            {
                var calcX = (offsetX + x);
                var calcY = (offsetY + y);

                var biome = _noise.Sample(calcX * _biomeScale, calcY * _biomeScale);
                var detail = _noise.Sample(calcX * _detailScale, calcY * _detailScale);

                chunk.Map[x, y] = GenerateTile(biome, detail);
                chunk.Ores[(calcX, calcY)] = GetOreType(chunk.Map[x, y]); 
            }
        }
    }

    private OreType GetOreType(Tile currentTile)
    {
        if (currentTile.Type == TileType.Mountain)
        {
            var random = _rand.Next(0, 100);

            if (random <= 3)
                return OreType.Bronze;
        }

        return OreType.None;
    }

    public void SpawnChunkOres(World world, Chunk chunk)
    {
        foreach (var oreEntry in chunk.Ores)
        {
            var (worldX, worldY) = oreEntry.Key;
            var oreType = oreEntry.Value;

            if (oreType == OreType.None)
                continue;

            Console.WriteLine($"X: {worldX * TileDatabase.TileSize} Y: {worldY  * TileDatabase.TileSize}");
            var e = EntityFactory.CreateBronzeRock(
                worldX * TileDatabase.TileSize, worldY * TileDatabase.TileSize
                );
            
            world.Entities.Add(e);
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