using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DelayAudio : MonoBehaviour
{
    [Header("Settings")]
    public float delay = 0.5f; // seconds to wait before playing
    public bool loop = false; // should it loop after playing?

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false; // make sure it doesn't auto-play
    }

    void OnEnable()
    {
        Invoke(nameof(PlayAudio), delay);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(PlayAudio));
    }

    private void PlayAudio()
    {
        audioSource.loop = loop;
        audioSource.Play();
    }
}
