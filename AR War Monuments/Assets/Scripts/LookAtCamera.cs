using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    private void Awake()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(2 * transform.position - cameraTransform.position);
    }
}
