using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Soldier : Unit
{
    [SerializeField, Tooltip("How long should it take to rotate 1 degree?")] private float rotationDuration = 0.01f;
    [SerializeField] private Animator animator;
    protected override bool CanAttack()
    {
        return base.CanAttack() && !IsMoving;
    }

    protected override void Update()
    {
        attackTimer += Time.deltaTime;
        if (IsMoving)
        {
            moveTimer += Time.deltaTime;
            animator.SetBool("walking", true);
            animator.SetBool("firing", false);
        }
        else
        {
            animator.SetBool("walking", false);
            if(moveTimer >= moveEveryXSeconds)
                MoveToTarget();
            if (CanAttack())
            {
                animator.SetBool("firing", true);
                Attack();
                attackTimer = 0;
            }
            else
            {
                animator.SetBool("firing", false);
                MoveToTarget();
            }
        }
        
    }

    // protected override void SetRotation()
    // {
    //     if(isDead) return;
    //     if(IsMoving)
    //         base.SetRotation();
    //     else
    //     {
    //         if(CurrentTarget != null)
    //             modelTransformParent.rotation = Quaternion.LookRotation(CurrentTarget.transform.position - transform.position);
    //     }
    // }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("dead_front");
    }

    protected override void AttackTarget(Transform target)
    {
        // var duration = rotationDuration /
        //                Vector3.Angle(transform.forward, target.transform.position - transform.position);
        // transform.DOLookAt(target.position, duration, AxisConstraint.Y)
        //     .OnComplete(base.Attack);
    }
}
