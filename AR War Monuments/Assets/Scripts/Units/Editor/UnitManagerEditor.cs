using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Custom editor for UnitManager
/// </summary>
[CustomEditor(typeof(UnitManager))]
public class UnitManagerEditor : Editor
{
    private UnitManager manager;
    private GUIStyle labelStyle;
    private void OnEnable()
    {
        manager = target as UnitManager;
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 36;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Spawn Units"))
        {
            manager.SpawnUnitGroups();
        }
        if (GUILayout.Button("Destroy Units"))
        {
            manager.DestroyUnits();
        }
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        
        for (int i = 0; i < manager.GroupParents.Count; i++)
        {
            EditorGUI.BeginChangeCheck();    
            Vector3 position = manager.GroupParents[i].position;
            position = Handles.PositionHandle(position, Quaternion.identity);
            Handles.color = Color.green;
            var size = 5f;
            Handles.SphereHandleCap(0, position, Quaternion.identity, size, EventType.Repaint);
            Handles.Label(position + size * Vector3.up, $"G{i}", labelStyle);
            
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(manager, $"Moved Group {i}");
                manager.GroupParents[i].position = position;
            }        
        }
    }
}
