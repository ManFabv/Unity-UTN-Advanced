using UnityEngine;

public class AffectedByGravity : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float GravityAcceleration = -15.0f;
#pragma warning restore 0649
    
    private Vector3 movePosition = Vector3.zero;

    private CharacterController cachedLocalCharacterController;
    private Transform cachedLocalTransform;

    private void Awake()
    {
        cachedLocalCharacterController = this.GetComponent<CharacterController>();
        cachedLocalTransform = this.GetComponent<Transform>();
    }

    private void Update()
    {
        if (cachedLocalCharacterController.isGrounded)
            movePosition.y = 0;
        
        movePosition.y += GravityAcceleration * Time.deltaTime * Time.deltaTime;
        
        cachedLocalCharacterController.Move(movePosition);
    }
}
