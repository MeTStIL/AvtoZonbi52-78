using System.Collections.Generic;
using Fighting;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MovementTips : Sounds
{
    [SerializeField] GameObject[] buttons;
    private static char[] ButtonsChar => new[] { 'W', 'A', 'S', 'D' };
    private static int _correctCount;

    private static Dictionary<string, Texture2D> ButtonTextures => LettersTo2DTextures.ConnectCharWithTexture("WASD");
    
    private void Update()
    {
        if (_correctCount < 4)
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                if (Input.GetKeyDown(ButtonsGenerationInfo.p_ButtonsEducation[ButtonsChar[i]]))
                    MakeButtonCorrect(buttons[i], ButtonsChar[i]);
            }
        }
        else
        {
            _correctCount = 0;
            DestroyImmediate(gameObject);
            //gameObject.SetActive(false);
        }
    }

    private void MakeButtonCorrect(GameObject button, char letter)
    {
        PlaySound(objectSounds[0], volume: 0.8f, fadeInTime: 0);
        _correctCount += 1;
        var texture = ButtonTextures[letter + "apply"];
        button.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, ButtonTextures[letter.ToString()].width, 
            ButtonTextures[letter.ToString()].height), Vector2.zero);
       
    }
}
