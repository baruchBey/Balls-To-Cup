using Baruch.Core;
using Baruch.Extension;
using DG.Tweening;
using UnityEngine;

namespace Baruch
{
    [DisallowMultipleComponent]
    public class TutorialManager : Singleton<TutorialManager>, IInit
    {
        [Save] public static byte TutorialStage;

        [SerializeField] GameObject _tutorial;
        [SerializeField] Transform _hand;

        public void Init()
        {
            Game.OnStateChange += Game_OnStateChange;
            DOVirtual.Float(0f, 1f, 2f, SetHandPosition).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        }

        private void Game_OnStateChange(GameState obj)
        {
            if (obj == GameState.Idle)
            {
                Invoke(nameof(Play), 1f);
            }
            else if (obj == GameState.Play)
            {
                Stop();
            }
        }


        private void Play()
        {
            _tutorial.SetActive(true);
        }
        private void Stop()
        {
            CancelInvoke();
            _tutorial.SetActive(false);
        }
        private void SetHandPosition(float x)
        {
            _hand.position = Vector3.Slerp(Vector3.up + Vector3.right * 0.01f, Vector3.down, x.Remap(0f, 1f, 0.9f, 0.5f)) * 30f;

        }
    }
}