using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class ImageMessageConverter
{
    public static Dictionary<int, Texture2D> GetImages(string imagesPath)
    {
        var images = new Dictionary<int, Texture2D>();
        for (var i = 0; i < Directory.GetFiles(imagesPath).Length; i++)
        {
            var fileData = File.ReadAllBytes(imagesPath + $"/{i}.png");
            var texture = new Texture2D(1000, 500);
            texture.LoadImage(fileData);
            images[i] = texture;
        }
        return images;
    }

    public static List<string> GetTexts(string messagesPath)
    {
        var fileContent = File.ReadAllText(messagesPath);
        var sentences = fileContent.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        return sentences.Select(sentence => sentence.Trim()).ToList();
    }
}