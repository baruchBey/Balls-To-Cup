using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class FailUI : SubUI
    {
        [SerializeField] Button _restartButton;
        Tween _scaleTween;

        public override void Init()
        {
            _restartButton.onClick.AddListener(LevelManager.RestartLevel);
        }

        public override void Enable()
        {
            base.Enable();
            
            if (!_scaleTween.IsActive())
            {
                _scaleTween = _restartButton.transform.parent.DOScale(new Vector3(-1.1f,1.1f,1.1f), 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
        }
        public override void Disable()
        {
            base.Disable();
            _scaleTween.Kill();
        }

    }
}