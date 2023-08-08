using System;
using System.IO;
using UnityEditor;
using UnityEngine;

using static Baruch.UtilEditor.SVGLevelCreator;
using static UnityEngine.Rendering.DebugUI;

namespace Baruch.UtilEditor
{
    public class SundayWindow : EditorWindow
    {
        
        private void OnProjectChange()
        {
            if (EditorPrefs.GetBool("ShowSunday", true))
            {
                ShowWindow();
                EditorPrefs.SetBool("ShowSunday", false);
            }
        }
     

        public const int WINDOW_WIDTH = 512;

        private static readonly Vector2 _windowSize = new(WINDOW_WIDTH, 640f);

        [MenuItem("Sunday/BallsToCup")]
        public static void ShowWindow()
        {

            var window = GetWindow<SundayWindow>("BallsToCup");
            window.maxSize = _windowSize;
            window.minSize = _windowSize;
        }
        int[] _marbleCounts;
        int[] _targetCounts;
        bool[] _foldout;
        string _folderPath;

        SerializedObject _serializedPlayerController;
        SerializedProperty _rotationSensitivityProperty;
        SerializedProperty _rotationFromHandleOrMiddle;

        SerializedObject _defaultPhysicsObject;
        SerializedProperty _defaultFrictionProperty;
        SerializedProperty _defaultBouncinessProperty;
        SerializedObject _marblePhysicsObject;
        SerializedProperty _marbleFrictionProperty;
        SerializedProperty _marbleBouncinessProperty;

        private string[] _tabLabels;

        private static readonly Vector2 _headerSize = new(_windowSize.x, HEADER_HEIGHT);
        public const float HEADER_HEIGHT = 48f;
    

        public enum BallToCupModuleEnum : byte
        {
            GameProperties,
            LevelCreator,
            ReadMe
        }
        private BallToCupModuleEnum _activeTab = BallToCupModuleEnum.ReadMe;

