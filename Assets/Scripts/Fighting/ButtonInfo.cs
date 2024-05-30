using UnityEngine;

namespace Fighting
{
    public class ButtonInfo
    {
        public GameObject button;
        public char letter;
        public Vector3 buttonPosition;
        public Texture2D texture;
        public SpriteRenderer buttonSpriteRenderer;
        public SpriteRenderer parent;

        public ButtonInfo(GameObject button, char letter, SpriteRenderer parent)
        {
            this.button = button;
            this.parent = parent;
            this.letter = letter;
            texture = ButtonsGenerationInfo.p_ButtonTextures[letter.ToString()];
            buttonSpriteRenderer = button.GetComponent<SpriteRenderer>();
        }
    }
}