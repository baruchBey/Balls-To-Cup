using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Baruch
{

    public class SaveAttributeFinder
    {
        public static SaveData[] FindSaveFields()
        {
            var data = new List<SaveData>();
            var scripts = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => (type.Namespace == "Baruch" || type.Namespace == "Baruch.Core") && type.IsSubclassOf(typeof(MonoBehaviour)));

            foreach (var scriptType in scripts)
            {

                var saveFields = scriptType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                    .Where(member => Attribute.IsDefined(member, typeof(SaveAttribute)) && !Attribute.IsDefined(member, typeof(DeprecatedSaveAttribute)));

                foreach (var saveField in saveFields)
                {
                    if (Attribute.GetCustomAttribute(saveField, typeof(SaveAttribute)) is SaveAttribute saveAttribute)
                    {
                        var value = saveField.GetValue(scriptType);
                        SaveData saveData = new(saveField, value);
                        data.Add(saveData);
                    }
                }
            }
            return data.ToArray();
        }
    }
}

