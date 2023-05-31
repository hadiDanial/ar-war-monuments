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
    [SerializeField] private Transform modelTransformParent;
    [SerializeField] private TMP_Text countryName;
    [SerializeField] private RawImage backgroundColorImage;

    private UnitMapView mapView;
    private NavMeshAgent navMeshAgent;
    private GameObject spawnedUnitModel;
    private AudioSource audioSource;
    private int currentHealth;
    private float timer;

    public delegate void UnitDestroyed();
    public event UnitDestroyed OnUnitDestroyed;
    
    private bool isDead, isInARView = true;
    private List<Unit> enemyUnits;
    private Unit currentTarget;
    protected bool IsMoving => navMeshAgent.hasPath && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    public CountrySettings CountrySettings => countrySettings;

    private void Awake()
    {
        mapView = GetComponent<UnitMapView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        if(bulletSpawnPoint == null)
            Debug.LogWarning($"{gameObject.name} is missing a bullet spawn point!");
        mapView.SetEnabled(false);
        navMeshAgent.speed = movementSpeed;

        SetRandomTimerValue();
        

    }

    private void SetRandomTimerValue()
    {
        timer = UnityEngine.Random.Range(0f, timeBetweenShots/10f);
    }

    protected virtual void Update()
    {
        // if(!isActive) 
        //     return;
        timer += Time.deltaTime;

        if (CanAttack())
        {
            Attack();
            SetRandomTimerValue();
        }
    }
    
    protected virtual bool CanAttack()
    {
        return timer > timeBetweenShots;
    }

    protected virtual void Attack()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        if(isInARView)
            AudioManager.Instance.PlayFromList(bulletSpawnPoint.position, attackSounds);
    }
    protected abstract void AttackTarget(Transform target);
    public void Move(Transform target)
    {
        if(!canMove || isDead) return;
        Move(target.position);
    }
    public virtual void Move(Vector3 targetPosition)
    {
        navMeshAgent.Move(targetPosition);
    }


    public void Damage(int damage)
    {
        if(isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, currentHealth);
        if (currentHealth == 0)
        {
            isDead = true;
            navMeshAgent.speed = 0;
            navMeshAgent.enabled = false;
            if(isInARView)
                AudioManager.Instance.PlayFromList(bulletSpawnPoint.position, deathSounds);
            OnUnitDestroyed?.Invoke();
        }
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
        this.enemyUnits = enemyUnits;
        SelectTarget();
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        if(currentTarget != null)
            Move(currentTarget.transform);
    }

    private void SelectTarget()
    {
        currentTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Unit possibleTarget in enemyUnits)
        {
            if (possibleTarget.isDead) continue;
            Vector3 directionToTarget = possibleTarget.transform.position - currentPosition;
            // Use square magnitude because we don't care about exact distance, just who is closest, and square distance is much cheaper to calculate
            float distanceSqr = directionToTarget.sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                currentTarget = possibleTarget;
                closestDistanceSqr = distanceSqr;
            }
        }
        
    }
}
