using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    
    private bool isDead, isActive = true;
    protected bool IsMoving => navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

    private void Awake()
    {
        mapView = GetComponent<UnitMapView>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        if(bulletSpawnPoint == null)
            Debug.LogWarning($"{gameObject.name} is missing a bullet spawn point!");
        mapView.SetEnabled(false);

        SetRandomTimerValue();
    }

    private void Start()
    {
        Manager.Instance.AddUnit(this);
    }

    private void SetRandomTimerValue()
    {
        timer = UnityEngine.Random.Range(0f, timeBetweenShots/10f);
    }

    protected virtual void Update()
    {
        if(!isActive) 
            return;
        timer += Time.deltaTime;

        if (CanAttack())
        {
            Attack();
            timer = 0;
        }
    }

    public abstract UnitMapType GetUnitMapType();
    
    protected virtual bool CanAttack()
    {
        return timer > timeBetweenShots;
    }

    protected virtual void Attack()
    {
        GameObject.Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        AudioManager.Instance.PlayFromList(audioSource, attackSounds);
    }
    protected abstract void AttackTarget(Transform target);
    public void Move(Transform target)
    {
        if(!canMove || isDead) return;
        Move(target.position);
    }
    public virtual void Move(Vector3 targetPosition)
    {
        navMeshAgent.speed = movementSpeed;
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
            AudioManager.Instance.PlayFromList(audioSource, deathSounds);
            OnUnitDestroyed?.Invoke();
        }
    }

    public void ToggleMapMode()
    {
        isActive = !isActive;
        modelTransformParent.gameObject.SetActive(isActive);
        mapView.SetEnabled(!isActive);
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
}
