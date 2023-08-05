using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class CompleteUI : SubUI
    {
        [SerializeField] Button _continueButton;
        [SerializeField] Button _restartButton;

        [SerializeField] Image[] _stars;

        int _target;
        int _totalMarbleCount;

        Tween _scaleTween;

        public override void Init()
        {
            _continueButton.onClick.AddListener(LevelManager.NextLevel);
            _restartButton.onClick.AddListener(LevelManager.RestartLevel);
            Finish.OnTargetReached += Finish_OnTargetReached;
            BottleExit.OnMarbleExit+= BottleExit_OnMarbleExit;
            
        }

        private void BottleExit_OnMarbleExit()
        {
            if (LevelManager.Instance.CurrentLevel.MarbleInBottleCount==0 && !_scaleTween.IsActive())
            {
                _scaleTween = _continueButton.transform.parent.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void Finish_OnTargetReached()
        {
            _stars[1].color = Finish.FinishedMarbleCount >= (_target + _totalMarbleCount) / 2 ? Color.yellow : Color.white;
            _stars[2].color = Finish.FinishedMarbleCount == _totalMarbleCount ? Color.yellow : Color.white;

            if(Finish.FinishedMarbleCount == _totalMarbleCount && !_scaleTween.IsActive())
            {
                _scaleTween = _continueButton.transform.parent.DOScale(1.1f,0.3f).SetLoops(-1,LoopType.Yoyo);
            }
        }
        public override void Disable()
        {
            base.Disable();
            _scaleTween.Kill();
            _continueButton.transform.parent.localScale = Vector3.one;


        }
        public override void Enable()
        {
            base.Enable();

            _target = LevelManager.Instance.CurrentLevel.Target;
            _totalMarbleCount = LevelManager.Instance.CurrentLevel.TotalMarbleCount;

            _stars[0].color = Color.yellow;
            _stars[1].color = Color.white;
            _stars[2].color = Color.white;

        }
    }
}