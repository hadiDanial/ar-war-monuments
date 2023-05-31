using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data container for unit groups - a list of units, how many of them are in the group, a formation to spawn them in, and spacing between units.
/// </summary>
[CreateAssetMenu(menuName = "Unit Group")]
public class UnitGroup : ScriptableObject
{
    public List<UnitAmount> unitsInGroup;
}

[Serializable]
public struct UnitAmount
{
    public Unit unit;
    public int amount;
    public UnitFormation formation;
    public float spaceBetweenUnits;
}

public enum UnitFormation {HorizontalLine, VerticalLine, Circle, Square}
