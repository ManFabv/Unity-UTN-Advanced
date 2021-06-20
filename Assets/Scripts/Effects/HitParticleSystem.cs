using UnityEngine;

public class HitParticleSystem : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private ParticleSystem cachedParticleSystem;
#pragma warning restore 0649

    public void InstantiateHitParticles(RaycastHit hitInfo)
    {
        if (cachedParticleSystem != null)
        {
            cachedParticleSystem.transform.position = hitInfo.point;
            cachedParticleSystem.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            cachedParticleSystem.Play();
        }
    }
    
    public void InstantiateHitParticles(Collider other)
    {
        if (cachedParticleSystem != null)
        {
            cachedParticleSystem.transform.position = other.transform.position;
            cachedParticleSystem.transform.rotation = other.transform.rotation;
            cachedParticleSystem.Play();
        }
    }
}