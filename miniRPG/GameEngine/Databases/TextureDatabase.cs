using System.IO;

namespace miniRPG.GameEngine.Databases;

using Helpers;
using Components;

// ReSharper disable InconsistentNaming
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

public static class TextureDatabase
{       
        private static readonly Dictionary<string, Texture> _database = new();
        private static readonly Dictionary<string, Texture[]> _animationDatabase = new();

        public static void Initialize()
        {
                foreach (var path in _imagePaths)
                {
                        var name = Path.GetFileNameWithoutExtension(path);
                        Console.WriteLine($"Populated texture database: {name} from {path}");
                        Load(name, path);
                }

                foreach (var path in _animationPaths)
                {
                        var name = Path.GetFileNameWithoutExtension(path);
                        Console.WriteLine($"Populated animation database: {name} from {path}");
                        Load(name, path, true);
                }
        }
        
        
        public static void Load(string name, string path, bool isAnimation = false)
        {
                if (!isAnimation)
                        _database[name] = TextureLoader.LoadTexture(path)!;
                else
                        _animationDatabase[name] = TextureLoader.LoadAnimation(path);
        }

        // Database getters
        public static Texture Get(string name) => _database[name];
        public static Texture[] GetAnimation(string name) => _animationDatabase[name];
        
        // Check if exist
        public static bool Contains(string name) => _database.ContainsKey(name);
        public static bool AnimationContains(string name) => _animationDatabase.ContainsKey(name);
        
        #region BASE PATHS
        private static readonly string APP_BASE = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string BASE_TERRAIN = Path.Join(APP_BASE, "Art/Terrain");
        private static readonly string BASE_ROCKS = Path.Join(BASE_TERRAIN, "Objects/Rocks");
        private static readonly string BASE_CHARACTER = Path.Join(APP_BASE, "Art/Character");
        private static readonly string BASE_ENEMY = Path.Join(APP_BASE, "Art/Enemy");
        private static readonly string BASE_TOOLS = Path.Join(APP_BASE, "Art/Terrain/Items/Tools");
        private static readonly string BASE_ORES =  Path.Join(BASE_TERRAIN, "Items/Ores");
        private static readonly string BASE_INGOTS = Path.Join(BASE_TERRAIN, "Items/Ingots");
        private static readonly string BASE_UI = Path.Join(BASE_TERRAIN, "Ui");
        private static readonly string BASE_FOOD  = Path.Join(BASE_TERRAIN, "Items/Food");
        private static readonly string BASE_POTION = Path.Join(BASE_TERRAIN, "Items/Potions");
        private static readonly string BASE_CHEST = Path.Join(BASE_TERRAIN, "Objects/Chests");
        #endregion
        
        #region ADD TEXTURES ONLY HERE!
        private static readonly string[] _animationPaths =
        {
                $"{BASE_CHARACTER}/idle",
                $"{BASE_CHARACTER}/run_down",
                $"{BASE_CHARACTER}/run_left",
                $"{BASE_CHARACTER}/run_right",
                $"{BASE_CHARACTER}/run_up",
                
                $"{BASE_ENEMY}/idle_enemy",
                $"{BASE_ENEMY}/run_down_enemy",
                $"{BASE_ENEMY}/run_left_enemy",
                $"{BASE_ENEMY}/run_right_enemy",
                $"{BASE_ENEMY}/run_up_enemy",
                
                $"{BASE_ROCKS}/coal_rock",
                $"{BASE_ROCKS}/bronze_rock",
                $"{BASE_ROCKS}/iron_rock",
                $"{BASE_ROCKS}/gold_rock",
                
                
        };

        private static readonly string[] _imagePaths =
        {
                
                $"{BASE_UI}/Inventory/inventory_hotbar.png",
                $"{BASE_UI}/Inventory/inventory_open.png",
                
                $"{BASE_ORES}/bronze_ore.png",
                $"{BASE_ORES}/iron_ore.png",
                $"{BASE_ORES}/coal_ore.png",
                $"{BASE_ORES}/gold_ore.png",
                
                $"{BASE_INGOTS}/bronze_ingot.png",
                $"{BASE_INGOTS}/iron_ingot.png",
                $"{BASE_INGOTS}/coal_ingot.png",
                $"{BASE_INGOTS}/gold_ingot.png",
                
                $"{BASE_TOOLS}/axe.png",
                $"{BASE_TOOLS}/pickaxe.png",
                $"{BASE_TOOLS}/shovel.png",
                $"{BASE_TOOLS}/sword.png",
                
                $"{BASE_FOOD}/brown_meat.png",
                $"{BASE_FOOD}/light_meat.png",
                
                $"{BASE_POTION}/small_health_potion.png",
                
                $"{BASE_CHEST}/small_iron_chest.png"
                
                
                
        };
        
        #endregion
}