        public BallToCupModuleEnum ActiveTab
        {
            get => _activeTab;
            private set
            {
                if (_activeTab == value)
                    return;
                _activeTab = value;
                EditorPrefs.SetInt("ActiveTab", (int)value);

            }
        }
        private void OnDisable()
        {
            EditorPrefs.SetInt("ActiveTab", (int)ActiveTab);

        }
        private void OnEnable()
        {
            ActiveTab = (BallToCupModuleEnum)EditorPrefs.GetInt("ActiveTab", 2);

            _tabLabels = Enum.GetNames(typeof(BallToCupModuleEnum));


            _serializedPlayerController = new(FindObjectOfType<PlayerController>());
            _rotationSensitivityProperty = _serializedPlayerController.FindProperty("_rotationSpeed");
            _rotationFromHandleOrMiddle = _serializedPlayerController.FindProperty("_fromHandle");

            _defaultPhysicsObject = new(AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Physics/Default.physicsMaterial2D", typeof(PhysicsMaterial2D)));
            _defaultFrictionProperty = _defaultPhysicsObject.FindProperty("friction");
            _defaultBouncinessProperty = _defaultPhysicsObject.FindProperty("bounciness");

            _marblePhysicsObject = new(AssetDatabase.LoadAssetAtPath("Assets/_BallsToCup/Physics/Marble.physicsMaterial2D", typeof(PhysicsMaterial2D)));
            _marbleFrictionProperty = _marblePhysicsObject.FindProperty("friction");
            _marbleBouncinessProperty = _marblePhysicsObject.FindProperty("bounciness");

            _folderPath = Application.dataPath + "/_BallsToCup/SVGs";

            var vectorCount = Directory.GetFiles(_folderPath, "*.svg").Length;
            _foldout = new bool[vectorCount];
            _marbleCounts = new int[vectorCount];
            _targetCounts = new int[vectorCount];

            // Load the data from EditorPrefs and convert back to arrays
            string foldoutJson = EditorPrefs.GetString("FoldoutData", "");
            string marbleCountsJson = EditorPrefs.GetString("MarbleCountsData", "");
            string targetCountsJson = EditorPrefs.GetString("TargetCountsData", "");

            if (!string.IsNullOrEmpty(foldoutJson))
            {
                _foldout = JsonUtility.FromJson<Wrapper<bool>>(foldoutJson).Data;
            }

            if (!string.IsNullOrEmpty(marbleCountsJson))
            {
                _marbleCounts = JsonUtility.FromJson<Wrapper<int>>(marbleCountsJson).Data;
            }

            if (!string.IsNullOrEmpty(targetCountsJson))
            {
                _targetCounts = JsonUtility.FromJson<Wrapper<int>>(targetCountsJson).Data;
            }

            SaveArraysToEditorPrefs();

        }

        private void OnGUI()
        {
            EditorPrefs.DeleteAll();


            var rect = EditorGUILayout.BeginVertical();
            rect.height = _headerSize.y;
            rect.width = _headerSize.x;

            ActiveTab = (BallToCupModuleEnum)GUI.Toolbar(rect, (int)ActiveTab, _tabLabels);
            
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(HEADER_HEIGHT);

            switch (ActiveTab)
            {
                case BallToCupModuleEnum.GameProperties:
                    GameProperties();
                    break;
                case BallToCupModuleEnum.LevelCreator:
                    LevelCreator();
                    break;
                case BallToCupModuleEnum.ReadMe:
                    ReadMe();
                    break;
            }


        }

        private void ReadMe()
        {
            EditorGUILayout.LabelField("Lorem Ipsum");

        }

        private void LevelCreator()
        {
            EditorGUILayout.LabelField("SVG Reader");
            string[] filePaths = Directory.GetFiles(_folderPath, "*.svg");
            GUI.enabled = true;
            if (GUILayout.Button("Bulk Create", GUILayout.Height(100)))
            {
                GUI.enabled = false;
                for (int i = 0; i < filePaths.Length; i++)
                {
                    string filePath = filePaths[i];
                    CreateLevel(filePath, filePath[^5] - '0', _marbleCounts[i], _targetCounts[i]);
                }


            }
            var tempFoldout = _foldout;
            _foldout = new bool[filePaths.Length];
            Array.Copy(tempFoldout,_foldout,Mathf.Min(tempFoldout.Length, _foldout.Length));

            var tempMarble = _marbleCounts;
            _marbleCounts = new int[filePaths.Length];
            Array.Copy(tempMarble, _marbleCounts, Mathf.Min(tempMarble.Length, _marbleCounts.Length));

            var tempTarget = _targetCounts;
            _targetCounts = new int[filePaths.Length];
            Array.Copy(tempTarget, _targetCounts, Mathf.Min(tempTarget.Length, _targetCounts.Length));


            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];
                var name = filePath.Split('\\')[^1];
                _foldout[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_foldout[i], name);
                if (!_foldout[i])
                {
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    EditorGUI.EndChangeCheck();
                    continue;
                }
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Level Parameters");
                _marbleCounts[i] = EditorGUILayout.IntField("Marble Count:", _marbleCounts[i]);
                _targetCounts[i] = EditorGUILayout.IntField("Target Count:", _targetCounts[i]);

                EditorGUILayout.EndVertical();

                if (GUILayout.Button($"Create {name}", GUILayout.Height(100)))
                {
                    GUI.enabled = false;
                    CreateLevel(filePath, filePath[^5] - '0', _marbleCounts[i], _targetCounts[i]);

                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            if (EditorGUI.EndChangeCheck())
                SaveArraysToEditorPrefs();
        }

        private void GameProperties()
        {
            EditorGUILayout.LabelField("Game Properties");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_rotationSensitivityProperty, new GUIContent("Control Sensitivity"));
            EditorGUILayout.PropertyField(_rotationFromHandleOrMiddle, new GUIContent("From Handle(Or Middle)","Default Is True"));
            _serializedPlayerController.ApplyModifiedProperties();



            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(_defaultFrictionProperty, new GUIContent("Default Friction"));
            EditorGUILayout.PropertyField(_defaultBouncinessProperty, new GUIContent("Default Bounciness"));
            _defaultPhysicsObject.ApplyModifiedProperties();


            EditorGUILayout.Space();


            EditorGUILayout.PropertyField(_marbleFrictionProperty, new GUIContent("Marble Friction"));
            EditorGUILayout.PropertyField(_marbleBouncinessProperty, new GUIContent("Marble Bounciness"));
            _marblePhysicsObject.ApplyModifiedProperties();
            EditorGUILayout.Space();

            Physics2D.gravity = Vector2.down * EditorGUILayout.FloatField(new GUIContent("Gravity Power","12 for more snappy gravity Default is 9.81"), -Physics2D.gravity.y);
        }

        private void SaveArraysToEditorPrefs()
        {
            string foldoutJson = JsonUtility.ToJson(new Wrapper<bool> { Data = _foldout });
            string marbleCountsJson = JsonUtility.ToJson(new Wrapper<int> { Data = _marbleCounts });
            string targetCountsJson = JsonUtility.ToJson(new Wrapper<int> { Data = _targetCounts });

            EditorPrefs.SetString("FoldoutData", foldoutJson);
            EditorPrefs.SetString("MarbleCountsData", marbleCountsJson);
            EditorPrefs.SetString("TargetCountsData", targetCountsJson);
        }

        [System.Serializable]
        public class Wrapper<T>
        {
            public T[] Data;
        }



    }
}
