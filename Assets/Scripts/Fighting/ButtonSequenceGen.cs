using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Fighting
{
    public class ButtonSequenceGen
    {
        private bool IsButtonsGenerated { get; set; }
        
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

        public void GenerateSprites(ButtonInfo buttonInfo, int count, Queue<char> sequence, List<GameObject> instances)
        {
            var koef = 0.3f;
            var buttons = GenerateButtonSeq(count);
            var buttonPosition = buttonInfo.parent.transform.position + new Vector3((-count)*koef, 1, 0);
            foreach (var letter in buttons)
            {
                sequence.Enqueue(letter);
                buttonInfo.button.transform.localScale = new Vector3(2, 2, 2);
                buttonInfo.buttonSpriteRenderer.sortingOrder = 10;
                buttonInfo.buttonSpriteRenderer.sprite = 
                    Sprite.Create(buttonInfo.texture, new Rect(0, 0, buttonInfo.texture.width, 
                        buttonInfo.texture.height), Vector2.zero);
                buttonPosition += new Vector3(1*0.8f, 0, 0);
                instances.Add(buttonInfo.button);
            }
        }

        public void GenerateSprite(ButtonInfo buttonInfo, Queue<char> sequence, List<GameObject> instances, int count, Vector3 position)
        {
            var koef = 0.3f;
            
            sequence.Enqueue(buttonInfo.letter);
            buttonInfo.button.transform.localScale = new Vector3(2, 2, 2);
            buttonInfo.buttonSpriteRenderer.sortingOrder = 10;
            buttonInfo.buttonSpriteRenderer.sprite = 
                Sprite.Create(buttonInfo.texture, new Rect(0, 0, buttonInfo.texture.width, 
                    buttonInfo.texture.height), Vector2.zero);
            
            instances.Add(buttonInfo.button);
        }
    
    
    
    }
}
