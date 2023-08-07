using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Baruch.UI
{
    public class CompleteUI : SubUI
    {
        [SerializeField] Button _continueButton;
        [SerializeField] Button _restartButton;

        [SerializeField] Image[] _stars;
        [SerializeField] Transform[] _glows;
        [SerializeField] TMP_Text _levelText;

        int _target;
        int _totalMarbleCount;

        Tween _scaleTween;

        private void Awake()
        {
            foreach (var item in _glows)
                item.DOLocalRotate(Vector3.forward * 360, 10f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);//RotateGlow Objects


            _stars[0].color = Color.yellow;
            _glows[0].localScale = Vector2.one * 3;

        }
        public override void Init()
        {
            _continueButton.onClick.AddListener(LevelManager.NextLevel);
            _restartButton.onClick.AddListener(LevelManager.RestartLevel);
            Finish.OnTargetReached += Finish_OnTargetReached;
            BottleExit.OnMarbleExit += BottleExit_OnMarbleExit;

        }

        private void BottleExit_OnMarbleExit()
        {
            if (LevelManager.Instance.CurrentLevel.MarbleInBottleCount == 0 && !_scaleTween.IsActive())
            {
                _scaleTween = _continueButton.transform.parent.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void Finish_OnTargetReached()
        {
            var star2 = Finish.FinishedMarbleCount >= (_target + _totalMarbleCount) / 2;
            var star3 = Finish.FinishedMarbleCount == _totalMarbleCount;

            if (star2 && _stars[1].color == Color.white)
            {
                AudioManager.Instance.Play(Audio.AudioItemType.StarPop);
                _stars[1].transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 3);
            }

            _stars[1].color = star2 ? Color.yellow : Color.white;
            _glows[1].localScale = star2 ? Vector2.one * 3 : Vector2.one;


            if (star3)
            {
                AudioManager.Instance.Play(Audio.AudioItemType.StarPop);
                _stars[2].transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 3);
            }

            _stars[2].color = star3 ? Color.yellow : Color.white;
            _glows[2].localScale = star3 ? Vector2.one * 3 : Vector2.one;



            if (Finish.FinishedMarbleCount == _totalMarbleCount && !_scaleTween.IsActive())
            {
                _scaleTween = _continueButton.transform.parent.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);

                AudioManager.Instance.Play(Audio.AudioItemType.LevelCompleteGlass);

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
            _levelText.text = $"Level {LevelManager.LevelIndex + 1}\r\nComplete";
            base.Enable();

            _target = LevelManager.Instance.CurrentLevel.Target;
            _totalMarbleCount = LevelManager.Instance.CurrentLevel.TotalMarbleCount;



            _stars[1].color = Color.white;
            _stars[2].color = Color.white;

            _glows[1].localScale = Vector2.one;
            _glows[2].localScale = Vector2.one;



        }

    }
}