using System.Collections.Generic;
using UnityEngine;

namespace Fighting
{
    public static class ButtonsGenerationInfo
    {
        public const string Letters = "EQZCFR";
        public static readonly Dictionary<string, Texture2D> p_ButtonTextures = LettersTo2DTextures
            .ConnectCharWithTexture(Letters);
        
        public static readonly Dictionary<char, KeyCode> p_Buttons = new()
        {
            { 'E', KeyCode.E },
            { 'Q', KeyCode.Q },
            { 'Z', KeyCode.Z },
            { 'C', KeyCode.C },
            { 'F', KeyCode.F },
            { 'R', KeyCode.R },
        };

        public static readonly Dictionary<char, KeyCode> p_ButtonsEducation = new()
        {
            { 'W', KeyCode.W },
            { 'A', KeyCode.A },
            { 'S', KeyCode.S },
            { 'D', KeyCode.D }
        };

        public static readonly Dictionary<char, KeyCode> p_ExceptionButtons = new()
        {
            { 'W', KeyCode.W },
            { 'A', KeyCode.A },
            { 'S', KeyCode.S },
            { 'D', KeyCode.D }
        };
    }
}