namespace Baruch.UtilEditor
{
    using System;
    using UnityEditor;
    using UnityEngine;

    public class SaveAttributeValidator
    {
        [RuntimeInitializeOnLoadMethod]
        private static void ValidateSaveAttributes()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var fields = type.GetFields(System.Reflection.BindingFlags.Instance |
                                            System.Reflection.BindingFlags.Public |
                                            System.Reflection.BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    var attributes = field.GetCustomAttributes(typeof(SaveAttribute), false);
                    if (attributes.Length > 0)
                    {
                        UnityEngine.Debug.LogError($"SaveAttribute can only be applied to static fields. Field: {type.Name}.{field.Name}", TypeToScript(type));
                    }
                }
            }
        }

        private static UnityEngine.Object TypeToScript(Type scriptType)
        {

            var assets = UnityEditor.AssetDatabase.FindAssets("t:Script");
            foreach (var guid in assets)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var script = UnityEditor.AssetDatabase.LoadAssetAtPath<MonoScript>(path);
                if (script.GetClass() == scriptType)
                {
                    return script;
                }
            }
            return null;
        }
    }

}
