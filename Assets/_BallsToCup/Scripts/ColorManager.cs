using System;
using UnityEngine;

namespace Baruch
{
    internal class ColorManager : Singleton<ColorManager>, IInit
    {
        [SerializeField] Material _groundMaterial;
        public static int ColorCount => CurrentPalette.Colors.Length;

        public static ColorPaletteSO CurrentPalette { get => _currentPalette; set { _currentPalette = value; _currentPalette.Configure(); } }

        [SerializeField] ColorPaletteSO[] _colorPaletteSOs;
        static ColorPaletteSO _currentPalette;

        public void Init()
        {
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;
        }

        private void LevelManager_OnLevelBuild()
        {
            CurrentPalette = _colorPaletteSOs[LevelManager.LevelIndex % _colorPaletteSOs.Length];
            _groundMaterial.color = CurrentPalette.GroundColor;
        }

        internal Color GetColor(byte value)
        {

            return _currentPalette.Colors[value];
        }

        internal string GetHex(byte value)
        {
            return _currentPalette.HexCodes[value];
        }
    }
}