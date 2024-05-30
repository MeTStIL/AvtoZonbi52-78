using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    public class ButtonSpitesGenerator : MonoBehaviour
    {

        public void GenerateButtonSequence(int count, SpriteRenderer parent, Queue<char> sequence, List<GameObject> instances)
        {
            var buttonGenerator = new ButtonSequenceGen();
            var buttonPrefab = Resources.Load<GameObject>("button");
            var koef = 0.3f;
            var buttons = buttonGenerator.GenerateButtonSeq(count);
            var buttonPosition = parent.transform.position + new Vector3((-count)*koef, 1, 0);
        
            foreach (var letter in buttons)
            {
                var newButton = Instantiate(buttonPrefab, buttonPosition, Quaternion.identity, transform);
                var buttonInfo = new ButtonInfo(newButton, letter, parent);
                buttonGenerator.GenerateSprite(buttonInfo, sequence, instances, count, buttonPosition);
                
            }
        }
    }
}