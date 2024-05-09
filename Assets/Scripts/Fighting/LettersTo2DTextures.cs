using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    
    public class LettersTo2DTextures
    {
        public static Dictionary<string, Texture2D> ConnectCharWithTexture(string sequence)
        {
            var result = new Dictionary<string, Texture2D>();
            foreach (var s in sequence)
            {
                var letter = s.ToString().ToUpper();
                
                var imagePath = $"Assets/Buttons/{letter}/{letter}.png";
                var texture = LoadTextureFromPath(imagePath);
                result[letter] = texture;
                imagePath = $"Assets/Buttons/{letter}/{letter}_apply.png";
                texture = LoadTextureFromPath(imagePath);
                result[letter+"apply"] = texture;
                imagePath = $"Assets/Buttons/{letter}/{letter}_cancel.png";
                texture = LoadTextureFromPath(imagePath);
                result[letter+"cancel"] = texture;
            }
            return result;
        }
        
        private static Texture2D LoadTextureFromPath(string path)
        {
            Texture2D texture = null;
            byte[] fileData;

            if (System.IO.File.Exists(path))
            {
                fileData = System.IO.File.ReadAllBytes(path);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData); // Загрузка данных изображения в текстуру
            }
            else
            {
                Debug.LogError("Файл не найден: " + path);
            }

            return texture;
        }
            
    }
}