namespace miniRPG.Helpers;

using System.IO;
using System.Drawing;
using GameEngine.Components;


public static class TextureLoader
{
    private static readonly string[] Allowed = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };

    public static Texture ToTexture(Image image) => new Texture { Image = image };

    // Old method use only if needed
    public static Image? LoadImage(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            if (!Allowed.Contains(Path.GetExtension(filePath)))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            using var memoryStream = new MemoryStream(bytes);

            return Image.FromStream(memoryStream);
        }
        catch
        {
            return null;
        }
    }

    public static Texture[]? LoadTiles(string dirPath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dirPath))
                return null;

            var currentTextures = Directory.EnumerateFiles(dirPath)
                .Where(f => Allowed.Contains(Path.GetExtension(f))).Select(SafeLoad).ToArray();

            return currentTextures;
        }
        catch
        {
            return null;
        }
    }
    
    public static Texture? LoadTexture(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            if (!Allowed.Contains(Path.GetExtension(filePath)))
                return null;

            byte[] bytes = File.ReadAllBytes(filePath);
            using var memoryStream = new MemoryStream(bytes);

            return ToTexture(Image.FromStream(memoryStream));
        }
        catch
        {
            return null;
        }
    }
    
    public static Texture[] LoadAnimation(string dirPath)
    {
        try
        {
            var currentAnimation = Directory.EnumerateFiles(dirPath)
                .Where(f => Allowed.Contains(Path.GetExtension(f))).Select(SafeLoad).ToArray();

            return currentAnimation;
        }
        catch
        {
            return [];
        }
    }
    
    // Helper for performance improvement, prevents "File in use" error
    private static Texture SafeLoad(string filePath)
    {
        using var bytes = new MemoryStream(File.ReadAllBytes(filePath));
        return ToTexture(Image.FromStream(bytes));
    }
}