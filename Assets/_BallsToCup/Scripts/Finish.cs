using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Baruch
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] TextMeshPro _marbleCountText;


        static readonly HashSet<int> _finishedMarbles = new();
        public static int FinishedMarbleCount => _finishedMarbles.Count;
        int _targetCount;

        public static event Action OnTargetReached;
        private void Start()
        {
            _finishedMarbles.Clear();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {            
            _finishedMarbles.Add(collision.gameObject.GetHashCode());
            _marbleCountText.text = $"{_finishedMarbles.Count}/{_targetCount}";

            if(_finishedMarbles.Count == _targetCount)
                OnTargetReached.Invoke();

            
        }

        internal void SetTarget(int target)
        {
            _targetCount = target;
        }
    }
}
