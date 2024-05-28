using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MovementTips : Sounds
{
    [SerializeField] GameObject[] buttons;
    private char[] buttonsChar => new[] { 'W', 'A', 'S', 'D' };
    private static int correctCount;

    private Dictionary<string, Texture2D> buttonTextures => Fighting.LettersTo2DTextures.ConnectCharWithTexture("WASD");
    
    private void Update()
    {
        if (correctCount < 4)
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                if (Input.GetKeyDown(Fighting.ButtonSequenceGen.buttonsEducation[buttonsChar[i]]))
                    MakeButtonCorrect(buttons[i], buttonsChar[i]);
            }
        }
        else
            DestroyImmediate(gameObject);
    }

    private void MakeButtonCorrect(GameObject button, char letter)
    {
        PlaySound(objectSounds[0], volume: 0.8f, fadeInTime: 0);
        correctCount += 1;
        var texture = buttonTextures[letter + "apply"];
        button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, buttonTextures[letter.ToString()].width, 
            buttonTextures[letter.ToString()].height), Vector2.zero);
       
    }
}
