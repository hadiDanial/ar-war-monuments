using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform terrain;
    [SerializeField] private CinemachineVirtualCamera virtualARCamera;
    [SerializeField] private CinemachineVirtualCamera virtualOverheadCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private ARCameraBackground ARCameraBackground;
    [SerializeField] private ARCameraManager ARCameraManager;
    
    private bool flag = false;
    private WaitForSeconds waitForSeconds;
    private Coroutine coroutine;

    private void Awake()
    {
        ToggleCamera();
    }

    public void ToggleCamera()
    {
        flag = !flag;
        ARCameraBackground.enabled = flag;
        ARCameraManager.enabled = flag;
        virtualARCamera.Priority = flag ? 10 : 5;
        virtualOverheadCamera.Priority = !flag ? 10 : 5;
        terrain.gameObject.SetActive(!flag);
        //
        // if (flag)
        // {
        //     if(coroutine != null) StopCoroutine(coroutine);
        //     if(cinemachineBrain.ActiveBlend != null)
        //         coroutine = StartCoroutine(DisableTerrain());
        //     else
        //         terrain.gameObject.SetActive(false);
        // }
        // else
        //     terrain.gameObject.SetActive(true);
    }

    public IEnumerator DisableTerrain()
    {
        waitForSeconds = new WaitForSeconds(cinemachineBrain.ActiveBlend.Duration);
        yield return waitForSeconds;
        terrain.gameObject.SetActive(false);
    }
}
