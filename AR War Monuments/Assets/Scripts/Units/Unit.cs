using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Base class for all units - Tanks, artillery, soldiers...
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(UnitMapView), typeof(BoxCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class Unit : MonoBehaviour
{
    [Header("Unit Settings")]
    
    [SerializeField, Range(0.1f, 5f)] private float timeBetweenShots = 5f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private bool canMove = true;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject unitModel;
    [FormerlySerializedAs("attackSound")] [SerializeField] private AudioClipList attackSounds;
    [FormerlySerializedAs("deathSound")] [SerializeField] private AudioClipList deathSounds;

    [Header("Unit Settings")]
    [SerializeField] private CountrySettings countrySettings;
    [SerializeField] protected Transform modelTransformParent;
    [SerializeField] private TMP_Text countryName;
    [SerializeField] private RawImage backgroundColorImage;

    private BoxCollider boxCollider;
    private UnitMapView mapView;
    private NavMeshAgent navMeshAgent;
    private GameObject spawnedUnitModel;
    private AudioSource audioSource;
    private int currentHealth;
    protected float attackTimer, moveTimer, moveEveryXSeconds = 0.75f;

    public delegate void UnitDestroyed();
    public event UnitDestroyed OnUnitDestroyed;
    
    protected bool isDead, isInARView = true;
    protected List<Unit> EnemyUnits;
    protected Unit CurrentTarget;
    protected bool IsMoving => navMeshAgent.hasPath && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;
    public CountrySettings CountrySettings => countrySettings;

    private void Awake()
    {
        mapView = GetComponent<UnitMapView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        currentHealth = maxHealth;
        if(bulletSpawnPoint == null)
            Debug.LogWarning($"{gameObject.name} is missing a bullet spawn point!");
        mapView.SetEnabled(false);
        navMeshAgent.speed = movementSpeed;
        moveTimer = 0;
        SetRandomTimerValue();
        

    }

    private void SetRandomTimerValue()
    {
        attackTimer = UnityEngine.Random.Range(0f, timeBetweenShots/10f);
    }

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;
        if (IsMoving) moveTimer += Time.deltaTime;
        if(moveTimer >= moveEveryXSeconds)
            MoveToTarget();
        if (CanAttack())
        {
            Attack();
            SetRandomTimerValue();
        }
        else MoveToTarget();

        SetRotation();
    }

    protected virtual void SetRotation()
    {
        if(isDead) return;
        if(IsMoving)
            modelTransformParent.rotation = Quaternion.LookRotation(navMeshAgent.velocity);
        else
        {
            if(CurrentTarget != null)
                modelTransformParent.rotation = Quaternion.LookRotation(CurrentTarget.transform.position - transform.position);
        }
    }

    protected virtual bool CanAttack()
    {
        return !isDead && !IsMoving && attackTimer > timeBetweenShots;
    }

    protected virtual void Attack()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        if(isInARView)
            AudioManager.Instance.PlayFromList(bulletSpawnPoint.position, attackSounds);
    }
    protected abstract void AttackTarget(Transform target);
    protected void MoveToTarget()
    {
        moveTimer = 0;
        if(!canMove || isDead) return;
        if(CurrentTarget != null)
            Move(CurrentTarget.transform.position);
    }
    public virtual void Move(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }


    public void Damage(int damage)
    {
        if(isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        currentHealth = 0;
        navMeshAgent.speed = 0;
        navMeshAgent.enabled = false;
        boxCollider.enabled = false;
        if (isInARView)
            AudioManager.Instance.PlayFromList(bulletSpawnPoint.position, deathSounds);
        OnUnitDestroyed?.Invoke();
    }

    public void ToggleMapMode()
    {
        isInARView = !isInARView;
        modelTransformParent.gameObject.SetActive(isInARView);
        mapView.SetEnabled(!isInARView);
    }
    
    private void SetupUnit()
    {
        countryName.text = countrySettings.countryName;
        backgroundColorImage.color = countrySettings.countryColor;
        spawnedUnitModel = Instantiate(unitModel, modelTransformParent);
        spawnedUnitModel.transform.localPosition= Vector3.zero;
        spawnedUnitModel.transform.localRotation = Quaternion.identity;
    }

    private void ResetUnit(bool isInEditMode = false)
    {
        countryName.text = string.Empty;
        backgroundColorImage.color = Color.clear;
        if(spawnedUnitModel != null)
            DestroyImmediate(spawnedUnitModel);
    }

    [ContextMenu("Initialize Unit")]
    public void InitializeUnit()
    {
        ResetUnit();
        SetupUnit();
    }

    public void SetEnemies(List<Unit> enemyUnits)
    {
        this.EnemyUnits = enemyUnits;
        SelectTarget();
        MoveToTarget();
    }


    private void SelectTarget()
    {
        if (CurrentTarget != null)
            CurrentTarget.OnUnitDestroyed -= SelectTarget;
        CurrentTarget = null;
        
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Unit possibleTarget in EnemyUnits)
        {
            if (possibleTarget.isDead) continue;
            Vector3 directionToTarget = possibleTarget.transform.position - currentPosition;
            // Use square magnitude because we don't care about exact distance, just who is closest, and square distance is much cheaper to calculate
            float distanceSqr = directionToTarget.sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                CurrentTarget = possibleTarget;
                closestDistanceSqr = distanceSqr;
            }
        }

        if (CurrentTarget != null)
            CurrentTarget.OnUnitDestroyed += SelectTarget;
    }
}
