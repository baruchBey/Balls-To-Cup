using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Baruch.Core
{
    [DefaultExecutionOrder(-5)]
    public class FrameWork : MonoBehaviour
    {
        [Save] private static byte _version = default;
        
      
        private void Awake()
        {
            InitializeGame();


        }

        private static void InitializeGame()
        {

            var _cachedVersion = byte.Parse(Application.version.Replace(".", ""));

            Game.Initialize();

            if (_cachedVersion != _version)
            {
                _version = _cachedVersion;
                NewVersion();
            }
        }

        private static void NewVersion()
        {

#if UNITY_EDITOR
            Debug.Log($"NewVersion : {_version}");
#endif

        }

        private void OnValidate()
        {
            hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        }

#if UNITY_ANDROID
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                Save.SaveGame();
            }
        }
#endif

#if UNITY_IOS || UNITY_EDITOR
        private void OnApplicationQuit()
        {
            Save.SaveGame();
        }
#endif
    }
}
