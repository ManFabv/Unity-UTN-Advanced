using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Button))]
public class SoundButton : MonoBehaviour
{
    private AudioSource cachedAudioSource;
    private Button cachedButton;
    
    private void Awake()
    {
        cachedAudioSource = this.GetComponent<AudioSource>();
        cachedAudioSource.loop = false;
        cachedAudioSource.playOnAwake = false;
        cachedAudioSource.Stop();
        
        cachedButton = this.GetComponent<Button>();
        cachedButton.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        cachedAudioSource.Play();
    }
}