using Baruch.Extension;
using System;
using UnityEngine;
using static Baruch.Util.BaruchUtil;

namespace Baruch.Core
{
    public static class Game
    {
        public static bool IsInitialized;

        private static GameState _gameState;
        public static GameState GameState { get => _gameState; set { _gameState = value; OnStateChange?.Invoke(value); } }
        public static event Action<GameState> OnStateChange;

        public static void Initialize()
        {
            FrameWork.OnFirstClick += FrameWork_OnFirstClick;
            LevelManager.OnLevelBuild += LevelManager_OnLevelBuild;
            LevelManager.OnLevelEnd += LevelManager_OnLevelEnd;


            IsInitialized = false;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 120;

            Save.Load();

            FindInterfacesOfType<IInit>(includeInActive: true).ForEach(iinit => iinit.Init());


            LevelManager.Instance.Build();

            IsInitialized = true;

        }

        private static void FrameWork_OnFirstClick()
        {
            GameState = GameState.Play;
        }

        private static void LevelManager_OnLevelEnd()
        {
            GameState = LevelManager.Instance.IsLevelSuccess ? GameState.LevelCompleted : GameState.LevelFailed;

        }

        private static void LevelManager_OnLevelBuild()
        {
            Save.SaveGame();

            GameState = GameState.Idle;
        }

        internal static void Pause()
        {
            GameState = GameState.Pause;
            Time.timeScale = 0;
        }

        internal static void Resume()
        {
            GameState = GameState.Idle;
            Time.timeScale = 1;
        }
    }
}
