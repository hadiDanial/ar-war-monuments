using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [SerializeField, Range(0.5f,10)] private float speed = 5;
    private float originalY;
    
    private void Start()
    {
        originalY = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, originalY + Mathf.Sin(Time.time * speed), transform.position.z);
    }
    
}
