using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Baruch.UtilEditor;

namespace Baruch.Util
{
    public static class Utilities
    {


        public static void Setup(string gameName)
        {
            if (SetupValidate()) return;
            
            EditorSettings.projectGenerationRootNamespace = "Baruch";
            AssetDatabase.CreateFolder("Assets", gameName);
            AssetDatabase.CreateFolder($"Assets/{gameName}", "3D");
            AssetDatabase.CreateFolder($"Assets/{gameName}/3D", "Environment");
            AssetDatabase.CreateFolder($"Assets/{gameName}/3D", "Character");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "Animation");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "Materials");
            AssetDatabase.CreateFolder($"Assets/{gameName}/Materials", "_Shaders");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "Prefabs");
            AssetDatabase.CreateFolder($"Assets/{gameName}/Prefabs", "LevelPrefabs");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "Scenes");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "Scripts");            
            AssetDatabase.CreateFolder($"Assets/{gameName}/Scripts", "LevelScripts");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "Textures");
            AssetDatabase.CreateFolder($"Assets/{gameName}", "UI");

            Scene init = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            EditorSceneManager.SaveScene(init, $"Assets/{gameName}/Scenes/Init.unity");
            EditorSceneManager.OpenScene($"Assets/{gameName}/Scenes/Init.unity");

            
    

           
            
            GameObject eventSystemGameObject = ObjectFactory.CreateGameObject("EventSystem");
            StageUtility.PlaceGameObjectInCurrentStage(eventSystemGameObject);
            ObjectFactory.AddComponent<EventSystem>(eventSystemGameObject);
            ObjectFactory.AddComponent<StandaloneInputModule>(eventSystemGameObject);
            
            PlayerSettings.companyName = "Baruch Games";
            PlayerSettings.productName = gameName.StartsWith("_") ? gameName.Remove(0,1).Trim() : gameName.Trim();
            var original = EditorBuildSettings.scenes;
            var newSettings = new EditorBuildSettingsScene[original.Length + 1];
            System.Array.Copy(original, newSettings, original.Length);
            var sceneToAdd = new EditorBuildSettingsScene("Assets/BaruchCasualBase/Scenes/Loading.unity", true);
            newSettings[newSettings.Length - 1] = sceneToAdd;
            EditorBuildSettings.scenes = newSettings;

            RenderSettings.skybox = AssetDatabase.LoadAssetAtPath("Assets/BaruchCasualBase/Assets/Materials/GradientSkybox.mat", typeof(Material)) as Material;

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS,"com.gorillazone."+ PlayerSettings.productName.ToLower());
            PlayerSettings.gcIncremental = true;
            Screen.orientation = ScreenOrientation.Portrait;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            Debug.Log("Project has been setup!!");
        }
        
        private static bool SetupValidate()
        {
            string m_Path = Application.dataPath;
            return Directory.GetDirectories(m_Path, "_*", SearchOption.TopDirectoryOnly).Length>0;
        } 
        
      
       
        
       

        [MenuItem("Baruch/Utilities Panel")]
        private static void OpenAnalyticsUtilitiesWindow()
        {
            UtilitiesWindow.Open();
        }
      
         [MenuItem("Baruch/DeleteSave")]
        private static void DeleteSave()
        {
            PlayerPrefs.DeleteAll();
            Save.DeleteSave();
        }
    }
}