using UnityEngine;

public class FaceCamera : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Transform targetCamera;
#pragma warning restore 0649

    private Transform cachedTransform;

    private void Awake()
    {
        cachedTransform = this.GetComponent<Transform>();
        
        if (targetCamera == null)
            targetCamera = Camera.main?.transform;
    }

    private void Update()
    {
        cachedTransform.LookAt(targetCamera);
    }
}