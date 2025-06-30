using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlatformManager))]
public class PlatformManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlatformManager manager = (PlatformManager)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Reset all platforms"))
        {
            manager.RespawnPlatforms();
        }

        GUILayout.Space(10);
        
        if (GUILayout.Button("Test Fall All"))
        {
            manager.TestFallAll();
        }
    }
}
