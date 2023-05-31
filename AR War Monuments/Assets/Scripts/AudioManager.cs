using UnityEngine;

/// <summary>
/// Singleton for playing AudioClips.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Play(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void Play(AudioSource source, AudioClip audioClip)
    {
        source.PlayOneShot(audioClip);
    }
    public void PlayFromList(Vector3 position, AudioClipList audioClipList)
    {
        int index = UnityEngine.Random.Range(0, audioClipList.clips.Count);
        AudioClip audioClip = audioClipList.clips[index];
        AudioSource.PlayClipAtPoint(audioClip, position);
        
//        source.pitch = UnityEngine.Random.Range(0.98f, 1.02f);
  //      source.PlayOneShot(audioClip);
    }
    
    public void PlayAtPoint(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position);
    }
} 