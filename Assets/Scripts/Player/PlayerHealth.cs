using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Vida))]
public class PlayerHealth : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private GameLevelManager GameManager;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private Image HealthBarFill;
    [SerializeField] private Gradient healthBarGradient;
#pragma warning restore 0649

    private Vida cachedVida;

    private void Awake()
    {
        cachedVida = this.GetComponent<Vida>();
        
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
            HealthBar.maxValue = cachedVida.CurrentVida;
            UpdateUIVida();
        }
    }

    public void SetGameManager(GameLevelManager gameManager)
    {
        if (gameManager == null)
            Debug.LogError("EL " + typeof(GameLevelManager) + " ES NULO EN " + nameof(gameManager));
        else
            GameManager = gameManager;
    }

    public void Murio()
    {
        GameManager?.OnJugadorMurio();
    }

    public void TakeDamage()
    {
        UpdateUIVida();
    }

    private void UpdateUIVida()
    {
        if (HealthBar != null)
        {
            HealthBar.value = cachedVida.CurrentVida;
            if (HealthBarFill != null && healthBarGradient != null)
            {
                HealthBarFill.color = healthBarGradient.Evaluate(HealthBar.normalizedValue);
            }
        }
    }
}