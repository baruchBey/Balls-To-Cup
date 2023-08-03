using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using Baruch.Extension;
using System.Reflection;

namespace Baruch.UtilEditor
{
    public class SaveDataViewerWindow : EditorWindow
    {
        private SaveData[] _saveDataArray;

        [MenuItem("Baruch/Save Data Viewer")]
        public static void OpenWindow()
        {
            SaveDataViewerWindow window = GetWindow<SaveDataViewerWindow>();
            window.titleContent = new GUIContent("Save Data Viewer");
            window.Show();
        }
        private void OnEnable()
        {
            _saveDataArray = SaveAttributeFinder.FindSaveFields();


        }
        
        private Texture2D CreateBackgroundTexture(Color color)
        {
            // Create a 1x1 pixel texture with the specified color
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            return texture;
        }
        private void OnGUI()
        {
            GUIStyle customNormalTextStyle = new(EditorStyles.label);
            customNormalTextStyle.normal.background = CreateBackgroundTexture(Color.white * 32f / 255f);

            var customHeaderTextStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12
            };
            customHeaderTextStyle.normal.background = CreateBackgroundTexture(Color.white * 54f / 255f);

            EditorGUILayout.BeginVertical();
            string _classNameCached = string.Empty;
            int i = 0;
            foreach (var saveData in _saveDataArray)
            {
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();

                var className = saveData.FieldInfo.DeclaringType.ToString().Split('.')[^1];
                if (!_classNameCached.Equals(className))
                {
                    _classNameCached = className;
                    if (GUILayout.Button(className, customHeaderTextStyle, GUILayout.Width(120)))
                    {
                        OpenScript(saveData.FieldInfo.DeclaringType, saveData.FieldName);
                    }
                }
                if (i == 0)
                {
                    EditorGUILayout.LabelField("Default Values", customHeaderTextStyle);
                    i++;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(saveData.FieldName.ToPascalCase(), customNormalTextStyle, GUILayout.Width(120)))
                {
                    OpenScript(saveData.FieldInfo.DeclaringType, saveData.FieldName);
                }

                if (saveData.Value is Array array)
                {
                    string arrayString = $"[{string.Join(", ", array.Cast<object>().Select(x => x.ToString()))}]";
                    if (GUILayout.Button(arrayString, customNormalTextStyle))
                    {
                        OpenScript(saveData.FieldInfo.DeclaringType, saveData.FieldName);
                    }
                }
                else
                {
                    if (GUILayout.Button(saveData.Value.ToString(), customNormalTextStyle))
                    {
                        OpenScript(saveData.FieldInfo.DeclaringType, saveData.FieldName);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

        }

        private void OpenScript(Type scriptType, string fieldName)
        {
            if (scriptType != null)
            {
                var assets = UnityEditor.AssetDatabase.FindAssets("t:Script");
                foreach (var guid in assets)
                {
                    string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    var script = UnityEditor.AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                    if (script.GetClass() == scriptType)
                    {
                        var lineNumber = 1 + script.text.Split('\n').Select((line, index) => (line, index)).First(x => x.line.Contains("[Save]") && x.line.Contains(fieldName)).index;

                        UnityEditor.AssetDatabase.OpenAsset(script, lineNumber);
                        break;
                    }
                }
            }
        }
    }
}