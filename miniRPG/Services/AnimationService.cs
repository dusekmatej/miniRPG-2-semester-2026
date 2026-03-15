using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace miniRPG.Services;

public static class AnimationService
{
    private static List<Image?>? _images;
    private static String[] _allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
    
    public static List<Image>? LoadAllDirImages(string filePath)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filePath) || !Directory.Exists(filePath))
                return new List<Image>();

            var files = Directory.GetFiles(filePath)
                .Where(f => _allowedExtensions.Contains(
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
}