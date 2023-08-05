using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Baruch
{
    public class LevelManager : Singleton<LevelManager>, IInit
    {
        [Save] public static ushort LevelIndex = 0;

        public static event UnityAction OnLevelEnd;
        public static event UnityAction OnLevelBuild;

        [SerializeField] Level[] _levels;

       
        [HideInInspector]public Level CurrentLevel;
        internal bool IsLevelSuccess => CurrentLevel.IsCompleted;

        public static bool Active { get; internal set; }

        public override void Awake()
        {
            
        }

        public void Init()
        {
            Level.OnLevelComplete += OnLevelEnd.Invoke;
            Level.OnLevelFail += OnLevelEnd.Invoke;
        }

       

        public void Build()
        {
            CurrentLevel = Instantiate(_levels[LevelIndex%_levels.Length],transform);            
            CurrentLevel.Configure();
            OnLevelBuild?.Invoke();
            Active = true;
        }
        public void Unload()
        {
            Destroy(CurrentLevel.gameObject);

            Active = false;
        }

        internal static void NextLevel()
        {
            Instance.Unload();
            LevelIndex++;
            DOVirtual.DelayedCall(Time.deltaTime * 1.1f, Instance.Build);//1 FrameDelay
        }

        internal static void RestartLevel()
        {
            Instance.Unload();
            DOVirtual.DelayedCall(Time.deltaTime * 1.1f, Instance.Build);//1 FrameDelay
        }
    }
}