using UnityEngine;

namespace Fighting
{
    public class ButtonInfo
    {
        public readonly GameObject Button;
        public readonly char Letter;
        public readonly Texture2D Texture;
        public readonly SpriteRenderer ButtonSpriteRenderer;

        public ButtonInfo(GameObject button, char letter)
        {
            Button = button;
            Letter = letter;
            Texture = ButtonsGenerationInfo.p_ButtonTextures[letter.ToString()];
            ButtonSpriteRenderer = button.GetComponent<SpriteRenderer>();
        }
    }
}