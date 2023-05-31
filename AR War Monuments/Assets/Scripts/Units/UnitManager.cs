using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages all units - spawning in the editor, giving them targets etc...
/// </summary>
public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<UnitGroup> groups;
    [SerializeField] private List<Transform> groupParents;
    [SerializeField] private CountrySettings israel, syria;
    
    public static UnitManager Instance { get; private set; }

    public List<Transform> GroupParents => groupParents;
    public List<UnitGroup> Groups => groups;

    /// <summary>
    /// Saves all the units for each country.
    /// </summary>
    private Dictionary<CountrySettings, List<Unit>> countryUnitsDictionary =
        new Dictionary<CountrySettings, List<Unit>>();
    private HashSet<Unit> units = new HashSet<Unit>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        if (countryUnitsDictionary == null || countryUnitsDictionary.Count == 0)
        {
            Unit[] allUnits = FindObjectsOfType<Unit>();
            foreach (Unit unit in allUnits)
            {
                AddUnitToDictionary(unit);
                AddUnitToSet(unit);
            }
        }

        // DebugCountryDictionary();

        SetTargets();
    }

    private void SetTargets()
    {
        // TODO: Give each unit a list of enemy units and ask it to navigate towards it using its NavMeshAgent and then attack...
        List<Unit> israelUnits = countryUnitsDictionary[israel], syriaUnits = countryUnitsDictionary[syria];

        foreach (Unit unit in israelUnits)
        {
            unit.SetEnemies(syriaUnits);
        }
        foreach (Unit unit in syriaUnits)
        {
            unit.SetEnemies(israelUnits);
        }
    }

    public void ToggleMapView()
    {
        foreach (Unit unit in units)
        {
            unit.ToggleMapMode();
        }
    }


    private void AddUnitToDictionary(Unit unit)
    {
        List<Unit> countryUnits;
        if (countryUnitsDictionary.ContainsKey(unit.CountrySettings))
        {
            countryUnits = countryUnitsDictionary[unit.CountrySettings];
        }
        else
        {
            countryUnits = new List<Unit>();
        }
        if(!countryUnits.Contains(unit))
            countryUnits.Add(unit);
        countryUnitsDictionary[unit.CountrySettings] = countryUnits;
    }
    private void AddUnitToSet(Unit unit) => units.Add(unit);



#if UNITY_EDITOR
    /// <summary>
    /// This function should only be used in the editor! For easier editing.
    /// </summary>
    public void SpawnUnitGroups()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            UnitGroup group = groups[i];
            DestroyExistingObjects(groupParents[i]);
            Transform parent = groupParents[i];
            for (int j = 0; j < group.unitsInGroup.Count; j++)
            {
                var unitAmount = group.unitsInGroup[j];
                SpawnUnit(unitAmount, j, parent);
            }
        }
    }

    private void SpawnUnit(UnitAmount unitAmount, int index, Transform parent)
    {
        GameObject parentObj = new GameObject($"Units_{index}");
        parentObj.transform.SetParent(parent);
        parentObj.transform.localPosition = Vector3.zero;
        for (int i = 0; i < unitAmount.amount; i++)
        {
            Vector3 position = CalculateInitialUnitPositionByFormation(unitAmount.formation, unitAmount.spaceBetweenUnits, i, unitAmount.amount);
            
            Unit unit = PrefabUtility.InstantiatePrefab(unitAmount.unit) as Unit;
            AddUnitToDictionary(unit);
            Transform unitTransform = unit.gameObject.transform;
            unitTransform.localPosition = position + parent.position;
            unitTransform.SetParent(parentObj.transform);
        }
    }

    /// <summary>
    /// Given a formation and an index, calculate the position that the unit should be spawned on.
    /// </summary>
    /// <param name="formation">The formation to spawn the units in. (Horizontal/Vertical Line, Square, Circle)</param>
    /// <param name="spaceBetweenUnits">How much space should be between the different units? (If Formation is Circle, it represents the radius)</param>
    /// <param name="index">Index of this unit</param>
    /// <param name="amount">Total amount of units in the group</param>
    /// <returns></returns>
    private Vector3 CalculateInitialUnitPositionByFormation(UnitFormation formation, float spaceBetweenUnits, int index, int amount)
    {
        switch (formation)
        {
            case UnitFormation.Circle:
            {
                float angle = ((float)index / amount) * 360f * Mathf.Deg2Rad;
                return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spaceBetweenUnits; // spaceBetweenUnits = radius in this case
            }
            case UnitFormation.Square:
            {
                int amountPerRow = Mathf.CeilToInt(Mathf.Sqrt(amount));
                float initX = -(amountPerRow * spaceBetweenUnits) / 2f;
                float initY = initX;
                float x = spaceBetweenUnits * (index % amountPerRow), y = spaceBetweenUnits * (index / amountPerRow);
                return new Vector3(initX + x, 0, initY + y);
            }
            case UnitFormation.VerticalLine:
            {
                float initPos = -(amount * spaceBetweenUnits) / 2f;
                return new Vector3(0, 0, initPos + index * amount * spaceBetweenUnits);
            }
            case UnitFormation.HorizontalLine:
            default:
            {
                float initPos = -(amount * spaceBetweenUnits) / 2f;
                return new Vector3(initPos + index * amount * spaceBetweenUnits, 0, 0);
            }
          
           
        }
    }

    public void DestroyUnits()
    {
        foreach (Transform gTransform in groupParents)
        {
            DestroyExistingObjects(gTransform);
        }
    }
    
    private void DestroyExistingObjects(Transform groupParent)
    {
        for (int i = groupParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(groupParent.GetChild(i).gameObject);
        }
    }

    private void DebugCountryDictionary()
    {
        foreach (var country in countryUnitsDictionary.Keys)
        {
            foreach (var unit in countryUnitsDictionary[country])
            {
                Debug.Log($"{country.name} | {unit.name}");
            }
        }
    }



    /// <summary>
    /// Updates values and objects in the scene whenever values change in the inspector - used for object setup and validation, making sure things are correctly setup.
    /// </summary>
    private void OnValidate()
    {
        if (groups.Count == 0)
        {
            foreach (Transform parent in groupParents)
            {
                UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(parent.gameObject); };
            }
            groupParents.Clear();
            return;
        }
        while (groupParents.Count < groups.Count)
        {
            GameObject newObject = new GameObject($"Group_{GroupParents.Count}");
            newObject.transform.SetParent(transform);
            groupParents.Add(newObject.transform);
        }

        while (groupParents.Count > groups.Count)
        {
            Transform toRemove = groupParents[groupParents.Count - 1];
            groupParents.RemoveAt(groupParents.Count - 1);
            UnityEditor.EditorApplication.delayCall+=()=> { DestroyImmediate(toRemove.gameObject); };
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
