using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Controls the map view aspects of units: the trail they traveled on, displaying symbols etc...
/// </summary>
[RequireComponent(typeof(Unit), typeof(LineRenderer))]
public class UnitMapView : MonoBehaviour
{
    [SerializeField, Range(1, 50)] private float minDistanceBetweenPoints = 10;
    [SerializeField] private CountrySettings countrySettings;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private RawImage unitImage;

    private Unit unit;
    private LineRenderer lineRenderer;
    private Transform unitImageTransform;
    private static readonly Quaternion additionalRotation = Quaternion.Euler(90,-90,0);
    
    private void Awake()
    {
        unit = GetComponent<Unit>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = countrySettings.countryColor;
        lineRenderer.material = countrySettings.arrowTrailMaterial;
        unitImageTransform = unitImage.transform;
        SetEnabled(false);
        AddPosition(transform.position);
    }

    private void UpdateLineRendererPositions()
    {
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void Update()
    {
        AddPosition(transform.position);
    }

    public void AddPosition(Vector3 position)
    {
        if (positions.Count < 2)
        {
            positions.Add(position);
            UpdateLineRendererPositions();
            return;
        }
        
        if(Vector3.Distance(position, positions[^1]) < minDistanceBetweenPoints)
            return;
        positions.Add(position + Vector3.up * 0.15f);
        UpdateLineRendererPositions();
    }

    public void SetRotation(Quaternion rotation)
    {
        unitImageTransform.rotation = rotation * additionalRotation;
    }

    public void SetEnabled(bool isEnabled)
    {
        unitImage.gameObject.SetActive(isEnabled);
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = isEnabled;
    }
}