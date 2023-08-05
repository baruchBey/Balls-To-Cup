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
            
        }
    }
}