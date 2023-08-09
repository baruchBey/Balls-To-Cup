using Baruch.Extension;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class IdleUI : SubUI
    {
        [SerializeField] TMP_Text _levelText;
        


        [Header("Buttons")]
        [SerializeField] Button _settingsButton;

        [Header("Components")]
        [SerializeField] SettingsPanel _settingsPanel;



        public override void Enable()
        {
            _levelText.text = $"LEVEL {LevelManager.LevelIndex+1}";
            base.Enable();
        }

        public override void Init()
        {
            _settingsButton.onClick.AddListener(_settingsPanel.OpenSettings);
            _settingsButton.onClick.AddListener(Core.Game.Pause);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);//Juice
        }


       
        void OnSettingsButtonClicked()
        {
            _settingsButton.transform.DORotate(Vector3.back * 90, 0.4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuint);
        }

    }
}