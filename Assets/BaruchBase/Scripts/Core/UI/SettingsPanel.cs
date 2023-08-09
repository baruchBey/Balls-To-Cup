using DG.Tweening;
using UnityEngine;
using Baruch.Audio;
using UnityEngine.UI;

namespace Baruch.UI
{
    [DisallowMultipleComponent]
    public class SettingsPanel : MonoBehaviour, IInit
    {
        [SerializeField] GameObject _parent;

        [Header("Buttons")]
        [SerializeField] private Toggle _audioToggle = default;
        [SerializeField] private Toggle _hapticToggle = default;

        [SerializeField] RectTransform _panel;

        [SerializeField] Button _resume;
        [SerializeField] Button _background;


        bool _haptic = true;
        public void Init()
        {
            _audioToggle.isOn = AudioManager.AudioEnabled;

            //Didnt Include Haptic
            _hapticToggle.isOn = _haptic;

            _audioToggle.onValueChanged.AddListener(OnVolumeToggle);
            _hapticToggle.onValueChanged.AddListener(OnHapticToggle);

            _resume.onClick.AddListener(Resume);
            _background.onClick.AddListener(Resume);
        }

        public void OpenSettings()
        {
            gameObject.SetActive(true);
            _parent.SetActive(true);
            _resume.interactable = true;
            _background.interactable = true;

            _panel.localScale = Vector3.one * 0.5f;
            _panel.DOScale(1f, 0.2f).SetEase(Ease.OutSine).SetUpdate(true);

            _audioToggle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine).SetUpdate(true);
            _hapticToggle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine).SetUpdate(true);
        }

        private void Resume()
        {
            gameObject.SetActive(false);
            Core.Game.Resume();


            _resume.interactable = false;
            _background.interactable = false;
            _panel.DOScale(0.5f, 0.2f).SetEase(Ease.InSine).OnComplete(() => _parent.SetActive(false));

            _audioToggle.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
            _hapticToggle.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
        }


        

        private void OnVolumeToggle(bool value)
        {
            AudioManager.AudioEnabled = value;
            AudioManager.Instance.Play(AudioItemType.BallInCup);
        }

        private void OnHapticToggle(bool value)
        {
            _haptic = value;
            //HapticManager.HapticEnabled = value;
            //HapticManager.Instance.Play(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
        }

      
    }
}