using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Baruch.UI
{
    public class GameUI : Singleton<GameUI>,IInit
    {
        [Header("Buttons")]
        [SerializeField] Button _settingsButton;
        
        [Header("Components")]
        [SerializeField] SettingsPanel _settingsPanel;
        

        
        public void Init()
        {

            LevelManager.OnLevelBuild += OnLevelBuild;

            _settingsButton.onClick.AddListener(_settingsPanel.Activate);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);//Juice


        }

        void OnSettingsButtonClicked()
        {
            _settingsButton.transform.DORotate(Vector3.back * 90, 0.4f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuint);
        }
   

        private void OnLevelBuild()
        {
            LevelManager.OnLevelBuild -= OnLevelBuild;
        }

    }
}