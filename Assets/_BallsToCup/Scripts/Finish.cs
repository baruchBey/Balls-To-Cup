using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_finishedMarbles.Add(collider.GetHashCode()))
            {

                var marble = collider.GetComponent<Marble>();
                marble.Finished();
                

                var hex = ColorManager.Instance.GetHex(marble.ID);
                _marbleCountText.text = $"<color=#{hex}>{_finishedMarbles.Count}</color>/{_targetCount}";
                if (!DOTween.IsTweening(_marbleCountText.transform))
                    _marbleCountText.transform.DOPunchScale(Vector2.one * 0.13f, 0.2f, 3);
            }

            if (_finishedMarbles.Count >= _targetCount)
                OnTargetReached.Invoke();


        }

        internal void SetTarget(int target)
        {
            _targetCount = target;
            _marbleCountText.text = $"0/{_targetCount}";

        }
    }
}
