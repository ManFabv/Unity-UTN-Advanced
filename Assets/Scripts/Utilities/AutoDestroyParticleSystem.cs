using UnityEngine;

public class AutoDestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem cachedParticleSystem;

    private void Awake()
    {
        cachedParticleSystem = this.GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if(!cachedParticleSystem.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }
}