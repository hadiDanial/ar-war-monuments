using UnityEngine;

/// <summary>
/// Singleton for playing AudioClips.
/// </summary>
[RequireComponent(typeof(PreferenceManager))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public static AudioManager Instance { get; private set; }
    private bool isMuted = false;

    private PreferenceManager preferenceManager;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            preferenceManager = GetComponent<PreferenceManager>();
            isMuted = preferenceManager.GetMutedSetting();
        }
    }

    public void Play(AudioClip audioClip)
    {
        if (isMuted) return;
        audioSource.PlayOneShot(audioClip);
    }

    public void Play(AudioSource source, AudioClip audioClip)
    {
        if (isMuted) return;
        source.PlayOneShot(audioClip);
    }
    public void PlayFromList(Vector3 position, AudioClipList audioClipList)
    {
        if (isMuted) return;
        int index = UnityEngine.Random.Range(0, audioClipList.clips.Count);
        AudioClip audioClip = audioClipList.clips[index];
        AudioSource.PlayClipAtPoint(audioClip, position);
        
//        source.pitch = UnityEngine.Random.Range(0.98f, 1.02f);
  //      source.PlayOneShot(audioClip);
    }
    
    public void PlayAtPoint(AudioClip audioClip, Vector3 position)
    {
        if (isMuted) return;
        AudioSource.PlayClipAtPoint(audioClip, position);
    }
} 