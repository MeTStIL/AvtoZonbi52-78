using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Fighting
{
    
    public static class LettersTo2DTextures
    {
        public static Dictionary<string, Texture2D> ConnectCharWithTexture(string sequence)
        {
            var basePath = "Assets/Buttons";
            return sequence
                .Select(letter => letter.ToString().ToUpper())
                .SelectMany(letter => new[] { "", "apply", "cancel" }
                    .Select(suffix => new
                    {
                        Key = $"{letter.ToUpper()}{suffix}",
                        TexturePath = suffix.Length>0? Path.Combine(basePath, letter, $"{letter}_{suffix}.png"):
                            Path.Combine(basePath, letter, $"{letter}{suffix}.png")
                    }))
                .Where(item => File.Exists(item.TexturePath))
                .ToDictionary(
                    item => item.Key,
                    item => LoadTextureFromPath(item.TexturePath)
                );
        }
        
        public static Texture2D LoadTextureFromPath(string path)
        {
            var fileData = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return texture;
        }
            
    }
}