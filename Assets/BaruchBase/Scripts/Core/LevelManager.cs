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
        internal bool IsLevelSuccess => CurrentLevel.IsLevelSuccess;

        public static bool Active { get; internal set; }

        public override void Awake()
        {
            
        }

        public void Init()
        {
            
        }

       

        public void Build()
        {
            CurrentLevel = Instantiate(_levels[LevelIndex],transform);            
            CurrentLevel.Configure();
            OnLevelBuild?.Invoke();
            Active = true;
        }
        public void Unload()
        {
            Active = false;
            Destroy(CurrentLevel);
        }

        internal void NextLevel()
        {
            throw new NotImplementedException();
        }

        internal void RestartLevel()
        {
            throw new NotImplementedException();
        }
    }
}