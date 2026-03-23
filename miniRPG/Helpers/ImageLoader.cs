namespace miniRPG.Helpers;

public static class ImageLoader
{
    private static readonly String[] AllowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
    
    public static List<Image>? LoadAnimation(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !Directory.Exists(filePath))
                return new List<Image>();

            var files = Directory.GetFiles(filePath)
                .Where(f => AllowedExtensions.Contains(
                    Path.GetExtension(f),
                    StringComparer.OrdinalIgnoreCase));

            var images = new List<Image>();
            
            foreach (var file in files)
                images.Add(Image.FromFile(file));

            return images;
        }
        catch
        {
            return new List<Image>();
        }
    }

    public static Image? LoadImage(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            return Image.FromFile(filePath);
        }
        catch
        {
            return null;
        }
    }
}