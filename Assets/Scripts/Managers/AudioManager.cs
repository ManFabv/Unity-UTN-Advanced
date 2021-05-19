using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private AudioSource MusicAudioSource;
    [SerializeField] private AudioSource EndLevelAudioSource;
    [SerializeField] private AudioSource AmbientAudioSource;
    [SerializeField] private AudioMixerSnapshot GameplayAudioSnapShot;
    [SerializeField] private AudioMixerSnapshot MuteAudioSnapShot;
    [SerializeField] private AudioClip WinMusic;
    [SerializeField] private AudioClip LoseMusic;
    [SerializeField] private float SecondsToTransitionAudioSnapshot = 1;
#pragma warning restore 0649

    private void Awake()
    {
        if(AmbientAudioSource == null)
            Debug.LogError("EL " + typeof(AudioSource) + " ES NULO EN " + nameof(AmbientAudioSource));
        if(EndLevelAudioSource == null)
            Debug.LogError("EL " + typeof(AudioSource) + " ES NULO EN " + nameof(EndLevelAudioSource));
        if(MusicAudioSource == null)
            Debug.LogError("EL " + typeof(AudioSource) + " ES NULO EN " + nameof(MusicAudioSource));
        if(WinMusic == null)
            Debug.LogError("EL " + typeof(AudioClip) + " ES NULO EN " + nameof(WinMusic));
        if(LoseMusic == null)
            Debug.LogError("EL " + typeof(AudioClip) + " ES NULO EN " + nameof(LoseMusic));
        if(MuteAudioSnapShot == null)
            Debug.LogError("EL " + typeof(AudioMixerSnapshot) + " ES NULO EN " + nameof(MuteAudioSnapShot));
        if (GameplayAudioSnapShot == null)
        {
            Debug.LogError("EL " + typeof(AudioMixerSnapshot) + " ES NULO EN " + nameof(GameplayAudioSnapShot));
        }
        else
        {
            StartCoroutine(TransitionToAudioSnapshot(GameplayAudioSnapShot));
        }
    }

    private IEnumerator TransitionToAudioSnapshot(AudioMixerSnapshot mixerSnapshot)
    {
        yield return new WaitForEndOfFrame();
        AmbientAudioSource.Play();
        MusicAudioSource.Play();
        yield return new WaitForEndOfFrame();
        mixerSnapshot.TransitionTo(SecondsToTransitionAudioSnapshot);
    }

    public void PlayWinMusic()
    {
        if(WinMusic != null && MusicAudioSource != null)
        {
            MusicAudioSource.Stop();
            EndLevelAudioSource.clip = WinMusic;
            EndLevelAudioSource.PlayDelayed(1);
        }
    }

    public void PlayLoseMusic()
    {
        if(LoseMusic != null && MusicAudioSource != null)
        {
            MusicAudioSource.Stop();
            EndLevelAudioSource.clip = LoseMusic;
            EndLevelAudioSource.PlayDelayed(2);
        }
    }

    public void TransitionToMuteSnapshot()
    {
        StartCoroutine(TransitionToAudioSnapshot(MuteAudioSnapShot));
    }
}
