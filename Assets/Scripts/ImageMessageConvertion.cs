using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public static class ImageMessageConvertion
    {

        public static Dictionary<int, Texture2D> GetImages(string imagesPath)
        {
            var images = new Dictionary<int, Texture2D>();
            

            for (var i = 0; i < Directory.GetFiles(imagesPath).Length; i++)
            {
                if (System.IO.File.Exists(imagesPath + $"/{i}.png"))
                {
                    var fileData = System.IO.File.ReadAllBytes(imagesPath + $"/{i}.png");
                    var texture = new Texture2D(1000, 500);
                    texture.LoadImage(fileData); // Загрузка данных изображения в текстуру
                    images[i] = texture;
                }
                else
                {
                    Debug.Log($"не найден {imagesPath + $"/{i}.png"}");
                }
            }

            return images;
        }

        public static List<string> GetTexts(string messagesPath)
        {
            var sentencesList = new List<string>();
            string fileContent = File.ReadAllText(messagesPath);

            // Разделяем содержимое файла на предложения, используя точку с запятой или точку в качестве разделителя
            string[] sentences = fileContent.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);

            // Добавляем каждое предложение в список
            foreach (var sentence in sentences)
            {
                sentencesList.Add(sentence.Trim());
            }
            return sentencesList;
        }
    }
}