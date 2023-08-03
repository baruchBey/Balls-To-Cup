using System.IO;
using UnityEditor;
using UnityEngine;
using Baruch.Util;
using UnityEditor.Build.Content;
using UnityEngine.SceneManagement;


namespace Baruch.UtilEditor
{
    public class UtilitiesWindow : EditorWindow
    {
        #region Window Properties

        private const string windowTitle = "Utility Panel";
        private static readonly Vector2 windowSize = new Vector2(420f, 540f);
        private static readonly Vector2 headerSize = new Vector2(windowSize.x, 48f);
        private readonly string[] tabLabels = {"Utilities", "Video" };
        private int activeTabIndex = -1;

        public int ActiveTabIndex
        {
            get => activeTabIndex;
            private set
            {
                if (activeTabIndex == value) return;
                activeTabIndex = value;
                GUI.FocusControl(null);
                if (activeTabIndex == 0) OnUtilitiesTabOpened();
                if (activeTabIndex == 1) OnVideoTabOpened();
            }
        }

        #endregion

  


        #region Utilities Tab

        private string gameName = default;
        private bool showEmptyNameWarning = default;
        bool _advancedFoldout;
        #endregion

        public static void Open()
        {
            EditorWindow window = GetWindow<UtilitiesWindow>(false, windowTitle, true);
            window.maxSize = windowSize;
            window.ShowUtility();
        }

        private void OnEnable()
        {
            ActiveTabIndex = 0;
        }

        private void OnGUI()
        {
            this.ActiveTabIndex = GUI.Toolbar(new Rect(0.0f, 0.0f, headerSize.x, 30f), ActiveTabIndex, tabLabels);
            GUILayout.Space(headerSize.y - 5f);
            if (activeTabIndex == 0) DrawUtilitiesTab();
            if (activeTabIndex == 1) DrawVideoTab();
        }


        private void DrawAnalyticsTab()
        {

            GUILayout.Space(5f);

            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            GUILayout.EndHorizontal();

            GUILayout.Space(10f);



            // Elephant Section
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Elephant", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            GUILayout.EndHorizontal();



            GUILayout.Space(10f);

            // Facebook Section
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Facebook", EditorStyles.boldLabel);
            GUILayout.Space(5f);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(15f);

            GUILayout.EndHorizontal();

            // Setup Section
            GUILayout.Space(10f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);

            GUI.enabled = SceneManager.sceneCountInBuildSettings != 3;
          
            GUILayout.Space(10f);
            GUILayout.EndHorizontal();


        }



        private void OnUtilitiesTabOpened()
        {
        }


        private void OnVideoTabOpened()
        {
        }

        private void DrawVideoTab()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(10f);
            if (GUILayout.Button("Create Hand Canvas"))
            {
                VideoUtilities.CursorCanvas();
            }
            GUILayout.Space(10f);
            if (GUILayout.Button("Create Joystick"))
            {
                VideoUtilities.JoystickCanvas();
            }
            GUILayout.EndVertical();

        }
        private void DrawUtilitiesTab()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10f);

            GUILayout.BeginVertical();

            EditorGUILayout.LabelField("Save & Load", EditorStyles.boldLabel);
            GUILayout.Space(2f);
            if (GUILayout.Button("Delete Save"))
            {
                Save.DeleteSave();
            }
            GUILayout.Space(10f);

            GUI.enabled = true;
            EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
            GUILayout.Space(2f);
            gameName = EditorGUILayout.TextField("Game Name:", gameName);
            GUILayout.Space(2f);
            bool gameNameIsEmpty = string.IsNullOrEmpty(gameName) || string.IsNullOrWhiteSpace(gameName);
            if (gameNameIsEmpty && showEmptyNameWarning) EditorGUILayout.HelpBox("You need to specify the game name before setup!!", MessageType.Error);
            else if (showEmptyNameWarning && !gameNameIsEmpty) showEmptyNameWarning = false;
            if (GUILayout.Button("Setup"))
            {
                if (!gameNameIsEmpty) Utilities.Setup(gameName.Trim());
                else showEmptyNameWarning = true;
                GUIUtility.ExitGUI();
            }
            GUILayout.EndVertical();

            GUILayout.Space(10f);

            GUILayout.EndHorizontal();
        }
    }
}
