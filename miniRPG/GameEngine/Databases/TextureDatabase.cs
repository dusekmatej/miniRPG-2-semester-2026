namespace miniRPG.GameEngine.Databases;

using Helpers;
using Components;

public static class TextureDatabase
{       
        private static readonly Dictionary<string, Texture> _database = new();
        private static readonly Dictionary<string, Texture[]> _animationDatabase = new();
        
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
}