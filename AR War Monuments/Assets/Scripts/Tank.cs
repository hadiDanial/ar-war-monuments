
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Arrow))]
public class Tank : MonoBehaviour
{
    public TankSettings tankSettings;
    public Transform modelTransformParent;
    public TMP_Text countryName;
    public RawImage backgroundColorImage;
    
    private void Start()
    {
        InitializeTank();
    }

    [ContextMenu("Initialize Tank")]
    public void InitializeTankEditMode()
    {
        ResetTank(true);
        SetupTank();
    }    public void InitializeTank()
    {
        ResetTank();
        SetupTank();
    }

    private void SetupTank()
    {
        countryName.text = tankSettings.countryName;
        backgroundColorImage.color = tankSettings.countryColor;
        GameObject tankModel = Instantiate(tankSettings.tankModel, modelTransformParent);
        tankModel.transform.localPosition= Vector3.zero;
        tankModel.transform.localRotation = Quaternion.identity;
    }

    private void ResetTank(bool isInEditMode = false)
    {
        countryName.text = string.Empty;
        backgroundColorImage.color = Color.clear;
        for (int i = modelTransformParent.childCount - 1; i >= 0; i--)
        {
            if(isInEditMode)
                DestroyImmediate(modelTransformParent.GetChild(i).gameObject);
            else
                Destroy(modelTransformParent.GetChild(i).gameObject);
        }
    }
}
