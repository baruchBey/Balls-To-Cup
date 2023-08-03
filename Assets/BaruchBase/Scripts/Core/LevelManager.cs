using System;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Baruch
{
    public class LevelManager : Singleton<LevelManager>, IInit
    {
        [Save] public static ushort LevelIndex = 0;

        public static event UnityAction OnLevelEnd;
        public static event UnityAction OnLevelBuild;

        [SerializeField] Level[] _levels;

       
        [HideInInspector]public Level CurrentLevel;

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

       
    }
}