using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(Tank))]
[CanEditMultipleObjects]
public class TankEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Initialize Tank"))
        {
            foreach (var selectedTarget in targets)
            {
                (selectedTarget as Tank)?.InitializeTankEditMode();
            }
        }
    }
}
