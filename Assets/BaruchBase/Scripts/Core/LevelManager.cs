using Baruch.Extension;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static Baruch.Util.BaruchUtil;

namespace Baruch
{
    public class LevelManager : Singleton<LevelManager>, IInit
    {
        [Save] public static ushort LevelIndex = 0;

        public static event UnityAction OnLevelEnd;
        public static event UnityAction OnLevelBuild;

        [SerializeField] Level[] _levels;

        public Level[] Levels => _levels;

       
        [HideInInspector]public Level CurrentLevel;
        internal bool IsLevelSuccess => CurrentLevel.IsCompleted;

        public static bool Active { get; internal set; }

        IEnumerable<IPoolOwner> _poolOwner;
        public override void Awake()
        {
            
        }

        public void Init()
        {
            Level.OnLevelComplete += OnLevelEnd.Invoke;
            Level.OnLevelFail += OnLevelEnd.Invoke;
            _poolOwner = FindInterfacesOfType<IPoolOwner>(includeInActive: true);

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
            _poolOwner.ForEach(poolOwner => poolOwner.ReleaseAll());

            Destroy(CurrentLevel.gameObject);

            Active = false;
        }

        internal static void NextLevel()
        {
            
            Instance.Unload();
            LevelIndex++;

            Instance.DelayedBuild();
        }
        public void DelayedBuild()
        {
            StartCoroutine(DelayedBuildCoroutine());
        }
        public IEnumerator DelayedBuildCoroutine()
        {
            yield return null;
            Build();
        }

        internal static void RestartLevel()
        {
            Instance.Unload();
            Instance.DelayedBuild();

        }
    }
}