using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Switches from AR camera to Map camera and back
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Renderer terrain;
    [SerializeField] private CinemachineVirtualCamera virtualARCamera;
    [SerializeField] private CinemachineVirtualCamera virtualOverheadCamera;
    [SerializeField] private ARCameraBackground ARCameraBackground;
    [SerializeField] private ARCameraManager ARCameraManager;
    [SerializeField] private GameObject cameraOverlay;
    [SerializeField] private TMP_Text mapButtonText;
    private bool flag = false;
    private WaitForSeconds waitForSeconds;
    private Coroutine coroutine;

    private void Awake()
    {
        ToggleCamera();
        #if !UNITY_EDITOR
        Time.timeScale = 0;
        ARCameraManager.frameReceived += ARCameraManagerOnframeReceived;
        #endif
    }

    private void ARCameraManagerOnframeReceived(ARCameraFrameEventArgs obj)
    {
        Time.timeScale = 1;
        ARCameraManager.frameReceived -= ARCameraManagerOnframeReceived;
    }

    public void ToggleCamera()
    {
        flag = !flag;
        ARCameraBackground.enabled = flag;
        ARCameraManager.enabled = flag;
        virtualARCamera.Priority = flag ? 10 : 5;
        virtualOverheadCamera.Priority = !flag ? 10 : 5;
        terrain.enabled = !flag;
        if(cameraOverlay != null)
            cameraOverlay.SetActive(flag);
        mapButtonText.text = flag ? "מפה" : "קרב";
    }
}
