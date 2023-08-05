using Baruch.Extension;
using System;
using UnityEngine;

namespace Baruch
{

    [CreateAssetMenu(fileName = "Palette 0", menuName = "Baruch/ColorPalettePalette", order = -1)]

    public class ColorPaletteSO : ScriptableObject, IConfigure
    {
        public Color[] Colors;
        public SkyboxColor SkyboxColor;

        public string[] HexCodes;

        public Color GroundColor;

        public void Configure()
        {
            var skybox = RenderSettings.skybox;
            skybox.SetColor("_Top", SkyboxColor.TopColor);
            skybox.SetColor("_Bottom", SkyboxColor.BottomColor);
        }
        private void OnValidate()
        {
            // Make sure the HexCodes array has the same length as the Colors array
            if (Colors != null && Colors.Length > 0)
            {
                if (HexCodes == null || HexCodes.Length != Colors.Length)
                    HexCodes = new string[Colors.Length];

                // Convert each color in the Colors array to its corresponding hex code
                for (int i = 0; i < Colors.Length; i++)
                {
                    HexCodes[i] = ColorUtility.ToHtmlStringRGB(Colors[i].AsValue(1f));
                }
            }
            else
            {
                // If the Colors array is empty or null, clear the HexCodes array
                HexCodes = null;
            }
        }
    }
}