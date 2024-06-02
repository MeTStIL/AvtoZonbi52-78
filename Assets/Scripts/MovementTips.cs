using System.Collections.Generic;
using Fighting;
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
                if (Input.GetKeyDown(ButtonsGenerationInfo.p_ButtonsEducation[buttonsChar[i]]))
                    MakeButtonCorrect(buttons[i], buttonsChar[i]);
            }
        }
        else
        {
            correctCount = 0;
            DestroyImmediate(gameObject);
            //gameObject.SetActive(false);
        }
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
