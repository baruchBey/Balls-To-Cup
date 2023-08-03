using UnityEditor;
using UnityEngine;

namespace Baruch.Util
{
    public static class VideoUtilities
    {
        static readonly string _handCanvasPath = "Assets/BaruchCasualBase/Prefabs/Video/CursorCanvas.prefab";
        static readonly string _joyStickCanvasPath = "Assets/BaruchCasualBase/Prefabs/Video/JoyStickCanvas.prefab";

        public static void CursorCanvas()
        {
            if(!GameObject.Find("CursorCanvas"))
                PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(_handCanvasPath));
        }
        public static void JoystickCanvas()
        {
            if (!GameObject.Find("JoyStickCanvas"))
                PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(_joyStickCanvasPath));
        }

    }
}
