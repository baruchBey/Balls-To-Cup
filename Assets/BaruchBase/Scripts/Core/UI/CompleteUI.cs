using DG.Tweening;
using System;
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
        [SerializeField] TMP_Text[] _starTargetsText;
        int[] _starTargets = new int[3];

        [SerializeField] TMP_Text _levelText;
        [SerializeField] TMP_Text _gainedText;

        int _target;

        int _totalMarbleCount;

        Tween _scaleTween;

        int _currentStar;
        private void Awake()
        {
            foreach (var item in _glows)
                item.DOLocalRotate(Vector3.forward * 360, 10f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);//RotateGlow Objects


            _stars[0].color = Color.yellow;
            _glows[0].localScale = Vector2.one * 3;

        }
        public override void Init()
        {
            _continueButton.onClick.AddListener(Gain);
            _continueButton.onClick.AddListener(LevelManager.NextLevel);
            _restartButton.onClick.AddListener(LevelManager.RestartLevel);
            Level.OnLevelComplete += Level_OnLevelComplete;
            Finish.OnTargetReached += Finish_OnTargetReached;
            BottleExit.OnMarbleExit += BottleExit_OnMarbleExit;

        }

        private void Level_OnLevelComplete()
        {
            _starTargets = new int[3] { _target, (_target + _totalMarbleCount) / 2, _totalMarbleCount };
            for (int i = 0; i < _starTargetsText.Length; i++)
            {
                _starTargetsText[i].text = _starTargets[i].ToString(); 
            }
        }

        private void BottleExit_OnMarbleExit()
        {
            if (LevelManager.Instance.CurrentLevel.MarbleInBottleCount == 0 && !_scaleTween.IsActive())
            {
                _scaleTween = _continueButton.transform.parent.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
                AudioManager.Instance.Play(Audio.AudioItemType.LevelCompleteGlass);

            }
        }

        private void Finish_OnTargetReached()
        {
            bool star1 = Finish.FinishedMarbleCount >= _starTargets[1];
            bool star2 = Finish.FinishedMarbleCount == _starTargets[2];

            if (star1 && _currentStar == 0)
            {
                _currentStar = 1;

                _stars[1].transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 3);
                _gainedText.text = $"+{CurrencyManager.StarToCoin[1]}<sprite=0>";
            }

            _stars[1].color = star1 ? Color.yellow : Color.white;
            _glows[1].localScale = star1 ? Vector2.one * 3 : Vector2.one;

            if (star2)
            {
                _currentStar = 2;
                _gainedText.text = $"+{CurrencyManager.StarToCoin[2]}<sprite=0>";

                _stars[2].transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 3);
            }

            _stars[2].color = star2 ? Color.yellow : Color.white;
            _glows[2].localScale = star2 ? Vector2.one * 3 : Vector2.one;

            if (star2 && !_scaleTween.IsActive())
            {
                _scaleTween = _continueButton.transform.parent.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);
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


            _currentStar = 0;

            _gainedText.text = $"+{CurrencyManager.StarToCoin[0]}<sprite=0>";
            _stars[1].color = Color.white;
            _stars[2].color = Color.white;

            _glows[1].localScale = Vector2.one;
            _glows[2].localScale = Vector2.one;



        }
        private void Gain()
        {

            ParticleManager.Instance.PlayGained(CurrencyManager.StarToCoin[_currentStar]);
        }

    }
}