using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private SubtitledAudio subtitledAudio;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button startButton, stopButton;
    
    private Subtitle currentSubtitle;
    private float timer = 0;
    private bool isPlaying = false, isDone = false;
    private int index = 0;
    private void Awake()
    {
        GetFirstSubtitle();
        audioSource.clip = subtitledAudio.audioClip;
    }


    private void Update()
    {
        if(!isPlaying || isDone)
            return;
        timer += Time.deltaTime;
        if (!(timer > currentSubtitle.duration)) return;
        GetNextSubtitle();
        timer = 0;
    }
    
    private void GetFirstSubtitle()
    {
        index = 0;
        currentSubtitle = subtitledAudio.GetNextSubtitle(index);
        subtitleText.text = currentSubtitle.text;
        index++;
    }

    private void GetNextSubtitle()
    {
        currentSubtitle = subtitledAudio.GetNextSubtitle(index);
        index++;
        if (currentSubtitle == null)
        {
            isDone = true;
            StopAudio();
            return;
        }
        subtitleText.text = currentSubtitle.text;
    }

    public void PlayAudio()
    {
        isPlaying = true;
        isDone = false;
        startButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        GetFirstSubtitle();
        audioSource.Play();
    }

    public void StopAudio()
    {
        
        isPlaying = false;
        startButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        subtitleText.text = string.Empty;
        audioSource.Stop();
    }
}
