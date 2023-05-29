using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;
    public List<Transform> targets;
    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        foreach (Transform target in targets)
        {
            target.LookAt(2 * transform.position - cameraTransform.position);
        }
    }
}
