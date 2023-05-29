using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float timeBetweenShots = 5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeBetweenShots)
        {
            Shoot();
            timer = 0;
        }
    }

    private void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
