using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Arrow : MonoBehaviour
{
    public TankSettings tankSettings;
    public List<Vector3> positions;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = tankSettings.countryColor;
        lineRenderer.material = tankSettings.arrowTrailMaterial;
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
