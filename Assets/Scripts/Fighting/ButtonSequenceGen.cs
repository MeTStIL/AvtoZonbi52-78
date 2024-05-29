using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Fighting
{
    public class ButtonSequenceGen
    {
        public static Dictionary<char, KeyCode> buttons = new()
        {
            { 'E', KeyCode.E },
            { 'Q', KeyCode.Q },
            { 'Z', KeyCode.Z },
            { 'C', KeyCode.C },
            { 'F', KeyCode.F },
            { 'R', KeyCode.R },
        };

        public static Dictionary<char, KeyCode> buttonsEducation = new()
        {
            { 'W', KeyCode.W },
            { 'A', KeyCode.A },
            { 'S', KeyCode.S },
            { 'D', KeyCode.D }
        };
        
    public static char[] GenerateButtonSeq(string letters, int count)
        {
            var result = new char[count];
            var random = new Random();
            for (var i = 0; i < count; i++)
            {
                var index = random.Next(0, letters.Length);
                result[i] = letters[index];
            }
            return result;
        }

    public static char GenerateButton(string letters)
    {
        var random = new Random();
        return letters[random.Next(0, letters.Length)];
    } 
    
    }
    
    
    
}
