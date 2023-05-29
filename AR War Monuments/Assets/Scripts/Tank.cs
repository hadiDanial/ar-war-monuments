
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(UnitMapView))]
public class Tank : MonoBehaviour
{
    [FormerlySerializedAs("tankSettings")] public CountrySettings countrySettings;
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
        countryName.text = countrySettings.countryName;
        backgroundColorImage.color = countrySettings.countryColor;
        GameObject tankModel = Instantiate(countrySettings.tankModel, modelTransformParent);
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
