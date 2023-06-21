
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Tank : Unit
{
    [SerializeField] private AudioSource engineAudioSource, movingAudioSource;

    protected override bool CanAttack()
    {
        return base.CanAttack() && !IsMoving;
    }

    protected override void Update()
    {
        base.Update();
        bool muted = IsMoving && AudioManager.Instance.IsMuted;
        movingAudioSource.enabled = !muted;
        engineAudioSource.enabled = !muted;
    }

    protected override void AttackTarget(Transform target)
    {
        return;
    }

    protected override void Die()
    {
        engineAudioSource.enabled = false;
        movingAudioSource.enabled = false;
        base.Die();
    }
}
