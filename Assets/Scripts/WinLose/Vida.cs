using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Vida : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int vida = 100;
    [SerializeField] private AudioSource cachedDamageAudioSource;
#pragma warning restore 0649

    private bool toBeDestroy = false;
    private MeshRenderer[] MeshRenderers;
    private SkinnedMeshRenderer[] SkinnedMeshRenderers;
    private ParticleSystem[] ParticleSystems;
    private bool dead = false;
    private Collider[] cachedColliders;
    
    public int MaxLife { get; private set; }
    public int CurrentVida => vida;

    private void Awake()
    {
        MaxLife = vida;
        if(cachedDamageAudioSource == null)
            cachedDamageAudioSource = this.GetComponent<AudioSource>();
        MeshRenderers = this.GetComponentsInChildren<MeshRenderer>(true);
        SkinnedMeshRenderers = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        ParticleSystems = this.GetComponentsInChildren<ParticleSystem>(true);

        cachedColliders = this.GetComponentsInChildren<Collider>(true);
    }

    private void Update()
    {
        if (!dead && vida <= 0 && toBeDestroy == false)
        {
            dead = true;
            toBeDestroy = true;
            SendMessage("Murio");
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        StopAllParticleSystems();
        DisableAllColliders();
        yield return new WaitForSeconds(2);
        DisableAllMeshRenderers();
        DisableAllSkinnedMeshRenderers();
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
    }

    private void DisableAllColliders()
    {
        if (cachedColliders != null)
        {
            foreach (Collider col in cachedColliders)
            {
                col.enabled = false;
            }
        }
    }

    private void DisableAllMeshRenderers()
    {
        if (MeshRenderers != null)
        {
            foreach (MeshRenderer meshRenderer in MeshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }
    }
    
    private void DisableAllSkinnedMeshRenderers()
    {
        if (SkinnedMeshRenderers != null)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in SkinnedMeshRenderers)
            {
                skinnedMeshRenderer.enabled = false;
            }
        }
    }
    
    private void StopAllParticleSystems()
    {
        if (ParticleSystems != null)
        {
            foreach (ParticleSystem particleSystem in ParticleSystems)
            {
                particleSystem.Stop();
            }
        }
    }

    public void CambiarVida(int nuevaVida)
    {
        vida = nuevaVida;
    }

    public void Dañar(int daño)
    {
        vida -= daño;

        cachedDamageAudioSource?.Play();
        
        SendMessage("TakeDamage", SendMessageOptions.DontRequireReceiver);
    }

    public void Curar(int amount)
    {
        vida = Mathf.Min(vida + amount, MaxLife);
    }

    public void SetVidaCero()
    {
        vida = 0;
    }
}