using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<UnitGroup> groups;
    [SerializeField] private List<Vector3> groupLocations;
    [SerializeField] private List<Transform> groupParents;

    public List<Vector3> GroupLocations => groupLocations;
    public List<Transform> GroupParents => groupParents;
    public List<UnitGroup> Groups => groups;

    public void SpawnUnits()
    {
        throw new NotImplementedException();
    }
    
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (groups.Count == 0)
        {
            groupLocations.Clear();
            foreach (Transform parent in groupParents)
            {
                DestroyImmediate(parent.gameObject);
            }
            groupParents.Clear();
            return;
        }
        while (groupLocations.Count < groups.Count)
        {
            groupLocations.Add(new Vector3());
        }
        while (groupParents.Count < groups.Count)
        {
            GameObject newObject = new GameObject($"Group_{GroupParents.Count}");
            newObject.transform.SetParent(transform);
            groupParents.Add(newObject.transform);
        }

        while (groupLocations.Count > groups.Count)
            groupLocations.RemoveAt(groupLocations.Count - 1);
        while (groupParents.Count > groups.Count)
        {
            Transform toRemove = groupParents[groupParents.Count - 1];
            groupParents.RemoveAt(groupParents.Count - 1);
            UnityEditor.EditorApplication.delayCall+=()=>
            {
                DestroyImmediate(toRemove.gameObject);
            };
        }

        for (int i = 0; i < groups.Count; i++)
        {
            if(groupParents[i] == null)
            {
                GameObject newObject = new GameObject($"Group_{i}");
                newObject.transform.SetParent(transform);
                groupParents[i] = newObject.transform;
            }
        }
    }
#endif
    
}
