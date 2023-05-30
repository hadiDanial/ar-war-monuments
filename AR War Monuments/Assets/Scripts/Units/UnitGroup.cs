using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
