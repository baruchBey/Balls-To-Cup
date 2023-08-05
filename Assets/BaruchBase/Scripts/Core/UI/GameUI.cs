using Baruch.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch
{
    public class GameUI : SubUI
    {
        [SerializeField] RectTransform _ballCountParent;
        [SerializeField] TMP_Text _ballCount;
        [SerializeField] Image[] _balls;
        
        public override void Init()
        {
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;
            BottleExit.OnMarbleExit += BottleExit_OnMarbleExit;
        }

        private void LevelManager_OnLevelBuild()
        {
            for (int i = 0; i < _balls.Length; i++)
            {
                _balls[i].color = ColorManager.CurrentPalette.Colors[i];
            }
            _ballCount.text = LevelManager.Instance.CurrentLevel.MarbleInBottleCount.ToString();


        }

        private void BottleExit_OnMarbleExit()
        {
            _ballCount.text = LevelManager.Instance.CurrentLevel.MarbleInBottleCount.ToString();

        }


    }
}
