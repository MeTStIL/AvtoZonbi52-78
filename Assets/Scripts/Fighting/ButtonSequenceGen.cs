using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Fighting
{
    public class ButtonSequenceGen
    {
        public bool IsButtonsGenerated { get; set; }
        
        public readonly Func<char> GenerateButton = () => 
            ButtonsGenerationInfo.Letters[new Random().Next(0, ButtonsGenerationInfo.Letters.Length)];
        
        public IEnumerable<char> GenerateButtonSeq(int count)
        {
            IsButtonsGenerated = true;
            return Enumerable.Range(0, count).Select(_ => GenerateButton());
        }

        public bool? CheckForCorrectClick(Queue<char> buttonsSequence, List<GameObject> buttonInstances)
        {
            if (!IsButtonsGenerated || buttonsSequence.Count <= 0) return null;
            var buttonKeyCode = ButtonsGenerationInfo.p_Buttons[buttonsSequence.First()];
            var button = buttonInstances[^buttonsSequence.Count];
            if (Input.GetKeyDown(buttonKeyCode))
            {
                buttonsSequence.Dequeue();
                ChangeButtonColor("apply", buttonKeyCode, button);
                return true;
            }
            if (!Input.anyKeyDown || ButtonsGenerationInfo.p_ExceptionButtons.Any(pair => Input.GetKeyDown(pair.Value)))
                return null;
            ChangeButtonColor("cancel", buttonKeyCode, button);
            return false;
        }

        private void ChangeButtonColor(string buttonType, KeyCode buttonKeyCode, GameObject button)
        {
            var texture = ButtonsGenerationInfo.p_ButtonTextures[buttonKeyCode + buttonType];
            CreateSpriteToObject(texture, button);
        }

        private void CreateSpriteToObject(Texture2D texture, GameObject gameObject)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite =
                Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        public void GenerateSprite(ButtonInfo buttonInfo, Queue<char> sequence, List<GameObject> instances, int count, Vector3 position)
        {
            sequence.Enqueue(buttonInfo.Letter);
            buttonInfo.Button.transform.localScale = new Vector3(2, 2, 2);
            buttonInfo.ButtonSpriteRenderer.sortingOrder = 100;
            buttonInfo.ButtonSpriteRenderer.sortingLayerName = "Collisions";
            buttonInfo.ButtonSpriteRenderer.sprite = 
                Sprite.Create(buttonInfo.Texture, new Rect(0, 0, buttonInfo.Texture.width, 
                    buttonInfo.Texture.height), Vector2.zero);
            instances.Add(buttonInfo.Button);
        }
    }
}
