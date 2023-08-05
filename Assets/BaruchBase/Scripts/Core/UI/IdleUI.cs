using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class IdleUI : SubUI
    {
        [Header("Buttons")]
        [SerializeField] Button _settingsButton;

        [Header("Components")]
        [SerializeField] SettingsPanel _settingsPanel;
      


        public override void Init()
        {
            _settingsButton.onClick.AddListener(_settingsPanel.Activate);
            _settingsButton.onClick.AddListener(Core.Game.Pause);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);//Juice
        }

    
        public override void OnDisable()
        {
            throw new NotImplementedException();
        }

        public override void OnEnable()
        {

        }

        void OnSettingsButtonClicked()
        {
            _settingsButton.transform.DORotate(Vector3.back * 90, 0.4f, RotateMode.LocalAxisAdd).SetEase(Ease.OutQuint);
        }

    }
}