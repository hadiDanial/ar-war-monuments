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
            updateDestinationTimer += Time.deltaTime;
            animator.SetBool("walking", true);
            animator.SetBool("firing", false);
        }
        else
        {
            animator.SetBool("walking", false);
            if(updateDestinationTimer >= calcDestinationEveryXSeconds)
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
    
    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("dead_front");
    }

}
