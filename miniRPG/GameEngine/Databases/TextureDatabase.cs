using System.IO;
using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.Databases;

public class TextureDatabase
{
        private static readonly string[] AllowedExtensions = [".png", ".jpg", ".jpeg"];
        
        private static readonly Dictionary<string, Texture> _database = new();
        private static readonly Dictionary<string, List<Texture>> _animationDatabase = new();
        
        public static void Load(string name, Texture texture)
        {
                _database[name] = texture;
        }

        public static void Load(string name, string dirPath)
        {
                _animationDatabase[name] = LoadList(dirPath);
        }

        private static List<Texture> LoadList(string dirPath)
        {
                var textureList = new List<Texture>();

                try
                {
                        var animationList = Directory.EnumerateFiles(dirPath)
                                .Where(f => AllowedExtensions.Contains(Path.GetExtension(f))).Select(SafeLoad).ToArray();

                        for (int i = 0; i < animationList.Count(); i++)
                        {
                                textureList[i] = new Texture { Image = animationList[i] };
                        }

                        return textureList;
                }
                catch
                {
                        return [];
                }
        }
                
        public static Texture Get(string name) => _database[name];
        public static bool Contains(string name) => _database.ContainsKey(name);
        
        
        // Helper for performance improvement, prevents "File in use" error
        private static Image SafeLoad(string filePath)
        {
                using var bytes = new MemoryStream(File.ReadAllBytes(filePath));
                return Image.FromStream(bytes);
        }
}