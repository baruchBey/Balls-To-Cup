using Baruch.Extension;
using System;
using TMPro;
using UnityEngine;
using static Baruch.Finish;
using static Baruch.BottleExit;
using static Baruch.Ground;

namespace Baruch
{
    public class Level : MonoBehaviour, IConfigure
    {
        public Transform Tube;
        public Transform FreeMarbleParent;

        [SerializeField] int _target = 20;
        [SerializeField] Transform _marbleParent;
        [Header("Level")]
        [SerializeField] Finish _finish;
        [SerializeField] TextMeshPro _marbleCountText;

        int _marbleInBottleCount => _marbleParent.childCount;

        public bool IsLevelSuccess => _levelStatus == LevelStatus.Success;

#if UNITY_EDITOR
        [Header("EDITOR ONLY")]
        [SerializeField] Vector2[] _marblePositions;

#endif

        int _totalMarbleCount;

        public static event Action OnLevelComplete;
        public static event Action OnLevelFail;
        LevelStatus _levelStatus;

        public void Configure()
        {
            _finish.SetTarget(_target);

            OnTargetReached += CheckLevelStatus;
            OnMarbleExit += CheckLevelStatus;
            OnMarbleExit += UpdateText;
            OnMarbleWasted += CheckLevelStatus;

            _marbleCountText.text = _marbleInBottleCount.ToString();
        }

        private void UpdateText()
        {
            _marbleCountText.text = _marbleInBottleCount.ToString();
        }

        private void OnDisable()
        {
            OnTargetReached -= CheckLevelStatus;
            OnMarbleExit -= CheckLevelStatus;
            OnMarbleWasted -= CheckLevelStatus;
        }
        private void CheckLevelStatus()
        {
            _levelStatus = (FreeMarbleCount == _totalMarbleCount) ? LevelStatus.HasMarbleInBottle : LevelStatus.NoMarbleInBottle;

            _levelStatus |= (FinishedMarbleCount == _target) ? LevelStatus.TargetReached : 0;
            _levelStatus |= (_marbleInBottleCount < _target-FinishedMarbleCount) ? LevelStatus.TargetFailed : 0;

            if (_levelStatus.HasFlag(LevelStatus.TargetFailed))
            {
                LevelFail();
                return;
            }
            if (_levelStatus == LevelStatus.Success)
            {
                LevelComplete();
            }

        }

        private void LevelComplete()
        {
            OnLevelComplete?.Invoke();
        }

        private void LevelFail()
        {
            OnLevelFail?.Invoke();
        }

        private void OnDrawGizmos()
        {
            _marbleParent.ForEach(x => Gizmos.DrawSphere(x.position, x.localScale.x));
        }
        internal Transform[] GetMarbleTransforms()
        {
            var marbles = _marbleParent.GetChilds();
            SetColorIDs(marbles);

            return marbles;
        }
        

        private static void SetColorIDs(Transform[] marbles)
        {
            marbles.ForEach(marble => { marble.GetComponent<Marble>().ID = (byte)(marble.GetSiblingIndex() % ColorManager.ColorCount); });

        }
       

        private void OnValidate()
        {
            _target = Mathf.Min(_target, _marbleParent.childCount);
        }
#if UNITY_EDITOR
        [ContextMenu("CapturePositions")]
        void CaptureMarblePositions()
        {
            _marblePositions = new Vector2[_marbleParent.childCount];
            for (int i = 0; i < _marbleParent.childCount; i++)
            {
                _marblePositions[i] = _marbleParent.GetChild(i).position;
            }
        }
        [ContextMenu("Set")]
        void SetPositions()
        {
            for (int i = 0; i < _marbleParent.childCount; i++)
            {
                _marbleParent.GetChild(i).position = _marblePositions[i];
            }
        }

       
#endif

    }
}