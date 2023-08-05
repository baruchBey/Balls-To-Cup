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


        public void Init()
        {
            _audioToggle.onValueChanged.AddListener(OnVolumeToggle);

            _resume.onClick.AddListener(Deactivate);
            _resume.onClick.AddListener(Core.Game.Resume);
        }

        public void Activate()
        {
            _audioToggle.isOn = AudioManager.AudioEnabled;

            _parent.SetActive(true);
            _resume.interactable = true;

            _panel.localScale = Vector3.one * 0.5f;
            _panel.DOScale(1f, 0.2f).SetEase(Ease.OutSine);
            
            _audioToggle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);
            _hapticToggle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);
        }

        public void Deactivate()
        {
            _resume.interactable = false;
            _panel.DOScale(0.5f, 0.2f).SetEase(Ease.InSine).OnComplete(()=> _parent.SetActive(false));

            _audioToggle.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
            _hapticToggle.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InSine);
        }



        private void OnVolumeToggle(bool value)
        {
            AudioManager.AudioEnabled = value;
            AudioManager.Instance.Play(AudioItemType.Pop);
        }

       

    }
}