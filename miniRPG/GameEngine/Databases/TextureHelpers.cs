using System.IO;
using miniRPG.GameEngine.Components;

namespace miniRPG.GameEngine.Databases;

public class TextureHelpers
{
    private readonly string[] AllowedExtensions = [".png", ".jpg", ".jpeg"];
    
    public Texture[] LoadAnimation(string dirPath)
    {
        try
        {
            var currentAnimation = Directory.EnumerateFiles(dirPath)
                .Where(f => AllowedExtensions.Contains(Path.GetExtension(f))).Select(SafeLoadTexture).ToArray();

            return currentAnimation;
        }
        catch
        {
            return [];
        }
    }

    private Texture SafeLoadTexture(string filePath)
    {
        using var bytes = new MemoryStream(File.ReadAllBytes(filePath));
        return new Texture { Image = Image.FromStream(bytes) };
    }
}