using UnityEngine;

public class MovimientoContinuo : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float velocidadX;
    [SerializeField] private float velocidadY;
    [SerializeField] private float velocidadZ;
#pragma warning restore 0649

    private Transform cachedTransform;

    public void ChangeVelocity(float velX, float velY, float velZ, bool allowOnlyInXZ = true)
    {
        velocidadX = velX;
        velocidadY = allowOnlyInXZ ? 0 : velY;
        velocidadZ = velZ;
    }

    public void ChangeVelocity(Vector3 vel, bool allowOnlyInXZ = true)
    {
        ChangeVelocity(vel.x, vel.y, vel.z, allowOnlyInXZ);
    }

    private void Awake()
    {
        cachedTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        cachedTransform.Translate(velocidadX * Time.deltaTime, velocidadY * Time.deltaTime, velocidadZ * Time.deltaTime);
    }
}
