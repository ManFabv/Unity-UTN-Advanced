using UnityEngine;

public class Damage : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int Daño = 5;
#pragma warning restore 0649

    private string tagToApplyDamage = "Untagged";

    public void TagToApplyDamage(string tag)
    {
        if(string.IsNullOrEmpty(tag))
            Debug.LogError("NO HAY UN TAG SELECCIONADO EN " + nameof(tag) + ". USANDO POR DEFECTO UNTAGGED");
        else
            tagToApplyDamage = tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vida vida = other.gameObject.GetComponent<Vida>();
        
        if (vida != null && other.gameObject.CompareTag(tagToApplyDamage))
        {
            vida.Dañar(Daño);
        }

        Castle castle = other.gameObject.GetComponent<Castle>();
        castle?.Dañar(Daño);
        
        HitParticleSystem hitParticleSystem = other.transform.GetComponent<HitParticleSystem>();
        hitParticleSystem?.InstantiateHitParticles(other);

        DestructorTemporizado destructorTemporizado = other.gameObject.GetComponent<DestructorTemporizado>();

        if(destructorTemporizado != null)
            destructorTemporizado.Destruir();
        else
            Destroy(this.gameObject);
    }
}