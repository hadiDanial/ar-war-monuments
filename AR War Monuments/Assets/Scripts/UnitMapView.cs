using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(LineRenderer))]
public class UnitMapView : MonoBehaviour
{
    public CountrySettings countrySettings;
    public List<Vector3> positions;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = countrySettings.countryColor;
        lineRenderer.material = countrySettings.arrowTrailMaterial;
        UpdateLineRendererPositions(transform.position);
    }
    
    private void UpdateLineRendererPositions(Vector3 newPosition)
    {
        positions.Add(newPosition);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }


    public void AddPosition(Vector3 position)
    {
        UpdateLineRendererPositions(position);
    }
}
