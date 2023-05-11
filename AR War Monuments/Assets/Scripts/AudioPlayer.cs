using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public SubtitledAudio audio;
    public TMP_Text subtitleText;
    public AudioSource audioSource;
    public Button startButton;
    
    private Subtitle currentSubtitle;
    private float timer = 0;
    private bool isPlaying = false, isDone = false;
    private int index = 0;
    private void Awake()
    {
        currentSubtitle = audio.GetNextSubtitle(index);
        index++;
        audioSource.clip = audio.audioClip;
        subtitleText.text = currentSubtitle.text;
    }

    private void Update()
    {
        if(!isPlaying || isDone)
            return;
        timer += Time.deltaTime;
        if (timer > currentSubtitle.duration)
        {
            currentSubtitle = audio.GetNextSubtitle(index);
            index++;
            if (currentSubtitle == null)
            {
                isDone = true;
                return;
            }
            subtitleText.text = currentSubtitle.text;
            timer = 0;
        }
    }

    public void PlayAudio()
    {
        isPlaying = true;
        startButton.gameObject.SetActive(false);
        audioSource.Play();
    }
}
