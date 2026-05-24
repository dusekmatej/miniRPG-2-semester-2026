using System.IO;
using System.Text.Json;
using System.Windows.Shapes;

namespace miniRPG.GameEngine.SaveSystem;

public class SaveManager
{
    private const string SaveFileName = "save.json";
    
    private static string SavePath => $"{AppDomain.CurrentDomain.BaseDirectory}Saves/{SaveFileName}";
    
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true 
    };
    public void Save(SaveData data)
    {
        try
        {
            var dir = $"{AppDomain.CurrentDomain.BaseDirectory}Saves";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(SavePath, json);
            Console.WriteLine($"[SaveManager] Saved to: {SavePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SaveManager] Save failed: {ex.Message}");
        }
    }
    public SaveData? Load()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Console.WriteLine("[SaveManager] No save file found.");
                return null;
            }

            var json = File.ReadAllText(SavePath);
            var data = JsonSerializer.Deserialize<SaveData>(json, _options);
            Console.WriteLine("[SaveManager] Game loaded successfully.");
            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SaveManager] Failed to load: {ex.Message}");
            return null;
        }
    }

    public bool SaveExists() => File.Exists(SavePath);
}