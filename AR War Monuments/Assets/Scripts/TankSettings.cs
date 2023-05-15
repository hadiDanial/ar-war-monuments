using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tank Settings")]
public class TankSettings : ScriptableObject
{
    public GameObject tankModel;
    public string countryName;
    public Color countryColor;
    public Sprite countryFlag;
}
