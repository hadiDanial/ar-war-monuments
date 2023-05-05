using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraPositionUI : MonoBehaviour
{
    public TMP_Text cameraPositionText;
    public Transform ARCameraTransform;
    void Update()
    {
        cameraPositionText.text = ARCameraTransform.position.ToString();
    }
}
