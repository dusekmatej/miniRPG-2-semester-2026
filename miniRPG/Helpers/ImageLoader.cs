namespace miniRPG.Helpers;

using System.IO;
using GameEngine.Components;


public static class ImageLoader
{
    // HashSet is faster than array/list, 
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
    
    // TODO: Refactor to use arrays instead of lists for better performance and memory management
    public static List<Image>? LoadAnimation(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !Directory.Exists(filePath))
                return new List<Image>();

            return Directory.EnumerateFiles(filePath)
                .Where(f => AllowedExtensions.Contains(Path.GetExtension(f)))
                .Select(SafeLoad).ToList();
        }
        catch
        {
            return new List<Image>();
        }
    }

    public static Image? Image(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            if (!AllowedExtensions.Contains(Path.GetExtension(filePath)))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            using var memoryStream = new MemoryStream(bytes);

            return System.Drawing.Image.FromStream(memoryStream);
        }
        catch
        {
            return null;
        }
    }
    
    public static Texture? Texture(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            if (!AllowedExtensions.Contains(Path.GetExtension(filePath)))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            using var memoryStream = new MemoryStream(bytes);

            return new Texture { Image = System.Drawing.Image.FromStream(memoryStream) };
        }
        catch
        {
            return null;
        }
    }

    public static Image[]? AllImagesFromDir(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !Directory.Exists(filePath))
                return null;

            return Directory.EnumerateFiles(filePath)
                .Where(f => AllowedExtensions.Contains(Path.GetExtension(f)))
                .Select(SafeLoad).ToArray();
        }
        catch
        {
            return null;
        }
        
    }
    
    // Helper for performance improvement, prevents "File in use" error
    private static Image SafeLoad(string filePath)
    {
        using var bytes = new MemoryStream(File.ReadAllBytes(filePath));
        return System.Drawing.Image.FromStream(bytes);
    }
}