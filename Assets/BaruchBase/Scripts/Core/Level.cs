using Baruch.Extension;
using System;
using TMPro;
using UnityEngine;
using static Baruch.Finish;
using static Baruch.BottleExit;
using static Baruch.Ground;


#if UNITY_EDITOR
using UnityEditor;
#endif


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
        [SerializeField,Range(1f,3f)] float _marbleSize = 1f;

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
        public static float MarbleSize;
        public void Configure()
        {
            _finish.SetTarget(_target);
            MarbleSize = _marbleSize;
            OnTargetReached += CheckLevelStatus;
            //OnMarbleExit += CheckLevelStatus;
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
            //OnMarbleExit -= CheckLevelStatus;
            OnMarbleExit -= UpdateText;

            OnMarbleWasted -= CheckLevelStatus;
        }
        private void CheckLevelStatus()
        {
            _levelStatus = (FreeMarbleCount != _totalMarbleCount) ? LevelStatus.HasMarbleInBottle : LevelStatus.NoMarbleInBottle;

            _levelStatus |= (FinishedMarbleCount >= _target) ? LevelStatus.TargetReached : 0;
            _levelStatus |= (_totalMarbleCount - WastedMarbleCount < _target) ? LevelStatus.TargetFailed : 0;

            if (_levelStatus.HasFlag(LevelStatus.TargetFailed))
            {
                LevelFail();
                return;
            }
            if (_levelStatus.HasFlag(LevelStatus.TargetReached))
            {
                LevelComplete();
            }

        }

        private void LevelComplete()
        {

#if UNITY_EDITOR
            Debug.Log("LEVEL COMPLETE");
#endif
            OnLevelComplete?.Invoke();
        }

        private void LevelFail()
        {
#if UNITY_EDITOR
            Debug.Log("LEVEL FAIL");
#endif

            OnLevelFail?.Invoke();
        }

        private void OnDrawGizmos()
        {
            _marbleParent.ForEach(x => Gizmos.DrawSphere(x.position, x.GetComponent<CircleCollider2D>().radius));
        }
        internal Transform[] GetMarbleTransforms()
        {
            var marbles = _marbleParent.GetChilds();
            _totalMarbleCount = marbles.Length;
            SetColorIDs(marbles);

            return marbles;
        }


        private static void SetColorIDs(Transform[] marbles)
        {
            marbles.ForEach(marble =>
            {
                marble.GetComponent<Marble>().ID = (byte)(marble.GetSiblingIndex() % ColorManager.ColorCount);
            });

        }


        private void OnValidate()
        {
            _target = Mathf.Min(_target, _marbleParent.childCount);

            _marbleParent.GetComponentsInChildren<CircleCollider2D>().ForEach(cl => cl.radius = _marbleSize);
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

        private void Update()
        {
            EditorUtility.SetDirty(this);

        }
#endif

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        int _total;
        int _inBottle => _marbleParent.childCount;
        Transform _marbleParent;
        private void OnEnable()
        {
            _marbleParent = ((Level)target).transform.FindDeepChild("Marbles");
            _total = _inBottle;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"Total: {_total}");
            EditorGUILayout.LabelField($"InBottle: {_inBottle}");
            EditorGUILayout.LabelField($"Free: {FreeMarbleCount}");
            EditorGUILayout.LabelField($"Wasted: {WastedMarbleCount}");
            EditorGUILayout.LabelField($"Finished: {FinishedMarbleCount}");
            EditorGUILayout.EndVertical();
        }
        
    }
#endif
    
}