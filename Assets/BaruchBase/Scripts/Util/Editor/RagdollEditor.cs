using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Baruch.UtilEditor
{
    [CustomEditor(typeof(Ragdoll))]
    public class RagdollEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Ragdoll myTarget = (Ragdoll)target;
           
            if (myTarget.showInspector) {                 
                DrawDefaultInspector();
            }
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = myTarget.HasRagdoll();
            if (GUILayout.Button("Clear"))
            {
                myTarget.Clear();
            }
            GUI.enabled = !myTarget.HasRagdoll();
            if (GUILayout.Button("Build"))
            {
                myTarget.Build();
                myTarget.Toggle();
                myTarget.showInspector = false;

            }
            EditorGUILayout.EndHorizontal();


        }
    }
}
