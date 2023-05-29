using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioClip List")]
public class AudioClipList : ScriptableObject
{
    public List<AudioClip> clips;
}
