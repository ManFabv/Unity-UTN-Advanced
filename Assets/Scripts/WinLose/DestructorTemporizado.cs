using UnityEngine;

public class DestructorTemporizado : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float TimeToExplode = 5;
    [SerializeField] private bool ReturnObjectToPoolInstead = false;
#pragma warning restore 0649

    public ObjectPool ObjectPool { get; set; }

    private void OnEnable()
    {
        Invoke("Destruir", TimeToExplode);
    }

    private void OnDisable()
    {
        CancelInvoke("Destruir");
    }

    public void Destruir()
    {
        if(ReturnObjectToPoolInstead)
            ObjectPool?.ReturnObjectToPool(this.gameObject);
        else
            Destroy(this.gameObject);
    }
}