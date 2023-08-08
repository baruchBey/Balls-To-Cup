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

        public int Target => _target;
        [SerializeField] int _target = 20;

        [SerializeField] Transform _marbleParent;
        [Header("Level")]
        [SerializeField] Finish _finish;
        [SerializeField,Range(0.3f,3f)] float _marbleSize = 1f;

        public int MarbleInBottleCount => _marbleParent.childCount;


        public bool IsCompleted => _levelStatus.HasFlag(LevelStatus.TargetReached);
#if UNITY_EDITOR
        [Header("EDITOR ONLY")]
        [SerializeField] Vector2[] _marblePositions;

#endif
        public int TotalMarbleCount => _totalMarbleCount;
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
            OnMarbleWasted += CheckLevelStatus;

        }
        public void Create(int target, float marbleRadius)
        {
            _marbleSize = marbleRadius;
            _target = target;
            _marbleParent = transform.FindDeepChild("Marbles");
            _finish = transform.FindDeepChild("Finish").GetComponent<Finish>();
            Tube = transform.FindDeepChild("BottleExit").parent;
            FreeMarbleParent = transform.FindDeepChild("FreeMarbleParent");
            OnValidate();
        }

        private void OnDisable()
        {
            OnTargetReached -= CheckLevelStatus;

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

            OnLevelComplete?.Invoke();
        }

        private void LevelFail()
        {
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
            if (!_marbleParent)
                return;

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
        int _inBottle => ((Level)target).MarbleInBottleCount;

        private void OnEnable()
        {
            _total = _inBottle;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("SetMarblePositions"))
            {
                Physics2D.simulationMode = SimulationMode2D.Script;
                for (int i = 0; i < 1200; i++)
                {
                    Physics2D.Simulate(1 / 240f);

                }
                Physics2D.simulationMode = SimulationMode2D.Update;
            }
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