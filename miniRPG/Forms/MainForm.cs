using miniRPG.Helpers;
using miniRPG.GameEngine;
using miniRPG.GameEngine.Components;
using miniRPG.GameEngine.Entities;
using miniRPG.GameEngine.Other;

namespace miniRPG.Forms;
    
public partial class MainForm : Form
{
    private readonly Engine _engine;
    private readonly Terrain _terrain;
    private long _lastElapsedMs;

    private int _renderCoordsX = 0;
    private int _renderCoordsY = 0;
    private int _sum = 0;

    private int width = 100;
    private int height = 100;

    private const int TileSize = 98;
    
    public MainForm()
    {
        InitializeComponent();
        
        // Performance optimizations for smoother rendering
        var player = EntityFactory.CreatePlayer();
        var camera = EntityFactory.CreateCamera();
        var testEntity = EntityFactory.TestEntity();
        
        _engine = new Engine();
        
        // Generate test terrain
        _terrain = new Terrain(10, 10);
        _terrain.GenerateTestMap();

        /*var _tiles = File.ReadAllLines("Resources/Terrain/Biome/TestMap.txt");
        var _tilesChars = _tiles.SelectMany(line => line.ToCharArray()).ToArray();
        

        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 99)
                {
                    _renderCoordsY += TileSize;
                    _renderCoordsX = 0;
                }
                
                if (_tilesChars[_sum] == 'G')
                    _engine.World.Entities.Add(EntityFactory.CreateGrassEntity(_renderCoordsX, _renderCoordsY));
                else if (_tilesChars[_sum] == 'M')
                    _engine.World.Entities.Add(EntityFactory.CreateGrassEntity(_renderCoordsX, _renderCoordsY));
                else if (_tilesChars[_sum] == 'O')
                    _engine.World.Entities.Add(EntityFactory.CreateGrassEntity(_renderCoordsX, _renderCoordsY));

                _sum++;
                _renderCoordsX += TileSize;
            }
        }*/
        
        _engine.World.Entities.Add(camera);
        _engine.World.Entities.Add(player);
        _engine.World.Entities.Add(testEntity);
        
        
        // start stopwatch to calculate delta time
        _lastElapsedMs = 0;
    }

    private void GameTimerLoop(object sender, EventArgs e)
    {
        var now = _stopwatch.ElapsedMilliseconds;
        var delta = (int)(now - _lastElapsedMs);
        if (delta <= 0) delta = 1;
        _lastElapsedMs = now;

        _engine.Update(delta);
        Invalidate();
    }

    // Run render method of the engine, passing with all the context
    private void MainForm_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        var context = new RenderContext
        {
            Graphics = e.Graphics,
            X = ClientSize.Width / 2f,
            Y = ClientSize.Height / 2f,
        };
        
        // Render first the terrain because of the layering
        // terrain will be behind everything else
        _engine.TerrainRender.Render(_terrain, _engine.World, context);
        
        // Render the items
        _engine.Render.Render(_engine.World, context);
        g.ResetTransform();
    }
    
    // Prevent background from being erased (avoids flicker when using double buffering)
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // Intentionally empty - full redraw performed in OnPaint
    }
    
    // Methods providing information for my helper class so the keyboard
    // is "global" to the whole project
    private void MainForm_KeyDown(object sender, KeyEventArgs e) 
    {
        Keyboard.KeyDown(e.KeyCode);
    }

    private void MainForm_KeyUp(object sender, KeyEventArgs e) 
    {
        Keyboard.KeyUp(e.KeyCode);
    }
}