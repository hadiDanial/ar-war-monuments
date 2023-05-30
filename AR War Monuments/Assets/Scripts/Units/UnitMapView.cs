using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Unit), typeof(LineRenderer))]
public class UnitMapView : MonoBehaviour
{
    [SerializeField, Range(1, 50)] private float minDistanceBetweenPoints = 10;
    [SerializeField] private CountrySettings countrySettings;
    [SerializeField] private List<Vector3> positions;
    [SerializeField] private RawImage unitImage;

    private Unit unit;
    private LineRenderer lineRenderer;
    private UnitMapType unitMapType;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = countrySettings.countryColor;
        lineRenderer.material = countrySettings.arrowTrailMaterial;
        SetEnabled(false);
        AddPosition(transform.position);
    }
    private void SetMaterialProperties()
    {
        // lineRenderer.material = countrySettings.arrowTrailMaterial;
        // unitMapType = unit.GetUnitMapType();
        // Material material = unitImage.material;
        // foreach (UnitMapType type in Enum.GetValues(typeof(UnitMapType)))
        // {
        //     if(type == unitMapType)
        //         material.EnableKeyword($"{type.ToString().ToUpper()}");
        //     else
        //         material.DisableKeyword($"{type.ToString().ToUpper()}");
        // }
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
        positions.Add(position + Vector3.up * 0.1f);
        UpdateLineRendererPositions();
    }

    public void SetEnabled(bool isEnabled)
    {
        // SetMaterialProperties();
        unitImage.gameObject.SetActive(isEnabled);
        lineRenderer.enabled = isEnabled;
    }
}

public enum UnitMapType {Triangle, Circle, Square}
