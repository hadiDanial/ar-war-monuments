using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    [SerializeField] private Animator animator;
    
    public override UnitMapType GetUnitMapType()
    {
        return UnitMapType.Circle;
    }

    protected override void AttackTarget(Transform target)
    {
        
    }
}
