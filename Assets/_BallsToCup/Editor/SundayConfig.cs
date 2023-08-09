using UnityEditor;
using UnityEngine;

namespace Baruch.UtilEditor
{
    public class SundayConfig : ScriptableObject
    {
        private void Awake()
        {
            if (!EditorPrefs.GetBool("SundayInitialized", false))
            {
                EditorPrefs.SetBool("SundayInitialized", true);
                SundayWindow.ShowWindow();
            }
        }

    }
}