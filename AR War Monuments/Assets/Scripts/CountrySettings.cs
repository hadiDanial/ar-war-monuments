using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Country Settings")]
public class CountrySettings : ScriptableObject
{
    public GameObject tankModel;
    public string countryName;
    public Color countryColor;
    public Sprite countryFlag;
    public Material arrowTrailMaterial;
}
