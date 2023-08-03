using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Baruch.Screenshot
{
    internal static class ReflectionExtensions
    {
        internal static object FetchField(this Type type, string field)
        {
            return type.GetFieldRecursive(field, true).GetValue(null);
        }

        internal static object FetchField(this object obj, string field)
        {
            return obj.GetType().GetFieldRecursive(field, false).GetValue(obj);
        }

        internal static object FetchProperty(this Type type, string property)
        {
            return type.GetPropertyRecursive(property, true).GetValue(null, null);
        }

        internal static object FetchProperty(this object obj, string property)
        {
            return obj.GetType().GetPropertyRecursive(property, false).GetValue(obj, null);
        }

        internal static object CallMethod(this Type type, string method, params object[] parameters)
        {
            return type.GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, parameters);
        }

        internal static object CallMethod(this object obj, string method, params object[] parameters)
        {
            return obj.GetType().GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, parameters);
        }

        internal static object CreateInstance(this Type type, params object[] parameters)
        {
            Type[] parameterTypes;
            if (parameters == null)
                parameterTypes = null;
            else
            {
                parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    parameterTypes[i] = parameters[i].GetType();
            }

            return CreateInstance(type, parameterTypes, parameters);
        }

        internal static object CreateInstance(this Type type, Type[] parameterTypes, object[] parameters)
        {
            return type.GetConstructor(parameterTypes).Invoke(parameters);
        }

        private static FieldInfo GetFieldRecursive(this Type type, string field, bool isStatic)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | (isStatic ? BindingFlags.Static : BindingFlags.Instance);
            do
            {
                FieldInfo fieldInfo = type.GetField(field, flags);
                if (fieldInfo != null)
                    return fieldInfo;

                type = type.BaseType;
            } while (type != null);

            return null;
        }

        private static PropertyInfo GetPropertyRecursive(this Type type, string property, bool isStatic)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | (isStatic ? BindingFlags.Static : BindingFlags.Instance);
            do
            {
                PropertyInfo propertyInfo = type.GetProperty(property, flags);
                if (propertyInfo != null)
                    return propertyInfo;

                type = type.BaseType;
            } while (type != null);

            return null;
        }
    }

    public class TakeScreenShotWindow : EditorWindow
    {
        private bool playModeStateChangedHandler;

        private class CustomResolution
        {
            public readonly int width, height;
            private int originalIndex, newIndex;

            private bool m_isActive;
            public bool IsActive
            {
                get { return m_isActive; }
                set
                {
                    if (m_isActive != value)
                    {
                        m_isActive = value;

                        int resolutionIndex;
                        if (m_isActive)
                        {
                            originalIndex = (int)GameView.FetchProperty("selectedSizeIndex");

                            object customSize = GetFixedResolution(width, height);
                            SizeHolder.CallMethod("AddCustomSize", customSize);
                            newIndex = (int)SizeHolder.CallMethod("IndexOf", customSize) + (int)SizeHolder.CallMethod("GetBuiltinCount");
                            resolutionIndex = newIndex;
                        }
                        else
                        {
                            SizeHolder.CallMethod("RemoveCustomSize", newIndex);
                            resolutionIndex = originalIndex;
                        }

                        GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);
                        GameView.Repaint();
                    }
                }
            }

            public CustomResolution(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }
        private const string TEMPORARY_RESOLUTION_LABEL = "MSC_temp";
        private readonly GUILayoutOption GL_WIDTH_25 = GUILayout.Width(25f);
        private readonly GUILayoutOption GL_EXPAND_WIDTH = GUILayout.ExpandWidth(true);

        private static object SizeHolder { get { return GetType("GameViewSizes").FetchProperty("instance").FetchProperty("currentGroup"); } }
        private static EditorWindow GameView { get { return GetWindow(GetType("GameView")); } }
        //private static EditorWindow GameView { get { return (EditorWindow) GetType( "GameView" ).CallMethod( "GetMainGameView" ); } }

        readonly static private Vector2Int[] resolutions = new Vector2Int[]{
            new Vector2Int(1242, 2688),
            new Vector2Int(1242,2208),
            new Vector2Int(2048, 2732),
            new Vector2Int(1080, 1920) };

        static string targetPath;
        static private Dictionary<Vector2Int, string> _dictionary = new Dictionary<Vector2Int, string>();


        private float prevTimeScale;

        private Vector2 scrollPos;

        private readonly List<CustomResolution> queuedScreenshots = new List<CustomResolution>();

        [MenuItem("Tools/Take ScreenShot")]
        private static void Init()
        {
            TakeScreenShotWindow window = GetWindow<TakeScreenShotWindow>();
            window.titleContent = new GUIContent("Screenshot");
            window.minSize = new Vector2(325f, 150f);
            window.Show();



        }
        private void OnEnable()
        {
            targetPath = Directory.GetParent(Application.dataPath).FullName;
            _dictionary = new Dictionary<Vector2Int, string>(){
            { new Vector2Int(1242, 2688),targetPath + "/Screenshots/iPhone6.5" },
            { new Vector2Int(1242,2208),targetPath + "/Screenshots/iPhone5.5"},
            { new Vector2Int(2048, 2732),targetPath + "/Screenshots/iPad" },
            { new Vector2Int(1080, 1920),targetPath + "/Screenshots/Android" },};

            CreateDirectory();
            EditorApplication.playModeStateChanged += OnPlay;
            playModeStateChangedHandler = true;
        }

       

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlay;
            playModeStateChangedHandler = false;


        }
        private void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= OnPlay;
            playModeStateChangedHandler = false;
        }

        private void OnPlay(PlayModeStateChange obj)
        {
            if (obj.Equals(PlayModeStateChange.EnteredPlayMode))
            {
                EditorApplication.pauseStateChanged += OnPause;
            }
            else if (obj.Equals(PlayModeStateChange.ExitingPlayMode))
            {
                EditorApplication.pauseStateChanged -= OnPause;
            }

        }



        private void OnPause(PauseState obj)
        {
            if (obj.Equals(PauseState.Paused))
            {
                CaptureScreenshots();
            }
        }

        private void ShowScreenShots()
        {
            CreateDirectory();
            ShowExplorer(targetPath + "/Screenshots/iPad");
        }

        private static void CreateDirectory()
        {
            if (Directory.Exists(targetPath + "/Screenshots/"))
                return;

            Directory.CreateDirectory(targetPath + "/Screenshots/");
            Directory.CreateDirectory(targetPath + "/Screenshots/iPhone6.5");
            Directory.CreateDirectory(targetPath + "/Screenshots/iPhone5.5");
            Directory.CreateDirectory(targetPath + "/Screenshots/iPad");
            Directory.CreateDirectory(targetPath + "/Screenshots/Android");
        }

        private void ShowExplorer(string itemPath)
        {
            itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
            System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
        }

        private void OnGUI()
        {
            

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();

            GUILayout.Label("Resolutions:", GL_EXPAND_WIDTH);


            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();


            GUI.enabled = true;
            GUILayout.Box(GUIContent.none, playModeStateChangedHandler ? GetGreenBoxStyle() : GetRedBoxStyle());

            GUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(true);

            for (int i = 0; i < resolutions.Length; i++)
            {
                GUILayout.BeginHorizontal();
                resolutions[i] = EditorGUILayout.Vector2IntField(GUIContent.none, resolutions[i]);
                GUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            if (GUILayout.Button("Show Screen Shots Location"))
            {
                ShowScreenShots();
            }
            GUILayout.Label("Pause Game To Take Screenshots (Ctrl+Shift+P)");
            EditorGUILayout.EndScrollView();
        }

     
        private GUIStyle GetGreenBoxStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.normal.background = MakeTexture(2, 2, Color.green);
            return style;
        }

        private GUIStyle GetRedBoxStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.normal.background = MakeTexture(2, 2, Color.red);
            return style;
        }

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        private void CaptureScreenshots()
        {
            CreateDirectory();

            if (!EditorApplication.isPlaying)
                return;

            for (int i = 0; i < resolutions.Length; i++)
            {
                CaptureScreenshot(resolutions[i]);
            }


            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            EditorApplication.update -= CaptureQueuedScreenshots;
            EditorApplication.update += CaptureQueuedScreenshots;

        }

        private void CaptureScreenshot(Vector2Int resolution)
        {
            int width = resolution.x;
            int height = resolution.y;

            queuedScreenshots.Add(new CustomResolution(width, height));
        }

        private void CaptureQueuedScreenshots()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorApplication.update -= CaptureQueuedScreenshots;
                return;
            }

            if (queuedScreenshots.Count == 0)
            {
                EditorApplication.update -= CaptureQueuedScreenshots;
                return;
            }

            CustomResolution resolution = queuedScreenshots[0];
            if (!resolution.IsActive)
            {
                resolution.IsActive = true;
                EditorApplication.Step();

                return;
            }
            EditorApplication.Step();

            CaptureScreenshotWithUI();

            resolution.IsActive = false;
            queuedScreenshots.RemoveAt(0);

            if (queuedScreenshots.Count == 0)
            {

                EditorApplication.Step();
                Time.timeScale = prevTimeScale;
                Repaint();
                EditorApplication.isPaused = false;

                return;
            }
            CaptureQueuedScreenshots();


        }



        private void CaptureScreenshotWithUI()
        {
            RenderTexture temp = RenderTexture.active;

            RenderTexture renderTex = (RenderTexture)GameView.FetchField("m_TargetTexture");
            Texture2D screenshot = null;

            int width = renderTex.width;
            int height = renderTex.height;

            try
            {
                RenderTexture.active = renderTex;

                screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
                screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);

                if (SystemInfo.graphicsUVStartsAtTop)
                {
                    Color32[] pixels = screenshot.GetPixels32();
                    for (int i = 0; i < height / 2; i++)
                    {
                        int startIndex0 = i * width;
                        int startIndex1 = (height - i - 1) * width;
                        for (int x = 0; x < width; x++)
                        {
                            Color32 color = pixels[startIndex0 + x];
                            pixels[startIndex0 + x] = pixels[startIndex1 + x];
                            pixels[startIndex1 + x] = color;
                        }
                    }

                    screenshot.SetPixels32(pixels);
                }

                screenshot.Apply(false, false);

                File.WriteAllBytes(GetUniqueFilePath(width, height), screenshot.EncodeToPNG());
            }
            finally
            {
                RenderTexture.active = temp;

                if (screenshot != null)
                    DestroyImmediate(screenshot);
            }
        }


        private string GetUniqueFilePath(int width, int height)
        {
            string filename = string.Concat("{0}", ".png");
            int fileIndex = 0;
            string path;
            do
            {
                var key = new Vector2Int(width, height);
                path = Path.Combine(_dictionary[key], string.Format(filename, ++fileIndex));

            } while (File.Exists(path));

            return path;
        }

        private static object GetFixedResolution(int width, int height)
        {
            object sizeType = Enum.Parse(GetType("GameViewSizeType"), "FixedResolution");
            return GetType("GameViewSize").CreateInstance(sizeType, width, height, TEMPORARY_RESOLUTION_LABEL);
        }

        private static Type GetType(string type)
        {
            return typeof(EditorWindow).Assembly.GetType("UnityEditor." + type);
        }
    }
}