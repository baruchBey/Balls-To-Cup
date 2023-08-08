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
        [SerializeField] private Button _audioToggle = default;
        [SerializeField] private GameObject _audioShadow= default;
        [SerializeField] private Button _hapticToggle = default;


        [SerializeField] Button _resume;


        public void Init()
        {
            _audioToggle.onClick.AddListener(OnVolumeToggle);

            _resume.onClick.AddListener(Deactivate);
            _resume.onClick.AddListener(Core.Game.Resume);
        }

        public void Activate()
        {
            _audioShadow.SetActive(!AudioManager.AudioEnabled);

            _parent.SetActive(true);
            _resume.interactable = true;

       
            
            _audioToggle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);
            _hapticToggle.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);
        }

        public void Deactivate()
        {
            _resume.interactable = false;

          
        }



        private void OnVolumeToggle()
        {
            AudioManager.AudioEnabled = !AudioManager.AudioEnabled;
            AudioManager.Instance.Play(AudioItemType.BalanceGain);
        }

       

    }
}