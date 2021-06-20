using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameLevelManager GameManager;
    [SerializeField] private int vida = 100;
    [SerializeField] private AudioSource cachedDamageAudioSource;
    [SerializeField] private ParticleSystem cachedParticleSystem;
    [SerializeField] private Vida playerVida;
    
    [SerializeField] private Slider HealthBar;
    [SerializeField] private Image HealthBarFill;
    [SerializeField] private Gradient healthBarGradient;
#pragma warning restore 0649

    private bool toBeDestroy = false;

    private void Awake()
    {
        if (healthBarGradient == null)
            Debug.LogError("EL " + typeof(Gradient) + " ES NULO EN " + nameof(healthBarGradient));
        if (HealthBarFill == null)
            Debug.LogError("EL " + typeof(Image) + " ES NULO EN " + nameof(HealthBarFill));

        if (HealthBar == null)
        {
            Debug.LogError("EL " + typeof(Slider) + " ES NULO EN " + nameof(HealthBar));
        }
        else
        {
            HealthBar.minValue = 0;
            HealthBar.maxValue = vida;
            UpdateUIVida();
        }
    }

    private void Update()
    {
        if (vida <= 0 && toBeDestroy == false)
        {
            toBeDestroy = true;
            playerVida.SetVidaCero();
            GameManager.OnCastleDestroyed();
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.0f);
        
        cachedParticleSystem?.Play();
    }
    
    public void Dañar(int daño)
    {
        vida -= daño;

        cachedDamageAudioSource?.Play();
        
        UpdateUIVida();
    }
    
    private void UpdateUIVida()
    {
        if (HealthBar != null)
        {
            HealthBar.value = vida;
            if (HealthBarFill != null && healthBarGradient != null)
            {
                HealthBarFill.color = healthBarGradient.Evaluate(HealthBar.normalizedValue);
            }
        }
    }
}