using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

[CustomEditor(typeof(UnitManager))]
public class UnitManagerEditor : Editor
{
    private UnitManager manager;

    private void OnEnable()
    {
        manager = target as UnitManager;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Spawn Units"))
        {
            manager.SpawnUnits();
        }
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        
        for (int i = 0; i < manager.GroupLocations.Count; i++)
        {
            EditorGUI.BeginChangeCheck();    
            Vector3 position = manager.GroupLocations[i];
            position = Handles.PositionHandle(position, Quaternion.identity);
            Handles.color = Color.green;
            Handles.SphereHandleCap(0, position, Quaternion.identity, 2f, EventType.Repaint);
            Handles.Label(position + 0.2f * Vector3.up, $"G{i}");
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(manager, $"Moved Group {i}");
                manager.GroupLocations[i] = position;
            }        
        }
    }
}
