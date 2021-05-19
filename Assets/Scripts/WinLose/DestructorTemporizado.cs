using UnityEngine;

public class DestructorTemporizado : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float TimeToExplode = 5;
#pragma warning restore 0649

    private void Awake()
    {
        Invoke("Destruir", TimeToExplode);
    }

    private void Destruir()
    {
        Destroy(this.gameObject);
    }
}