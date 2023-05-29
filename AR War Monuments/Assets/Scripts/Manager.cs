using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{ 
    public static Manager Instance { get; private set; }
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

    public void AddUnit(Unit unit) => units.Add(unit);

    public void ToggleMapView()
    {
        foreach (Unit unit in units)
        {
            unit.ToggleMapMode();
        }
    }
}
