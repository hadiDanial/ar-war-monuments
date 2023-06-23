using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferenceManager : MonoBehaviour
{
    [SerializeField] private Toggle mutedToggle;
    
    private bool? isMuted;
    private void Awake()
    {
        if (mutedToggle != null)
            mutedToggle.isOn = GetMutedSetting();
    }

    public void ToggleMuteSounds(bool val)
    {
        isMuted = val;
        PlayerPrefs.SetInt("Muted", val ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetMutedSetting()
    {
        //if (isMuted.HasValue)
        //   return isMuted.Value;
        //else
        //{
            isMuted = PlayerPrefs.HasKey("Muted") && (PlayerPrefs.GetInt("Muted") == 1 ? true : false);
            return isMuted.Value;
        //}
    }
}
