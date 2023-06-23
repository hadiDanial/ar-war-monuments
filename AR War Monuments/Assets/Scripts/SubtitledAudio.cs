using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Subtitled Audio")]
public class SubtitledAudio : ScriptableObject
{
    public AudioClip audioClip;
    public List<Subtitle> subtitles;
    
    public Subtitle GetNextSubtitle(int index)
    {
        if(index < subtitles.Count)
        {
            Subtitle subtitle = subtitles[index];
            return subtitle;
        }
        return null;
    }
}

[Serializable]
public class Subtitle
{
    public string text;
    public float duration;
}
