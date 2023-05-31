
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Tank : Unit
{
    [SerializeField, Tooltip("How long should it take to rotate 1 degree?")] private float rotationDuration = 0.05f;
    protected override void SetRotation()
    {
        if(isDead) return;
        if(IsMoving)
            base.SetRotation();
        else
        {
            modelTransformParent.rotation = Quaternion.LookRotation(CurrentTarget.transform.position - transform.position);
        }
    }
    protected override bool CanAttack()
    {
        return base.CanAttack() && !IsMoving;
    }

    protected override void AttackTarget(Transform target)
    {
        return;
    }

    // protected override void Attack()
    // {
    //     AttackTarget(CurrentTarget.transform);
    // }
    //
    // protected override void AttackTarget(Transform target)
    // {
    //     var duration = rotationDuration /
    //                    Vector3.Angle(transform.forward, target.transform.position - transform.position);
    //     transform.DOLookAt(target.position, duration, AxisConstraint.Y)
    //         .OnComplete(base.Attack);
    // }
}
