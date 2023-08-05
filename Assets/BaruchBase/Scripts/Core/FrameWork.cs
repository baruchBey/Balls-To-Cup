using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Baruch.Core
{
    [DefaultExecutionOrder(-5)]
    public class FrameWork : MonoBehaviour
    {
        [Save] private static byte _version = default;

        public static event Action OnFirstClick;
      
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
        private void Update()
        {
            if (Game.GameState==GameState.Idle && Input.GetMouseButtonDown(0))
            {
                if (!IsMouseOverUIElements())//Mousenot over ui elemenmts
                    OnFirstClick.Invoke();
            }
        }
        private bool IsMouseOverUIElements()
        {
            // Check if the mouse is over any UI elements using the EventSystem and GraphicRaycaster
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            // Create a list to store the Raycast results
            var results = new List<RaycastResult>();

            // Perform the Raycast
            EventSystem.current.RaycastAll(eventData, results);

            // If the list is empty, the mouse is not over any UI elements
            return results.Count > 0;
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
