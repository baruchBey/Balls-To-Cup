using System.IO;
using UnityEditor;
using UnityEngine;

using static Baruch.UtilEditor.SVGLevelCreator;
using static Baruch.UtilEditor.SVGUtility;

namespace Baruch.UtilEditor
{
    public class SVGReaderWindow : EditorWindow
    {
        public const int WINDOW_WIDTH = 512;

        private static readonly Vector2 _windowSize = new(WINDOW_WIDTH, 640f);

        [MenuItem("Sunday/SVGReader")]
        public static void ShowWindow()
        {
            var window = GetWindow<SVGReaderWindow>("SVGReader");
            window.maxSize = _windowSize;
            window.minSize = _windowSize;
        }
        int[] _marbleCounts;
        int[] _targetCounts;
        bool[] _foldout;
        string _folderPath;
        private void OnGUI()
        {

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

        private void OnEnable()
        {
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
