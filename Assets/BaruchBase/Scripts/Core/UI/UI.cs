using UnityEngine;
using Baruch.Core;

namespace Baruch.UI
{
    public class UI : Singleton<UI>, IInit
    {


        [Header("Sub UIs")]

        [SerializeField] IdleUI _idleUI;
        [SerializeField] GameUI _gameUI;
        [SerializeField] CompleteUI _completeUI;
        [SerializeField] FailUI _failUI;


        public void Init()
        {
            Game.OnStateChange += Game_OnStateChange;
        }

        private void Game_OnStateChange(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Idle:
                    _idleUI.SetActive(true);

                    _gameUI.SetActive(false);
                    _completeUI.SetActive(false);
                    _failUI.SetActive(false);
                    break;
                case GameState.Play:
                    _gameUI.SetActive(true);

                    _completeUI.SetActive(false);
                    _idleUI.SetActive(false);
                    _failUI.SetActive(false);
                    break;
                case GameState.LevelFailed:
                    _failUI.SetActive(true);

                    _completeUI.SetActive(false);
                    _gameUI.SetActive(false);
                    _idleUI.SetActive(false);
                    break;
                case GameState.LevelCompleted:
                    _completeUI.SetActive(true);

                    _failUI.SetActive(false);
                    _idleUI.SetActive(false);
                    _gameUI.SetActive(false);

                    break;
                default:
                    break;
            }
        }
    }
}